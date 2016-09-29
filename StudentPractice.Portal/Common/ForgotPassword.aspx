<%@ Page Language="C#" MasterPageFile="~/Portal.master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs"
    Inherits="StudentPractice.Portal.Common.ForgotPassword" Title="Υπενθύμιση Κωδικού" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Υπενθύμιση Κωδικού Πρόσβασης
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
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <h2>
        <asp:Literal runat="server" Text="<%$ Resources:CommonPages, ForgotPassword_Header %>" />
    </h2>

    <div class="sub-description">
        <p>
            <asp:Literal runat="server" Text="<%$ Resources:CommonPages, ForgotPassword_Message1 %>" />
        </p>
        <p>
            <em>
                <asp:Literal runat="server" Text="<%$ Resources:CommonPages, ForgotPassword_Message2 %>" />
            </em>
            <asp:Literal runat="server" Text="<%$ Resources:CommonPages, ForgotPassword_Message3 %>" />
        </p>
    </div>

    <asp:ValidationSummary ID="vsStudentValidation" runat="server" CssClass="validation-summary"
        HeaderText="<%$ Resources:CommonPages, ForgotPassword_Validation %>" />

    <table style="width: 400px" class="dv">
        <tr>
            <th colspan="2" class="header">&raquo; 
                <asp:Literal runat="server" Text="<%$ Resources:CommonPages, ForgotPassword_User %>" />
            </th>
        </tr>
        <tr>
            <th style="width: 36%">E-mail:
            </th>
            <td>
                <asp:TextBox ID="txtEmail" runat="server" MaxLength="50" Width="85%" />

                <asp:RequiredFieldValidator ID="rfvEmail" Display="Dynamic" runat="server" ControlToValidate="txtEmail"
                    ErrorMessage="<%$ Resources:CommonPages, ForgotPassword_EmailRequired %>">
                    <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:CommonPages, ForgotPassword_EmailRequired %>" />
                </asp:RequiredFieldValidator>

                <asp:RegularExpressionValidator ID="revEmail" Display="Dynamic" ControlToValidate="txtEmail"
                    runat="server" ValidationExpression="^([a-zA-Z0-9_\-])+(\.([a-zA-Z0-9_\-])+)*@((\[(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5]))\]))|((([a-zA-Z0-9])+(([\-])+([a-zA-Z0-9])+)*\.)+([a-zA-Z])+(([\-])+([a-zA-Z0-9])+)*))$"
                    ErrorMessage="<%$ Resources:CommonPages, ForgotPassword_EmailRegex %>">
                    <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:CommonPages, ForgotPassword_EmailRegex %>" />
                </asp:RegularExpressionValidator>
            </td>
        </tr>
    </table>

    <br />
    <lc:BotShield ID="bsPublisher" runat="server" ClientIDMode="Static" />

    <br />
    <asp:LinkButton ID="btnSend" runat="server" CssClass="icon-btn bg-email" Text="<%$ Resources:CommonPages, ForgotPassword_Btn %>"
        OnClick="btnSend_Click" OnClientClick="javascript:return ValidatePage();" />

    <br />
    <br />
    <asp:Label runat="server" ID="lblInfo" Font-Bold="true" ForeColor="Red"></asp:Label>
</asp:Content>
