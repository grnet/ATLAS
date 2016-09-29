<%@ Page Title="Μεταφορά Θέσεων" Language="C#" MasterPageFile="~/Secure/BackOffice.master"
    AutoEventWireup="true" CodeBehind="TransferPositions.aspx.cs" Inherits="StudentPractice.Portal.Secure.Helpdesk.TransferPositions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table class="dv">
        <tr>
            <th colspan="4" class="popupHeader">
                Μεταφορά προδεσμευμένης θέσης πρακτικής σε νέο γραφείο
            </th>
        </tr>
        <tr>
            <th>
                ID Θέσης:
            </th>
            <td>
                <asp:TextBox ID="txtPositionID" runat="server" />
            </td>
        </tr>
        <tr>
            <th>
                ID Νέου Γραφείου:
            </th>
            <td>
                <asp:TextBox ID="txtNewOfficeID" runat="server" />
            </td>
        </tr>
    </table>
    <div style="padding: 5px 0px 20px;">
        <asp:LinkButton ID="btnTransfer" runat="server" Text="Μεταφορά Θέσης" OnClick="btnTransfer_Click"
            CssClass="icon-btn bg-accept" />
    </div>
    <br />
    <asp:Label ID="lblResult" runat="server" Font-Bold="true" />
</asp:Content>
