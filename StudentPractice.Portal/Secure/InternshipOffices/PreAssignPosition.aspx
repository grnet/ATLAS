<%@ Page Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.InternshipOffices.PreAssignPosition"
    CodeBehind="PreAssignPosition.aspx.cs" Title="Προδέσμευση Θέσης" %>

<%@ Register TagPrefix="my" Src="~/UserControls/InternshipPositionControls/ViewControls/PositionGroupView.ascx" TagName="PositionGroupView"  %>

<asp:Content ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <div style="margin: 10px;">
        <div id="divErrors" runat="server" class="reminder" style="text-align: left" visible="false">
            <asp:Label ID="lblErrors" runat="server" Font-Bold="true" ForeColor="Red" />
            <br />
        </div>
        <table style="width: 100%" class="dv">
            <tr>
                <th colspan="2" class="header">
                    &raquo; Προδέσμευση Θέσεων
                </th>
            </tr>
            <tr>
                <th style="width: 170px">
                    Αριθμός Διαθέσιμων Θέσεων:
                </th>
                <td>
                    <asp:Label ID="lblAvailablePositions" runat="server" Font-Bold="true" ForeColor="Blue" />
                </td>
            </tr>
            <tr>
                <th style="width: 170px">
                    Θέσεις που θέλετε να προδεσμεύσετε:
                </th>
                <td>
                    <asp:TextBox ID="txtPositionCount" runat="server" Width="50px" />
                    <asp:RequiredFieldValidator ID="rfvPositionCount" runat="server" ControlToValidate="txtPositionCount"
                        Display="Dynamic" ErrorMessage="Το πεδίο 'Θέσεις που θέλετε να προδεσμεύσετε' είναι υποχρεωτικό">Το πεδίο είναι υποχρεωτικό</asp:RequiredFieldValidator>
                    <asp:RangeValidator ID="rvPositionCount" runat="server" MinimumValue="1" MaximumValue="99" Type="Integer"
                        ControlToValidate="txtPositionCount" ErrorMessage="Ο αριθμός των θέσεων πρέπει να είναι θετικός ακέραιος αριθμός μικρότερος από τον αριθμό των διαθέσιμων θέσεων."
                        ValidateEmptyText="false" Display="Dynamic">
                        Ο αριθμός των θέσεων πρέπει να είναι θετικός ακέραιος αριθμός μικρότερος από τον αριθμό των διαθέσιμων θέσεων.
                    </asp:RangeValidator>
                </td>
            </tr>
            <tr>
                <th style="width: 170px">
                    Τμήμα για το οποίο θέλετε να προδεσμεύσετε τις θέσεις:
                </th>
                <td>
                    <asp:DropDownList ID="ddlDepartment" runat="server" Width="60%" OnInit="ddlDepartment_Init" />
                    <asp:RequiredFieldValidator ID="rfvDepartment" runat="server" ControlToValidate="ddlDepartment"
                        Display="Dynamic" ErrorMessage="Το πεδίο 'Τμήμα για το οποίο θέλετε να προδεσμεύσετε τις θέσεις' είναι υποχρεωτικό">Το πεδίο είναι υποχρεωτικό</asp:RequiredFieldValidator>
                    <asp:PlaceHolder ID="phDepartmentInfo" runat="server">
                        <br />
                        <span style="font-size: 10px">(Εμφανίζονται μόνο τα ενεργά Τμήματα για τα οποία ο Φορέας Υποδοχής έχει δηλώσει ότι η θέση είναι διαθέσιμη.)</span> 
                    </asp:PlaceHolder>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:LinkButton ID="btnSubmit" runat="server" Text="Προδέσμευση" CssClass="icon-btn bg-accept"
                        OnClick="btnSubmit_Click" />
                </td>
            </tr>
        </table>
    </div>
    <div style="margin: 20px 10px 10px;">
        <my:PositionGroupView ID="ucPositionGroupView" runat="server" />
    </div>
</asp:Content>
