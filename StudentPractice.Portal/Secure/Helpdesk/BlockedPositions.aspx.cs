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
using StudentPractice.Utils;

namespace StudentPractice.Portal.Secure.Helpdesk
{
    public partial class BlockedPositions : BaseEntityPortalPage<object>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ddlInstitution_Init(object sender, EventArgs e)
        {
            ddlInstitution.Items.Add(new ListItem("-- αδιάφορο --", ""));

            foreach (var item in CacheManager.Institutions.GetItems())
            {
                ddlInstitution.Items.Add(new ListItem(item.Name, item.ID.ToString()));
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gvBlockedPositionGroups.PageIndex = 0;
            gvBlockedPositionGroups.DataBind();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            gvBlockedPositionsExport.Visible = true;
            gveBlockedPositionsExporter.FileName = String.Format("BlockedPositions_{0}", DateTime.Now.ToString("yyyyMMdd"));
            gveBlockedPositionsExporter.WriteXlsxToResponse(true);
        }

        protected void odsBlockedPositionGroups_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<BlockedPositionGroup> criteria = new Criteria<BlockedPositionGroup>();

            criteria.Include(x => x.MasterAccount.Academics)
                .Include(x => x.InternshipPositionGroup);

            int officeID;
            if (int.TryParse(txtOfficeID.Text.ToNull(), out officeID) && officeID > 0)
                criteria.Expression = criteria.Expression.Where(x => x.MasterAccountID, officeID);

            int providerID;
            if (int.TryParse(txtProviderID.Text.ToNull(), out providerID) && providerID > 0)
                criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.ProviderID, providerID);

            int groupID;
            if (int.TryParse(txtGroupID.Text.ToNull(), out groupID) && groupID > 0)
                criteria.Expression = criteria.Expression.Where(x => x.GroupID, groupID);

            int institutionID;
            if (int.TryParse(ddlInstitution.SelectedItem.Value, out institutionID) && institutionID >= 0)
                criteria.Expression = criteria.Expression.Where(x => x.MasterAccount.InstitutionID, institutionID);

            e.InputParameters["criteria"] = criteria;
        }

        protected void gvBlockedPositionGroups_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();
            var groupID = int.Parse(parameters[1]);

            if (action == "deleteblockedpositiongroup")
            {
                using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
                {
                    BlockedPositionGroup blockedPositionGroup = new BlockedPositionGroupRepository(unitOfWork).Load(groupID, x => x.CascadedBlocks);
                    try
                    {
                        var cascadedBlocks = blockedPositionGroup.CascadedBlocks.ToList();

                        for (int i = cascadedBlocks.Count - 1; i >= 0; i--)
                        {
                            unitOfWork.MarkAsDeleted(cascadedBlocks[i]);
                        }

                        unitOfWork.MarkAsDeleted(blockedPositionGroup);
                        unitOfWork.Commit();
                    }
                    catch (Exception ex)
                    {
                        LogHelper.LogError<BlockedPositions>(ex, this, string.Format("Unable to delete BlockedPositionGroup with ID: {0}", groupID));
                    }
                }

                gvBlockedPositionGroups.DataBind();
            }
        }

        protected void gveBlockedPositionsExporter_RenderBrick(object sender, DevExpress.Web.ASPxGridView.Export.ASPxGridViewExportRenderingEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                var blockedPosition = gvBlockedPositionsExport.GetRow(e.VisibleIndex) as BlockedPositionGroup;

                if (blockedPosition != null)
                {
                    switch (e.Column.Name)
                    {
                        case "Institution":
                            e.TextValue = e.Text = CacheManager.Institutions.Get(blockedPosition.MasterAccount.InstitutionID.GetValueOrDefault()).Name;
                            break;
                        case "BlockingReasonInt":
                            e.TextValue = e.Text = blockedPosition.BlockingReason.GetLabel();
                            break;
                        case "ProviderID":
                            e.TextValue = e.Text = blockedPosition.InternshipPositionGroup.ProviderID.ToString();
                            break;
                    }
                }
            }
        }

        protected string GetBlockedOfficeDetails(BlockedPositionGroup group)
        {
            if (group == null)
                return string.Empty;

            string blockedOfficeDetails = string.Empty;

            var blockedOffice = group.MasterAccount;
            var institution = CacheManager.Institutions.Get(blockedOffice.InstitutionID.Value);

            switch (blockedOffice.OfficeType)
            {
                case enOfficeType.None:
                    blockedOfficeDetails = string.Format("Ίδρυμα: {0}<br/>Τμήματα: <span style='color: Red'>-</span>", institution.Name);
                    break;
                case enOfficeType.Institutional:
                    if (blockedOffice.CanViewAllAcademics.GetValueOrDefault())
                        blockedOfficeDetails = string.Format("Ίδρυμα: {0}", institution.Name);
                    else
                        blockedOfficeDetails = string.Format("Ίδρυμα: {0}<br/>Τμήματα: <a runat='server' href='javascript:void(0)' onclick='popUp.show(\"ViewOfficeAcademics.aspx?oID={1}\",\"Προβολή Σχολών/Τμημάτων\")'><img src='/_img/iconInformation.png' width='16px' alt='Τμήματα' /></a>", institution.Name, blockedOffice.ID);
                    break;
                case enOfficeType.Departmental:
                    var academic = blockedOffice.Academics.ToList()[0];

                    blockedOfficeDetails = string.Format("Ίδρυμα: {0}<br/>Τμήμα: {1}", institution.Name, academic.Department);
                    break;
                case enOfficeType.MultipleDepartmental:
                    blockedOfficeDetails = string.Format("Ίδρυμα: {0}<br/>Τμήματα: <a runat='server' href='javascript:void(0)' onclick='popUp.show(\"ViewOfficeAcademics.aspx?oID={1}\",\"Προβολή Σχολών/Τμημάτων\")'><img src='/_img/iconInformation.png' width='16px' alt='Τμήματα' /></a>", institution.Name, blockedOffice.ID);
                    break;
                default:
                    break;
            }

            return blockedOfficeDetails;
        }

        protected bool CanDeleteBlock(BlockedPositionGroup group)
        {
            if (group == null)
                return false;

            return !group.MasterBlockID.HasValue;
        }

    }
}
