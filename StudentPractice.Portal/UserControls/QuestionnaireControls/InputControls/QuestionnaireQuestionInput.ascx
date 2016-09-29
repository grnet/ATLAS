<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuestionnaireQuestionInput.ascx.cs" Inherits="StudentPractice.Portal.UserControls.QuestionnaireControls.InputControls.QuestionnaireQuestionInput" %>

<h3>
    <asp:Literal ID="litTitle" runat="server" />
</h3>

<asp:MultiView ID="mv" runat="server">
    <asp:View ID="vFreeText" runat="server">
        <table style="width: 100%;">
            <tr>
                <td>
                    <asp:TextBox ID="txtAnswer" runat="server" Rows="4" MaxLength="500" TextMode="MultiLine" Width="95%" />
                </td>
                <td style="width: 10px;">
                    <asp:RequiredFieldValidator ID="rfFreeText" runat="server" ControlToValidate="txtAnswer" ErrorMessage="<%$ Resources:Evaluation, RequiredField %>" Display="Dynamic">
                        <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:Evaluation, RequiredField %>" />
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="reFreeText" runat="server" ControlToValidate="txtAnswer" ErrorMessage="<%$ Resources:Evaluation, MaxTextError %>" Display="Dynamic">
                        <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:Evaluation, MaxTextErrorLong %>" />
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
        </table>
    </asp:View>
    <asp:View ID="vMultipleAnswer" runat="server">
        <table style="width: 100%;">
            <tr>
                <td>
                    <asp:RadioButtonList ID="rblMultipleAnswer" runat="server" RepeatDirection="Horizontal" />
                </td>
                <td style="width: 10px;">
                    <asp:RequiredFieldValidator ID="rfMultipleAnswer" runat="server" ControlToValidate="rblMultipleAnswer" ErrorMessage="<%$ Resources:Evaluation, RequiredField %>" Display="Dynamic">
                        <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:Evaluation, RequiredField %>" />
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
        </table>
    </asp:View>
</asp:MultiView>
