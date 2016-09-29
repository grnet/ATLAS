<%@ Page Language="C#" MasterPageFile="~/Secure/BackOffice.master" AutoEventWireup="true"
    Inherits="StudentPractice.Portal.Secure.Helpdesk.OnlineReports" Title="Αναζήτηση Συμβάντων"
    CodeBehind="OnlineReports.aspx.cs" %>

<%@ Register Src="~/Secure/Helpdesk/UserControls/IncidentReportFilters.ascx" TagName="IncidentReportFilters"
    TagPrefix="my" %>
<%@ Register Src="~/Secure/Helpdesk/UserControls/IncidentReportsGridview.ascx" TagName="IncidentReportsGridview"
    TagPrefix="my" %>

<%@ Import Namespace="StudentPractice.BusinessModel" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .dxgvHeader td
        {
            font-size: 11px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        var isValueChanged = false;
        var ddlReportStatus = null;
        $(function () {
            ddlReportStatus = $('#<%= ucIncidentReportFilters.ReportStatusClientID %>');
            isValueChanged = ddlReportStatus.val() != '';
        });
    </script>
    <asp:LinkButton ID="cmdRefresh" runat="server" Style="display: none;" OnClick="cmdRefresh_Click" />
    <my:incidentreportfilters id="ucIncidentReportFilters" runat="server" helpdeskreports="false" />
    <div style="padding: 5px 0px 10px;">
        <asp:LinkButton ID="btnSearch" runat="server" Text="Αναζήτηση" OnClick="btnSearch_Click"
            CssClass="icon-btn bg-search" />
        <asp:LinkButton ID="btnExport" runat="server" Text="Εξαγωγή σε Excel" OnClick="btnExport_Click"
            CssClass="icon-btn bg-excel" />
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <my:incidentreportsgridview runat="server" id="gvIncidentReports">
                <CustomTemplate>
                    <a id="lnkViewIncidentReport" runat="server" style="text-decoration: none" href="javascript:void(0)"
                        onclick='<%# string.Format("popUp.show(\"ViewIncident.aspx?ID={0}\",\"Προβολή Συμβάντος\", cmdRefresh, 800, 700);", Eval("ID"))%>'>
                        <img src="/_img/iconView.png" alt="Προβολή Συμβάντος" />
                    </a><a id="lnkAddIncidentReportPost" runat="server" style="text-decoration: none"
                        href="javascript:void(0)" onclick='<%# string.Format("popUp.show(\"AddIncidentReportPost.aspx?ID={0}\",\"Προσθήκη Απάντησης\", cmdRefresh, 800, 700);", Eval("ID"))%>'>
                        <img src="/_img/iconAddNewItem.png" alt="Προσθήκη Απάντησης" />
                    </a>
                </CustomTemplate>
            </my:incidentreportsgridview>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ObjectDataSource ID="odsIncidentReports" runat="server" TypeName="StudentPractice.Portal.DataSources.IncidentReports"
        SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
        EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsIncidentReports_Selecting">
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
