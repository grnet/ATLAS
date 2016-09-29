<%@ Page Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true" CodeBehind="ProviderRegistration.aspx.cs"
    Inherits="StudentPractice.Portal.Common.ProviderRegistration" Title="Δημιουργία Λογαριασμού" %>

<%@ Register Src="~/UserControls/InternshipProviderControls/InputControls/ProviderRegisterUserInput.ascx"
    TagName="RegisterUserInput" TagPrefix="my" %>
<%@ Register Src="~/UserControls/InternshipProviderControls/InputControls/ProviderInput.ascx"
    TagName="ProviderInput" TagPrefix="my" %>
<%@ Register Src="~/UserControls/GenericControls/LanguageBar.ascx" TagName="LanguageBar" TagPrefix="my" %>

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
                <a runat="server" target="_blank" href="<%$ Resources:PrimaryMenu, ProviderManual %>" class="help">
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
    <div style="margin: 3px 0px; float:right;">
        <my:LanguageBar ID="ucLanguageBar" runat="server" Visible="false" />
    </div>
    <asp:MultiView ID="mvRegistration" runat="server" ActiveViewIndex="1">

        <asp:View ID="vTerms" runat="server">
            <div id="terms">
                <h2>
                    <asp:Literal runat="server" Text="<%$ Resources:RegistrantionConditions, Provider_ConditionHeader %>" />
                </h2>
                <ol>
                    <li>
                        <asp:Literal runat="server" Text="<%$ Resources:RegistrantionConditions, Provider1 %>" />
                    </li>
                    <li>
                        <asp:Literal runat="server" Text="<%$ Resources:RegistrantionConditions, Provider2 %>" />
                    </li>
                    <li>
                        <asp:Literal runat="server" Text="<%$ Resources:RegistrantionConditions, Provider3 %>" />
                    </li>
                    <li>
                        <asp:Literal runat="server" Text="<%$ Resources:RegistrantionConditions, Provider4 %>" />
                    </li>
                    <li>
                        <asp:Literal runat="server" Text="<%$ Resources:RegistrantionConditions, Provider5 %>" />
                    </li>
                    <li>
                        <asp:Literal runat="server" Text="<%$ Resources:RegistrantionConditions, Provider6 %>" />
                    </li>
                    <li>
                        <asp:Literal runat="server" Text="<%$ Resources:RegistrantionConditions, Provider7 %>" />
                    </li>
                    <li>
                        <asp:Literal runat="server" Text="<%$ Resources:RegistrantionConditions, Provider8 %>" />
                    </li>
                    <li>
                        <asp:Literal runat="server" Text="<%$ Resources:RegistrantionConditions, Provider9 %>" />
                    </li>
                </ol>
            </div>
            <div id="declaration">
                <p>
                    <em>
                        <asp:Literal runat="server" Text="<%$ Resources:RegistrantionConditions, Provider_AcceptHeader %>" /></em>
                </p>
                <p>
                    <asp:Literal runat="server" Text="<%$ Resources:RegistrantionConditions, Provider_Accept %>" />
                </p>
            </div>
            <asp:LinkButton ID="btnSubmit" runat="server" Text="<%$ Resources:RegistrantionConditions, Provider_Continue %>" CssClass="btn" ClientIDMode="Static" OnClick="btnSubmit_Click" />
        </asp:View>

        <asp:View ID="vRegister" runat="server">
            <asp:ValidationSummary ID="vdSummary" runat="server" CssClass="validation-summary" ValidationGroup="vdRegistration" HeaderText="<%$ Resources:RegistrantionConditions, Provider_FillError %>" />
            <asp:Label ID="lblErrors" runat="server" CssClass="error" />
            <my:RegisterUserInput ID="ucRegisterUserInput" runat="server" ValidationGroup="vdRegistration" />

            <br />
            <my:ProviderInput ID="ucProviderInput" runat="server" ValidationGroup="vdRegistration" />

            <br />
            <br />

            <div style="clear: both; text-align: left; margin-top: 10px">
                <table>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkTerms" runat="server" ClientIDMode="Static" />
                        </td>
                        <td style="padding-left: 5px;">
                            <asp:Literal runat="server" Text="<%$ Resources:RegistrantionConditions, Provider_Check1 %>" />
                            <a runat="server" href='<%$ Resources:RegistrantionConditions, RegistrationPdf %>' target='_blank'>
                                <asp:Literal runat="server" Text="<%$ Resources:RegistrantionConditions, Provider_Check2 %>" />
                            </a>
                            <asp:Literal runat="server" Text="<%$ Resources:RegistrantionConditions, Provider_Check3 %>" />

                            <asp:CustomValidator ID="cvTerms" runat="server" OnServerValidate="cvTerms_ServerValidate" Display="Dynamic" ErrorMessage="<%$ Resources:RegistrantionConditions, Provider_CheckError %>"
                                ClientValidationFunction="cvTerms_validate" ValidationGroup="vdRegistration">
                                    <img class="errortip" runat="server" title="<%$ Resources:RegistrantionConditions, Provider_CheckError %>" src="/_img/errorGray.gif" alt="<%$ Resources:RegistrantionConditions, Provider_CheckError %>" />
                            </asp:CustomValidator>

                            <script type="text/javascript">
                                function cvTerms_validate(s, e) {
                                    e.IsValid = $('#chkTerms').is(':checked');
                                }
                            </script>
                        </td>
                    </tr>
                </table>
            </div>

            <div style="clear: both; text-align: left; margin-top: 50px">
                <lc:BotShield ID="bsPublisher" runat="server" ValidationGroup="vdRegistration" ClientIDMode="Static" />
            </div>

            <div style="clear: both; text-align: left; margin-top: 20px">
                <asp:LinkButton ID="btnCreate" runat="server" Text="<%$ Resources:RegistrantionConditions, RegistrationBtn %>" CssClass="btn"
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
                <asp:Literal runat="server" Text="<%$ Resources:RegistrantionConditions, Provider_vNotAllowed %>" />
            </div>
        </asp:View>

        <asp:View ID="vCyprusNotAllowd" runat="server">
            <div class="reminder">
                <asp:Literal runat="server" Text="<%$ Resources:RegistrantionConditions, Provider_vCyprusNotAllowd %>" />
            </div>
        </asp:View>

        <asp:View ID="vForeignNotAllowd" runat="server">
            <div class="reminder">
                <asp:Literal runat="server" Text="<%$ Resources:RegistrantionConditions, Provider_vForeignNotAllowd %>" />
            </div>
        </asp:View>

    </asp:MultiView>
</asp:Content>
