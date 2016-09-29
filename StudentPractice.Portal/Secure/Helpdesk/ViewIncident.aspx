<%@ Page Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" EnableViewState="false"
    Inherits="StudentPractice.Portal.Secure.Helpdesk.ViewIncident" CodeBehind="ViewIncident.aspx.cs"
    Title="Προβολή Συμβάντος" %>

<%@ Register TagPrefix="my" TagName="IncidentReportView" Src="~/Secure/Helpdesk/UserControls/IncidentReportView.ascx" %>
<%@ Register TagPrefix="my" TagName="IncidentReportPostsView" Src="~/Secure/Helpdesk/UserControls/IncidentReportPostsView.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div>
        <asp:PlaceHolder ID="phFromSuccess" runat="server" Visible='<%# !string.IsNullOrEmpty(Request.QueryString["s"]) %>'>
            <div class="reminder">
                Η αναφορά καταχωρήθηκε με επιτυχία.
            </div>
        </asp:PlaceHolder>
        <my:IncidentReportView ID="incidentReportView" runat="server" />
        <br />
        <my:IncidentReportPostsView ID="incidentReportPostsView" runat="server" />
        <asp:PlaceHolder ID="phCannotSendEmail" runat="server" Visible="false">
            <br />
            <div class="reminder" style="text-align: left">
                Δεν μπορείτε να στείλετε την τελευταία απάντηση με email, γιατί έχει είδη σταλεί
                email στο χρήστη μέσα στα τελευταία 30 λεπτά.
            </div>
        </asp:PlaceHolder>
        <p style="text-align: center">
            <a href="javascript:void(0)" onclick="window.parent.popUp.hide();" class="icon-btn bg-cancel">
                Κλείσιμο</a>
            <asp:LinkButton ID="btnSendEmail" runat="server" Text="Αποστολή Απάντησης" CssClass="icon-btn bg-email"
                OnClick="btnSendEmail_Click" OnClientClick="return confirm('Είστε σίγουροι ότι θέλετε να στείλετε την απάντηση με e-mail;');" />
        </p>
    </div>
    <asp:ObjectDataSource ID="odsIncidentReportPosts" runat="server" TypeName="StudentPractice.Portal.DataSources.IncidentReportPosts"
        SelectMethod="FindByIncidentReportID">
        <SelectParameters>
            <asp:QueryStringParameter Name="incidentReportID" Type="Int32" QueryStringField="id"
                DefaultValue="-1" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
