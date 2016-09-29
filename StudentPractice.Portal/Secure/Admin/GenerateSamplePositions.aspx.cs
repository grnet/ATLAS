using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;
using StudentPractice.BusinessModel.Flow;
using DevExpress.Web.ASPxGridView;
using Imis.Domain;
using System.Threading;

namespace StudentPractice.Portal.Secure.Admin
{
    public partial class GenerateSamplePositions : System.Web.UI.Page
    {
        List<InternshipPositionGroup> TempGeneratedPositionGroups = new List<InternshipPositionGroup>();

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            Page.ClientScript.RegisterForEventValidation(ddlProviders.UniqueID, string.Empty);
            Page.ClientScript.RegisterForEventValidation(ddlInstitutions.UniqueID, string.Empty);
            Page.ClientScript.RegisterForEventValidation(ddlPositionGroups.UniqueID, string.Empty);
            base.Render(writer);
        }

        protected void ddlProviders_Init(object sender, EventArgs e)
        {
            foreach (var item in new InternshipProviderRepository().LoadAll())
            {
                ddlProviders.Items.Add(new ListItem(string.Format("{0} - {1}", item.ID, item.Name), item.ID.ToString()));
            }
        }

        protected void ddlInstitutions_Init(object sender, EventArgs e)
        {
            ddlInstitutions.Items.Add(new ListItem("-- αδιάφορο --", ""));
            foreach (var item in CacheManager.Institutions.GetItems())
            {
                ddlInstitutions.Items.Add(new ListItem(item.Name, item.ID.ToString()));
            }
        }

        protected void ddlPositionGroups_Init(object sender, EventArgs e)
        {
            for (int i = 1; i <= 15; i++)
            {
                ddlPositionGroups.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }

        protected void gvAcademics_DataBound(object sender, EventArgs e)
        {

        }

        protected void btnbtnGeneratePositions_Click(object sender, EventArgs e)
        {
            #region Parse PostBack Data
            Random random = new Random();
            List<int> selectedAcademics = new List<int>();
            bool IsAvailableByAllAcademics = chk_IsAvailableByAllAcademics.Checked;
            if (!IsAvailableByAllAcademics)
            {
                int institutionID;
                if (int.TryParse(ddlInstitutions.SelectedItem.Value, out institutionID) && institutionID > 0)
                    selectedAcademics = CacheManager.Academics.GetItems().Where(x => x.InstitutionID == institutionID).Select(x => x.ID).ToList();
                else
                    selectedAcademics.AddRange(GetSelectedId(gvAcademics));
            }

            if (!IsAvailableByAllAcademics && selectedAcademics.Count() == 0)
            {
                lblErrors.Text = "Επιλέξτε τουλάχιστον μία σχολή";
                return;
            }


            int providerID;
            int.TryParse(ddlProviders.SelectedItem.Value, out providerID);
            InternshipProvider provider = new InternshipProviderRepository().Load(providerID);

            int numberOfGroups;
            int.TryParse(ddlPositionGroups.SelectedItem.Value, out numberOfGroups);

            int numberOfPositions;
            int.TryParse(tbxPositions.Text, out numberOfPositions);
            if (numberOfPositions <= 0)
            {
                lblErrors.Text = "Δώστε πλήθως θέσεων";
                return;
            }
            else if (numberOfPositions > 50) numberOfPositions = 50;

            #endregion

            using (IUnitOfWork UnitOfWork = UnitOfWorkFactory.Create())
            {
                for (int i = 0; i < numberOfGroups; i++)
                {
                    #region Group

                    var group = new InternshipPositionGroup();
                    group.ProviderID = provider.ID;
                    group.TotalPositions = numberOfPositions;
                    group.AvailablePositions = numberOfPositions;
                    group.PreAssignedPositions = 0;
                    group.IsVisibleToAllAcademics = IsAvailableByAllAcademics;
                    group.Title = RandomText(15);
                    group.Description = RandomText(50);
                    int duration = random.Next(8, 40);
                    group.Duration = duration;
                    bool noTimeLimit = (random.Next(0, 100) > 50);
                    group.NoTimeLimit = noTimeLimit;
                    if (!noTimeLimit)
                    {
                        var startDate = DateTime.Now.AddDays(random.Next(1, 90));
                        group.StartDate = startDate;
                        group.EndDate = startDate.AddDays(7 * duration);
                    }
                    int cityID = random.Next(9001, 9325);
                    group.CityID = cityID;
                    group.PrefectureID = CacheManager.Cities.Get(cityID).PrefectureID;
                    group.CountryID = StudentPracticeConstants.GreeceCountryID;

                    group.PositionTypeInt = (random.Next(0, 100) > 50) ? 1 : 2;
                    group.ContactPhone = provider.ContactPhone;
                    group.Supervisor = provider.ContactName;
                    group.SupervisorEmail = provider.ContactEmail;
                    group.PositionGroupStatus = enPositionGroupStatus.Published;
                    group.PositionCreationType = enPositionCreationType.FromProvider;
                    group.FirstPublishedAt = DateTime.Now;
                    group.LastPublishedAt = DateTime.Now;
                    UnitOfWork.MarkAsNew(group);

                    var phID = new PhysicalObjectRepository(UnitOfWork).Load(random.Next(1, 21));
                    group.PhysicalObjects.Add(phID);
                    if (!IsAvailableByAllAcademics)
                    {
                        foreach (var item in selectedAcademics)
                        {
                            var academic = new AcademicRepository(UnitOfWork).Load(item);
                            group.Academics.Add(academic);
                        }
                    }
                    #endregion

                    for (int j = 0; j < numberOfPositions; j++)
                    {

                        #region Position
                        var position = new InternshipPosition();
                        position.InternshipPositionGroup = group;
                        position.PositionStatusInt = (int)enPositionStatus.Available;
                        position.CancellationReason = 0;
                        #endregion

                        #region Position Log
                        InternshipPositionLog logEntry = new InternshipPositionLog();
                        logEntry.InternshipPosition = position;
                        logEntry.OldStatus = enPositionStatus.UnPublished;
                        logEntry.NewStatus = enPositionStatus.Canceled;
                        logEntry.CreatedAt = DateTime.Now;
                        logEntry.CreatedAtDateOnly = DateTime.Now.Date;
                        logEntry.CreatedBy = Thread.CurrentPrincipal.Identity.Name;
                        UnitOfWork.MarkAsNew(logEntry);
                        #endregion

                    }

                    #region Group Log
                    InternshipPositionGroupLog log = new InternshipPositionGroupLog();
                    log.InternshipPositionGroup = group;
                    log.OldStatus = group.PositionGroupStatus;
                    log.NewStatus = group.PositionGroupStatus;
                    log.CreatedAt = DateTime.Now;
                    log.CreatedAtDateOnly = DateTime.Now.Date;
                    log.CreatedBy = Thread.CurrentPrincipal.Identity.Name;
                    log.UpdatedAt = DateTime.Now;
                    log.UpdatedAtDateOnly = DateTime.Now.Date;
                    log.UpdatedBy = Thread.CurrentPrincipal.Identity.Name;
                    #endregion
                    TempGeneratedPositionGroups.Add(group);

                }
                UnitOfWork.Commit();
                List<InternshipPositionGroup> GeneratedPositionGroups = new List<InternshipPositionGroup>();
                foreach (var item in TempGeneratedPositionGroups)
                {
                    GeneratedPositionGroups.Add(new InternshipPositionGroupRepository(UnitOfWork).Load(item.ID, x => x.Provider, x => x.Academics, x => x.PhysicalObjects));
                }


                gvGeneratedPosition.Visible = true;
                gvGeneratedPosition.DataSource = GeneratedPositionGroups.OrderBy(x => x.ID);
                gvGeneratedPosition.DataBind();

                gveGeneratedPosition.FileName = String.Format("GeneratedPositions_{0}", DateTime.Now.ToString("yyyyMMdd_hhmm"));
                gveGeneratedPosition.WriteXlsxToResponse(true);

                lblErrors.Text = "Η δημιουργία των δοκιμαστικών δεδομένων ολοκληρώθηκε με επιτυχία";
            }
        }

        private string RandomText(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
        }

        protected string GetPositionName(InternshipPositionGroup ip)
        {
            if (ip == null)
                return string.Empty;
            return string.Format("{0}  {1}", ip.Title, ip.Description);
        }

        protected string GetCityPrefecture(InternshipPositionGroup ip)
        {
            if (ip == null)
                return string.Empty;

            if (ip.CountryID == StudentPracticeConstants.GreeceCountryID)
            {
                var city = ip.CityID.HasValue ? CacheManager.Cities.Get(ip.CityID.Value) : null;
                var prefecture = ip.PrefectureID.HasValue ? CacheManager.Prefectures.Get(ip.PrefectureID.Value) : null;
                if (city != null && prefecture != null)
                    return string.Format("{0}  {1}", city.Name, prefecture.Name);
                else
                    return string.Empty;
            }
            else if (ip.CountryID == StudentPracticeConstants.CyprusCountryID)
            {
                return string.Format("{0} {1}", StudentPracticeConstants.CyprusCountryName, ip.CityText);
            }
            else
            {
                var country = CacheManager.Countries.Get(ip.CountryID);
                return string.Format("{0} {1}", country.Name, ip.CityText);
            }
        }

        protected string GetTimeConstrain(InternshipPositionGroup ip)
        {
            if (ip == null)
                return string.Empty;
            if (ip.NoTimeLimit)
                return "Κανένας";
            else
                return string.Format("Από: {0}  Έως: {1}", ip.StartDate, ip.EndDate);
        }

        protected string GetAcademics(InternshipPositionGroup ip)
        {
            if (ip == null)
                return string.Empty;
            if (ip.IsVisibleToAllAcademics.GetValueOrDefault())
                return "Όλα";
            else
                return string.Join("  ", ip.Academics.Select(x => string.Format("({0}) - {1} - {2}", x.ID, x.Institution, x.Department)));
        }

        protected string GetPhysicalObjects(InternshipPositionGroup ip)
        {
            if (ip == null)
                return string.Empty;
            else
                return string.Join(";", ip.PhysicalObjects.Select(x => x.Name));
        }

        private List<int> GetSelectedId(ASPxGridView grid)
        {
            return grid.GetSelectedFieldValues("ID").OfType<int>().ToList();
        }

        protected void gveIntershipPositionGroups_RenderBrick(object sender, DevExpress.Web.ASPxGridView.Export.ASPxGridViewExportRenderingEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                var position = gvGeneratedPosition.GetRow(e.VisibleIndex) as InternshipPositionGroup;

                if (position != null)
                {
                    switch (e.Column.Name)
                    {
                        case "GroupID":
                            e.TextValue = e.Text = position.ID.ToString();
                            break;
                        case "ProviderID":
                            e.TextValue = e.Text = position.ProviderID.ToString();
                            break;
                        case "ProviderName":
                            e.TextValue = e.Text = position.Provider.Name;
                            break;
                        case "Duration":
                            e.TextValue = e.Text = position.Duration.ToString();
                            break;
                        case "ContactPhone":
                            e.TextValue = e.Text = position.ContactPhone;
                            break;
                        case "TitleDescription":
                            e.TextValue = e.Text = GetPositionName(position).Replace("<br />", "\n");
                            break;
                        case "CityPrefecture":
                            e.TextValue = e.Text = GetCityPrefecture(position).Replace("<br />", "\n");
                            break;
                        case "TimeConstrain":
                            e.TextValue = e.Text = GetTimeConstrain(position).Replace("<br />", "\n");
                            break;
                        case "PositionType":
                            e.TextValue = e.Text = position.PositionType.GetLabel();
                            break;
                        case "IsPublished":
                            e.TextValue = e.Text = position.PositionGroupStatus == enPositionGroupStatus.Published ? "Ναι" : "Όχι";
                            break;
                        case "Academics":
                            e.TextValue = e.Text = GetAcademics(position).Replace("<br />", "\n");
                            break;
                        case "PhysicalObjects":
                            e.TextValue = e.Text = GetPhysicalObjects(position).Replace("<br />", "\n");
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}