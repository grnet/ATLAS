<%@ Page Title="" Language="C#" MasterPageFile="~/Secure/BackOffice.Master" AutoEventWireup="true" CodeBehind="MassCancelGroups.aspx.cs" Inherits="StudentPractice.Portal.Secure.Admin.MassCancelGroups" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table class="dv" style="width: 800px">
        <tr>
            <th>GroupIDs:</th>
            <td>
                <asp:TextBox ID="txtGroupIDs" runat="server" Width="100%" /></td>
        </tr>
        <tr>
            <th colspan="2" style="text-align: right;">
                <asp:Button ID="btnRun" runat="server" Text="Run" OnClick="btnRun_Click" />
            </th>
        </tr>
    </table>
</asp:Content>
