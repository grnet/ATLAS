<%@ Page Language="C#" MasterPageFile="~/Secure/BackOffice.master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.Helpdesk.SearchSupervisorReports"
    Title="Αναζήτηση Συμβάντων" CodeBehind="SearchSupervisorReports.aspx.cs" %>

<%@ Register TagPrefix="my" TagName="IncidentReportsGridview" Src="~/Secure/Helpdesk/UserControls/IncidentReportsGridview.ascx" %>

<%@ Import Namespace="StudentPractice.BusinessModel" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .dxgvHeader td {
            font-size: 11px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        var isValueChanged = false;
        var ddlReportStatus = null;
        $(function () {
            ddlReportStatus = $('#ddlReportStatus');
            isValueChanged = ddlReportStatus.val() != '';
        });
    </script>

    <asp:LinkButton ID="cmdRefresh" runat="server" Style="display: none;" />
    <table id="tbFilters" runat="server" style="width: 500px" class="dv">
        <tr>
            <th colspan="4" class="popupHeader">Στοιχεία Αναφοράς
            </th>
        </tr>
        <tr>
            <th style="width: 115px">Κατάσταση:
            </th>
            <td style="width: 120px">
                <asp:DropDownList ID="ddlReportStatus" runat="server" Width="115px" TabIndex="1" OnInit="ddlReportStatus_Init" ClientIDMode="Static" />
            </td>
            <th style="width: 90px">Τύπος Κλήσης:
            </th>
            <td style="width: 120px">
                <asp:DropDownList ID="ddlCallType" runat="server" Width="115px" TabIndex="6" OnInit="ddlCallType_Init" />
            </td>
        </tr>
        <tr>
            <th style="width: 115px">Αναφορά από:
            </th>
            <td style="width: 120px">
                <asp:DropDownList ID="ddlReportedBy" runat="server" TabIndex="2" OnInit="ddlReportedBy_Init" Width="115px" />
            </td>
            <th style="width: 90px">Ημ/νία (από):
            </th>
            <td style="width: 120px">
                <dx:ASPxDateEdit ID="deIncidentReportDateFrom" runat="server" TabIndex="7" Width="115px" />
            </td>
        </tr>
        <tr>
            <th style="width: 115px">Πηγή αναφοράς:
            </th>
            <td style="width: 120px">
                <asp:DropDownList ID="ddlReporterType" runat="server" TabIndex="3" OnInit="ddlReporterType_Init" Width="115px" />
            </td>
            <th style="width: 90px">Ημ/νία (έως):
            </th>
            <td style="width: 120px">
                <dx:ASPxDateEdit ID="deIncidentReportDateTo" runat="server" TabIndex="8" Width="115px" />
            </td>
        </tr>
        <tr>
            <th style="width: 115px">Είδος αναφοράς:
            </th>
            <td colspan="3" style="width: 350px">
                <asp:DropDownList ID="ddlIncidentType" runat="server" TabIndex="4" Width="355px" DataTextField="Name" DataValueField="ID" />
                <act:CascadingDropDown ID="cddIncidentType" runat="server" TargetControlID="ddlIncidentType" ParentControlID="ddlReporterType"
                    Category="IncidentTypes" PromptText="-- επιλέξτε πηγή αναφοράς --" ServicePath="~/PortalServices/Services.asmx" ServiceMethod="GetIncidentTypes"
                    LoadingText="Παρακαλω περιμένετε">
                </act:CascadingDropDown>
            </td>
        </tr>
        <tr>
            <th style="width: 115px">Επικοινωνία:
            </th>
            <td colspan="3" style="width: 350px">
                <asp:DropDownList ID="ddlHandlerStatus" runat="server" Width="355px" TabIndex="5" OnInit="ddlHandlerStatus_Init" ClientIDMode="Static" />
            </td>
        </tr>
    </table>
    <div style="padding: 5px 0px 10px;">
        <asp:LinkButton ID="btnSearch" runat="server" Text="Αναζήτηση" OnClick="btnSearch_Click" CssClass="icon-btn bg-search" />
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <my:IncidentReportsGridview runat="server" ID="gvIncidentReports" DataSourceID="odsIncidentReports">
                <CustomTemplate>
                    <a id="lnkEditIncidentReport" runat="server" style="text-decoration: none" href="javascript:void(0)" visible='<%# !Roles.IsUserInRole(Page.User.Identity.Name, RoleNames.Supervisor) %>'
                        onclick='<%# string.Format("popUp.show(\"EditIncidentReport.aspx?irID={0}\",\"Επεξεργασία Συμβάντος\", cmdRefresh);", Eval("ID"))%>'>
                        <img src="/_img/iconReportEdit.png" alt="Επεξεργασία Συμβάντος" />
                    </a>

                    <a id="lnkViewIncidentReport" runat="server" style="text-decoration: none" href="javascript:void(0)" 
                        onclick='<%# string.Format("popUp.show(\"ViewIncident.aspx?ID={0}\",\"Προβολή Συμβάντος\");", Eval("ID"))%>'>
                        <img src="/_img/iconView.png" alt="Προβολή Συμβάντος" />
                    </a>

                    <a id="lnkAddIncidentReportPost" runat="server" style="text-decoration: none" href="javascript:void(0)" 
                        onclick='<%# string.Format("popUp.show(\"AddIncidentReportPost.aspx?ID={0}\",\"Προσθήκη Απάντησης\", cmdRefresh, 800, 610);", Eval("ID"))%>'>
                        <img src="/_img/iconAddNewItem.png" alt="Προσθήκη Απάντησης" />
                    </a>
                </CustomTemplate>
            </my:IncidentReportsGridview>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ObjectDataSource ID="odsIncidentReports" runat="server" TypeName="StudentPractice.Portal.DataSources.IncidentReports" SelectMethod="FindWithCriteria"
        SelectCountMethod="CountWithCriteria" EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsIncidentReports_Selecting">
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
