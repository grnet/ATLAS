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
using System.Web.Script.Serialization;

namespace StudentPractice.Portal.Secure.InternshipOffices
{
    public partial class SelectedPositions : BaseEntityPortalPage<InternshipOffice>
    {
        #region [ Databind Methods ]

        List<InternshipProvider> _providers;
        List<Academic> _academics;
        List<Country> _countries;

        protected override void Fill()
        {
            Entity = new InternshipOfficeRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name, x => x.Academics);
            Entity.SaveToCurrentContext();

            Criteria<InternshipPosition> criteria = new Criteria<InternshipPosition>();
            criteria.UsePaging = false;
            criteria.Include(x => x.InternshipPositionGroup)
                    .Include(x => x.InternshipPositionGroup.Provider)
                    .Include(x => x.PreAssignedForAcademic);

            //criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.PositionCreationType, enPositionCreationType.FromOffice, Imis.Domain.EF.Search.enCriteriaOperator.NotEquals);
            if (Entity.IsMasterAccount)
                criteria.Expression = criteria.Expression.Where(x => x.PreAssignedByMasterAccountID, Entity.ID);
            else
                criteria.Expression = criteria.Expression.Where(x => x.PreAssignedByMasterAccountID, Entity.MasterAccountID);

            int positionCount;
            var positions = new InternshipPositionRepository(UnitOfWork).FindWithCriteria(criteria, out positionCount);
            _providers = positions
                .Select(x => x.InternshipPositionGroup.Provider)
                .Distinct()
                .OrderBy(x => x.TradeName)
                .ThenBy(x => x.Name)
                .ToList();

            _academics = positions
                .Where(x => x.PreAssignedForAcademicID.HasValue)
                .Select(x => x.PreAssignedForAcademic)
                .Distinct()
                .OrderBy(x => x.Department)
                .ToList();

            _countries = positions
                .Where(x => x.InternshipPositionGroup.CountryID != 0)
                .Select(x => CacheManager.Countries.Get(x.InternshipPositionGroup.CountryID))
                .Distinct()
                .OrderBy(x => x.NameInGreek)
                .ToList();
        }

        #endregion

        #region [ Control Inits ]

        protected void ddlPhysicalObject_Init(object sender, EventArgs e)
        {
            ddlPhysicalObject.Items.Add(new ListItem("-- αδιάφορο --", ""));

            foreach (var item in CacheManager.PhysicalObjects.GetItems().OrderBy(x => x.NameInGreek))
            {
                ddlPhysicalObject.Items.Add(new ListItem(item.Name, item.ID.ToString()));
            }
        }

        protected void ddlPositionStatus_Init(object sender, EventArgs e)
        {
            ddlPositionStatus.Items.Add(new ListItem("-- αδιάφορο --", ""));
            ddlPositionStatus.Items.Add(new ListItem(enPositionStatus.PreAssigned.GetLabel(), ((int)enPositionStatus.PreAssigned).ToString()));
            ddlPositionStatus.Items.Add(new ListItem(enPositionStatus.Assigned.GetLabel(), ((int)enPositionStatus.Assigned).ToString()));
            ddlPositionStatus.Items.Add(new ListItem(enPositionStatus.UnderImplementation.GetLabel(), ((int)enPositionStatus.UnderImplementation).ToString()));
            ddlPositionStatus.Items.Add(new ListItem(enPositionStatus.Completed.GetLabel(), ((int)enPositionStatus.Completed).ToString()));
            ddlPositionStatus.Items.Add(new ListItem(enPositionStatus.Canceled.GetLabel(), ((int)enPositionStatus.Canceled).ToString()));
            ddlPositionStatus.Items.Add(new ListItem("Ημιτελής Θέση ΓΠΑ", ((int)enPositionStatus.UnPublished).ToString()));
        }

        protected void ddlDepartment_Init(object sender, EventArgs e)
        {
            ddlDepartment.Items.Add(new ListItem("-- αδιάφορο --", ""));

            foreach (var item in _academics)
            {
                ddlDepartment.Items.Add(new ListItem(item.Department, item.ID.ToString()));
            }
        }

        protected void ddlCountry_Init(object sender, EventArgs e)
        {
            ddlCountry.Items.Add(new ListItem("-- αδιάφορο --", ""));
            foreach (var item in _countries)
            {
                if (item.ID == StudentPracticeConstants.GreeceCountryID || item.ID == StudentPracticeConstants.CyprusCountryID)
                    ddlCountry.Items.Insert(1, new ListItem(item.Name, item.ID.ToString()));
                else
                    ddlCountry.Items.Add(new ListItem(item.Name, item.ID.ToString()));
            }
        }

        protected void ddlCreationType_Init(object sender, EventArgs e)
        {
            ddlCreationType.Items.Add(new ListItem("-- αδιάφορο --", ""));
            ddlCreationType.Items.Add(new ListItem("Απο Φορέα Υποδοχής", enPositionCreationType.FromProvider.GetValue().ToString()));
            ddlCreationType.Items.Add(new ListItem("Απο Γραφείο Πρακτικής", enPositionCreationType.FromOffice.GetValue().ToString()));
        }

        //ToFix: FundingType
        //protected void ddlFundingType_Init(object sender, EventArgs e)
        //{
        //    ddlFundingType.Items.Add(new ListItem("-- αδιάφορο --", ""));
        //    foreach (enFundingType item in Enum.GetValues(typeof(enFundingType)))
        //    {
        //        ddlFundingType.Items.Add(new ListItem(item.GetLabel(), item.ToString("D")));
        //    }
        //}

        #endregion

        #region [ Page Methods ]

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "providers", string.Format("var _providers = {0};", new JavaScriptSerializer().Serialize(_providers.Select(x => x.Name).ToList())), true);


            Page.ClientScript.RegisterForEventValidation(ddlCity.UniqueID, string.Empty);
            Page.ClientScript.RegisterForEventValidation(ddlPrefecture.UniqueID, string.Empty);

            IList<City> cities = CacheManager.Cities.GetItems();
            foreach (City city in cities)
            {
                Page.ClientScript.RegisterForEventValidation(ddlCity.UniqueID, city.ID.ToString());
            }
            IList<Prefecture> prefectures = CacheManager.Prefectures.GetItems();
            foreach (Prefecture prefecture in prefectures)
            {
                Page.ClientScript.RegisterForEventValidation(ddlPrefecture.UniqueID, prefecture.ID.ToString());
            }

            base.Render(writer);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Entity.CanViewAllAcademics.Value && Entity.Academics.Count == 0)
            {
                Response.Redirect("OfficeDetails.aspx");
            }

            if (!Entity.IsEmailVerified)
            {
                mvAccount.SetActiveView(vAccountNotVerified);
                lblVerificationError.Text = "Δεν μπορείτε να αναζητήσετε τις θέσεις πρακτικής άσκησης, γιατί δεν έχετε ενεργοποιήσει το e-mail σας.";
            }
            else if (Entity.VerificationStatus != enVerificationStatus.Verified)
            {
                mvAccount.SetActiveView(vAccountNotVerified);
                lblVerificationError.Text = "Δεν μπορείτε να αναζητήσετε τις θέσεις πρακτικής άσκησης, γιατί δεν έχει πιστοποιηθεί ο λογαριασμός σας.<br/>Παρακαλούμε εκτυπώστε τη Βεβαίωση Συμμετοχής και αποστείλτε τη με ΦΑΞ στο Γραφείο Αρωγής για να πιστοποιηθεί.";
            }
            else
            {
                if (!Config.PreAssignmentAllowed)
                {
                    mvAccount.SetActiveView(vAccountNotVerified);
                    lblVerificationError.Text = Config.PreAssignmentMessage;
                }
                else
                {
                    mvAccount.SetActiveView(vAccountVerified);
                }
            }

            if (!Page.IsPostBack)
            {
                txtGroupID.Attributes["onkeydown"] = String.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, String.Empty));
                txtPositionID.Attributes["onkeydown"] = String.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, String.Empty));
                txtTitle.Attributes["onkeydown"] = String.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, String.Empty));
                txtFirstName.Attributes["onkeydown"] = String.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, String.Empty));
                txtLastName.Attributes["onkeydown"] = String.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, String.Empty));
                txtStudentNumber.Attributes["onkeydown"] = String.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, String.Empty));
            }

            gvPositions.DataBind();
        }

        #endregion

        #region [ Button Methods ]

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            gvPositions.DataBind();
        }

        public void btnSearch_Click(object sender, EventArgs e)
        {
            gvPositions.PageIndex = 0;
            gvPositions.DataBind();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            var criteria = new Criteria<InternshipPosition>();
            criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.PositionGroupStatus, enPositionGroupStatus.Deleted, Imis.Domain.EF.Search.enCriteriaOperator.NotEquals);
            ParseFilters(criteria);

            var positions = new Positions().FindInternshipPositionReport(criteria, 0, int.MaxValue, "InternshipPositionGroup.ID");

            gvPositionsExport.DataSource = positions;
            gveIntershipPositions.FileName = string.Format("IntershipPositions_GPA_{0}", DateTime.Now.ToString("yyyyMMdd"));

            gveIntershipPositions.WriteXlsxToResponse();
        }

        #endregion

        #region [ Grid Methods ]

        protected void ParseFilters(Criteria<InternshipPosition> criteria)
        {
            criteria.Sort.OrderByDescending(x => x.UpdatedAt);
            criteria.Expression = criteria.Expression.Where(x => x.CancellationReason, enCancellationReason.CanceledGroupCascade, Imis.Domain.EF.Search.enCriteriaOperator.NotEquals);
            criteria.Expression = criteria.Expression.Where(x => x.CancellationReason, enCancellationReason.FromHelpdesk, Imis.Domain.EF.Search.enCriteriaOperator.NotEquals);

            var orCreationExpression = Imis.Domain.EF.Search.Criteria<InternshipPosition>.Empty;
            var andCreationExpression = Imis.Domain.EF.Search.Criteria<InternshipPosition>.Empty;
            orCreationExpression = orCreationExpression.Where(x => x.InternshipPositionGroup.PositionCreationType, enPositionCreationType.FromProvider);
            andCreationExpression = andCreationExpression.Where(x => x.InternshipPositionGroup.PositionCreationType, enPositionCreationType.FromOffice)
                                                         .And(x => x.PositionStatus, enPositionStatus.Canceled, Imis.Domain.EF.Search.enCriteriaOperator.NotEquals);
            orCreationExpression = orCreationExpression.Or(andCreationExpression);
            criteria.Expression = criteria.Expression.And(orCreationExpression);



            if (Entity.IsMasterAccount)
            {
                criteria.Expression = criteria.Expression.Where(x => x.PreAssignedByMasterAccountID, Entity.ID);
            }
            else
            {
                Criteria<InternshipPosition> andCrit = new Criteria<InternshipPosition>();
                andCrit.Expression = andCrit.Expression.Where(x => x.PreAssignedByMasterAccountID, Entity.MasterAccountID);
                andCrit.Expression = andCrit.Expression.Where(string.Format("it.PreAssignedForAcademicID IN MULTISET ({0})", string.Join(",", Entity.Academics.Select(x => x.ID))));
                Criteria<InternshipPosition> orCrit = new Criteria<InternshipPosition>();
                orCrit.Expression = andCrit.Expression.Or(x => x.PreAssignedByOfficeID, Entity.ID);
                criteria.Expression = criteria.Expression.And(andCrit.Expression);
            }


            int academicID;
            if (int.TryParse(ddlDepartment.SelectedItem.Value, out academicID) && academicID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.PreAssignedForAcademicID, academicID);
            }

            int groupID;
            if (int.TryParse(txtGroupID.Text.ToNull(), out groupID) && groupID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.ID, groupID);
            }

            int positionID;
            if (int.TryParse(txtPositionID.Text.ToNull(), out positionID) && positionID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.ID, positionID);
            }

            if (!string.IsNullOrEmpty(txtProviderAFM.Text.ToNull()))
            {
                criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.Provider.AFM, txtProviderAFM.Text);
            }

            if (!string.IsNullOrEmpty(txtTitle.Text))
            {
                var orTitleExpression = Imis.Domain.EF.Search.Criteria<InternshipPosition>.Empty;

                orTitleExpression = orTitleExpression.Where(x => x.InternshipPositionGroup.Title, txtTitle.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);
                orTitleExpression = orTitleExpression.Or(x => x.InternshipPositionGroup.Description, txtTitle.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);

                criteria.Expression = criteria.Expression.And(orTitleExpression);
            }

            int physicalObjectID;
            if (int.TryParse(ddlPhysicalObject.SelectedItem.Value, out physicalObjectID) && physicalObjectID > 0)
            {
                criteria.Expression = criteria.Expression.Where(string.Format("(it.InternshipPositionGroup.PhysicalObjects) OVERLAPS (SELECT VALUE it1 FROM PhysicalObjectSet as it1 WHERE it1.ID = {0} )", physicalObjectID));
            }

            int countryID;
            if (int.TryParse(ddlCountry.SelectedItem.Value, out countryID) && countryID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.CountryID, countryID);
            }

            int prefectureID;
            if (int.TryParse(ddlPrefecture.SelectedItem.Value, out prefectureID) && prefectureID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.PrefectureID, prefectureID);
            }

            int cityID;
            if (int.TryParse(ddlCity.SelectedItem.Value, out cityID) && cityID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.CityID, cityID);
            }

            if (!string.IsNullOrEmpty(txtCity.Text.ToNull()))
            {
                criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.CityText, txtCity.Text);
            }

            if (!string.IsNullOrEmpty(txtProvider.Text.ToNull()))
            {
                criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.Provider.Name, txtProvider.Text, Imis.Domain.EF.Search.enCriteriaOperator.Like);
            }

            int positionStatus;
            if (int.TryParse(ddlPositionStatus.SelectedItem.Value, out positionStatus) && positionStatus >= 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.PositionStatusInt, positionStatus);
            }

            int creationType;
            if (int.TryParse(ddlCreationType.SelectedItem.Value, out creationType) && creationType >= 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.PositionCreationTypeInt, creationType);
            }

            int firstPublishedAt;
            if (int.TryParse(ddlFirstPublishedAt.SelectedItem.Value, out firstPublishedAt) && firstPublishedAt > 0)
            {
                switch (firstPublishedAt)
                {
                    case 1:
                        criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.FirstPublishedAt, DateTime.Now.Date.AddDays(-1), Imis.Domain.EF.Search.enCriteriaOperator.GreaterThanEquals);
                        break;
                    case 2:
                        criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.FirstPublishedAt, DateTime.Now.Date.AddDays(-7), Imis.Domain.EF.Search.enCriteriaOperator.GreaterThanEquals);
                        break;
                    case 3:
                        criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.FirstPublishedAt, DateTime.Now.Date.AddDays(-31), Imis.Domain.EF.Search.enCriteriaOperator.GreaterThanEquals);
                        break;
                }
            }

            if (!string.IsNullOrEmpty(txtFirstName.Text))
            {
                criteria.Expression = criteria.Expression.Where(x => x.AssignedToStudent.GreekFirstName, txtFirstName.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);
            }

            if (!string.IsNullOrEmpty(txtLastName.Text))
            {
                criteria.Expression = criteria.Expression.Where(x => x.AssignedToStudent.GreekLastName, txtLastName.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);
            }

            if (!string.IsNullOrEmpty(txtStudentNumber.Text))
            {
                criteria.Expression = criteria.Expression.Where(x => x.AssignedToStudent.StudentNumber, txtStudentNumber.Text.ToNull());
            }

            //ToFix: FundingType
            //int fundingType;
            //if (int.TryParse(ddlFundingType.SelectedItem.Value, out fundingType) && fundingType >= 0)
            //{
            //    criteria.Expression = criteria.Expression.Where(x => x.FundingTypeInt, fundingType);
            //}
        }

        protected void odsPositions_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<InternshipPosition> criteria = new Criteria<InternshipPosition>();

            criteria.Include(x => x.InternshipPositionGroup)
                .Include(x => x.InternshipPositionGroup.Provider)
                .Include(x => x.InternshipPositionGroup.PhysicalObjects)
                .Include(x => x.InternshipPositionGroup.Academics)
                .Include(x => x.PreAssignedForAcademic)
                .Include(x => x.PreAssignedByOffice)
                .Include(x => x.PreAssignedByMasterAccount)
                .Include(x => x.AssignedToStudent)
                .Include(x => x.CanceledStudent);

            ParseFilters(criteria);

            e.InputParameters["criteria"] = criteria;
        }

        protected void gvPositions_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            InternshipPosition position = gvPositions.GetRow(e.VisibleIndex) as InternshipPosition;

            if (position != null)
            {
                switch (position.PositionStatus)
                {
                    case enPositionStatus.UnPublished:
                        e.Row.BackColor = Color.LightGray;
                        break;
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
                        break;
                }
            }
        }

        protected void gvPositions_CustomCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();
            var positionID = int.Parse(parameters[1]);

            var position = new InternshipPositionRepository(UnitOfWork).Load(positionID,
                x => x.InternshipPositionGroup,
                x => x.PreAssignedByMasterAccount,
                x => x.PreAssignedByMasterAccount.Academics,
                x => x.PreAssignedForAcademic,
                x => x.CanceledStudent,
                x => x.AssignedToStudent);

            if (action == "rollbackpreassignment" || action == "rollbackpreassignment2")
            {
                InternshipPositionTriggersParams triggersParams = new InternshipPositionTriggersParams();

                triggersParams.OfficeID = Entity.ID;

                if (Entity.MasterAccountID.HasValue)
                {
                    triggersParams.MasterAccountID = Entity.MasterAccountID.Value;
                }
                else
                {
                    triggersParams.MasterAccountID = Entity.ID;
                }

                triggersParams.Username = Thread.CurrentPrincipal.Identity.Name;
                triggersParams.ExecutionDate = DateTime.Now;
                triggersParams.UnitOfWork = UnitOfWork;

                var stateMachine = new InternshipPositionStateMachine(position);

                if (position.InternshipPositionGroup.PositionGroupStatus == enPositionGroupStatus.Revoked)
                {
                    triggersParams.CancellationReason = enCancellationReason.CanceledGroupCascade;

                    stateMachine.Cancel(triggersParams);
                    UnitOfWork.Commit();
                }
                else if (position.InternshipPositionGroup.PositionGroupStatus == enPositionGroupStatus.UnPublished)
                {
                    stateMachine.UnPublish(triggersParams);
                    UnitOfWork.Commit();
                }
                else
                {
                    if (position.DaysLeftForAssignment < StudentPracticeConstants.Default_MaxDaysForAssignment)
                    {
                        triggersParams.MasterAccountID = position.PreAssignedByMasterAccountID.Value;
                        triggersParams.BlockingReason = enBlockingReason.RolledbackPreAssignment;
                    }
                    else
                    {
                        triggersParams.BlockingReason = enBlockingReason.None;
                    }

                    stateMachine.RollbackPreAssignment(triggersParams);
                    UnitOfWork.Commit();
                }
            }
            else if (action == "deleteassignment" || action == "deleteassignment2")
            {
                InternshipPositionTriggersParams triggersParams = new InternshipPositionTriggersParams();

                triggersParams.OfficeID = Entity.ID;

                if (Entity.MasterAccountID.HasValue)
                {
                    triggersParams.MasterAccountID = Entity.MasterAccountID.Value;
                }
                else
                {
                    triggersParams.MasterAccountID = Entity.ID;
                }

                triggersParams.Username = Thread.CurrentPrincipal.Identity.Name;
                triggersParams.ExecutionDate = DateTime.Now;
                triggersParams.UnitOfWork = UnitOfWork;

                var stateMachine = new InternshipPositionStateMachine(position);

                if (position.InternshipPositionGroup.PositionGroupStatus == enPositionGroupStatus.Revoked)
                {
                    if (position.DaysLeftForAssignment == 0)
                    {
                        triggersParams.CancellationReason = enCancellationReason.CanceledGroupCascade;
                        stateMachine.Cancel(triggersParams);
                    }
                    else
                    {
                        triggersParams.BlockingReason = enBlockingReason.None;
                        stateMachine.DeleteAssignment(triggersParams);
                    }

                    UnitOfWork.Commit();
                }
                else if (position.InternshipPositionGroup.PositionGroupStatus == enPositionGroupStatus.UnPublished)
                {
                    if (position.DaysLeftForAssignment == 0)
                    {
                        stateMachine.UnPublish(triggersParams);
                    }
                    else
                    {
                        triggersParams.BlockingReason = enBlockingReason.None;
                        stateMachine.DeleteAssignment(triggersParams);
                    }

                    UnitOfWork.Commit();
                }
                else
                {
                    if (position.DaysLeftForAssignment == 0)
                    {
                        triggersParams.BlockingReason = enBlockingReason.RolledbackAssignmentOutOfTime;
                    }

                    stateMachine.DeleteAssignment(triggersParams);
                }
            }
            else if (action == "rollbackcompletion")
            {
                InternshipPositionTriggersParams triggersParams = new InternshipPositionTriggersParams();

                triggersParams.OfficeID = Entity.ID;

                if (Entity.MasterAccountID.HasValue)
                {
                    triggersParams.MasterAccountID = Entity.MasterAccountID.Value;
                }
                else
                {
                    triggersParams.MasterAccountID = Entity.ID;
                }

                triggersParams.Username = Thread.CurrentPrincipal.Identity.Name;
                triggersParams.ExecutionDate = DateTime.Now;
                triggersParams.UnitOfWork = UnitOfWork;

                var stateMachine = new InternshipPositionStateMachine(position);
                stateMachine.RollbackCompletion(triggersParams);
            }
            else if (action == "rollbackcancellation")
            {
                if (position.CanceledStudent.IsAssignedToPosition.GetValueOrDefault())
                {
                    fm.Text = "Η θέση δεν μπορεί να τεθεί ξανά σε κατάσταση 'Υπό Διενέργεια', γιατί ο φοιτητής έχει ήδη αντιστοιχιστεί σε άλλη θέση Πρακτικής Άσκησης";
                }
                else
                {
                    InternshipPositionTriggersParams triggersParams = new InternshipPositionTriggersParams();

                    triggersParams.OfficeID = Entity.ID;

                    if (Entity.MasterAccountID.HasValue)
                    {
                        triggersParams.MasterAccountID = Entity.MasterAccountID.Value;
                    }
                    else
                    {
                        triggersParams.MasterAccountID = Entity.ID;
                    }

                    triggersParams.Username = Thread.CurrentPrincipal.Identity.Name;
                    triggersParams.ExecutionDate = DateTime.Now;
                    triggersParams.UnitOfWork = UnitOfWork;

                    var stateMachine = new InternshipPositionStateMachine(position);
                    stateMachine.RollbackCancellation(triggersParams);
                }
            }
            else if (action == "deletefinishedposition")
            {
                var positionOldStatus = position.PositionStatus;
                var groupOldStatus = position.InternshipPositionGroup.PositionGroupStatus;

                position.InternshipPositionGroup.PositionGroupStatus = enPositionGroupStatus.Deleted;
                position.PositionStatus = enPositionStatus.Canceled;

                InternshipPositionGroupLog gLog = new InternshipPositionGroupLog();
                gLog.CreatedAt = DateTime.Now;
                gLog.CreatedAtDateOnly = DateTime.Now.Date;
                gLog.CreatedBy = Page.User.Identity.Name;
                gLog.GroupID = position.GroupID;
                gLog.OldStatus = groupOldStatus;
                gLog.NewStatus = enPositionGroupStatus.Deleted;

                InternshipPositionLog pLog = new InternshipPositionLog();
                pLog.CreatedAt = DateTime.Now;
                pLog.CreatedAtDateOnly = DateTime.Now.Date;
                pLog.CreatedBy = Page.User.Identity.Name;
                pLog.InternshipPositionID = position.ID;
                pLog.OldStatus = positionOldStatus;
                pLog.NewStatus = enPositionStatus.Canceled;

                UnitOfWork.MarkAsNew(pLog);
                UnitOfWork.MarkAsNew(gLog);
                UnitOfWork.Commit();
            }


            gvPositions.DataBind();
        }

        protected void gveIntershipPositions_RenderBrick(object sender, DevExpress.Web.ASPxGridView.Export.ASPxGridViewExportRenderingEventArgs e)
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
                        case "Country":
                            if (position.InternshipPositionGroup.CountryID == StudentPracticeConstants.GreeceCountryID)
                                e.TextValue = e.Text = StudentPracticeConstants.GreeceCountryName;
                            else if (position.InternshipPositionGroup.CountryID == StudentPracticeConstants.CyprusCountryID)
                                e.TextValue = e.Text = StudentPracticeConstants.CyprusCountryName;
                            else
                                e.TextValue = e.Text = "Άλλη";
                            break;
                        case "ProviderType":
                            e.TextValue = e.Text = position.InternshipPositionGroup.Provider.ProviderType.GetLabel();
                            break;
                        case "Prefecture":
                            e.TextValue = e.Text = position.InternshipPositionGroup.PrefectureID.HasValue
                                                    ? CacheManager.Prefectures.Get(position.InternshipPositionGroup.PrefectureID.Value).Name
                                                    : string.Empty;
                            break;
                        case "City":
                            e.TextValue = e.Text = position.InternshipPositionGroup.CityID.HasValue
                                                    ? CacheManager.Cities.Get(position.InternshipPositionGroup.CityID.Value).Name
                                                    : string.Empty;
                            break;
                        case "PositionStatus":
                            e.TextValue = e.Text = GetPositionStatus(position);
                            break;
                        case "PositionType":
                            e.TextValue = e.Text = position.InternshipPositionGroup.PositionType.GetLabel();
                            break;
                        case "PreAssignedAt":
                            e.TextValue = e.Text = GetPreAssignedAt(position);
                            break;
                        case "PreAssignedForAcademic.Department":
                            e.TextValue = e.Text = (position.PreAssignedForAcademic == null ? string.Empty : position.PreAssignedForAcademic.Department);
                            break;
                        case "AssignedAt":
                            e.TextValue = e.Text = GetAssignedAt(position);
                            break;
                        case "AssignedToStudent.ContactName":
                            e.TextValue = e.Text = (position.AssignedToStudent == null ? string.Empty : position.AssignedToStudent.ContactName);
                            break;
                        case "AssignedToStudent.StudentNumber":
                            e.TextValue = e.Text = (position.AssignedToStudent == null ? string.Empty : position.AssignedToStudent.StudentNumber);
                            break;
                        case "ImplementationStartDate":
                            e.TextValue = e.Text = GetImplementationStartDate(position);
                            break;
                        case "ImplementationEndDate":
                            e.TextValue = e.Text = GetImplementationEndDate(position);
                            break;
                        //ToFix: FundingType
                        //case "FundingType":
                        //    if (position.FundingTypeInt.HasValue)
                        //    {
                        //        e.TextValue = e.Text = position.FundingType.GetLabel();
                        //    }
                        //    break;
                        case "CompletedAt":
                            e.TextValue = e.Text = GetCompletedAt(position);
                            break;
                        case "PositionCreationType":
                            e.TextValue = e.Text = position.InternshipPositionGroup.PositionCreationType.GetLabel();
                            break;
                        case "OfficeUser":
                            e.TextValue = e.Text = position.PreAssignedByOffice == null
                                ? position.PreAssignedByMasterAccount == null
                                ? string.Empty : position.PreAssignedByMasterAccount.UserName : position.PreAssignedByOffice.UserName;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        #endregion

        #region [ Helper Methods Get]

        protected string GetStudentDetails(InternshipPosition position)
        {
            if (position == null || !position.PreAssignedForAcademicID.HasValue)
                return string.Empty;

            Student student;
            if (position.PositionStatus == enPositionStatus.Canceled)
                student = position.CanceledStudent;
            else
                student = position.AssignedToStudent;

            if (student != null)
                return string.Format("{0}<br/><br/><b>{1} {2}<br/>{3}</b>", position.PreAssignedForAcademic.Department, student.GreekFirstName, student.GreekLastName, student.StudentNumber);
            else
                return string.Format("{0}", position.PreAssignedForAcademic.Department);
        }

        protected string GetPositionStatus(InternshipPosition ip)
        {
            if (ip == null)
                return string.Empty;
            else if (ip.PositionStatus == enPositionStatus.Canceled && ip.CancellationReason > enCancellationReason.FromOffice)
                return "Αποσυρμένη";
            else if (ip.InternshipPositionGroup.PositionCreationType == enPositionCreationType.FromOffice && ip.PositionStatus == enPositionStatus.UnPublished)
                return "Ημιτελής Ολοκληρωμένη";
            else
                return ip.PositionStatus.GetLabel();
        }

        protected string GetPreAssignedAt(InternshipPosition ip)
        {
            if (ip == null || !ip.PreAssignedAt.HasValue)
                return string.Empty;
            else
                return ip.PreAssignedAt.Value.ToString("dd/MM/yyyy");
        }

        protected string GetAssignedAt(InternshipPosition ip)
        {
            if (ip == null || !ip.AssignedAt.HasValue || ip.AssignedToStudent == null)
                return string.Empty;
            else
                return ip.AssignedAt.Value.ToString("dd/MM/yyyy");
        }

        protected string GetImplementationStartDate(InternshipPosition ip)
        {
            if (ip == null || !ip.ImplementationStartDate.HasValue || !ip.ImplementationEndDate.HasValue)
                return string.Empty;
            else
                return ip.ImplementationStartDate.Value.ToString("dd/MM/yyyy");
        }

        protected string GetImplementationEndDate(InternshipPosition ip)
        {
            if (ip == null || !ip.ImplementationStartDate.HasValue || !ip.ImplementationEndDate.HasValue)
                return string.Empty;
            else
                return ip.ImplementationEndDate.Value.ToString("dd/MM/yyyy");
        }

        protected string GetCompletedAt(InternshipPosition ip)
        {
            if (ip == null || !ip.CompletedAt.HasValue)
                return string.Empty;
            else
                return ip.CompletedAt.Value.ToString("dd/MM/yyyy");
        }

        protected string GetUpdatedAt(InternshipPosition ip)
        {
            if (ip == null || !ip.UpdatedAt.HasValue)
                return String.Empty;

            return ip.UpdatedAt.Value.ToString();
        }

        #endregion

        #region [ Helper Methods Actions]

        protected bool RollbackPreAssignmentWithoutPenalty(InternshipPosition position)
        {
            if (position == null)
                return false;

            return position.DaysLeftForAssignment.Value == StudentPracticeConstants.Default_MaxDaysForAssignment
                || position.InternshipPositionGroup.Academics.Count == 1;
        }

        protected bool RollbackPreAssignmentWithPenalty(InternshipPosition position)
        {
            if (position == null)
                return false;

            return position.DaysLeftForAssignment.Value < StudentPracticeConstants.Default_MaxDaysForAssignment
                && position.InternshipPositionGroup.Academics.Count != 1;
        }

        protected bool DeleteAssignmentWithoutPenalty(InternshipPosition position)
        {
            if (position == null)
                return false;

            return position.DaysLeftForAssignment.Value > 0;
        }

        protected bool DeleteAssignmentWithPenalty(InternshipPosition position)
        {
            if (position == null)
                return false;

            return position.DaysLeftForAssignment.Value == 0;
        }

        #endregion

        #region [ Helper Methods Can]

        protected bool CanDeleteAssignment(InternshipPosition position)
        {
            if (position == null)
                return false;

            var stateMachine = new InternshipPositionStateMachine(position);

            return stateMachine.CanFire(enInternshipPositionTriggers.DeleteAssignment);
        }

        protected bool CanCompleteImplementation(InternshipPosition position)
        {
            if (position == null)
                return false;

            var stateMachine = new InternshipPositionStateMachine(position);

            return stateMachine.CanFire(enInternshipPositionTriggers.CompleteImplementation);
        }

        protected bool CanRollbackCompletion(InternshipPosition position)
        {
            if (position == null)
                return false;

            var stateMachine = new InternshipPositionStateMachine(position);

            //return stateMachine.CanFire(enInternshipPositionTriggers.RollbackCompletion)
            //    && position.InternshipPositionGroup.PositionCreationType != enPositionCreationType.FromOffice
            //    && position.AssignedToStudent.IsAssignedToPosition == false;

            return stateMachine.CanFire(enInternshipPositionTriggers.RollbackCompletion)
                 && position.InternshipPositionGroup.PositionCreationType != enPositionCreationType.FromOffice
                 && !(new InternshipPositionRepository().FindActiveByStudent(position.AssignedToStudentID.Value).Any());

        }

        protected bool CanRollbackCancellation(InternshipPosition position)
        {
            if (position == null)
                return false;

            var stateMachine = new InternshipPositionStateMachine(position);

            //return stateMachine.CanFire(enInternshipPositionTriggers.RollbackCancellation);
            //    && (position.AssignedToStudent == null || position.AssignedToStudent.IsAssignedToPosition == false);

            return stateMachine.CanFire(enInternshipPositionTriggers.RollbackCancellation)
                && (position.AssignedToStudent == null || !(new InternshipPositionRepository().FindActiveByStudent(position.AssignedToStudentID.Value).Any()));
        }

        protected bool CanRollbackPreAssignment(InternshipPosition position)
        {
            if (position == null)
                return false;

            var stateMachine = new InternshipPositionStateMachine(position);

            return stateMachine.CanFire(enInternshipPositionTriggers.RollbackPreAssignment);
        }

        protected bool CanAssign(InternshipPosition position)
        {
            if (position == null)
                return false;

            var stateMachine = new InternshipPositionStateMachine(position);

            return position.PositionStatus == enPositionStatus.PreAssigned;
        }

        protected bool CanEditAssignment(InternshipPosition position)
        {
            if (position == null)
                return false;

            return position.PositionStatus == enPositionStatus.Assigned || position.PositionStatus == enPositionStatus.UnderImplementation;
        }

        protected bool CanDelete(InternshipPosition position)
        {
            return position != null
                && position.InternshipPositionGroup.PositionCreationType == enPositionCreationType.FromOffice
                && position.PositionStatus != enPositionStatus.Canceled;
        }

        protected bool CanEdit(InternshipPosition position)
        {
            return position != null
                && position.InternshipPositionGroup.PositionCreationType == enPositionCreationType.FromOffice
                && position.PositionStatus == enPositionStatus.UnPublished;
        }

        #endregion
    }
}
