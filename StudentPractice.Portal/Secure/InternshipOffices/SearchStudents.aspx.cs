using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using DevExpress.Web.ASPxGridView;
using Imis.Domain;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;
using StudentPractice.BusinessModel.Flow;
using System.Threading;
using System.Text;
using StudentPractice.Portal.DataSources;

namespace StudentPractice.Portal.Secure.InternshipOffices
{
    public partial class SearchStudents : BaseEntityPortalPage<InternshipOffice>
    {
        protected override void Fill()
        {
            Entity = new InternshipOfficeRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name, x => x.Academics);
            Entity.SaveToCurrentContext();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Entity.IsEmailVerified)
            {
                mvAccount.SetActiveView(vAccountNotVerified);
                lblVerificationError.Text = "Δεν μπορείτε να αναζητήσετε τους εγγεγραμμένους φοιτητές, γιατί δεν έχετε ενεργοποιήσει το e-mail σας.";
            }
            else if (Entity.VerificationStatus != enVerificationStatus.Verified)
            {
                mvAccount.SetActiveView(vAccountNotVerified);
                lblVerificationError.Text = "Δεν μπορείτε να αναζητήσετε τους εγγεγραμμένους φοιτητές, γιατί δεν έχει πιστοποιηθεί ο λογαριασμός σας.<br/>Παρακαλούμε εκτυπώστε τη Βεβαίωση Συμμετοχής και αποστείλτε τη με ΦΑΞ στο Γραφείο Αρωγής για να πιστοποιηθεί.";
            }
            else
            {
                mvAccount.SetActiveView(vAccountVerified);
            }

            if (!Page.IsPostBack)
            {
                txtStudentID.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
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
            }
        }

        protected void ddlDepartment_Init(object sender, EventArgs e)
        {
            ddlDepartment.Items.Add(new ListItem("-- αδιάφορο --", ""));

            List<Academic> academics;

            if (Entity.CanViewAllAcademics.GetValueOrDefault())
            {
                academics = CacheManager.Academics.GetItems().Where(x => x.InstitutionID == Entity.InstitutionID.Value).OrderBy(x => x.Department).ToList();
            }
            else
            {
                academics = Entity.Academics.OrderBy(x => x.Department).ToList();
            }

            foreach (var item in academics)
            {
                ddlDepartment.Items.Add(new ListItem(item.Department, item.ID.ToString()));
            }
        }

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            gvStudents.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gvStudents.PageIndex = 0;
            gvStudents.DataBind();
        }

        protected void gvStudents_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != DevExpress.Web.ASPxGridView.GridViewRowType.Data)
                return;

            Student student = (Student)gvStudents.GetRow(e.VisibleIndex);

            if (student.PreviousAcademicID.HasValue && Entity.Academics.Any(x => x.ID == student.PreviousAcademicID.Value))
                e.Row.BackColor = Color.Yellow;
        }

        protected void odsStudents_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<Student> criteria = new Criteria<Student>();
            criteria.Expression = criteria.Expression.Where(x => x.DeclarationType, enReporterDeclarationType.FromRegistration);

            int academicID;
            var orExpression = Imis.Domain.EF.Search.Criteria<Student>.Empty;
            if (int.TryParse(ddlDepartment.SelectedItem.Value, out academicID) && academicID > 0)
            {
                orExpression = orExpression.Where(x => x.AcademicID, academicID);
                orExpression = orExpression.Or(x => x.PreviousAcademicID, academicID);
            }
            else
            {
                orExpression = orExpression.Where(string.Format("it.AcademicID IN MULTISET ({0})", string.Join(",", Entity.Academics.Select(x => x.ID))));
                orExpression = orExpression.Or(string.Format("it.PreviousAcademicID IN MULTISET ({0})", string.Join(",", Entity.Academics.Select(x => x.ID))));
            }
            criteria.Expression = criteria.Expression.And(orExpression);

            if (!string.IsNullOrEmpty(txtFirstName.Text))
            {
                criteria.Expression = criteria.Expression.Where(x => x.OriginalFirstName, txtFirstName.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);
            }

            if (!string.IsNullOrEmpty(txtLastName.Text))
            {
                criteria.Expression = criteria.Expression.Where(x => x.OriginalLastName, txtLastName.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);
            }

            if (!string.IsNullOrEmpty(txtStudentNumber.Text))
            {
                var orExpression1 = Imis.Domain.EF.Search.Criteria<Student>.Empty;
                orExpression1 = orExpression1.Where(x => x.StudentNumber, txtStudentNumber.Text.ToNull());
                orExpression1 = orExpression1.Or(x => x.PreviousStudentNumber, txtStudentNumber.Text.ToNull());
                criteria.Expression = criteria.Expression.And(orExpression1);
            }

            int studentID;
            if (int.TryParse(txtStudentID.Text, out studentID) && studentID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.ID, studentID);
            }
            e.InputParameters["criteria"] = criteria;
        }

        protected string GetStudentDetails(Student student)
        {
            if (student == null)
                return string.Empty;

            string studentDetails = string.Empty;

            studentDetails = string.Format("{0} {1}", student.GreekFirstName ?? student.OriginalFirstName, student.GreekLastName ?? student.OriginalLastName);

            return studentDetails;
        }

        protected string GetAcademicDetails(Student student)
        {
            if (student == null)
                return string.Empty;

            string academicDetails = string.Empty;

            var academic = CacheManager.Academics.Get((int)student.AcademicID);

            academicDetails = string.Format("Ίδρυμα: {0}<br/>Σχολή: {1}<br/>Τμήμα: {2}<br/>Α.Μ.: {3}", academic.Institution, academic.School ?? "-", academic.Department ?? "-", student.StudentNumber);
            if (student.PreviousAcademicID.HasValue)
            {
                academic = CacheManager.Academics.Get((int)student.PreviousAcademicID);
                academicDetails += string.Format("<br/><br/>Προηγούμενο Ίδρυμα: {0}<br/>Προηγούμενη Σχολή: {1}<br/>Προηγούμενο Τμήμα: {2}<br/>Προηγούμενος Α.Μ.: {3}", academic.Institution, academic.School ?? "-", academic.Department ?? "-", student.PreviousStudentNumber);
            }

            return academicDetails;
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
