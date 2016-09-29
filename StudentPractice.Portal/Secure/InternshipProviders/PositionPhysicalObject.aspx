<%@ Page Language="C#" MasterPageFile="~/Secure/SecurePages.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.InternshipProviders.PositionPhysicalObject"
    Title="Γνωστικό Αντικείμενο Θέσης" CodeBehind="PositionPhysicalObject.aspx.cs" %>

<%@ Register TagPrefix="my" Src="~/UserControls/InternshipPositionControls/InputControls/PhysicalObjectsInput.ascx" TagName="PhysicalObjectsInput" %>

<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <link href="/_css/wizard.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ContentPlaceHolderID="cphBeforeHeaderTitle" runat="server">
    <asp:PlaceHolder ID="phSteps" runat="server">
        <ul id="mainNav" runat="server" clientidmode="Static" class="fourStep">
            <li class="lastDone">
                <a runat="server" id="btnPositionDetails" title="<%$ Resources:PositionWizard, Step1Title %>">
                    <em><asp:Literal runat="server" Text="<%$ Resources:PositionWizard, Step1 %>" /></em>
                    <span><asp:Literal runat="server" Text="<%$ Resources:PositionWizard, Step1Title %>" /></span>
                </a>
            </li>

            <li class="current">
                <a runat="server" id="btnPositionPhysicalObject" title="<%$ Resources:PositionWizard, Step2Title %>">
                    <em><asp:Literal runat="server" Text="<%$ Resources:PositionWizard, Step2 %>" /></em>
                    <span><asp:Literal runat="server" Text="<%$ Resources:PositionWizard, Step2Title %>" /></span>
                </a>
            </li>

            <li>
                <a runat="server" id="btnPositionAcademics" title="<%$ Resources:PositionWizard, Step3Title %>">
                    <em><asp:Literal runat="server" Text="<%$ Resources:PositionWizard, Step3 %>" /></em>
                    <span><asp:Literal runat="server" Text="<%$ Resources:PositionWizard, Step3Title %>" /></span>
                </a>
            </li>

            <li class="preview">
                <a runat="server" title="<%$ Resources:PositionWizard, Step4Title %>">
                    <em><asp:Literal runat="server" Text="<%$ Resources:PositionWizard, Step4 %>" /></em>
                    <span><asp:Literal runat="server" Text="<%$ Resources:PositionWizard, Step4Title %>" /></span>
                </a>
            </li>
        </ul>      
    </asp:PlaceHolder>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <my:PhysicalObjectsInput ID="ucPhysicalObjectsInput" runat="server" OnComplete="ucPhysicalObjectsInput_Complete" OnCancel="ucPhysicalObjectsInput_Cancel" />
</asp:Content>
