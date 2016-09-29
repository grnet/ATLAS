using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.Portal.Controls;
using StudentPractice.BusinessModel;
using System.Drawing;

namespace StudentPractice.Portal.Secure.Helpdesk
{
    public partial class TransferPositions : BaseEntityPortalPage<object>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void btnTransfer_Click(object sender, EventArgs e)
        {
            lblResult.ForeColor = Color.Red;

            int posID = -1;
            if (!int.TryParse(txtPositionID.Text, out posID))
            {
                lblResult.Text = "Λάθος ID Θέσης.";
                return;
            }

            int newOfficeID = -1;
            if (!int.TryParse(txtNewOfficeID.Text, out newOfficeID))
            {
                lblResult.Text = "Λάθος ID Γραφείου.";
                return;
            }


            var position = new InternshipPositionRepository(UnitOfWork).Load(posID,
                x => x.PreAssignedByMasterAccount,
                x => x.PreAssignedForAcademic,
                x => x.AssignedToStudent,
                x => x.LogEntries);
            if (position == null)
            {
                lblResult.Text = "Δεν βρέθηκε η Θέση Πρακτικής.";
                return;
            }

            var newOffice = new InternshipOfficeRepository(UnitOfWork).Load(newOfficeID, x => x.Academics);
            if (newOffice == null)
            {
                lblResult.Text = "Δεν βρέθηκε το γραφείο.";
                return;
            }

            var result = BusinessHelper.TransferPosition(position, newOffice);
            switch (result)
            {
                case enPositionTransferResult.Success:
                    UnitOfWork.Commit();
                    lblResult.Text = "Η μεταφορά ολοκληρώθηκε με επιτυχία.";
                    lblResult.ForeColor = Color.Green;
                    break;
                case enPositionTransferResult.InvalidStatus:
                    lblResult.Text = "Η Θέση πρέπει να είναι τουλάχιστον προδεσμευμένη.";
                    break;
                case enPositionTransferResult.NewOfficeIsTheSame:
                    lblResult.Text = "Το νέο γραφείο είναι ίδιο με το παλιό.";
                    break;
                case enPositionTransferResult.NewOfficeIsNotVerified:
                    lblResult.Text = "Το νέο γραφείο δεν είναι πιστοποιημένο.";
                    break;
                case enPositionTransferResult.NewOfficeDoesNotServeAcademic:
                    lblResult.Text = "Το νέο γραφείο δεν εξυπηρετεί την σχολή για την οποία έχει προδεσμευτεί η θέση.";
                    break;
                case enPositionTransferResult.NewOfficeDoesNotServeStudent:
                    lblResult.Text = "Το νέο γραφείο δεν εξυπηρετεί τον φοιτητή που έχει αντιστοιχιστεί στην θέση.";
                    break;
            }

        }

    }
}