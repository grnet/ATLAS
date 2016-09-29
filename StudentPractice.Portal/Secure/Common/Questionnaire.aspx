<%@ Page Title="" Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" CodeBehind="Questionnaire.aspx.cs" Inherits="StudentPractice.Portal.Secure.Common.Questionnaire" %>

<%@ Register TagPrefix="my" TagName="QuestionnaireInput" Src="~/UserControls/QuestionnaireControls/InputControls/QuestionnaireInput.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

    <style type="text/css">
        body {
            background-color: #eeeeee;
        }
    </style>

    <my:QuestionnaireInput ID="ucQuestionnaireInput" runat="server" ValidationGroup="vgQuestionnaire" IsRequired="true" OnControlInit="ucQuestionnaireInput_ControlInit" />

    <table>
        <tr>
            <td>
                <asp:LinkButton ID="btnSubmit" runat="server" ValidationGroup="vgQuestionnaire" CssClass="icon-btn bg-accept"
                    OnClick="btnSubmit_Click" Text="<%$ Resources:Evaluation, Save %>" />
            </td>
            <td colspan="2" style="text-align: center;">
                <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:Evaluation, Close %>" CssClass="icon-btn bg-cancel"
                    CausesValidation="false" OnClientClick="window.parent.popUp.hideWithoutRefresh();" />
            </td>
        </tr>
    </table>
</asp:Content>
