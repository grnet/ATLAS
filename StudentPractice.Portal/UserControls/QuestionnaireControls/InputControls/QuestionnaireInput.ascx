<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuestionnaireInput.ascx.cs" Inherits="StudentPractice.Portal.UserControls.QuestionnaireControls.InputControls.QuestionnaireInput" %>

<%@ Register TagPrefix="my" TagName="QuestionInput" Src="~/UserControls/QuestionnaireControls/InputControls/QuestionnaireQuestionInput.ascx" %>

<h2>
    <asp:Literal ID="litTitle" runat="server" />
</h2>

<div id="divAtlasEval" runat="server" visible="false" class="reminder">
    <asp:Literal runat="server" Text="<%$ Resources:Evaluation, EvalHeader %>" />
</div>

<div style="width: 750px">
    <asp:Repeater ID="rpt" runat="server" OnItemDataBound="rpt_ItemDataBound">
        <ItemTemplate>
            <tr>
                <my:QuestionInput ID="ucQuestionInput" runat="server" QuestionID='<%# Eval("ID") %>' />
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</div>
