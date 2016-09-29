using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;

namespace StudentPractice.Portal.UserControls.Exporters
{
    public partial class InternshipPositionsExporter : BaseHtmlExporter
    {
        protected override Tuple<object, ExcelFormat?> GetRowValue(object value, string columnName)
        {
            var format = (ExcelFormat?)null;            

            switch (columnName)
            {
                case "CountryID":
                    var countryID = (int?)value;

                    if (!countryID.HasValue)
                    {
                        value = null;
                    }
                    else
                    {
                        if (countryID == StudentPracticeConstants.GreeceCountryID)
                        {
                            value = StudentPracticeConstants.GreeceCountryName;
                        }
                        else if (countryID == StudentPracticeConstants.CyprusCountryID)
                        {
                            value = StudentPracticeConstants.CyprusCountryName;
                        }
                        else
                        {
                            value = "Άλλη";
                        }
                    }

                    break;                
                case "PositionType":
                    if (value != null)
                    {
                        value = ((enPositionType)value).GetLabel();
                    }
                    break;
                case "PositionGroupStatus":
                    if (value != null)
                    {
                        value = ((enPositionGroupStatus)value).GetLabel();
                    }
                    break;
                case "PositionStatus":
                    if ((int)value == -1)
                    {
                        value = "Αποσυρμένη";
                    }
                    else
                    {
                        value = ((enPositionStatus)value).GetLabel();
                    }
                    
                    break;
                case "PositionCreationType":
                    if (value != null)
                    {
                        value = ((enPositionCreationType)value).GetLabel();
                    }
                    break;
                case "FundingType":
                    if (value != null)
                    {
                        value = ((enFundingType)value).GetLabel();
                    }
                    break;
                case "CreatedAtDateOnly":
                case "FirstPublishedAt":
                case "StartDate":
                case "EndDate":
                case "PreAssignedAt":
                case "ImplementationStartDate":
                case "ImplementationEndDate":
                case "CompletedAt":                    
                    var dt = (DateTime?)value;
                    value = dt.HasValue
                            ? dt.Value.ToString("dd/MM/yyyy HH:mm")
                            : null;
                    break;
            }

            return Tuple.Create(value, format);
        }

        protected override string GetCaption(string columnName)
        {
            switch (columnName)
            {
                case "GroupID":
                    return "ID Group";
                case "PositionID":
                    return "ID Θέσης";                
                case "CreatedAtDateOnly":
                    return "Ημ/νία Δημιουργίας";
                case "FirstPublishedAt":
                    return "Ημ/νία Δημοσίευσης";
                case "ProviderID":
                    return "ID Φορέα";
                case "ProviderName":
                    return "Επωνυμία Φορέα";
                case "Duration":
                    return "Διάρκεια (εβδομάδες)";
                case "Title":
                    return "Τίτλος";
                case "Description":
                    return "Περιγραφή";
                case "Country":
                    return "Χώρα";
                case "PrefectureID":
                    return "ID Περιφερειακής Ενότητας";
                case "Prefecture":
                    return "Περιφερειακή Ενότητα";
                case "CityID":
                    return "ID Καλλικρατικού Δήμου";
                case "City":
                    return "Καλλικρατικός Δήμος";
                case "StartDate":
                    return "Ημ/νία Έναρξης";
                case "EndDate":
                    return "Ημ/νία Λήξης";
                case "PositionType":
                    return "Είδος Θέσης";
                case "ContactPhone":
                    return "Τηλέφωνο Θέσης";
                case "Supervisor":
                    return "Ον/μο Επόπτη";
                case "SupervisorEmail":
                    return "E-mail Επόπτη";
                case "PositionGroupStatus":
                    return "Κατάσταση Group";
                case "PositionStatus":
                    return "Κατάσταση Θέσης";
                case "PreAssignedAt":
                    return "Ημ/νία Προδέσμευσης";
                case "PreAssignedByOfficeID":
                    return "ID Γραφείου";
                case "PreAssignedForInstitution":
                    return "Ίδρυμα Προδέσμευσης";
                case "PreAssignedForDepartment":
                    return "Τμήμα Προδέσμευσης";
                case "AssignedAt":
                    return "Ημ/νία Αντιστοίχισης";
                case "AssignedToStudentID":
                    return "ID Φοιτητή";
                case "AssignedToContactName":
                    return "Ον/μο Αντιστοιχισμένου Φοιτητή";
                case "AssignedToStudentNumber":
                    return "Α.Μ. Φοιτητή";
                case "AssignedToAcademicIDNumber":
                    return "12ψήφιος Ακαδημαϊκής Ταυτότητας";
                case "ImplementationStartDate":
                    return "Ημ/νία Έναρξης Εκτέλεσης";
                case "ImplementationEndDate":
                    return "Ημ/νία Λήξης Εκτέλεσης";
                case "FundingType":
                    return "Τρόπος Χρηματοδότησης";
                case "CompletedAt":
                    return "Ημ/νία Ολοκλήρωσης";
                case "CompletionComments":
                    return "Παρατηρήσεις Ολοκλήρωσης";
                case "PhysicalObjects":
                    return "Αντικείμενο Θέσης";
                case "PositionCreationType":
                    return "Είδος Δημιουργίας Θέσης";
                default:
                    return string.Empty;
            }
        }
    }
}