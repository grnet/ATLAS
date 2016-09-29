<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginBar.ascx.cs" Inherits="StudentPractice.Portal.UserControls.GenericControls.LoginBar" %>
<%@ Register TagPrefix="my" TagName="ChangePassword" Src="~/UserControls/GenericControls/ChangePassword.ascx" %>

<asp:LoginView ID="loginView" runat="server">

    <AnonymousTemplate>
        <asp:Literal runat="server" Text="<%$ Resources:LoginBar, NotLogged %>" />
        <asp:LoginStatus ID="loginStatus" runat="server" />
    </AnonymousTemplate>

    <LoggedInTemplate>
        <table>
            <tr>
                <td style="padding-right: 10px">
                    <asp:LoginName ID="loginName" runat="server" FormatString="<%$ Resources:LoginBar, LoginUsername %>" />
                    <asp:Literal ID="txtUserDetails" runat="server" />
                </td>
                <td>
                    <asp:LoginStatus ID="loginStatus" runat="server" LogoutText="<%$ Resources:LoginBar, Logout %>" LogoutAction="RedirectToLoginPage" CssClass="icon-btn bg-logout" 
                        OnLoggingOut="LoginStatus1_OnLoggingOut" OnLoggedOut="LoginStatus1_LoggedOut"/>
                    <asp:LinkButton ID="lnkChangePassword" runat="server" Text="<%$ Resources:LoginBar, PasswordChange %>" CssClass="icon-btn bg-passwordEdit" />

                    <asp:Panel ID="panChangePassword" runat="server" Style="display: none; border: 1px solid #444; padding: 5px; z-index: 1000; position: absolute;" CssClass="modalPopup-cp">
                        <asp:UpdatePanel ID="updChangePassword" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:MultiView ID="mv" runat="server" ActiveViewIndex="0">

                                    <asp:View ID="vNormal" runat="server">
                                        <my:ChangePassword runat="server" ID="cp" ValidationGroup="cp" />
                                        <asp:Button ID="btnChangePassword" runat="server" Text="<%$ Resources:LoginBar, ChangePassword %>" CssClass="button" OnClick="btnChangePassword_Click" ValidationGroup="cp" />
                                        <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:GlobalProvider, Global_Cancel %>" CausesValidation="false" CssClass="button" OnClick="btnCancel_Click" />
                                        <p>
                                            <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red" />
                                        </p>
                                    </asp:View>

                                    <asp:View ID="vSuccess" runat="server">
                                        <div style="text-align: center">
                                            <em>
                                                <asp:Literal runat="server" Text="<%$ Resources:LoginBar, ChangeSuccess %>" />
                                            </em>
                                            <br />
                                            <br />
                                            <asp:Button ID="btnSuccess" runat="server" Text="<%$ Resources:GlobalProvider, Global_Close %>" CssClass="button" OnClick="btnSuccess_Click" />
                                        </div>
                                    </asp:View>

                                </asp:MultiView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </td>
            </tr>
        </table>

        <act:ModalPopupExtender ID="mpeChangePassword" runat="server" TargetControlID="lnkChangePassword" PopupControlID="panChangePassword" BackgroundCssClass="modalBackground" />

    </LoggedInTemplate>
</asp:LoginView>
