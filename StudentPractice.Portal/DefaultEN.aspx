<%@ Page Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true" CodeBehind="DefaultEN.aspx.cs"
    Inherits="StudentPractice.Portal.DefaultEN" Title="Αρχική Σελίδα" %>
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
                <a runat="server" href="<%$ Resources:PrimaryMenu, DefaultUrl %>" class="home">
                    <asp:Literal runat="server" Text="<%$ Resources:PrimaryMenu, HomePage %>" />
                </a>
            </li>
            <li>
                <a target="_blank" href="http://atlas.grnet.gr/FAQ.aspx" class="faq">
                    <asp:Literal runat="server" Text="<%$ Resources:PrimaryMenu, FAQ %>" />
                </a>
            </li>
            <li>
                <a target="_blank" href='<%= string.Format("{0}/RedirectFromPortal.ashx?id=3&language=1", ConfigurationManager.AppSettings["StudentPracticeMainUrl"]) %>' class="contact">
                    <asp:Literal runat="server" Text="<%$ Resources:PrimaryMenu, Contact %>" />
                </a>
            </li>
        </ul>
    </div>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">    
    <div style="float:right;">
        <my:LanguageBar ID="ucLanguageBar" runat="server" />
    </div>
    <h2>Welcome</h2>

    <div id="registration" class="column">

        <h3>Register</h3>
        <div class="inner">
            <p>
                <label>                    
                    To register on application, select the user group you belong to:
                </label>
            </p>           

            <div id="divForeign" runat="server" class="buttonwrapper">
                <asp:LinkButton ID="btnFrProvider" CssClass="squarebutton" runat="server" OnClick="btnFrProvider_Click">
                     <span>Register as Internship Host</span>
                </asp:LinkButton>
            </div>
        </div>
    </div>

    <div id="login-container" class="column">
        <h3>Login</h3>
        <div class="inner">
            <p>
                <label>
                    To log in to the application, select the user group you belong to:
                </label>
            </p>
            <div class="buttonwrapper login-btn-form">
                <a class="squarebutton" href="#">
                    <span>Internship Host</span>
                </a>
            </div>
            <div id="login-form" style="text-align: left">
                <asp:Login runat="server" ID="login" DestinationPageUrl="~/Default.aspx" LoginButtonText="Log in"
                    PasswordLabelText="Password:" PasswordRecoveryText="Forgot my password"
                    PasswordRecoveryUrl="~/Common/ForgotPassword.aspx" PasswordRequiredErrorMessage="Password is required"
                    RememberMeText="Remember me" TitleText="" UserNameLabelText="Username:" UserNameRequiredErrorMessage="Username is required"
                    FailureText="Wrong username or password." OnLoggingIn="login_LoggingIn" OnLoggedIn="login_LoggedIn">
                    <LayoutTemplate>
                        <div class="row">
                            <span class="label">
                                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" Text="Username:" />
                            </span>
                        </div>
                        <div class="row">
                            <span>
                                <asp:TextBox ID="UserName" runat="server" Columns="30" />
                            </span>
                        </div>
                        <div class="row">
                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" Display="Dynamic" ControlToValidate="UserName"
                                ErrorMessage="Username is required" ValidationGroup="login" Text="Username is required" />
                        </div>
                        <div class="row">
                            <span class="label">
                                <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" Text="Password:" />
                            </span>
                        </div>
                        <div class="row">
                            <span>
                                <asp:TextBox ID="Password" runat="server" TextMode="Password" Columns="30" />
                            </span>
                        </div>
                        <div class="row">
                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" Display="Dynamic" ControlToValidate="Password"
                                ErrorMessage="Password is required" ValidationGroup="login" Text="Password is required" />
                        </div>
                        <div class="row">
                            <span class="label error">
                                <asp:Literal ID="FailureText" runat="server" EnableViewState="False" />
                            </span>
                        </div>
                        <div class="row">
                            <span class="button">
                                <asp:Button ID="LoginButton" runat="server" CommandName="Login" CssClass="btn" Text="Log in" ValidationGroup="login" type="button" />
                            </span>
                            <span class="check">
                                <asp:CheckBox ID="RememberMe" runat="server" Text="Remember Me" />
                            </span>
                        </div>
                        <div class="row">
                            <span class="label">
                                <asp:HyperLink ID="PasswordRecoveryLink" runat="server" NavigateUrl="~/Common/ForgotPassword.aspx" Text="Forgotten password" />
                            </span>
                        </div>
                    </LayoutTemplate>
                </asp:Login>
            </div>
            <p>
                <%--If you encounter a problem, you can contact the Helpdesk by phone at <em>+302152157860</em>, (Monday-Friday, 09:00 to 17:00 (GMT +2.0))--%>
                If you encounter a problem, you can contact the <a href="http://atlas.grnet.gr/ContactEn.aspx">Helpdesk</a>
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
