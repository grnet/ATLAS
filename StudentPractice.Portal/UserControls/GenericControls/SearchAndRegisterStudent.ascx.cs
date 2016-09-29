using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Imis.Domain;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;
using StudentPractice.Utils;

namespace StudentPractice.Portal.UserControls.GenericControls
{
    public class StudentAssignedEventArgs : EventArgs
    {
        public int StudentID { get; set; }
    }

    public class InitAcademicsEventArgs : EventArgs
    {
        public InitAcademicsEventArgs()
        {
            Academics = new List<Academic>();
        }

        public List<Academic> Academics { get; set; }
        public bool AllowAllAcademics { get; set; }
    }

    public partial class SearchAndRegisterStudent : BaseEntityUserControl<object>
    {
        #region [ Properties ]

        protected enum enSearchMethod
        {
            None,
            AcademicID,
            StudentNumber,
            Normal
        }

        protected enSearchMethod SearchMethod
        {
            get
            {
                if (ViewState["SearchAndRegisterStudent_SearchMethod"] != null)
                    return (enSearchMethod)ViewState["SearchAndRegisterStudent_SearchMethod"];
                else
                    return enSearchMethod.None;
            }
            set { ViewState["SearchAndRegisterStudent_SearchMethod"] = value; }
        }

        private int _pageSize = 10;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        public IUnitOfWork UnitOfWork { get; set; }

        public int CurrentUserID
        {
            get
            {
                return Context.LoadReporterID();
            }
        }

        public bool ShowCancelButtons { get; set; }

        public bool ConfirmStudentRegistration { get; set; }

        public bool AllowStudentAssignment { get; set; }

        public InternshipPosition CurrentPosition { get; set; }

        public bool AllowAllInstitutions { get; set; }

        public bool IsHelpdesk { get; set; }

        private List<Academic> _academics = null;
        private bool _allowAllAcademics = false;

        #endregion

        #region [ Events ]

        public event EventHandler<InitAcademicsEventArgs> InitAcademics;

        public event EventHandler<StudentAssignedEventArgs> StudentAssigned;

        public event EventHandler Cancel;

        protected void FireStudentAssignedEvent(int studentID)
        {
            var handler = StudentAssigned;
            if (handler != null)
                handler(this, new StudentAssignedEventArgs() { StudentID = studentID });
        }

        protected void FireCancelEvent()
        {
            var handler = Cancel;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        #endregion

        #region [ DataBind ]

        protected override void Render(HtmlTextWriter writer)
        {
            IList<Academic> academics = CacheManager.Academics.GetItems().ToList();
            Page.ClientScript.RegisterForEventValidation(ddlDepartmentByInstSearch.UniqueID, string.Empty);
            foreach (Academic academic in academics)
            {
                Page.ClientScript.RegisterForEventValidation(ddlDepartmentByInstSearch.UniqueID, academic.ID.ToString());
            }

            base.Render(writer);
        }

        protected void ddlInstitution_Init(object sender, EventArgs e)
        {
            ddlInstitution.Items.Add(new ListItem("-- αδιάφορο --", ""));

            foreach (var item in CacheManager.Institutions.GetItems().OrderBy(x => x.NameInGreek))
            {
                ddlInstitution.Items.Add(new ListItem(item.Name, item.ID.ToString()));
            }
        }

        protected void ddlDepartment_Init(object sender, EventArgs e)
        {
            if (!AllowAllInstitutions)
            {
                EnsureAcademics();

                ddlDepartmentSearch.Items.Add(new ListItem("-- αδιάφορο --", ""));
                foreach (var item in _academics.OrderBy(x => x.DepartmentInGreek))
                    ddlDepartmentSearch.Items.Add(new ListItem(item.Department, item.ID.ToString()));
            }
        }

        protected override void OnInit(EventArgs e)
        {
            gvStudents.SettingsPager.PageSize = _pageSize;
            gvStudentsByAcademicID.SettingsPager.PageSize = _pageSize;
            gvStudentsByStudentNumber.SettingsPager.PageSize = _pageSize;

            btnPreviewByAcademicIDNumber.Visible = IsHelpdesk;
            btnPreviewByStudentNumber.Visible = IsHelpdesk;

            if (IsHelpdesk)
            {
                btnSearchByAcademicIDNumber.Text = "Αναζήτηση & Εγγραφή";
                btnSearchByStudentNumber.Text = "Αναζήτηση & Εγγραφή";
                dxPageControl.TabPages[2].Visible = false;
            }

            base.OnInit(e);
        }

        public override void Bind()
        {
            if (!Page.IsPostBack)
            {
                if (AllowAllInstitutions)
                {
                    phInstitutionSearch.Visible = true;
                }
                else
                {
                    phAcademicSearch.Visible = true;

                    gvStudents.Columns["Institution"].Visible = false;
                    gvStudentsByAcademicID.Columns["Institution"].Visible = false;
                    gvStudentsByStudentNumber.Columns["Institution"].Visible = false;

                    EnsureAcademics();
                    if (_allowAllAcademics)
                    {
                        litDepartment.Visible = false;
                    }
                    else
                    {
                        ddlDepartmentSearch.Visible = false;
                        rfvDepartmentDDL.Visible = false;
                        litDepartment.Text = CacheManager.Academics.Get(_academics.FirstOrDefault().ID).Department;
                    }
                }

                txtFirstName.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));
                txtLastName.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));

                if (!ShowCancelButtons)
                {
                    btnCancel.Visible =
                        btnCancelByAcademicIDNumber.Visible =
                        btnCancelByStudentNumber.Visible = false;
                }

                if (!AllowStudentAssignment)
                {
                    gvStudents.Columns["Actions"].Visible = false;
                    gvStudentsByAcademicID.Columns["Actions"].Visible = false;
                    gvStudentsByStudentNumber.Columns["Actions"].Visible = false;
                }
            }
        }

        public void ReBindGrid()
        {
            if (ActiveGrid != null)
            {
                ActiveGrid.PageIndex = 0;
                ActiveGrid.DataBind();
            }
        }

        #endregion

        #region [ Helpers ]

        private void EnsureAcademics()
        {
            if (_academics == null)
            {
                var args = new InitAcademicsEventArgs();
                if (InitAcademics != null)
                {
                    InitAcademics(this, args);
                    _academics = args.Academics;
                    _allowAllAcademics = args.AllowAllAcademics;
                }
            }
        }

        private void ShowActiveGrid()
        {
            switch (SearchMethod)
            {
                case enSearchMethod.AcademicID:
                    gvStudents.Visible = false;
                    gvStudentsByAcademicID.Visible = true;
                    gvStudentsByStudentNumber.Visible = false;
                    break;
                case enSearchMethod.StudentNumber:
                    gvStudents.Visible = false;
                    gvStudentsByAcademicID.Visible = false;
                    gvStudentsByStudentNumber.Visible = true;
                    break;
                case enSearchMethod.Normal:
                    gvStudents.Visible = true;
                    gvStudentsByAcademicID.Visible = false;
                    gvStudentsByStudentNumber.Visible = false;
                    break;
                case enSearchMethod.None:
                default:
                    gvStudents.Visible = false;
                    gvStudentsByAcademicID.Visible = false;
                    gvStudentsByStudentNumber.Visible = false;
                    break;
            }
        }

        protected ASPxGridView ActiveGrid
        {
            get
            {
                switch (SearchMethod)
                {
                    case enSearchMethod.AcademicID:
                        return gvStudentsByAcademicID;
                    case enSearchMethod.StudentNumber:
                        return gvStudentsByStudentNumber;
                    case enSearchMethod.Normal:
                        return gvStudents;
                    case enSearchMethod.None:
                    default:
                        return null;
                }
            }
        }

        private void HandleResponse(StudentDetailsFromAcademicID response, bool byAcademicIDNumber)
        {
            using (IUnitOfWork uow = UnitOfWorkFactory.Create())
            {
                var sRep = new StudentRepository(uow);
                bool foundByPrevious = false;
                var student = sRep.FindActiveByStudentNumberAndAcademicID(response.StudentNumber, response.AcademicID);
                if (student == null && !string.IsNullOrEmpty(response.PreviousStudentNumber) && response.PreviousAcademicID.HasValue)
                {
                    foundByPrevious = true;
                    student = sRep.FindActiveByStudentNumberAndAcademicID(response.PreviousStudentNumber, response.PreviousAcademicID.Value);
                }

                if (student != null)
                {
                    if (!byAcademicIDNumber && student.AcademicID != response.AcademicID && student.PreviousAcademicID == response.AcademicID)
                    {
                        Session["flashStudentNumber"] = "Ο φοιτητής βρέθηκε με τα παλιά ακαδημαικά του στοιχεία.";
                    }

                    if (byAcademicIDNumber && student.AcademicIDSubmissionDate.HasValue && response.AcademicIDSubmissionDate.HasValue
                        && student.AcademicIDSubmissionDate.Value > response.AcademicIDSubmissionDate.Value)
                    {
                        Session["flashAcademicNumber"] = "Βρέθηκε νεότερη κάρτα αποθηκευμένη στο σύστημα για τον φοιτητή από αυτήν που δώσατε.";
                    }

                    if (!AllowAllInstitutions && !_academics.Any(x => x.ID == student.AcademicID) && !_academics.Any(x => x.ID == student.PreviousAcademicID))
                    {
                        Session[byAcademicIDNumber ? "flashAcademicNumber" : "flashStudentNumber"] = "Ο φοιτητης δεν ανήκει σε τμήμα που εξυπηρετεί αυτό το γραφείο.";
                    }

                    if (foundByPrevious)
                    {
                        student.AcademicID = response.AcademicID;
                        student.StudentNumber = response.StudentNumber;
                        BusinessHelper.UpdateStudentGeneralInfo(student, response);
                    }

                    BusinessHelper.UpdateStudentAcademicInfo(student, response);
                    uow.Commit();
                }

                else
                {
                    var academic = CacheManager.Academics.Get(response.AcademicID);
                    if (academic != null && academic.Institution != null)// && academic.IsActive)
                        CreateStudent(response, academic);
                }
            }
        }

        private void ShowResultDialog(StudentDetailsFromAcademicID response, bool byAcademicIDNumber)
        {
            using (IUnitOfWork uow = UnitOfWorkFactory.Create())
            {
                var sRep = new StudentRepository(uow);
                var result = string.Empty;
                var student = sRep.FindActiveByStudentNumberAndAcademicID(response.StudentNumber, response.AcademicID);

                if (student == null && !string.IsNullOrEmpty(response.PreviousStudentNumber) && response.PreviousAcademicID.HasValue)
                    student = sRep.FindActiveByStudentNumberAndAcademicID(response.PreviousStudentNumber, response.PreviousAcademicID.Value);

                if (student != null)
                {
                    if (!byAcademicIDNumber && student.AcademicID != response.AcademicID && student.PreviousAcademicID == response.AcademicID)
                    {
                        result = string.Format("Ο Φοιτητής, <b>{0} {1}</b>, υπάρχει εγγρραμμένος στο σύστημα με νεότερα ακαδημαϊκά στοιχεία (<b>{2} - {3}</b>) από αυτά με τα οποία έγινε αναζήτηση (<b>{4} - {5}</b>).",
                                    student.OriginalFirstName,
                                    student.OriginalLastName,
                                    CacheManager.Academics.Get(student.AcademicID.Value).DepartmentInGreek,
                                    student.StudentNumber,
                                    CacheManager.Academics.Get(response.AcademicID).DepartmentInGreek,
                                    response.StudentNumber);
                    }

                    if (byAcademicIDNumber && student.AcademicIDSubmissionDate.HasValue && response.AcademicIDSubmissionDate.HasValue
                        && student.AcademicIDSubmissionDate.Value > response.AcademicIDSubmissionDate.Value)
                    {
                        result = string.Format("Ο Φοιτητής, <b>{0} {1}</b>, υπάρχει εγγρραμμένος στο σύστημα με νεότερα στοιχεία ακαδημαϊκής κάρτας (<b>{2}</b>) από αυτά με τα οποία έγινε αναζήτηση (<b>{3}</b>).",
                                    student.OriginalFirstName,
                                    student.OriginalLastName,
                                    student.AcademicIDNumber,
                                    response.AcademicIDNumber);
                    }
                }

                else
                {
                    result = string.Format(@"Βρέθηκε ο Φοιτητής, <b>{0} {1}</b>, με ακαδημαϊκά στοιχεία από το δράση του Φοιτητικού Πάσου:
                                            <ul>
                                                <li>Ίδρυμα: <b>{2}</b></li>
                                                <li>Τμήμα: <b>{3}</b></li>
                                                <li>ΑΜ: <b>{4}</b></li>
                                                <li>Αριθμό Ακαδημαϊκής κάρτας: <b>{5}</b></li>
                                          </ul>",
                                    response.OriginalFirstName,
                                    response.OriginalLastName,
                                    CacheManager.Academics.Get(response.AcademicID).InstitutionInGreek,
                                    CacheManager.Academics.Get(response.AcademicID).DepartmentInGreek,
                                    response.StudentNumber,
                                    response.AcademicIDNumber);
                }
                if (byAcademicIDNumber)
                {
                    ltrAcademicID.Text = result;
                    gvStudentsByAcademicID.Visible = false;
                }
                else
                {
                    ltrStudentNumber.Text = result;
                    gvStudentsByStudentNumber.Visible = false;
                }
            }
        }

        private void CreateStudent(StudentDetailsFromAcademicID response, Academic academic)
        {
            using (var ctx = UnitOfWorkFactory.Create() as DBEntities)
            {
                if (ctx.Connection.State != ConnectionState.Open)
                    ctx.Connection.Open();

                using (var ts = ctx.Connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        StudentRepository studentRepository = new StudentRepository(UnitOfWork);
                        var newStudent = studentRepository.CreateStudent(response);

                        var provider = Roles.Provider as StudentPracticeRoleProvider;
                        provider.AddUsersToRoles(new[] { newStudent.UserName }, new[] { RoleNames.Student }, ctx);
                        ts.Commit();
                    }
                    catch (Exception ex)
                    {
                        ts.Rollback();
                        LogHelper.LogError(ex, this, string.Format("Error while creating student from AcademicID with AcademicIDNumber {0}", txtAcademicIDNumberSearch.Text.ToNull()));
                    }
                }
            }
        }

        #endregion

        #region [ Button events ]

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            FireCancelEvent();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchMethod = enSearchMethod.Normal;
            ShowActiveGrid();
            ReBindGrid();
        }

        
        protected void btnSearchByAcademicIDNumber_Click(object sender, EventArgs e)
        {
            SearchMethod = enSearchMethod.AcademicID;
            ShowActiveGrid();

            var academicIDNumber = txtAcademicIDNumberSearch.Text.ToNull().Replace("-", string.Empty);
            var student = new StudentRepository(UnitOfWork).FindActiveByAcademicIDNumber(academicIDNumber);

            if (student == null)
            {
                var response = StudentCardNumberService.GetStudentInfoByAcademicCardNumber(CurrentUserID, academicIDNumber);
                if (response != null && response.Success)
                    HandleResponse(response, true);
            }

            ReBindGrid();
        }

        protected void btnPreviewByAcademicIDNumber_Click(object sender, EventArgs e)
        {
            SearchMethod = enSearchMethod.AcademicID;
            ShowActiveGrid();
            ltrAcademicID.Text = string.Empty;

            var academicIDNumber = txtAcademicIDNumberSearch.Text.ToNull().Replace("-", string.Empty);
            var student = new StudentRepository(UnitOfWork).FindActiveByAcademicIDNumber(academicIDNumber);

            if (student == null)
            {
                var response = StudentCardNumberService.GetStudentInfoByAcademicCardNumber(CurrentUserID, academicIDNumber);
                if (response != null && response.Success)
                    ShowResultDialog(response, true);                    
            }
            else
            {
                ReBindGrid();
            }
        }


        protected void btnSearchByStudentNumber_Click(object sender, EventArgs e)
        {
            SearchMethod = enSearchMethod.StudentNumber;
            ShowActiveGrid();
            EnsureAcademics();

            var studentNumber = txtStudentNumberSearch.Text.ToNull();
            int academicID;
            if (AllowAllInstitutions)
                int.TryParse(ddlDepartmentByInstSearch.SelectedItem.Value, out academicID);
            else
            {
                if (_allowAllAcademics)
                    int.TryParse(ddlDepartmentSearch.SelectedItem.Value, out academicID);
                else
                    academicID = _academics.FirstOrDefault().ID;
            }

            var student = new StudentRepository(UnitOfWork).FindActiveByStudentNumberAndAcademicID(studentNumber, academicID);

            if (student == null)
            {
                var response = StudentCardNumberService.GetStudentInfo(new AcademicIDCardRequest()
                {
                    AcademicID = academicID,
                    StudentNumber = studentNumber,
                    ServiceCallerID = CurrentUserID
                });

                if (response != null && response.Success)
                    HandleResponse(response, false);
            }

            ReBindGrid();
        }

        protected void btnPreviewByStudentNumber_Click(object sender, EventArgs e)
        {
            SearchMethod = enSearchMethod.StudentNumber;
            ShowActiveGrid();
            EnsureAcademics();
            ltrStudentNumber.Text = string.Empty;

            var studentNumber = txtStudentNumberSearch.Text.ToNull();
            int academicID;
            if (AllowAllInstitutions)
                int.TryParse(ddlDepartmentByInstSearch.SelectedItem.Value, out academicID);
            else
            {
                if (_allowAllAcademics)
                    int.TryParse(ddlDepartmentSearch.SelectedItem.Value, out academicID);
                else
                    academicID = _academics.FirstOrDefault().ID;
            }

            var student = new StudentRepository(UnitOfWork).FindActiveByStudentNumberAndAcademicID(studentNumber, academicID);

            if (student == null)
            {
                var response = StudentCardNumberService.GetStudentInfo(new AcademicIDCardRequest()
                {
                    AcademicID = academicID,
                    StudentNumber = studentNumber,
                    ServiceCallerID = CurrentUserID
                });

                if (response != null && response.Success)
                    ShowResultDialog(response, false);                   
            }
            else
            {
                ReBindGrid();
            }
        }

        #endregion

        #region [ DataSource Events ]

        protected void odsStudents_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            EnsureAcademics();
            if (CurrentPosition == null)
                CurrentPosition = HttpContext.Current.LoadPosition();

            Criteria<Student> criteria = new Criteria<Student>();

            criteria.Expression = criteria.Expression.Where(x => x.DeclarationType, enReporterDeclarationType.FromRegistration);
            criteria.Expression = criteria.Expression.Where(x => x.IsActive, true);

            if (CurrentPosition != null && CurrentPosition.AssignedToStudentID.HasValue)
                criteria.Expression = criteria.Expression.Where(x => x.ID, CurrentPosition.AssignedToStudentID.Value, Imis.Domain.EF.Search.enCriteriaOperator.NotEquals);

            if (!AllowAllInstitutions)
            {
                if (_allowAllAcademics)
                {
                    var orExpression = Imis.Domain.EF.Search.Criteria<Student>.Empty;
                    orExpression = orExpression.Where(string.Format("it.AcademicID IN MULTISET ({0})", string.Join(",", _academics.Select(x => x.ID))));
                    orExpression = orExpression.Or(string.Format("it.PreviousAcademicID IN MULTISET ({0})", string.Join(",", _academics.Select(x => x.ID))));
                    criteria.Expression = criteria.Expression.And(orExpression);
                }
                else
                {
                    criteria.Expression = criteria.Expression.Where(x => x.AcademicID, _academics.FirstOrDefault().ID);
                }
            }

            switch (SearchMethod)
            {
                case enSearchMethod.AcademicID:

                    if(!string.IsNullOrWhiteSpace(txtAcademicIDNumberSearch.GetText()))
                    {
                        string academicIDNumber = txtAcademicIDNumberSearch.GetText().Replace("-",string.Empty);
                        criteria.Expression = criteria.Expression.Where(x => x.AcademicIDNumber, academicIDNumber);
                    }
                    break;

                case enSearchMethod.StudentNumber:
                    int? academicID = null;
                    if (AllowAllInstitutions)
                        academicID = ddlDepartmentByInstSearch.GetInteger().GetValueOrDefault();
                    else if (!AllowAllInstitutions && _allowAllAcademics)
                        academicID = ddlDepartmentSearch.GetInteger().GetValueOrDefault();
                    else if (!AllowAllInstitutions && !_allowAllAcademics)
                        academicID = _academics.FirstOrDefault().ID;

                    if (academicID.HasValue)
                    {
                        var expA = Imis.Domain.EF.Search.Criteria<Student>.Empty
                            .Where(x => x.AcademicID, academicID.Value)
                            .Where(x => x.StudentNumber, txtStudentNumberSearch.GetText());

                        var expB = Imis.Domain.EF.Search.Criteria<Student>.Empty
                            .Where(x => x.PreviousAcademicID, academicID.Value)
                            .Where(x => x.PreviousStudentNumber, txtStudentNumberSearch.GetText());
                        criteria.Expression = criteria.Expression.And(expA.Or(expB));
                    }
                    break;

                case enSearchMethod.Normal:
                    if (!string.IsNullOrEmpty(txtFirstName.Text))
                        criteria.Expression = criteria.Expression.Where(x => x.OriginalFirstName, txtFirstName.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);
                    if (!string.IsNullOrEmpty(txtLastName.Text))
                        criteria.Expression = criteria.Expression.Where(x => x.OriginalLastName, txtLastName.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);
                    break;
            }

            criteria.Sort
                .OrderBy(x => x.IsAssignedToPosition)
                .ThenBy(x => x.Academic.DepartmentInGreek)
                .ThenBy(x => x.GreekLastName)
                .ThenBy(x => x.GreekFirstName)
                .ThenBy(x => x.OriginalLastName)
                .ThenBy(x => x.OriginalFirstName);
            e.InputParameters["criteria"] = criteria;
        }

        #endregion

        #region [ GridView Events ]

        protected void gvStudents_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.Name == "Actions")
            {
                var student = ActiveGrid.GetRow(e.VisibleIndex) as Student;

                var tipAlreadyAssignedStudentTip = (System.Web.UI.WebControls.Image)e.Cell.FindControlRecursive("tipAlreadyAssignedStudent");
                tipAlreadyAssignedStudentTip.Visible = false;

                var tipStudentAcademicNotSelectableTip = (System.Web.UI.WebControls.Image)e.Cell.FindControlRecursive("tipStudentAcademicNotSelectable");
                tipStudentAcademicNotSelectableTip.Visible = false;

                var addImplementationDataButton = (System.Web.UI.WebControls.LinkButton)e.Cell.FindControlRecursive("btnAddImplementationData");
                addImplementationDataButton.Visible = false;

                var btnChangeAssignedStudent = (System.Web.UI.WebControls.LinkButton)e.Cell.FindControlRecursive("btnChangeAssignedStudent");
                btnChangeAssignedStudent.Visible = false;

                if (AllowStudentAssignment)
                {
                    if (CurrentPosition.InternshipPositionGroup.PositionCreationType != enPositionCreationType.FromOffice
                        && student.IsAssignedToPosition.GetValueOrDefault())
                    {
                        tipAlreadyAssignedStudentTip.Visible = true;
                    }
                    else
                    {
                        //Εάν η θέση δεν είναι διαθέσιμη για το Τμήμα του φοιτητή κάντον μη επιλέξιμο
                        if (CurrentPosition.InternshipPositionGroup.PositionCreationType != enPositionCreationType.FromOffice
                            && !CurrentPosition.InternshipPositionGroup.IsVisibleToAllAcademics.GetValueOrDefault()
                            && !CurrentPosition.InternshipPositionGroup.Academics.Select(x => x.ID).Contains(student.AcademicID.Value))
                        {
                            tipStudentAcademicNotSelectableTip.Visible = true;
                        }
                        else
                        {
                            int academicID = 0;
                            if (AllowAllInstitutions)
                                academicID = ddlDepartmentByInstSearch.GetInteger().GetValueOrDefault();
                            else if (_allowAllAcademics)
                                academicID = ddlDepartmentSearch.GetInteger().GetValueOrDefault();

                            if (CurrentPosition.InternshipPositionGroup.PositionCreationType == enPositionCreationType.FromOffice || !CurrentPosition.AssignedToStudentID.HasValue)
                            {
                                addImplementationDataButton.Visible = true;
                                addImplementationDataButton.Attributes.Add("onclick", string.Format("popUp.show('AddImplementationData.aspx?sID={0}&pID={1}&aID={2}', 'Προσθήκη Στοιχείων Εκτέλεσης Πρακτικής Άσκησης',cmdRefresh);", student.ID, CurrentPosition.ID, academicID));
                            }
                            else
                            {
                                btnChangeAssignedStudent.Visible = true;
                                btnChangeAssignedStudent.Attributes.Add("onclick", string.Format("popUp.show('ChangeAssignedStudent.aspx?sID={0}&pID={1}&aID={2}', 'Επεξεργασία Στοιχείων Φοιτητή',cmdRefresh);", student.ID, CurrentPosition.ID, academicID));
                            }

                        }
                    }
                }
            }
        }

        protected void gvStudents_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            Student student = ActiveGrid.GetRow(e.VisibleIndex) as Student;

            if (student != null)
            {
                if (student.IsAssignedToPosition.GetValueOrDefault())
                {
                    e.Row.BackColor = Color.LightGray;
                }
            }
        }

        protected void gvStudents_RowCommand(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewRowCommandEventArgs e)
        {
            var student = ActiveGrid.GetRow(e.VisibleIndex) as Student;

            if (e.CommandArgs.CommandName == "ChangeAssignedStudent")
            {
                FireStudentAssignedEvent(student.ID);
            }
        }

        #endregion

        #region [ Grid Helpers ]

        protected string GetInstitutionDetails(Student student)
        {
            if (student == null)
                return String.Empty;

            string inst = String.Empty;

            var academic = CacheManager.Academics.Get(student.AcademicID.Value);

            if (academic != null)
            {
                var institution = CacheManager.Institutions.Get(academic.InstitutionID);
                if (institution != null)
                    inst = institution.Name;
            }

            return inst;
        }

        protected string GetDepartmentDetails(Student student)
        {
            if (student == null)
                return String.Empty;

            string department = String.Empty;
            var academic = CacheManager.Academics.Get(student.AcademicID.Value);
            var previousAcademic = student.PreviousAcademicID.HasValue ? CacheManager.Academics.Get(student.PreviousAcademicID.Value) : null;

            if (academic != null)
                department += string.Format("{0} - {1}", academic.Institution, academic.Department);

            if (previousAcademic != null)
                department += string.Format("<br />({0} - {1})", previousAcademic.Institution, previousAcademic.Department);

            return department;
        }

        protected string GetStudentNumber(Student student)
        {
            if (student == null)
                return String.Empty;

            var studentNumber = student.StudentNumber;

            if (!string.IsNullOrEmpty(student.PreviousStudentNumber))
                studentNumber += string.Format("<br />({0})", student.PreviousStudentNumber);

            return studentNumber;
        }

        #endregion
    }
}