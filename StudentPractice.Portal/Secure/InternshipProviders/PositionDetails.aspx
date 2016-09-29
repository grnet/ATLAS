<%@ Page Language="C#" MasterPageFile="~/Secure/SecurePages.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.InternshipProviders.PositionDetails"
    Title="Γενικά Στοιχεία Θέσης" CodeBehind="PositionDetails.aspx.cs" %>

<%@ Register TagPrefix="my" Src="~/UserControls/InternshipPositionControls/InputControls/PositionGroupInput.ascx" TagName="PositionGroupInput" %>

<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <link href="/_css/wizard.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ContentPlaceHolderID="cphBeforeHeaderTitle" runat="server">
    <asp:PlaceHolder ID="phSteps" runat="server">
        <ul id="mainNav" runat="server" clientidmode="Static" class="fourStep">
            <li class="current">
                <a runat="server" id="btnPositionDetails" title="<%$ Resources:PositionWizard, Step1Title %>">
                    <em>
                        <asp:Literal runat="server" Text="<%$ Resources:PositionWizard, Step1 %>" /></em>
                    <span>
                        <asp:Literal runat="server" Text="<%$ Resources:PositionWizard, Step1Title %>" /></span>
                </a>
            </li>

            <li>
                <a runat="server" id="btnPositionPhysicalObject" title="<%$ Resources:PositionWizard, Step2Title %>">
                    <em>
                        <asp:Literal runat="server" Text="<%$ Resources:PositionWizard, Step2 %>" /></em>
                    <span>
                        <asp:Literal runat="server" Text="<%$ Resources:PositionWizard, Step2Title %>" /></span>
                </a>
            </li>

            <li>
                <a runat="server" id="btnPositionAcademics" title="<%$ Resources:PositionWizard, Step3Title %>">
                    <em>
                        <asp:Literal runat="server" Text="<%$ Resources:PositionWizard, Step3 %>" /></em>
                    <span>
                        <asp:Literal runat="server" Text="<%$ Resources:PositionWizard, Step3Title %>" /></span>
                </a>
            </li>

            <li class="preview">
                <a runat="server" title="<%$ Resources:PositionWizard, Step4Title %>">
                    <em>
                        <asp:Literal runat="server" Text="<%$ Resources:PositionWizard, Step4 %>" /></em>
                    <span>
                        <asp:Literal runat="server" Text="<%$ Resources:PositionWizard, Step4Title %>" /></span>
                </a>
            </li>
        </ul>
    </asp:PlaceHolder>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <asp:MultiView ID="mvInternshipPosition" runat="server" ActiveViewIndex="0">

        <asp:View ID="vInternshipPosition" runat="server">

            <div id="divNote" runat="server" style="padding: 10px 0px">
                <asp:Label ID="lblNote" runat="server" Font-Bold="true" ForeColor="Red" Text="<%$ Resources:PositionPages, PositionDetails_Note %>" />
            </div>

            <asp:ValidationSummary ID="vsPosition" runat="server" CssClass="validation-summary" ValidationGroup="vgPosition" />
            <asp:Label ID="lblErrors" runat="server" Font-Bold="true" ForeColor="Red" />
            <my:PositionGroupInput ID="ucPositionGroupInput" runat="server" ValidationGroup="vgPosition" />

            <br />

            <div style="clear: both; text-align: left; margin-top: 15px;">
                <asp:LinkButton ID="btnSubmit" runat="server" CssClass="icon-btn bg-accept" ValidationGroup="vgPosition"
                    OnClick="btnSubmit_Click" OnClientClick="javascript:return ValidatePage('vdApplication');" Text="<%$ Resources:GlobalProvider, Global_SaveNContinue %>" />
                <asp:LinkButton ID="btnCancel" runat="server" CssClass="icon-btn bg-cancel" CausesValidation="false"
                    OnClick="btnCancel_Click" Text="<%$ Resources:GlobalProvider, Global_Cancel %>" />
            </div>
        </asp:View>

        <asp:View ID="vPositionCreationNotAllowed" runat="server">
            <div class="reminder">
                <asp:Literal runat="server" Text="<%$ Resources:PositionPages, PositionDetails_CreationNotAllowed %>" />
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
