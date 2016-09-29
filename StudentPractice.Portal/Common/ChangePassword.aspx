<%@ Page Language="C#" MasterPageFile="~/Portal.master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs"
    Inherits="StudentPractice.Portal.Common.ChangePassword" Title="Αλλαγή Κωδικού Πρόσβασης" %>
<%@ Register Src="~/UserControls/GenericControls/LanguageBar.ascx" TagName="LanguageBar" TagPrefix="my" %>

<asp:Content ContentPlaceHolderID="title" runat="server">
    Αλλαγή Κωδικού Πρόσβασης
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
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div style="float:right;">
        <my:LanguageBar ID="ucLanguageBar" runat="server" Visible="false" DisableValidation="true" />
    </div>
    <h2>
        <asp:Literal runat="server" Text="<%$ Resources:CommonPages, ChangePassword_Header %>" />
    </h2>
    <div class="sub-description">
        <p>
            <asp:Literal runat="server" Text="<%$ Resources:CommonPages, ChangePassword_Message1 %>" />
        </p>
        <p>
            <asp:Literal runat="server" Text="<%$ Resources:CommonPages, ChangePassword_Message2 %>" />
        </p>
    </div>
    <br />
    <asp:MultiView ID="mvChangePassword" runat="server" ActiveViewIndex="0">
        <asp:View ID="vChangePassword" runat="server">
            <table width="100%" class="dv">
                <tr>
                    <th colspan="2" class="header">
                        <asp:Literal runat="server" Text="<%$ Resources:CommonPages, ChangePassword_Header %>" />
                    </th>
                </tr>
                <tr>
                    <th style="width: 30%">
                        <asp:Literal runat="server" Text="<%$ Resources:CommonPages, ChangePassword_Old %>" />
                    </th>
                    <td align="left">
                        <asp:TextBox ID="txtOldPassword" runat="server" TextMode="Password" Width="30%" onkeyup="Imis.Lib.NoGreekCharacters(this,false)" />

                        <imis:CapsWarning ID="CapsWarning2" runat="server" TextBoxControlId="txtOldPassword"
                            CssClass="capsLockWarning" Text="<%$ Resources:GlobalProvider, CapsWarning %>">
                        </imis:CapsWarning>

                        <asp:RequiredFieldValidator ID="rfvOldPassword" Display="Dynamic" runat="server"
                            ControlToValidate="txtOldPassword" ErrorMessage="<%$ Resources:CommonPages, ChangePassword_OldRequired %>">
                            <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:CommonPages, ChangePassword_OldRequired %>" />  
                        </asp:RequiredFieldValidator>

                    </td>
                </tr>
                <tr>
                    <th style="width: 30%">
                        <asp:Literal runat="server" Text="<%$ Resources:CommonPages, ChangePassword_New %>" />
                    </th>
                    <td align="left">
                        <asp:TextBox ID="txtPassword1" runat="server" TextMode="Password" Width="30%" onkeyup="Imis.Lib.NoGreekCharacters(this,false)" />

                        <imis:CapsWarning ID="CapsWarning1" runat="server" TextBoxControlId="txtPassword1"
                            CssClass="capsLockWarning" Text="<%$ Resources:GlobalProvider, CapsWarning %>">
                        </imis:CapsWarning>

                        <asp:RequiredFieldValidator ID="rfvPassword1" Display="Dynamic" runat="server" ControlToValidate="txtPassword1"
                            ErrorMessage="<%$ Resources:CommonPages, ChangePassword_NewRequired %>">
                            <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:CommonPages, ChangePassword_NewRequired %>" />  
                        </asp:RequiredFieldValidator>

                        <asp:RegularExpressionValidator ID="revPassword" runat="server" ControlToValidate="txtPassword1"
                            Display="Dynamic" ValidationExpression="^(.){7,}$" ErrorMessage="<%$ Resources:CommonPages, ChangePassword_NewRegex %>">
                            <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:CommonPages, ChangePassword_NewRegex %>" />  
                        </asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <th style="width: 30%">
                        <asp:Literal runat="server" Text="<%$ Resources:CommonPages, ChangePassword_Conf %>" />
                    </th>
                    <td align="left">
                        <asp:TextBox ID="txtPassword2" runat="server" TextMode="Password" Width="30%" onkeyup="Imis.Lib.NoGreekCharacters(this,false)" />

                        <imis:CapsWarning ID="CapsWarning3" runat="server" TextBoxControlId="txtPassword2"
                            CssClass="capsLockWarning" Text="<%$ Resources:GlobalProvider, CapsWarning %>">
                        </imis:CapsWarning>

                        <asp:RequiredFieldValidator ID="rfvPassword2" Display="Dynamic" runat="server" ControlToValidate="txtPassword2"
                            ErrorMessage="<%$ Resources:CommonPages, ChangePassword_ConfRequired %>">
                            <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:CommonPages, ChangePassword_ConfRequired %>" />  
                        </asp:RequiredFieldValidator>

                        <asp:CompareValidator ID="cvPassword2" ControlToCompare="txtPassword1" ControlToValidate="txtPassword2"
                            runat="server" Display="Dynamic" Operator="Equal" Type="String" ValueToCompare="Text"
                            ErrorMessage="<%$ Resources:CommonPages, ChangePassword_Match %>">
                            <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:CommonPages, ChangePassword_Match %>" />  
                        </asp:CompareValidator>
                    </td>
                </tr>
            </table>
            <br />
            <lc:BotShield ID="bsPublisher" runat="server" ClientIDMode="Static" />
            <br />
            <asp:ValidationSummary ID="vsStudentValidation" CssClass="validation-summary" runat="server"
                HeaderText="<%$ Resources:CommonPages, ChangePassword_Validation %>" />

            <asp:LinkButton ID="btnSubmit" runat="server" CssClass="icon-btn bg-accept" Text="<%$ Resources:CommonPages, ChangePassword_Btn %>" OnClick="btnSubmit_Click" />
            <br />
            <br />
            <asp:Label runat="server" ID="lblInfo" Font-Bold="true" ForeColor="Red"></asp:Label>
        </asp:View>
        <asp:View ID="vPasswordChanged" runat="server">
            <span style="font-weight: bold; color: green">
                <asp:Literal runat="server" Text="<%$ Resources:CommonPages, ChangePassword_Success %>" />
            </span>
            <a runat="server" style="font-weight: bold; color: Blue" href="<%$ Resources:PrimaryMenu, DefaultUrl %>">
                <asp:Literal runat="server" Text="<%$ Resources:CommonPages, ChangePassword_Here %>" />
            </a>
        </asp:View>
    </asp:MultiView>
</asp:Content>
