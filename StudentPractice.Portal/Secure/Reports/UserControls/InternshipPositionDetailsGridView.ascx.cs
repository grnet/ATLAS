using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.Portal.Controls;
using StudentPractice.BusinessModel;
using DevExpress.Web.ASPxGridView;
using System.Drawing;

namespace StudentPractice.Portal.Secure.Reports.UserControls
{
    public partial class InternshipPositionDetailsGridView : System.Web.UI.UserControl
    {
        #region [ Events ]

        public event EventHandler<DevExpress.Web.ASPxGridView.Export.ASPxGridViewExportRenderingEventArgs> RenderBrick;

        #endregion

        #region [ Properties ]

        public string DataSourceID
        {
            get
            {
                return gvPositions.DataSourceID;
            }
            set
            {
                gvPositions.DataSourceID = value;
                gvPositionsExport.DataSourceID = value;
            }
        }

        public bool EnableExport
        {
            get;
            set;
        }

        #endregion

        #region [ Overrides ]

        public override void DataBind()
        {
            gvPositions.DataBind();
        }

        #endregion

        #region [ Gridview Methods ]

        protected string GetPositionTitle(InternshipPosition ip)
        {
            if (ip == null)
                return string.Empty;
            else
                return ip.InternshipPositionGroup.Title;
        }

        protected string GetProviderDetails(InternshipPosition ip)
        {
            if (ip == null) return string.Empty;
            else
                return string.Format("ID ΦΥΠΑ: {0}<br />Επωνυμία: {1}<br />Υπεύθυνος: {2}, {3}, {4}, {5}",
                    ip.InternshipPositionGroup.ProviderID,
                    ip.InternshipPositionGroup.Provider.Name,
                    ip.InternshipPositionGroup.Provider.ContactName,
                    ip.InternshipPositionGroup.Provider.ContactPhone,
                    ip.InternshipPositionGroup.Provider.ContactMobilePhone,
                    ip.InternshipPositionGroup.Provider.Email
                );

        }

        protected string GetOfficeDetails(InternshipPosition ip)
        {
            if (ip == null || ip.PreAssignedByMasterAccount == null) return string.Empty;

            string officeDetails;
            Institution institution = ip.PreAssignedByMasterAccount.InstitutionID.HasValue ? CacheManager.Institutions.Get(ip.PreAssignedByMasterAccount.InstitutionID.Value) : null;
            if (institution == null) return string.Empty;

            switch (ip.PreAssignedByMasterAccount.OfficeType)
            {
                case enOfficeType.None:
                    officeDetails = string.Format("ID ΓΠΑ: {0} <br />Ίδρυμα: {1}<br />Τμήματα: <span style='color: Red'>-</span>", ip.PreAssignedByMasterAccountID, institution.Name);
                    break;
                case enOfficeType.Institutional:
                    if (ip.PreAssignedByMasterAccount.CanViewAllAcademics.GetValueOrDefault())
                        officeDetails = string.Format("ID ΓΠΑ: {0} <br />Ίδρυμα: {1}", ip.PreAssignedByMasterAccountID, institution.Name);
                    else
                        officeDetails = string.Format("ID ΓΠΑ: {0} <br />Ίδρυμα: {1}<br />Τμήματα: <a runat='server' href='javascript:void(0)' onclick='popUp.show(\"ViewOfficeAcademics.aspx?oID={2}\",\"Προβολή Σχολών/Τμημάτων\")'><img src='/_img/iconInformation.png' width='16px' alt='Τμήματα' /></a>", ip.PreAssignedByMasterAccountID, institution.Name, ip.PreAssignedByMasterAccount.ID);
                    break;
                case enOfficeType.Departmental:
                    var academic = ip.PreAssignedByMasterAccount.Academics.ToList()[0];
                    officeDetails = string.Format("ID ΓΠΑ: {0} <br />Ίδρυμα: {1}<br />Τμήμα: {2}", ip.PreAssignedByMasterAccountID, institution.Name, academic.Department);
                    break;
                case enOfficeType.MultipleDepartmental:
                    officeDetails = string.Format("ID ΓΠΑ: {0} <br />Ίδρυμα: {1}<br />Τμήματα: <a runat='server' href='javascript:void(0)' onclick='popUp.show(\"ViewOfficeAcademics.aspx?oID={2}\",\"Προβολή Σχολών/Τμημάτων\")'><img src='/_img/iconInformation.png' width='16px' alt='Τμήματα' /></a>", ip.PreAssignedByMasterAccountID, institution.Name, ip.PreAssignedByMasterAccount.ID);
                    break;
                default:
                    return string.Empty;
            }
            officeDetails += "<br />Υπεύθυνος: " + ip.PreAssignedByMasterAccount.ContactName + ", "
                                                    + ip.PreAssignedByMasterAccount.ContactPhone + ", "
                                                    + ip.PreAssignedByMasterAccount.ContactMobilePhone + ", "
                                                    + ip.PreAssignedByMasterAccount.ContactEmail;
            return officeDetails;
        }

        protected string GetOfficeDetailsExport(InternshipPosition ip)
        {
            if (ip == null || ip.PreAssignedByMasterAccount == null) return string.Empty;

            if (ip == null || ip.PreAssignedByMasterAccount == null) return string.Empty;

            string officeDetails;
            Institution institution = ip.PreAssignedByMasterAccount.InstitutionID.HasValue ? CacheManager.Institutions.Get(ip.PreAssignedByMasterAccount.InstitutionID.Value) : null;
            if (institution == null) return string.Empty;

            switch (ip.PreAssignedByMasterAccount.OfficeType)
            {
                case enOfficeType.None:
                    officeDetails = string.Format("Id ΓΠΑ: {0} <br />Ίδρυμα: {1}<br />Τμήματα: <span style='color: Red'>-</span>", ip.ID, institution.Name);
                    break;
                case enOfficeType.Institutional:
                    officeDetails = string.Format("Id ΓΠΑ: {0} <br />Ίδρυμα: {1}", ip.ID, institution.Name);
                    break;
                case enOfficeType.Departmental:
                    var academic = ip.PreAssignedByMasterAccount.Academics.ToList()[0];
                    officeDetails = string.Format("Id ΓΠΑ: {0} <br />Ίδρυμα: {1}<br />Τμήμα: {2}", ip.ID, institution.Name, academic.Department);
                    break;
                case enOfficeType.MultipleDepartmental:
                    officeDetails = string.Format("Id ΓΠΑ: {0} <br />Ίδρυμα: {1}<br />Τμήματα: {2}", ip.ID, institution.Name,
                        string.Join(";", ip.PreAssignedByMasterAccount.Academics.Select(x => x.Department).ToArray()));
                    break;
                default:
                    return string.Empty;
            }
            officeDetails += "<br />Υπεύθυνος: " + ip.PreAssignedByMasterAccount.ContactName + ", "
                                                    + ip.PreAssignedByMasterAccount.ContactPhone + ", "
                                                    + ip.PreAssignedByMasterAccount.ContactMobilePhone + ", "
                                                    + ip.PreAssignedByMasterAccount.ContactEmail;
            return officeDetails;
        }

        protected string GetAssignedStudentDetails(InternshipPosition ip)
        {
            if (ip == null || ip.AssignedToStudentID == null)
                return string.Empty;

            string studentDetails = "Id Φοιτητή: " + ip.AssignedToStudentID + "<br />Στοιχεία: " + ip.AssignedToStudent.ContactName;
            studentDetails += string.IsNullOrEmpty(ip.AssignedToStudent.ContactPhone) ? string.Empty : ", " + ip.AssignedToStudent.ContactPhone;
            studentDetails += string.IsNullOrEmpty(ip.AssignedToStudent.ContactMobilePhone) ? string.Empty : ", " + ip.AssignedToStudent.ContactMobilePhone;
            studentDetails += string.IsNullOrEmpty(ip.AssignedToStudent.ContactEmail) ? string.Empty : ", " + ip.AssignedToStudent.ContactEmail;
            return studentDetails;
        }

        protected string GetAssignedStudentAcademicDetails(InternshipPosition ip)
        {
            if (ip == null || ip.AssignedToStudentID == null)
                return string.Empty;

            string studentDetails = string.IsNullOrEmpty(ip.AssignedToStudent.Academic.Institution) ? string.Empty : ip.AssignedToStudent.Academic.Institution;
            studentDetails += string.IsNullOrEmpty(ip.AssignedToStudent.Academic.Department) ? string.Empty : ", " + ip.AssignedToStudent.Academic.Department;
            studentDetails += string.IsNullOrEmpty(ip.AssignedToStudent.StudentNumber) ? string.Empty : ", " + ip.AssignedToStudent.StudentNumber;
            return studentDetails;
        }

        protected string GetPositionDuration(InternshipPosition ip)
        {
            if (ip == null || ip.AssignedToStudentID == null)
                return string.Empty;

            string positionDuration;
            positionDuration = ip.ImplementationStartDate.HasValue ? "Ημερομηνία έναρξης: " + ip.ImplementationStartDate.Value.ToShortDateString() + "<br />" : "Ημερομηνία έναρξης: --<br />";
            positionDuration += ip.ImplementationEndDate.HasValue ? "Ημερομηνία λήξης: " + ip.ImplementationEndDate.Value.ToShortDateString() + "<br />" : "Ημερομηνία λήξης: --<br />";
            positionDuration += ip.CompletedAt.HasValue ? "Ημερομηνία ολοκλήρωσης: " + ip.CompletedAt.Value.ToShortDateString() : "Ημερομηνία ολοκλήρωσης: --";
            return positionDuration;
        }

        protected string GetPreAssignedForAcademic(InternshipPosition ip)
        {
            if (ip == null || !ip.PreAssignedForAcademicID.HasValue)
                return string.Empty;
            return
                CacheManager.Academics.Get(ip.PreAssignedForAcademicID.Value).Department;
        }

        protected string GetPositionStatus(InternshipPosition ip)
        {
            if (ip == null)
                return string.Empty;

            if(ip.PositionStatus == enPositionStatus.Canceled && (ip.CancellationReason == enCancellationReason.FromProvider || ip.CancellationReason == enCancellationReason.CanceledGroupCascade))
                return "Αποσυρμένη";
            else if (ip.InternshipPositionGroup.PositionGroupStatus == enPositionGroupStatus.Deleted)
                return "Διεγραμμένη";
            else
                return ip.PositionStatus.GetLabel();
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (EnableExport)
            {
                btnExport.Visible = true;
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            gvPositionsExport.Visible = true;
            gvPositionsExport.PageIndex = 0;
            gvPositionsExport.DataBind();

            gvePositions.FileName = String.Format("InternshipPositions_{0}", DateTime.Now.ToString("yyyyMMdd"));
            gvePositions.WriteXlsxToResponse(true);

            gvPositionsExport.Visible = false;
        }

        protected void gvPositions_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data)
                return;

            InternshipPosition position = (InternshipPosition)gvPositions.GetRow(e.VisibleIndex);

            if (position != null)
            {
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
                        if (position.InternshipPositionGroup.PositionCreationType == enPositionCreationType.FromOffice)
                            e.Row.BackColor = Color.LightBlue;
                        break;
                    case enPositionStatus.Canceled:
                        e.Row.BackColor = Color.Tomato;
                        if (position.InternshipPositionGroup.PositionCreationType == enPositionCreationType.FromOffice)
                            e.Row.BackColor = Color.Purple;
                        break;
                }
                if (position.InternshipPositionGroup.PositionGroupStatus == enPositionGroupStatus.Deleted)
                {
                    e.Row.BackColor = Color.Tomato;
                    if (position.InternshipPositionGroup.PositionCreationType == enPositionCreationType.FromOffice)
                        e.Row.BackColor = Color.Purple;
                }
            }
            
        }

        protected void gvePositions_RenderBrick(object sender, DevExpress.Web.ASPxGridView.Export.ASPxGridViewExportRenderingEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                var position = gvPositionsExport.GetRow(e.VisibleIndex) as InternshipPosition;

                if (position != null)
                {
                    switch (e.Column.Name)
                    {
                        case "PhysicalObjects":
                            e.TextValue = e.Text = position.GetPhysicalObjectDetails().Replace("<br />", ";");
                            break;
                        case "PositionTitle":
                            e.TextValue = e.Text = position.InternshipPositionGroup.Title;
                            break;
                        case "ProviderDetails":
                            e.TextValue = e.Text = GetProviderDetails(position).Replace("<br />", "\n");
                            break;
                        case "OfficeDetails":
                            e.TextValue = e.Text = GetOfficeDetailsExport(position).Replace("<br />", "\n");
                            break;
                        case "PreAssignedAcademinDetails":
                            e.TextValue = e.Text = GetPreAssignedForAcademic(position).Replace("<br />", "\n");
                            break;
                        case "AssignedStudentDetails":
                            e.TextValue = e.Text = GetAssignedStudentDetails(position).Replace("<br />", "\n");
                            break;
                        case "AssignedStudentAcademicDetails":
                            e.TextValue = e.Text = GetAssignedStudentAcademicDetails(position).Replace("<br />", "\n");
                            break;
                        case "PositionDuration":
                            e.TextValue = e.Text = GetPositionDuration(position).Replace("<br />", "\n");
                            break;
                        case "PositionStatus":
                            e.TextValue = e.Text = GetPositionStatus(position);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}