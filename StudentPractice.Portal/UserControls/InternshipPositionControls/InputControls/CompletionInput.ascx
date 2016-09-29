<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompletionInput.ascx.cs"
    Inherits="StudentPractice.Portal.UserControls.InternshipPositionControls.InputControls.CompletionInput" %>

<%@ Import Namespace="StudentPractice.BusinessModel" %>
<script type="text/javascript">
    var rbtlCompletionVerdict;
    $(function () {
        rbtlCompletionVerdict = $('#rbtlCompletionVerdict input');
        rbtlCompletionVerdict.click(updateDisplay);
        updateDisplay();
    });

    function updateDisplay() {

        rbtlCompletionVerdict.each(function (index) {
            var btn = $(rbtlCompletionVerdict[index]);

            if (btn.attr('checked') == 'checked') {
                if (index == 0) {
                    $('#trImplementationStartDate').show();
                    $('#trImplementationEndDate').show();
                    //$('#trFundingType').show();
                }
                else {
                    $('#trImplementationStartDate').hide();
                    $('#trImplementationEndDate').hide();
                    //$('#trFundingType').hide();
                }
            }
        })
    }
</script>
<table width="600px" class="dv">
    <tr>
        <th class="header">&raquo; Αποτέλεσμα Ολοκλήρωσης
        </th>
    </tr>
    <tr>
        <td>
            <asp:RadioButtonList ID="rbtlCompletionVerdict" runat="server" RepeatDirection="Vertical"
                Font-Bold="true" Font-Size="11px" OnInit="rbtlCompletionVerdict_Init" ClientIDMode="Static">
            </asp:RadioButtonList>
            <asp:RequiredFieldValidator ID="rfvCompletionVerdict" Display="Static" runat="server"
                ControlToValidate="rbtlCompletionVerdict" ErrorMessage="Το πεδίο 'Αποτέλεσμα Ολοκλήρωσης' είναι υποχρεωτικό">Πρέπει να επιλέξετε το αποτέλεσμα της ολοκλήρωσης</asp:RequiredFieldValidator>
        </td>
    </tr>
</table>
<br />
<table width="600px" class="dv">
    <tr>
        <th colspan="2" class="header">&raquo; Λεπτομέρειες Ολοκλήρωσης
        </th>
    </tr>
    <tr id="trImplementationStartDate">
        <th style="width: 160px">Ημ/νία Έναρξης Πρακτικής:
        </th>
        <td>
            <asp:TextBox ID="txtImplementationStartDate" runat="server" MaxLength="10" ClientIDMode="Static" Width="25%" />
            <asp:HyperLink ID="lnkSelectImplementationStartDate" runat="server" NavigateUrl="#">
                <img runat="server" style="border: none; vertical-align: middle" src="~/_img/iconCalendar.png" />
            </asp:HyperLink>

            <act:CalendarExtender ID="ceSelectImplementationStartDate" runat="server" PopupButtonID="lnkSelectImplementationStartDate"
                TargetControlID="txtImplementationStartDate" Format="<%$ Resources:GlobalProvider, DateFormat %>" />

            <asp:RequiredFieldValidator ID="rfvImplementationStartDate" runat="server" ControlToValidate="txtImplementationStartDate"
                Display="Dynamic" ErrorMessage="Το πεδίο 'Ημ/νία Έναρξης Πρακτικής' είναι υποχρεωτικό">Το πεδίο είναι υποχρεωτικό</asp:RequiredFieldValidator>
            <asp:CompareValidator ID="cvImplementationStartDate" runat="server" Display="Dynamic" Type="Date"
                Operator="DataTypeCheck" ControlToValidate="txtImplementationStartDate" ErrorMessage="Το πεδίο δεν είναι έγκυρο">
                    Η ημ/νία πρέπει να είναι της μορφής dd/MM/yyyy (π.χ. 02/11/2012)
            </asp:CompareValidator>
        </td>
    </tr>
    <tr id="trImplementationEndDate">
        <th style="width: 160px">Ημ/νία Λήξης Πρακτικής:
        </th>
        <td>
            <asp:TextBox ID="txtImplementationEndDate" runat="server" MaxLength="10" ClientIDMode="Static" Width="25%" />
            <asp:HyperLink ID="lnkSelectImplementationEndDate" runat="server" NavigateUrl="#">
                    <img runat="server" style="border: none; vertical-align: middle" src="~/_img/iconCalendar.png" />
                </asp:HyperLink>
            
            <act:CalendarExtender ID="ceSelectImplementationEndDate" runat="server" PopupButtonID="lnkSelectImplementationEndDate"
                TargetControlID="txtImplementationEndDate" Format="<%$ Resources:GlobalProvider, DateFormat %>" />

            <asp:RequiredFieldValidator ID="rfvImplementationEndDate" runat="server" ControlToValidate="txtImplementationEndDate"
                Display="Dynamic" ErrorMessage="Το πεδίο 'Ημ/νία Λήξης Πρακτικής' είναι υποχρεωτικό">Το πεδίο είναι υποχρεωτικό</asp:RequiredFieldValidator>
            <asp:CompareValidator ID="cvImplementationEndDate" runat="server" Display="Dynamic" Type="Date"
                Operator="DataTypeCheck" ControlToValidate="txtImplementationEndDate" ErrorMessage="Το πεδίο δεν είναι έγκυρο">
                    Η ημ/νία πρέπει να είναι της μορφής dd/MM/yyyy (π.χ. 02/11/2012)
            </asp:CompareValidator>
        </td>
    </tr>
    <%--<tr id="fundingType">
        <th style="width: 180px">
            <asp:Literal ID="ltrFundingType" runat="server" Text="Τρόπος Χρηματοδότησης:" />
        </th>
        <td>
            <asp:DropDownList ID="ddlFundingType" runat="server" ClientIDMode="Static" OnInit="ddlFundingType_Init" Width="61%" />
        </td>
    </tr>--%>
    <tr>
        <th style="width: 160px">Παρατηρήσεις:
        </th>
        <td>
            <asp:TextBox ID="txtCompletionComments" runat="server" TextMode="MultiLine" Rows="9"
                Width="100%" />
        </td>
    </tr>
</table>
