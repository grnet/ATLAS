<%@ Page Language="C#" MasterPageFile="~/Secure/SecurePages.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.InternshipProviders.PositionPreview"
    Title="Προεπισκόπηση Θέσης Πρακτικής Άσκησης" CodeBehind="PositionPreview.aspx.cs" %>

<%@ Register TagPrefix="my" Src="~/UserControls/InternshipPositionControls/ViewControls/PositionGroupView.ascx" TagName="PositionGroupView" %>

<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <link href="/_css/wizard.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ContentPlaceHolderID="cphBeforeHeaderTitle" runat="server">
    <asp:PlaceHolder ID="phSteps" runat="server">
        <ul id="mainNav" runat="server" clientidmode="Static" class="fourStep">
            <li class="done">
                <a id="btnPositionDetails" runat="server" title="<%$ Resources:PositionWizard, Step1Title %>">
                    <em><asp:Literal runat="server" Text="<%$ Resources:PositionWizard, Step1 %>" /></em>
                    <span><asp:Literal runat="server" Text="<%$ Resources:PositionWizard, Step1Title %>" /></span>
                </a>
            </li>

            <li class="done">
                <a id="btnPositionPhysicalObject" runat="server" title="<%$ Resources:PositionWizard, Step2Title %>" href="PositionPhysicalObject.aspx">
                    <em><asp:Literal runat="server" Text="<%$ Resources:PositionWizard, Step2 %>" /></em>
                    <span><asp:Literal runat="server" Text="<%$ Resources:PositionWizard, Step2Title %>" /></span>
                </a>
            </li>

            <li class="lastDone">
                <a id="btnPositionAcademics" runat="server" title="<%$ Resources:PositionWizard, Step3Title %>" href="PositionAcademics.aspx">
                    <em><asp:Literal runat="server" Text="<%$ Resources:PositionWizard, Step3 %>" /></em>
                    <span><asp:Literal runat="server" Text="<%$ Resources:PositionWizard, Step3Title %>" /></span>
                </a>
            </li>

            <li class="preview current">
                <a runat="server" title="<%$ Resources:PositionWizard, Step4Title %>">
                    <em><asp:Literal runat="server" Text="<%$ Resources:PositionWizard, Step4 %>" /></em>
                    <span><asp:Literal runat="server" Text="<%$ Resources:PositionWizard, Step4Title %>" /></span>
                </a>
            </li>
        </ul>  
    </asp:PlaceHolder>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <div style="margin-top: 10px">
        <my:PositionGroupView ID="ucPositionGroupView" runat="server" />

        <div style="clear: both; text-align: left; margin-top:15px">
             <asp:LinkButton ID="btnSubmit" runat="server" CssClass="icon-btn bg-accept" OnClick="btnSubmit_Click" Text="<%$ Resources:PositionPages, PositionPreview_SavePosition %>" />
            <asp:LinkButton ID="btnCancel" runat="server" CssClass="icon-btn bg-cancel" CausesValidation="false" OnClick="btnCancel_Click" Text="<%$ Resources:GlobalProvider, Global_Previous %>" />
        </div>
    </div>
</asp:Content>
