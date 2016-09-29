<%@ Page Language="C#" MasterPageFile="~/Secure/SecurePages.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.InternshipProviders.Default"
    Title="Κεντρική Σελίδα" CodeBehind="Default.aspx.cs" %>

<%@ Import Namespace="StudentPractice.BusinessModel" %>

<asp:Content ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <asp:Literal runat="server" Text="<%$ Resources:Default, Title %>" />
    <br />

    <br />
    <ul>
        <li class="firstListItem">
            <asp:Literal runat="server" Text="<%$ Resources:Default, EditDetails1 %>" />
            <asp:HyperLink runat="server" NavigateUrl="~/Secure/InternshipProviders/ProviderDetails.aspx" Style="font-weight: bold; color: Blue;">
                <asp:Literal runat="server" Text="<%$ Resources:Default, EditDetails2 %>" />
            </asp:HyperLink>
        </li>
        <li class="firstListItem">
            <asp:Literal runat="server" Text="<%$ Resources:Default, InternshipPosition1 %>" />
            <asp:HyperLink runat="server" NavigateUrl="~/Secure/InternshipProviders/InternshipPositions.aspx" Style="font-weight: bold; color: Blue;">
                <asp:Literal runat="server" Text="<%$ Resources:Default, InternshipPosition2 %>" />
            </asp:HyperLink>
        </li>
        <li class="firstListItem">
            <asp:Literal runat="server" Text="<%$ Resources:Default, SelectedPosition1 %>" />
            <asp:HyperLink runat="server" NavigateUrl="~/Secure/InternshipProviders/SelectedPositions.aspx" Style="font-weight: bold; color: Blue;">
                <asp:Literal runat="server" Text="<%$ Resources:Default, SelectedPosition2 %>" />
            </asp:HyperLink>
        </li>
        <li class="firstListItem" id="liEvaluation" runat="server">
            <asp:Literal runat="server" Text="<%$ Resources:Default, Evaluation1 %>" />
            <asp:HyperLink runat="server" NavigateUrl="~/Secure/InternshipProviders/Evaluation.aspx" Style="font-weight: bold; color: Blue;">
                <asp:Literal runat="server" Text="<%$ Resources:Default, Evaluation2 %>" />
            </asp:HyperLink>
        </li>
        <li id="liProviderUSers" runat="server" class="firstListItem">
            <asp:Literal runat="server" Text="<%$ Resources:Default, ProviderUsers1 %>" />
            <asp:HyperLink runat="server" NavigateUrl="~/Secure/InternshipProviders/ProviderUsers.aspx" Style="font-weight: bold; color: Blue;">
                <asp:Literal runat="server" Text="<%$ Resources:Default, ProviderUsers2 %>" />
            </asp:HyperLink>
        </li>
        <li class="firstListItem">
            <asp:Literal runat="server" Text="<%$ Resources:Default, HelpdeskContact1%>" />
            <asp:HyperLink runat="server" NavigateUrl="~/Secure/Common/HelpdeskContact.aspx" Style="font-weight: bold; color: Blue;">
                <asp:Literal runat="server" Text="<%$ Resources:Default, HelpdeskContact2 %>" />
            </asp:HyperLink>
            <asp:Literal runat="server" Text="<%$ Resources:Default, HelpdeskContact3 %>" />
        </li>
    </ul>
    <br />

    <br />
    <a id="btnPrintCertification" runat="server" href="~/Secure/InternshipProviders/GenerateProviderCertificationPDF.ashx" class="icon-btn bg-print">
        <asp:Literal runat="server" Text="<%$ Resources:Default, CertificationPDF%>" />       
    </a>

    <script type="text/javascript">
        function showEvaluationPopup() {
            <%= string.Format("popUp.show('/Secure/Common/Questionnaire.aspx?qType={0}','{1}', null, 800, 610);", enQuestionnaireType.ProviderForAtlas.GetValue(), Resources.Evaluation.Evaluate) %>
        }
    </script>
</asp:Content>
