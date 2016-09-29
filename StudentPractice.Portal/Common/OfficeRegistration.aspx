<%@ Page Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true" CodeBehind="OfficeRegistration.aspx.cs"
    Inherits="StudentPractice.Portal.Common.OfficeRegistration" Title="Δημιουργία Λογαριασμού" %>

<%@ Register Src="~/UserControls/InternshipOfficeControls/InputControls/OfficeRegisterUserInput.ascx"
    TagName="RegisterUserInput" TagPrefix="my" %>
<%@ Register Src="~/UserControls/InternshipOfficeControls/InputControls/OfficeInput.ascx"
    TagName="OfficeInput" TagPrefix="my" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Δημιουργία Χρήστη
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphHead" runat="server">
    <link href="/_css/jquery-ui.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpHelpBar" runat="server">
    <div id="primary-menu">
        <ul>
            <li>
                <a runat="server" href="<%$ Resources:PrimaryMenu, DefaultUrl %>" class="home">
                    <asp:Literal runat="server" Text="<%$ Resources:PrimaryMenu, HomePage %>" />
                </a>
            </li>
            <li>
                <a target="_blank" href="http://atlas.grnet.gr/Files/Manual_GPA_Reg.pdf" class="help">
                    <asp:Literal runat="server" Text="<%$ Resources:PrimaryMenu, Manual %>" />
                </a>
            </li>
            <li>
                <a target="_blank" href="http://atlas.grnet.gr/FAQ.aspx" class="faq">
                    <asp:Literal runat="server" Text="<%$ Resources:PrimaryMenu, FAQ %>" />
                </a>
            </li>
            <li>
                <a target="_blank" href='<%= string.Format("{0}/RedirectFromPortal.ashx?id=3&language=0", ConfigurationManager.AppSettings["StudentPracticeMainUrl"]) %>' class="contact">
                    <asp:Literal runat="server" Text="<%$ Resources:PrimaryMenu, Contact %>" />
                </a>
            </li>
        </ul>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
    <h2>Δημιουργία Χρήστη</h2>
    <asp:MultiView ID="mvRegistration" runat="server" ActiveViewIndex="0">
        <asp:View ID="vRegister" runat="server">
            <asp:ValidationSummary ID="vdSummary" runat="server" CssClass="validation-summary"
                ValidationGroup="vdRegistration" HeaderText="Υπάρχει σφάλμα ή έλλειψη συμπλήρωσης ενός από τα πεδία της φόρμας. Παρακαλώ κάντε τις απαραίτητες διορθώσεις." />
            <asp:Label ID="lblErrors" runat="server" CssClass="error" />
            <my:RegisterUserInput ID="ucRegisterUserInput" runat="server" ValidationGroup="vdRegistration" />
            <br />
            <my:OfficeInput ID="ucOfficeInput" runat="server" ValidationGroup="vdRegistration" />
            <br />
            <lc:BotShield ID="bsPublisher" runat="server" ValidationGroup="vdRegistration" ClientIDMode="Static" />
            <br />
            <br />
            <div style="clear: both; text-align: left">
                <asp:LinkButton ID="btnCreate" runat="server" Text="Δημιουργία Λογαριασμού" CssClass="btn"
                    ValidationGroup="vdRegistration" OnClick="btnCreate_Click" ClientIDMode="Static" />
            </div>
        </asp:View>
        <asp:View ID="vComplete" runat="server">
            <div class="reminder">
                <asp:Label ID="lblCompletionMessage" runat="server" />
            </div>
        </asp:View>
        <asp:View ID="vNotAllowed" runat="server">
            <div class="reminder">
                Το σύστημα "Άτλας" θα είναι σύντομα διαθέσιμο για τα Γραφεία Πρακτικής Άσκησης.<br />
                Για την ακριβή ημερομηνία μπορείτε να ενημερώνεστε από το <a href="http://atlas.grnet.gr"
                    style="color: Blue">δικτυακό τόπο</a> της δράσης.
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
