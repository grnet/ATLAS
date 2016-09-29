<%@ Page Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.ColourExplanation" CodeBehind="ColourExplanation.aspx.cs" 
    Title="<%$ Resources:GlobalProvider, Button_Color %>" %>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <table width="450px" class="dv">
        <tr>
            <td style="background-color: #FFFFFF">
                <asp:Literal runat="server" Text="<%$ Resources:ColourExplanation, Preassigned %>" />
            </td>
        </tr>
        <tr>
            <td style="background-color: #FFB6C1">
                <asp:Literal runat="server" Text="<%$ Resources:ColourExplanation, Assigned %>" />
            </td>
        </tr>
        <tr>
            <td style="background-color: #FFFF00">
                <asp:Literal runat="server" Text="<%$ Resources:ColourExplanation, Undesimplementation %>" />
            </td>
        </tr>
        <tr>
            <td style="background-color: #90EE90">
                <asp:Literal runat="server" Text="<%$ Resources:ColourExplanation, Completed %>" />
            </td>
        </tr>
        <tr>
            <td style="background-color: #FF6347">
                <asp:Literal runat="server" Text="<%$ Resources:ColourExplanation, Cancelled %>" />
            </td>
        </tr>
        <tr>
            <td style="background-color: #ADD8E6">
                <asp:Literal runat="server" Text="<%$ Resources:ColourExplanation, Finished %>" />
            </td>
        </tr>
        <tr>
            <td style="background-color: #D3D3D3">
                <asp:Literal runat="server" Text="<%$ Resources:ColourExplanation, Incomplete %>" />
            </td>
        </tr>
    </table>
</asp:Content>
