<%@ Page Language="C#" MasterPageFile="~/Secure/SecurePages.Master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="StudentPractice.Portal.Secure.InternshipOffices.Default"
    Title="Κεντρική Σελίδα" %>

<%@ Import Namespace="StudentPractice.BusinessModel" %>

<asp:Content ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <asp:MultiView ID="mvDefault" runat="server" ActiveViewIndex="0">
        <asp:View ID="vEmpty" runat="server" />
        <asp:View ID="vMasterOffice" runat="server">
            Μέσα από την εφαρμογή του Γραφείου Πρακτικής Άσκησης μπορείτε να εκτελέσετε τις
            εξής λειτουργίες:
            <ul>
                <li class='firstListItem'>Να επεξεργαστείτε τα στοιχεία του Γραφείου μέσα από την καρτέλα
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Secure/InternshipOffices/OfficeDetails.aspx"
                        Style="font-weight: bold; color: Blue;">Στοιχεία Γραφείου</asp:HyperLink>
                </li>
                <li class='firstListItem'>Να αναζητήσετε θέσεις μέσα από την καρτέλα
                    <asp:HyperLink runat="server" NavigateUrl="~/Secure/InternshipOffices/SearchPositions.aspx"
                        Style="font-weight: bold; color: Blue;">Αναζήτηση Θέσεων</asp:HyperLink></li>
                <li class='firstListItem'>Να διαχειριστείτε τις θέσεις που έχετε δεσμεύσει μέσα από
                    την καρτέλα
                    <asp:HyperLink runat="server" NavigateUrl="~/Secure/InternshipOffices/SelectedPositions.aspx"
                        Style="font-weight: bold; color: Blue;">Επιλεγμένες Θέσεις</asp:HyperLink></li>
                <li class='firstListItem'>Να προβείτε σε αξιολόγηση των συνεργαζόμενων Φορέων Υποδοχής Πρακτικής Άσκησης και των Φοιτητών μέσα από την καρτέλα 
                    <asp:HyperLink runat="server" NavigateUrl="~/Secure/InternshipOffices/Evaluation.aspx"
                        Style="font-weight: bold; color: Blue;">Αξιολόγηση</asp:HyperLink></li>
                <li class='firstListItem'>Να ορίσετε νέους χρήστες του Γραφείου Πρακτικής μέσα από την
                    καρτέλα
                    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Secure/InternshipOffices/OfficeUsers.aspx"
                        Style="font-weight: bold; color: Blue;">Χρήστες Γραφείου</asp:HyperLink></li>
                <li class='firstListItem'>Να επικοινωνήσετε με το Γραφείο Αρωγής της δράσης μέσα από
                    την καρτέλα
                    <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/Secure/Common/HelpdeskContact.aspx"
                        Style="font-weight: bold; color: Blue;">Επικοινωνία με Γραφείο Αρωγής</asp:HyperLink></li>
            </ul>
            <br />
            <br />
            <a id="btnPrintCertification" runat="server" href="/Secure/InternshipOffices/GenerateOfficeCertificationPDF.ashx"
                class="icon-btn bg-print">Εκτύπωση Βεβαίωσης Συμμετοχής</a>
            <br />
            <br />
            <asp:PlaceHolder ID="phVideo" runat="server">
                <div style="margin: 5px auto 5px auto; width: 800px;">
                    <h2>Βίντεο - Οδηγός χρήσης εφαρμογής</h2>
                    <br />
                    <iframe width="800" height="500" src="https://www.youtube.com/embed/8WNZiQMqWKE" frameborder="0" allowfullscreen></iframe>
                </div>
            </asp:PlaceHolder>
        </asp:View>
        <asp:View ID="vOfficeUser" runat="server">
            Μέσα από την εφαρμογή του Γραφείου Πρακτικής Άσκησης μπορείτε να εκτελέσετε τις
            εξής λειτουργίες:
            <ul>
                <li class='firstListItem'>Να αναζητήσετε θέσεις μέσα από την καρτέλα
                    <asp:HyperLink runat="server" NavigateUrl="~/Secure/InternshipOffices/SearchPositions.aspx"
                        Style="font-weight: bold; color: Blue;">Αναζήτηση Θέσεων</asp:HyperLink></li>
                <li class='firstListItem'>Να διαχειριστείτε τις θέσεις που έχετε δεσμεύσει μέσα από
                    την καρτέλα
                    <asp:HyperLink runat="server" NavigateUrl="~/Secure/InternshipOffices/SelectedPositions.aspx"
                        Style="font-weight: bold; color: Blue;">Επιλεγμένες Θέσεις</asp:HyperLink></li>
                <li class='firstListItem'>Να προβείτε σε αξιολόγηση των συνεργαζόμενων Φορέων Υποδοχής Πρακτικής Άσκησης και των Φοιτητών μέσα από την καρτέλα 
                    <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/Secure/InternshipOffices/Evaluation.aspx"
                        Style="font-weight: bold; color: Blue;">Αξιολόγηση</asp:HyperLink></li>
                <li class='firstListItem'>Να επικοινωνήσετε με το Γραφείο Αρωγής της δράσης μέσα από
                    την καρτέλα
                    <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="~/Secure/Common/HelpdeskContact.aspx"
                        Style="font-weight: bold; color: Blue;">Επικοινωνία με Γραφείο Αρωγής</asp:HyperLink></li>
            </ul>
            <br />
            <br />
            <asp:PlaceHolder ID="phVideoUser" runat="server">
                <div style="margin: 5px auto 5px auto; width: 800px;">
                    <h2>Βίντεο - Οδηγός χρήσης εφαρμογής</h2>
                    <br />
                    <iframe width="800" height="500" src="https://www.youtube.com/embed/8WNZiQMqWKE" frameborder="0" allowfullscreen></iframe>
                </div>
            </asp:PlaceHolder>
        </asp:View>
    </asp:MultiView>
    
    <script type="text/javascript">
        function showEvaluationPopup() {
            <%= string.Format("popUp.show('/Secure/Common/Questionnaire.aspx?qType={0}','Αξιολόγηση', null, 800, 610);", enQuestionnaireType.OfficeForAtlas.GetValue()) %>
        }
    </script>
</asp:Content>
