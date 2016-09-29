using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;

namespace StudentPractice.Portal.UserControls.Exporters
{
    public partial class InternshipPositionGroupsExporter : BaseHtmlExporter
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
                case "PositionCreationType":
                    if (value != null)
                    {
                        value = ((enPositionCreationType)value).GetLabel();
                    }
                    break;
                case "CreatedAtDateOnly":
                case "FirstPublishedAt":
                case "StartDate":
                case "EndDate":                    
                    var dt = (DateTime?)value;
                    value = dt.HasValue 
                            ? dt.Value.ToString("dd/MM/yyyy") 
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
                case "TotalPositions":
                    return "Αριθμός Θέσεων";
                case "PreAssignedPositions":
                    return "Δεσμευμένες Θέσεις";
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
                case "PositionGroupStatus":
                    return "Κατάσταση Group";
                case "PhysicalObjects":
                    return "Αντικείμενο Θέσης";
                case "Supervisor":
                    return "Ον/μο Επόπτη";
                case "SupervisorEmail":
                    return "E-mail Επόπτη";
                case "AcademicIDs":
                    return "ID Τμήματος";
                case "Institution":
                    return "Ίδρυμα";
                case "Departments":
                    return "Τμήμα";
                case "PositionCreationType":
                    return "Είδος Δημιουργίας Θέσης";
                default:
                    return string.Empty;
            }
        }        
    }
}