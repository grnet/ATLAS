using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using System.Drawing;

namespace StudentPractice.Portal.UserControls.InternshipPositionControls.ViewControls
{
    public partial class HandledStudentsGridView : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public object DataSource
        {
            get { return gvPositions.DataSource; }
            set { gvPositions.DataSource = value; }
        }

        public string DataSourceID
        {
            get { return gvPositions.DataSourceID; }
            set { gvPositions.DataSourceID = value; }
        }

        public int PageIndex
        {
            get { return gvPositions.PageIndex; }
            set { gvPositions.PageIndex = value; }
        }

        public override void DataBind()
        {
            gvPositions.DataBind();
        }

        protected void gvPositions_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != DevExpress.Web.ASPxGridView.GridViewRowType.Data)
                return;

            InternshipPosition position = (InternshipPosition)gvPositions.GetRow(e.VisibleIndex);

            switch (position.PositionStatus)
            {
                case enPositionStatus.Assigned:
                    e.Row.BackColor = Color.LightPink;
                    break;
                case enPositionStatus.UnderImplementation:
                    e.Row.BackColor = Color.Yellow;
                    break;
                case enPositionStatus.Completed:
                    e.Row.BackColor = Color.LightGreen;
                    break;
            }
        }

        protected string GetStudentDetails(InternshipPosition position)
        {
            if (position == null)
                return string.Empty;

            var student = position.AssignedToStudent;
            return string.Format("{0} {1}<br />Α.Μ.: {2}", student.GreekFirstName ?? student.OriginalFirstName, student.GreekLastName ?? student.OriginalLastName, student.StudentNumber);
        }

        protected string GetStudentAcademicDetails(InternshipPosition position)
        {
            if (position == null)
                return string.Empty;

            var academic = CacheManager.Academics.Get((int)position.AssignedToStudent.AcademicID);
            return string.Format("Ίδρυμα: {0}<br />Σχολή: {1}<br />Τμήμα: {2}", academic.Institution, academic.School ?? "-", academic.Department ?? "-");
        }

        protected string GetOfficeDetails(InternshipPosition position)
        {
            if (position == null)
                return string.Empty;

            var office = position.PreAssignedByMasterAccount;

            string result = string.Format("<strong>{0}</strong><br />Ίδρυμα: {1}", office.OfficeType.GetLabel(),
                office.InstitutionID.HasValue ? CacheManager.Institutions.Get(office.InstitutionID.Value).Name : string.Empty);
            if (office.OfficeType == enOfficeType.Departmental)
                result += string.Format("<br />Τμήμα: {0}", office.Academics.FirstOrDefault().Department);
            else if (office.OfficeType == enOfficeType.MultipleDepartmental)
                result += string.Format("<br />Τμήματα: <a runat='server' href='javascript:void(0)' onclick='popUp.show(\"ViewOfficeAcademics.aspx?oID={0}\",\"Προβολή Σχολών/Τμημάτων\", null, 800, 300)'><img src='/_img/iconInformation.png' width='16px' alt='Τμήματα' /></a>", office.ID);

            return result;
        }
    }
}