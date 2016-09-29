<%@ Page Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs"
    Inherits="StudentPractice.Portal.Default" Title="Αρχική Σελίδα" %>

<%@ Register Src="~/UserControls/GenericControls/LanguageBar.ascx" TagName="LanguageBar" TagPrefix="my" %>

<asp:Content ContentPlaceHolderID="title" runat="server">
    Αρχική
</asp:Content>

<asp:Content ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="cpHelpBar" runat="server">
    <div id="primary-menu">
        <ul>
            <li>
                <a href="/Default.aspx" class="home">
                    <asp:Literal runat="server" Text="<%$ Resources:PrimaryMenu, HomePage %>" />
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

<asp:Content ContentPlaceHolderID="cphMain" runat="server">

    <div style="float: right;">
        <my:LanguageBar ID="ucLanguageBar" runat="server" Visible="false" />
    </div>
    <h2>Καλώς ήλθατε</h2>
    <div id="registration" class="column">
        <h3>Εγγραφή</h3>
        <div class="inner">
            <p>
                <label>
                    Για να εγγραφείτε στην εφαρμογή, επιλέξτε την κατηγορία χρήστη που ανήκετε:
                </label>
            </p>
            <div class="buttonwrapper">
                <asp:LinkButton ID="btnGrProvider" CssClass="squarebutton" runat="server" OnClick="btnGrProvider_Click">
                    <span>Φορέας Υποδοχής (από Ελλάδα)</span>
                </asp:LinkButton>

            </div>
            <div id="divCyprus" runat="server" class="buttonwrapper">
                <asp:LinkButton ID="btnCyProvider" CssClass="squarebutton" runat="server" OnClick="btnCyProvider_Click">
                    <span>Φορέας Υποδοχής (από Κύπρο)</span>
                </asp:LinkButton>
            </div>
            <div id="divForeign" runat="server" class="buttonwrapper">
                <asp:LinkButton ID="btnFrProvider" CssClass="squarebutton" runat="server" OnClick="btnFrProvider_Click">
                    <span>Φορέας Yποδοχής (από Εξωτερικό)</span>
                </asp:LinkButton>
            </div>

            <div class="buttonwrapper">
                <a class="squarebutton" runat="server" href="~/Common/OfficeRegistration.aspx">
                    <span>Γραφείο Πρακτικής</span>
                </a>
            </div>
            <p>
                Σημείωση: Οι <em>Προπτυχιακοί Φοιτητές</em> μπορούν να συνδεθούν κατευθείαν στην
                εφαρμογή χρησιμοποιώντας τα στοιχεία σύνδεσης από το Ίδρυμα στο οποίο ανήκουν επιλέγοντας
                "Φοιτητής" στο δεξί μέρος της σελίδας
            </p>
        </div>
    </div>

    <div id="login-container" class="column">
        <h3>Είσοδος</h3>
        <div class="inner">
            <p>
                <label>
                    Για να συνδεθείτε στην εφαρμογή, επιλέξτε την κατηγορία χρήστη που ανήκετε:
                </label>
            </p>
            <div>
                <a class="squarebutton" runat="server" href="~/Shib/Default.aspx">
                    <span>Προπτυχιακός Φοιτητής</span>
                </a>
            </div>
            <div class="buttonwrapper login-btn-form">
                <a class="squarebutton" href="#">
                    <span>Φορέας Υποδοχής</span>
                </a>
            </div>
            <div class="buttonwrapper login-btn-form">
                <a class="squarebutton" href="#">
                    <span>Γραφείο Πρακτικής</span>
                </a>
            </div>
            <div id="login-form" style="text-align: left">
                <asp:Login runat="server" ID="login" DestinationPageUrl="~/Default.aspx" LoginButtonText="Σύνδεση"
                    PasswordLabelText="Κωδικός πρόσβασης:" PasswordRecoveryText="Ξέχασα τον κωδικό μου"
                    PasswordRecoveryUrl="~/Common/ForgotPassword.aspx" PasswordRequiredErrorMessage="Ο κωδικός πρόσβασης είναι υποχρεωτικός"
                    RememberMeText="Θυμήσου με" TitleText="" UserNameLabelText="Όνομα χρήστη:" UserNameRequiredErrorMessage="Το όνομα χρήστη είναι υποχρεωτικό"
                    FailureText="Λάθος όνομα χρήστη ή κωδικός πρόσβασης." OnLoggingIn="login_LoggingIn" OnLoggedIn="login_LoggedIn">
                    <LayoutTemplate>
                        <div class="row">
                            <span class="label">
                                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" Text="Όνομα χρήστη:" />
                            </span>
                        </div>
                        <div class="row">
                            <span>
                                <asp:TextBox ID="UserName" runat="server" Columns="30" />
                            </span>
                        </div>
                        <div class="row">
                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" Display="Dynamic" ControlToValidate="UserName"
                                ErrorMessage="Το όνομα χρήστη είναι υποχρεωτικό" ValidationGroup="login" Text="Το όνομα χρήστη είναι υποχρεωτικό" />
                        </div>
                        <div class="row">
                            <span class="label">
                                <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" Text="Κωδικός πρόσβασης:" />
                            </span>
                        </div>
                        <div class="row">
                            <span>
                                <asp:TextBox ID="Password" runat="server" TextMode="Password" Columns="30" />
                            </span>
                        </div>
                        <div class="row">
                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" Display="Dynamic" ControlToValidate="Password"
                                ErrorMessage="Ο κωδικός πρόσβασης είναι υποχρεωτικός" ValidationGroup="login" Text="Ο κωδικός πρόσβασης είναι υποχρεωτικός" />
                        </div>
                        <div class="row">
                            <span class="label error">
                                <asp:Literal ID="FailureText" runat="server" EnableViewState="False" />
                            </span>
                        </div>
                        <div class="row">
                            <span class="button">
                                <asp:Button ID="LoginButton" runat="server" CommandName="Login" CssClass="btn" Text="Σύνδεση" ValidationGroup="login" />
                            </span>
                            <span class="check">
                                <asp:CheckBox ID="RememberMe" runat="server" Text="Θυμήσου με" />
                            </span>
                        </div>
                        <div class="row">
                            <span class="label">
                                <asp:HyperLink ID="PasswordRecoveryLink" runat="server" NavigateUrl="~/Common/ForgotPassword.aspx" Text="Υπενθύμιση κωδικού πρόσβασης" />
                            </span>
                        </div>
                    </LayoutTemplate>
                </asp:Login>
            </div>
            <p>
                <%--Εάν αντιμετωπίζετε πρόβλημα σύνδεσης με το λογαριασμό σας, μπορείτε να επικοινωνήσετε
                με το Γραφείο Αρωγής Χρηστών στο τηλέφωνο <em>215 215 7860</em>--%>
                Εάν αντιμετωπίζετε πρόβλημα σύνδεσης με το λογαριασμό σας, μπορείτε να επικοινωνήσετε με το <a href="http://atlas.grnet.gr/Contact.aspx">Γραφείο Αρωγής Χρηστών</a>
            </p>
        </div>
    </div>

    <script type="text/javascript">

        $(function () {
            $('.login-btn-form').click(function () {
                $('#login-form').slideToggle();
                return false;
            });

            <% if (IsPostBack)
               { %>
            $('#login-form').slideDown();
            <% } %>


        });

    </script>
</asp:Content>
