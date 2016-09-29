<%@ Page Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" CodeBehind="EmailVerificationInfo.aspx.cs"
    Inherits="StudentPractice.Portal.EmailVerificationInfo" Title="Οδηγίες Πιστοποίησης E-mail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div class="reminder" style="text-align: left">
        <asp:Literal runat="server" Text="<%$ Resources:EmailVerificationInfo, EmailVerification1 %>" />
        <br />
        <br />
        <asp:Literal runat="server" Text="<%$ Resources:EmailVerificationInfo, EmailVerification2 %>" />
        <br />
        <br />
        <asp:Literal runat="server" Text="<%$ Resources:EmailVerificationInfo, EmailVerification3 %>" />
        <ul>
            <li class='firstListItem'>
                <asp:Literal runat="server" Text="<%$ Resources:EmailVerificationInfo, EmailVerification4 %>" /></li>
            <li class='firstListItem'>
                <asp:Literal runat="server" Text="<%$ Resources:EmailVerificationInfo, EmailVerification5 %>" /></li>
        </ul>
    </div>
</asp:Content>
