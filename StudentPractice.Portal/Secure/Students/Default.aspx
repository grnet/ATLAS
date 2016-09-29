<%@ Page Language="C#" MasterPageFile="~/Secure/SecurePages.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="StudentPractice.Portal.Secure.Students.Default"
    Title="Κεντρική Σελίδα" %>

<%@ Import Namespace="StudentPractice.BusinessModel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    Μέσα από την εφαρμογή του Συστήματος Κεντρικής Υποστήριξης της Πρακτικής Άσκησης
    Φοιτητών ΑΕΙ μπορείτε να εκτελέσετε τις εξής λειτουργίες:
    <ul>
        <li class='firstListItem'>
            Να επεξεργαστείτε τα στοιχεία σας μέσα από την καρτέλα 
            <asp:HyperLink runat="server" NavigateUrl="~/Secure/Students/StudentDetails.aspx" Style="font-weight: bold; color: Blue;">
                Στοιχεία Φοιτητή
            </asp:HyperLink>

        </li>
        <li class='firstListItem'>
            Να αναζητήσετε θέσεις Πρακτικής Άσκησης μέσα από την καρτέλα 
            <asp:HyperLink runat="server" NavigateUrl="~/Secure/Students/SearchPositions.aspx" Style="font-weight: bold; color: Blue;">
                Αναζήτηση Θέσεων
            </asp:HyperLink>

        </li>
        <li class='firstListItem'>
            Να δείτε τις θέσεις Πρακτικής Άσκησης που σας έχουν ανατεθεί και να συμπληρώσετε το ερωτηματολόγιο αξιολόγησης για το συνεργαζόμενο Γραφείο Πρακτικής Άσκησης ή το Φορέα Υποδοχής μέσα από την καρτέλα  
            <asp:HyperLink runat="server" NavigateUrl="~/Secure/Students/AssignedPositions.aspx" Style="font-weight: bold; color: Blue;">
                Θέσεις Πρακτικής Άσκησης
            </asp:HyperLink>
        </li>
        <li class='firstListItem'>
            Να επικοινωνήσετε με το Γραφείο Αρωγής της δράσης μέσα από την καρτέλα 
            <asp:HyperLink runat="server" NavigateUrl="~/Secure/Common/HelpdeskContact.aspx" Style="font-weight: bold; color: Blue;">
                Επικοινωνία με Γραφείο Αρωγής
            </asp:HyperLink>
        </li>
    </ul>

    <script type="text/javascript">
        function showEvaluationPopup() {
            <%= string.Format("popUp.show('/Secure/Common/Questionnaire.aspx?qType={0}','Αξιολόγηση', null, 800, 610);", enQuestionnaireType.StudentForAtlas.GetValue()) %>
        }
    </script>
</asp:Content>
