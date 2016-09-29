using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using DevExpress.Web.ASPxClasses;
using StudentPractice.Portal.Controls;
using System.Drawing;
using StudentPractice.Portal.UserControls;
using DevExpress.Web.ASPxGridView;
using StudentPractice.Mails;
using Imis.Domain;
using System.Threading;
using StudentPractice.Portal.UserControls.GenericControls;
using System.Text;
using System.Web.Security;

namespace StudentPractice.Portal.Secure.Helpdesk
{
    public partial class SearchStudents : BaseEntityPortalPage<object>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "jsInit", string.Format("hd.init('{0}','{1}','{2}','{3}');", hfSchoolCode.ClientID, txtInstitutionName.ClientID, txtSchoolName.ClientID, txtDepartmentName.ClientID), true);

            txtInstitutionName.Attributes.Add("readonly", "readonly");
            txtSchoolName.Attributes.Add("readonly", "readonly");
            txtDepartmentName.Attributes.Add("readonly", "readonly");

            txtInstitutionName.Attributes.Add("onclick", "popUp.show('../../Common/SchoolSelectPopup.aspx', 'Επιλογή Σχολής');");
            txtInstitutionName.Attributes.Add("onfocus", "popUp.show('../../Common/SchoolSelectPopup.aspx', 'Επιλογή Σχολής');");

            txtSchoolName.Attributes.Add("onclick", "popUp.show('../../Common/SchoolSelectPopup.aspx', 'Επιλογή Σχολής');");
            txtSchoolName.Attributes.Add("onfocus", "popUp.show('../../Common/SchoolSelectPopup.aspx', 'Επιλογή Σχολής');");

            txtDepartmentName.Attributes.Add("onclick", "popUp.show('../../Common/SchoolSelectPopup.aspx', 'Επιλογή Σχολής');");
            txtDepartmentName.Attributes.Add("onfocus", "popUp.show('../../Common/SchoolSelectPopup.aspx', 'Επιλογή Σχολής');");

            if (!Page.IsPostBack)
            {
                txtStudentID.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));
                txtContactEmail.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));
                txtContactMobilePhone.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));
                txtFirstName.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));
                txtLastName.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));
                txtStudentNumber.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));

                int studentID;
                if (int.TryParse(Request.QueryString["sID"], out studentID) && studentID > 0)
                {
                    txtStudentID.Text = studentID.ToString();
                }

                if (!string.IsNullOrEmpty(Request.QueryString["mobile"]))
                {
                    txtContactMobilePhone.Text = Request.QueryString["mobile"];
                }
            }

            var isHelpdesk = Roles.IsUserInRole(RoleNames.Helpdesk) || Roles.IsUserInRole(RoleNames.SuperHelpdesk);
            var isReports = Roles.IsUserInRole(RoleNames.Reports) || Roles.IsUserInRole(RoleNames.SuperReports);

            if (isReports && !isHelpdesk)
            {
                gvStudents.Columns[6].Visible = false;
                gvStudents.Columns[7].Visible = false;
            }
        }

        //protected void OnEmailSending(object sender, EmailFormSendingEventArgs e)
        //{
        //    if (!e.SendToLoggedInUser)
        //    {
        //        int _pageSize = gvStudents.SettingsPager.PageSize;

        //        gvStudents.SettingsPager.PageSize = int.MaxValue;
        //        gvStudents.DataBind();

        //        if (_students != null && _students.Count > 0)
        //        {
        //            int verificationStatus;
        //            if (int.TryParse(ddlVerificationStatus.SelectedItem.Value, out verificationStatus) && verificationStatus >= 0)
        //            {
        //                ThreadPool.QueueUserWorkItem((criteriaObj) =>
        //                {
        //                    Criteria<Student> criteria = criteriaObj as Criteria<Student>;

        //                    criteria.UsePaging = false;

        //                    using (IUnitOfWork uow = UnitOfWorkFactory.Create())
        //                    {
        //                        try
        //                        {
        //                            MassMessage massMessage = new MassMessage();

        //                            massMessage.ReporterType = enReporterType.Student;
        //                            massMessage.DispatchType = enDispatchType.Email;
        //                            massMessage.SentAt = DateTime.Now;
        //                            massMessage.MessageText = EmailForm.EmailBody;

        //                            uow.MarkAsNew(massMessage);
        //                            uow.Commit();

        //                            StudentRepository sRep = new StudentRepository(uow);

        //                            int studentCount;
        //                            var students = sRep.FindStudentsWithCriteria(criteria, out studentCount);

        //                            foreach (var student in students)
        //                            {
        //                                try
        //                                {
        //                                    student.MassMessages.Add(massMessage);
        //                                    Thread.CurrentPrincipal = null;
        //                                    uow.Commit();
        //                                }
        //                                catch (Exception ex1)
        //                                {
        //                                    LogHelper.LogError<SearchStudents>(ex1);

        //                                    continue;
        //                                }
        //                            }
        //                        }
        //                        catch (Exception ex2)
        //                        {
        //                            LogHelper.LogError<SearchStudents>(ex2);
        //                        }
        //                    }
        //                }, GetCriteria());
        //            }

        //            e.EmailRecepients.AddRange(_students.Select(x => x.Email).Distinct());
        //            e.EmailRecepients.AddRange(_students.Select(x => x.ContactEmail).Distinct());
        //            e.EmailRecepients = e.EmailRecepients.Distinct().ToList();
        //            e.InfoMessage = string.Format("Η μαζική αποστολή ξεκίνησε για {0} μοναδικά Email χρηστών και {1} μοναδικά Email επικοινωνίας φοιτητών από {2} συνολικά φοιτητές.",
        //                _students.Select(x => x.Email).Distinct().Count(),
        //                _students.Select(x => x.ContactEmail).Distinct().Except(_students.Select(x => x.Email).Distinct()).Count(),
        //                _students.Count);
        //        }
        //        else
        //        {
        //            e.Cancel = true;
        //            e.InfoMessage = string.Format("Η μαζική αποστολή δεν πραγματοποιήθηκε.");
        //        }
        //        gvStudents.SettingsPager.PageSize = _pageSize;

        //    }
        //    else
        //    {
        //        e.InfoMessage = string.Format("Η δοκιμαστική αποστολή ολοκληρώθηκε. Παρακαλούμε ελέξτε το Email σας.");
        //    }
        //}

        //List<Student> _students = null;
        //protected void odsStudents_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        //{
        //    if (e.ReturnValue is List<Student>)
        //        _students = e.ReturnValue as List<Student>;
        //}

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            gvStudents.DataBind();
        }

        public void btnSearch_Click(object sender, EventArgs e)
        {
            gvStudents.PageIndex = 0;
            gvStudents.DataBind();
        }

        protected void gvStudents_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != DevExpress.Web.ASPxGridView.GridViewRowType.Data)
                return;

            Student student = (Student)gvStudents.GetRow(e.VisibleIndex);

            e.Row.BackColor = student.RegistrationType == enRegistrationType.Shibboleth
                ? e.Row.BackColor = Color.LightGreen
                : e.Row.BackColor = Color.LightBlue;
        }

        protected void odsStudents_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<Student> criteria = new Criteria<Student>();
            criteria.Expression = criteria.Expression.Where(x => x.DeclarationType, enReporterDeclarationType.FromRegistration);

            if (!string.IsNullOrEmpty(txtContactEmail.Text))
            {
                criteria.Expression = criteria.Expression.Where(x => x.ContactEmail, txtContactEmail.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);
            }

            if (!string.IsNullOrEmpty(txtContactMobilePhone.Text))
            {
                criteria.Expression = criteria.Expression.Where(x => x.ContactMobilePhone, txtContactMobilePhone.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);
            }

            if (!string.IsNullOrEmpty(txtFirstName.Text))
            {
                var orExp = Imis.Domain.EF.Search.Criteria<Student>.Empty;
                orExp = orExp
                    .Where(x => x.OriginalFirstName, txtFirstName.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like)
                    .Or(x => x.GreekFirstName, txtFirstName.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like)
                    .Or(x => x.LatinFirstName, txtFirstName.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);

                criteria.Expression = criteria.Expression.And(orExp);
            }

            if (!string.IsNullOrEmpty(txtLastName.Text))
            {
                var orExp = Imis.Domain.EF.Search.Criteria<Student>.Empty;
                orExp = orExp
                    .Where(x => x.OriginalLastName, txtLastName.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like)
                    .Or(x => x.GreekLastName, txtLastName.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like)
                    .Or(x => x.LatinLastName, txtLastName.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);

                criteria.Expression = criteria.Expression.And(orExp);
            }

            if (!string.IsNullOrEmpty(txtStudentNumber.Text))
            {
                criteria.Expression = criteria.Expression.Where(x => x.StudentNumber, txtStudentNumber.Text.ToNull());
            }

            int studentID;
            if (int.TryParse(txtStudentID.Text, out studentID) && studentID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.ID, studentID);
            }

            int schoolCode;
            if (int.TryParse(hfSchoolCode.Value, out schoolCode))
            {
                criteria.Expression = criteria.Expression.Where(x => x.AcademicID, schoolCode);
            }

            e.InputParameters["criteria"] = criteria;
        }

        protected string GetStudentDetails(Student student)
        {
            if (student == null)
                return string.Empty;

            string studentDetails = string.Empty;

            studentDetails += string.Format("<strong>Ελληνικά:</strong> {0} {1}<br />", student.GreekFirstName, student.GreekLastName);
            studentDetails += string.Format("<strong>Λατινικά:</strong> {0} {1}<br />", student.LatinFirstName, student.LatinLastName);
            studentDetails += string.Format("<strong>Στοιχεία Ιδρύματος:</strong> {0} {1}", student.OriginalFirstName, student.OriginalLastName);

            return studentDetails;
        }

        protected string GetContactDetails(Student student)
        {
            if (student == null)
                return string.Empty;

            string contactDetails = string.Empty;

            StringBuilder sb = new StringBuilder();

            if (!student.IsEmailVerified)
            {
                contactDetails = string.Format("<span style='color:red'>{0}</span><br/>{1}", student.ContactEmail, student.ContactMobilePhone);
            }
            else
            {
                contactDetails = string.Format("{0}<br/>{1}", student.ContactEmail, student.ContactMobilePhone);
            }

            return contactDetails;
        }

        protected string GetAcademicDetails(Student student)
        {
            if (student == null)
                return string.Empty;

            string academicDetails = string.Empty;

            var academic = CacheManager.Academics.Get((int)student.AcademicID);

            academicDetails = string.Format("Ίδρυμα: {0}<br/>Σχολή: {1}<br/>Τμήμα: {2}<br/>Α.Μ.: {3}", academic.Institution, academic.School ?? "-", academic.Department ?? "-", student.StudentNumber);

            return academicDetails;
        }

        protected string GetSearchIncidentReportLink(Student student)
        {
            if (student == null)
                return string.Empty;

            string link = string.Empty;

            link = string.Format("~/Secure/Helpdesk/SearchIncidentReports.aspx?hideFilters=true&rID={0}", student.ID);

            return link;
        }

        protected bool HasPositions(Student student)
        {
            if (student == null || student.PositionCount == 0)
                return false;
            else
                return true;
        }
    }
}