<%@ Page Title="" Language="C#" MasterPageFile="~/Secure/BackOffice.Master" AutoEventWireup="true" CodeBehind="SendCustomMassEmail.aspx.cs" Inherits="StudentPractice.Portal.Secure.Admin.SendCustomMassEmail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <table class="dv" style="width: 800px">
        <tr>
            <th>Senders:</th>
            <td><asp:TextBox ID="txtSenders" runat="server" Width="100%" /></td>
        </tr>
        <tr>
            <th>Subject:</th>
            <td><asp:TextBox ID="txtSubject" runat="server" Width="100%" /></td>
        </tr>
        <tr>
            <th>Body:</th>
            <td><asp:TextBox ID="txtBody" runat="server" Width="100%" TextMode="MultiLine" Rows="10" /></td>
        </tr>
        <tr>
            <th colspan="2" style="text-align:right;">
                <asp:Button ID="btnSend" runat="server" Text="Send" OnClick="btnSend_Click" />
            </th>
        </tr>
    </table>

</asp:Content>
