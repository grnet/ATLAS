using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using Imis.Domain;
using System.Globalization;
using StudentPractice.BusinessModel.Flow;
using System.Data.Objects;
using StudentPractice.Portal.Controls;
using StudentPractice.Utils.Worker;
using System.Net;
using System.IO;
using System.Xml.Serialization;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace StudentPractice.Portal
{
    public partial class TestShibLogin : BaseEntityPortalPage<object>
    {
        protected void Page_Load(object sender, EventArgs e)
        {   
            phPilotSite.Visible = Config.IsPilotApplication;

            Page.ClientScript.RegisterStartupScript(this.GetType(), "jsInit", string.Format("hd.init('{0}','{1}','{2}','{3}');", hfSchoolCode.ClientID, txtInstitutionName.ClientID, txtSchoolName.ClientID, txtDepartmentName.ClientID), true);

            txtInstitutionName.Attributes.Add("readonly", "readonly");
            txtSchoolName.Attributes.Add("readonly", "readonly");
            txtDepartmentName.Attributes.Add("readonly", "readonly");
            txtFirstName.Attributes["onkeyup"] = "Imis.Lib.ToUpperForNames(this)";
            txtLastName.Attributes["onkeyup"] = "Imis.Lib.ToUpperForNames(this)";

            btnSendNewlyPublishedPositions.Visible = User.Identity.IsAuthenticated && (Roles.IsUserInRole(RoleNames.SuperHelpdesk) || Roles.IsUserInRole(RoleNames.SystemAdministrator));
        }

        protected void btnCheckPreAssignedPositions_Click(object sender, EventArgs e)
        {
            WorkerActions.CheckPreAssignedPositionsTask();
            lblError.Text = "Ο έλεγχος των προδεσμευμένων θέσεων ολοκληρώθηκε επιτυχώς";
        }

        protected void btnCheckAssignedPositions_Click(object sender, EventArgs e)
        {
            WorkerActions.CheckAssignedPositionsTask();
            lblError.Text = "Ο έλεγχος έναρξης διενέργειας των δεσμευμένων θέσεων ολοκληρώθηκε επιτυχώς";
        }

        protected void btnCheckBlockedPositions_Click(object sender, EventArgs e)
        {
            WorkerActions.CheckBlockedPositionsTask();
            lblError.Text = "Ο έλεγχος των μπλοκαρισμένων θέσεων ολοκληρώθηκε επιτυχώς";
        }

        protected void btnSendNewlyPublishedPositions_Click(object sender, EventArgs e)
        {
            WorkerActions.CheckNewlyPublishedPositionsTask();
            lblError.Text = "Η ενημέρωση των Γραφείων Πρακτικής για τις νέες δημοσιευμένες θέσεις ολοκληρώθηκε επιτυχώς";
        }

        protected void btnChangePreAssignedDate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            int positionID;
            DateTime preAssignedAt;
            int daysLeftForAssignment;
            if (int.TryParse(txtPositionID.Text, out positionID) && positionID > 0)
            {
                if (DateTime.TryParse(txtStartDate.Text, out preAssignedAt))
                {
                    if (int.TryParse(txtDaysLeftAssignment.Text, out daysLeftForAssignment) && daysLeftForAssignment >= 0)
                    {
                        var position = new InternshipPositionRepository(UnitOfWork).Load(positionID);
                        position.ImplementationStartDate = preAssignedAt;
                        position.DaysLeftForAssignment = daysLeftForAssignment;

                        if (position.DaysLeftForAssignment <= 0)
                        {
                            InternshipPositionTriggersParams triggersParams = new InternshipPositionTriggersParams();
                            triggersParams.Username = "sysadmin";
                            triggersParams.ExecutionDate = DateTime.Now;
                            triggersParams.UnitOfWork = UnitOfWork;
                            var stateMachine = new InternshipPositionStateMachine(position);

                            if (position.InternshipPositionGroup.PositionGroupStatus == enPositionGroupStatus.Revoked)
                            {
                                triggersParams.CancellationReason = enCancellationReason.CanceledGroupCascade;
                                stateMachine.Cancel(triggersParams);
                            }
                            else if (position.InternshipPositionGroup.PositionGroupStatus == enPositionGroupStatus.UnPublished)
                            {
                                stateMachine.UnPublish(triggersParams);
                            }
                            else
                            {
                                triggersParams.MasterAccountID = position.PreAssignedByMasterAccountID.Value;
                                triggersParams.BlockingReason = enBlockingReason.TimeForAssignmentExpired;
                                stateMachine.RollbackPreAssignment(triggersParams);
                            }
                        }

                        UnitOfWork.Commit();
                    }
                }
            }

            lblError.Text = "Η ενημέρωση της προδεσμευμένης θέσης ολοκληρώθηκε επιτυχώς";
        }

        protected void btnChangeBlockedPositions_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            int groupID;
            int blockedDaysLeft;
            if (int.TryParse(txtBlockedGroupID.Text, out groupID) && groupID > 0)
            {
                if (int.TryParse(txtBlockedDaysLeft.Text, out blockedDaysLeft) && blockedDaysLeft >= 0)
                {
                    var blockedGroup = new BlockedPositionGroupRepository(UnitOfWork).FindByGroupID(groupID, x => x.CascadedBlocks);
                    if (blockedGroup != null)
                    {
                        blockedGroup.DaysLeft = blockedDaysLeft;
                        foreach (var item in blockedGroup.CascadedBlocks)
                            item.DaysLeft = blockedDaysLeft;

                        if (blockedGroup.DaysLeft <= 0)
                        {
                            var cascadedBlocks = blockedGroup.CascadedBlocks.ToList();
                            for (int i = cascadedBlocks.Count - 1; i >= 0; i--)
                                UnitOfWork.MarkAsDeleted(cascadedBlocks[i]);

                            UnitOfWork.MarkAsDeleted(blockedGroup);
                        }
                    }

                    UnitOfWork.Commit();
                }
            }

            lblError.Text = "Η ενημέρωση των ημερών μπλοκαρίσματος ολοκληρώθηκε επιτυχώς";
        }


        protected void btnLogin_Click(object sender, EventArgs e)
        {
            StudentRepository sRep = new StudentRepository();

            string username = txtUsername.Text.ToNull();
            var student = sRep.FindByStudentNumber(username);

            if (student == null)
            {
                lblError.Text = "Δεν βρέθηκε Προπτυχιακός Φοιτητής με τον Αρ. Μητρώου που εισάγατε.";
                return;
            }
            else
            {
                ShibDetails details = new ShibDetails()
                {
                    FullName = string.Format("{0} {1}", student.OriginalFirstName, student.OriginalLastName),
                    AcademicID = student.AcademicID.ToString(),
                    Affiliation = "student",
                    FirstName = student.OriginalFirstName,
                    LastName = student.OriginalLastName,
                    StudentCode = student.StudentNumber,
                    Email = "",
                    Username = string.IsNullOrEmpty(student.UsernameFromLDAP) ? string.Format("{0}@localhost.com", username) : student.UsernameFromLDAP,
                    HomeOrganization = "localhost.com"
                };

                ShibDetails.FakeLogin(details);
                Response.Redirect("~/Shib/Default.aspx");
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            int schoolCode;
            if (int.TryParse(hfSchoolCode.Value, out schoolCode))
            {
                ShibDetails details = new ShibDetails()
                {
                    FullName = string.Format("{0} {1}", txtFirstName.Text.ToNull(), txtLastName.Text.ToNull()),
                    AcademicID = schoolCode.ToString(),
                    Affiliation = "student",
                    FirstName = txtFirstName.Text.ToNull(),
                    LastName = txtLastName.Text.ToNull(),
                    StudentCode = txtStudentNumber.Text.ToNull(),
                    Email = "",
                    Username = string.Format("{0}@localhost.com", txtStudentNumber.Text.ToNull()),
                    HomeOrganization = "localhost.com"
                };

                ShibDetails.FakeLogin(details);
                Response.Redirect("~/Shib/Default.aspx");
            }
        }
    }
}