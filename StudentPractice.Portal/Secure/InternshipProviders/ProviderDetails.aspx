<%@ Page Language="C#" MasterPageFile="~/Secure/SecurePages.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.InternshipProviders.ProviderDetails"
    Title="Διαχείριση Στοιχείων Γραφείου" CodeBehind="ProviderDetails.aspx.cs" %>

<%@ Register TagPrefix="my" TagName="FlashMessage" Src="~/UserControls/GenericControls/FlashMessage.ascx" %>
<%@ Register TagPrefix="my" TagName="ProviderInput" Src="~/UserControls/InternshipProviderControls/InputControls/ProviderInput.ascx" %>
<%@ Register TagPrefix="my" TagName="ProviderUserInput" Src="~/UserControls/InternshipProviderControls/InputControls/ProviderUserInput.ascx" %>

<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <script type="text/javascript">
        function validate(group) {
            return Page_ClientValidate(group);
        }
    </script>
    <style type="text/css">
        #email {
            width: 30.667em;
        }

        #mobilePhone {
            width: 320px;
        }
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <script src="/_js/UserContactDetails.js" type="text/javascript"></script>
    <my:FlashMessage ID="fm" runat="server" CssClass="fade" />

    <dx:ASPxPageControl ID="dxTabs" runat="server" Width="100%">
        <TabPages>
            <dx:TabPage Text="<%$ Resources:ProviderDetails, TabProvider %>" Name="tabProvider">
                <ContentCollection>
                    <dx:ContentControl>

                        <asp:MultiView ID="mvProvider" runat="server">
                            <asp:View ID="vMasterAccount" runat="server">
                                <div id="divAccountVerified" runat="server" class="reminder">
                                    <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:ProviderDetails, AccountVerified %>" />
                                </div>
                                <asp:Label ID="lblErrors" runat="server" Font-Bold="true" ForeColor="Red" Visible="false" />
                                <asp:ValidationSummary ID="vdProvider" runat="server" ValidationGroup="vgProvider" />
                                <br />
                                <my:ProviderInput ID="ucProviderInput" runat="server" ValidationGroup="vgProvider" />
                                <div style="clear: both; text-align: left; margin-top: 30px">
                                    <asp:LinkButton ID="btnUpdateProvider" runat="server" Text="<%$ Resources:ProviderDetails, ChangeDetails %>"
                                        CssClass="icon-btn bg-accept" ValidationGroup="vgProvider" OnClick="btnUpdateProvider_Click" />
                                </div>
                            </asp:View>
                            <asp:View ID="vProviderUser" runat="server">
                                <div id="divNote" runat="server" class="reminder" style="text-align: left;">
                                    <asp:Label ID="lblMasterAccountDetails" runat="server" Font-Bold="true" />
                                </div>
                                <br />
                                <my:ProviderUserInput ID="ucProviderUserInput" runat="server" ReadOnly="true" />
                            </asp:View>
                        </asp:MultiView>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>

            <dx:TabPage Text="<%$ Resources:ProviderDetails, TabUser %>" Name="tabUser">
                <ContentCollection>
                    <dx:ContentControl>
                        <table style="width: 100%" class="dv">
                            <tr>
                                <th colspan="2" class="header">&raquo; <asp:Literal runat="server" Text="<%$ Resources:ProviderDetails, UserDetails %>" /></th>
                            </tr>
                            <tr>
                                <th>
                                    <label for="txtEmail">
                                        <asp:Literal runat="server" Text="<%$ Resources:ProviderDetails, TxtEmail %>" /></label>
                                </th>
                                <td>
                                    <asp:Label ID="lblUserName" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <label for="txtEmail">E-mail:</label>
                                </th>
                                <td>
                                    <asp:TextBox ID="txtEmail" runat="server" Width="300px" Enabled="false" ClientIDMode="Static" />
                                    <a id="btnEmail" clientidmode="Static" runat="server" title="<%$ Resources:ProviderDetails, ChangeEmail %>" href="#" class="icon-btn bg-emailEdit">
                                        <asp:Literal runat="server" Text="<%$ Resources:ProviderDetails, ChangeEmail %>" />
                                    </a>
                                    <div id="emailError" style="font-weight: bold;" />
                                </td>
                            </tr>
                        </table>

                        <asp:LinkButton ID="btnSendEmailVerificationCode" runat="server" Text="<%$ Resources:ProviderDetails, EmailResend %>"
                            CssClass="icon-btn bg-email" ValidationGroup="vgContactInfo" OnClick="btnSendEmailVerificationCode_Click" Style="margin-top: 10px;" />

                        <asp:Label ID="lblContactInfoErrors" runat="server" Font-Bold="true" ForeColor="Red" Style="margin-top: 10px;" />
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>

        </TabPages>
    </dx:ASPxPageControl>
</asp:Content>
