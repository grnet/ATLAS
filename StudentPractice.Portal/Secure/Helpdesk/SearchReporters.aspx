<%@ Page Language="C#" MasterPageFile="~/Secure/BackOffice.master" AutoEventWireup="true"
    Inherits="StudentPractice.Portal.Secure.Helpdesk.SearchReporters" Title="Αναζήτηση Αναφερόντων"
    CodeBehind="SearchReporters.aspx.cs" %>

<%@ Import Namespace="StudentPractice.BusinessModel" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .mGrid
        {
            background-color: White;
            border: 0 none;
            border-collapse: separate !important;
            color: Black;
            font: 9pt Tahoma;
            overflow: hidden;
            border-left: solid 1px #666;
        }
        
        .mGrid td
        {
            border-color: -moz-use-text-color #CFCFCF #CFCFCF -moz-use-text-color;
            border-style: none solid solid none;
            border-width: 0 1px 1px 0;
            overflow: hidden;
            padding: 3px 6px 4px;
        }
        
        .mGrid th
        {
            background-color: #DCDCDC;
            border: 1px solid #9F9F9F;
            font-weight: normal;
            font-size: 11px;
            overflow: hidden;
            padding: 4px 6px 5px;
            text-align: center;
        }
        
        .mGrid .pgr
        {
            background: #424242 url(grd_pgr.png) repeat-x top;
        }
        .mGrid .pgr table
        {
            margin: 5px 0;
        }
        .mGrid .pgr td
        {
            border-width: 0;
            padding: 0 6px;
            border-left: solid 1px #666;
            font-weight: bold;
            font-size: 20px;
            color: #fff;
            line-height: 12px;
        }
        .mGrid .pgr a
        {
            color: #666;
            text-decoration: none;
        }
        .mGrid .pgr a:hover
        {
            color: #000;
            text-decoration: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:LinkButton ID="cmdRefresh" runat="server" Style="display: none;" OnClick="cmdRefresh_Click" />
    <table cellspacing="0">
        <tr>
            <td style="vertical-align: top">
                <table style="width: 850px" class="dv">
                    <tr>
                        <th colspan="4" class="popupHeader">
                            Γενικά Στοιχεία Αναφέροντος
                        </th>
                    </tr>
                    <tr>
                        <th style="width: 200px">
                            Ον/μο ατόμου επικοινωνίας:
                        </th>
                        <td style="width: 230px">
                            <asp:TextBox ID="txtContactName" runat="server" Width="98%" />
                        </td>
                        <th style="width: 200px">
                            Κατηγορία Αναφέροντος:
                        </th>
                        <td style="width: 230px">
                            <asp:DropDownList ID="ddlReporterType" runat="server" Width="98%" OnInit="ddlReporterType_Init" />
                        </td>
                    </tr>
                    <tr>
                        <th style="width: 200px">
                            Τηλέφωνο ατόμου επικοινωνίας:
                        </th>
                        <td style="width: 230px">
                            <asp:TextBox ID="txtContactPhone" runat="server" Width="98%" />
                        </td>
                        <th style="width: 200px">
                            Τρόπος Αναφοράς:
                        </th>
                        <td style="width: 230px">
                            <asp:DropDownList ID="ddlDeclarationType" runat="server" Width="98%" OnInit="ddlDeclarationType_Init" />
                        </td>
                    </tr>
                    <tr>
                        <th style="width: 200px">
                            E-mail ατόμου επικοινωνίας:
                        </th>
                        <td style="width: 230px">
                            <asp:TextBox ID="txtContactEmail" runat="server" Width="98%" />
                        </td>
                        <th style="width: 200px">
                            Αρ. Βεβαίωσης:
                        </th>
                        <td style="width: 230px">
                            <asp:TextBox ID="txtCertificationNumber" runat="server" Width="98%" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="vertical-align: top">
                <table style="width:500px" class="dv">
                    <tr>
                        <th colspan="4" class="popupHeader">
                            Ειδικά Στοιχεία Αναφέροντος
                        </th>
                    </tr>
                    <tr>
                        <th style="width: 200px">
                            Επωνυμία/Διακριτικός Τίτλος:
                        </th>
                        <td style="width: 230px">
                            <asp:TextBox ID="txtProviderName" runat="server" Width="98%" />
                        </td>
                    </tr>
                    <tr>
                        <th style="width: 200px">
                            Α.Φ.Μ.:
                        </th>
                        <td style="width: 230px">
                            <asp:TextBox ID="txtProviderAFM" runat="server" Width="98%" />
                        </td>
                    </tr>
                    <tr>
                        <th style="width: 200px">
                            Ίδρυμα:
                        </th>
                        <td style="width: 230px">
                            <asp:DropDownList ID="ddlInstitution" runat="server" Width="98%" OnInit="ddlInstitution_Init" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div style="padding: 5px 0px 10px;">
        <asp:LinkButton ID="btnSearch" runat="server" Text="Αναζήτηση" OnClick="btnSearch_Click"
            CssClass="icon-btn bg-search" />
        <a id="lnkReportIncident" runat="server" class="icon-btn bg-addNewItem" href="javascript:void(0)"
            onclick='popUp.show("ReportIncident.aspx","Αναφορά Συμβάντος", cmdRefresh, 800, 600)'>Αναφορά
            Συμβάντος </a>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:GridView ID="gvReporters" runat="server" CssClass="mGrid" AllowPaging="True"
                AutoGenerateColumns="False" DataSourceID="odsReporters" DataKeyNames="ID" OnRowDataBound="gvReporters_RowDataBound"
                Width="100%">
                <EmptyDataTemplate>
                    <b>Δεν βρέθηκαν αποτελέσματα. Βεβαιωθείτε ότι έχετε εισάγει τουλάχιστον ένα κριτήριο
                        αναζήτησης.</b>
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField HeaderText="Κατηγορία Αναφέροντος" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <%# GetReporterTypeDetails((Reporter)Container.DataItem) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Τρόπος Αναφοράς" HeaderStyle-HorizontalAlign="Left"
                        HeaderStyle-Width="120px">
                        <ItemTemplate>
                            <%# ((enReporterDeclarationType)Eval("DeclarationType")).GetLabel()%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Στοιχεία Ατόμου Επικοινωνίας" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <%# GetContactDetails((Reporter)Container.DataItem) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ειδικά Στοιχεία Αναφέροντος" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <%# GetReporterDetails(Container.DataItem) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Στοιχεία Λογαριασμού" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <a runat="server" style="text-decoration: none" href="javascript:void(0)" visible='<%# CanEditAccountDetails((Reporter)Container.DataItem) %>'
                                onclick='<%# string.Format("popUp.show(\"ViewAccountDetails.aspx?rid={0}&t={1}\", \"Στοιχεία Λογαριασμού\", null, 700, 250);", Eval("ID"), ((enReporterType)Eval("ReporterType")).ToString("D"))%>' >
                                <img src="/_img/iconUserEdit.png" alt="Στοιχεία Λογαριασμού" />
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Επεξεργασία Αναφέροντα" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <a runat="server" style="text-decoration: none" href="javascript:void(0)" visible='<%# CanEditReporter((Reporter)Container.DataItem) %>'
                                onclick='<%# string.Format("popUp.show(\"EditReporter.aspx?rID={0}\", \"Επεξεργασία Αναφέροντα\", cmdRefresh, 800, 600);", Eval("ID"))%>' >
                                <img src="/_img/iconEdit.png" alt="Επεξεργασία Αναφέροντα" />
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Προβολή Συμβάντων" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <a runat="server" style="text-decoration: none" href='<%# GetSearchIncidentReportLink((Reporter)Container.DataItem) %>'>
                                <img src="/_img/iconView.png" alt="Προβολή Συμβάντων" />
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Αναφορά Συμβάντος" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <a runat="server" style="text-decoration: none" href="javascript:void(0)" 
                                onclick='<%# string.Format("popUp.show(\"ReportIncident.aspx?rID={0}\",\"Αναφορά Συμβάντος\", cmdRefresh, 800, 600);", Eval("ID"))%>' >
                                <img src="/_img/iconAddNewItem.png" alt="Προσθήκη Συμβάντος" />
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ObjectDataSource ID="odsReporters" runat="server" TypeName="StudentPractice.Portal.DataSources.Reporters"
        SelectMethod="FindWithReporterCriteria" SelectCountMethod="CountWithReporterCriteria"
        EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsReporters_Selecting">
        <SelectParameters>
            <asp:Parameter Name="criteria" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <script type="text/javascript">
        function cmdRefresh() {
            <%= this.Page.ClientScript.GetPostBackEventReference(this.cmdRefresh, string.Empty) %>;
        }
    </script>
</asp:Content>
