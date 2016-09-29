<%@ Page Title="Μετρητές Προδεσμευμένων - Αντιστοιχισμένων Θέσεων ΓΠΑ" Language="C#"
    MasterPageFile="~/Secure/BackOffice.master" AutoEventWireup="true" CodeBehind="InternshipOfficeCounters.aspx.cs"
    Inherits="StudentPractice.Portal.Secure.Reports.InternshipOfficeCounters" %>

<%@ Import Namespace="StudentPractice.BusinessModel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="padding: 5px 0 10px;">
        <asp:LinkButton ID="btnExport" runat="server" Text="Εξαγωγή σε Excel" OnClick="btnExport_Click"
            CssClass="icon-btn bg-excel" />
    </div>
    <dx:ASPxGridView ID="gvOfficeCounters" runat="server" AutoGenerateColumns="False"
        DataSourceID="sdsOfficeCounters" EnableRowsCache="false" EnableCallBacks="true">
        <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
        <SettingsPager PageSize="50" Summary-Text="Σελίδα {0} από {1} ({2} Γραφεία Πρακτικής Άσκησης)"
            Summary-Position="Left" />
        <Settings ShowFooter="true" ShowGroupFooter="VisibleAlways" ShowFilterRow="true" />
        <Styles Footer-BackColor="DarkGray" Footer-Font-Bold="true" />
        <SettingsBehavior AllowSort="True" AutoFilterRowInputDelay="900" />
        <Templates>
            <EmptyDataRow>
                Δεν βρέθηκαν αποτελέσματα
            </EmptyDataRow>
        </Templates>
        <Columns>
            <dx:GridViewDataTextColumn FieldName="OfficeID" Caption="ID Γραφείου" CellStyle-HorizontalAlign="Center" />
            <dx:GridViewDataTextColumn FieldName="OfficeType" Caption="Είδος Γραφείου">
                <Settings AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Institution" Caption="Ίδρυμα">
                <Settings AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Academics" Caption="Τμήμα(τα)">
                <Settings AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="TotalPreAssignedPositions" Caption="Counter Προδεσμεύσεων"
                Width="70px" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="True">
                <PropertiesTextEdit DisplayFormatString="{0:n0}" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="TotalAssignedPositions" Caption="Counter Αντιστοιχίσεων"
                Width="70px" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="True">
                <PropertiesTextEdit DisplayFormatString="{0:n0}" />
            </dx:GridViewDataTextColumn>
        </Columns>
        <TotalSummary>
            <dx:ASPxSummaryItem FieldName="TotalPreAssignedPositions" SummaryType="Sum" DisplayFormat="{0:n0}" />
            <dx:ASPxSummaryItem FieldName="TotalAssignedPositions" SummaryType="Sum" DisplayFormat="{0:n0}" />
        </TotalSummary>
    </dx:ASPxGridView>
    <dx:ASPxGridViewExporter ID="gveOfficeCounters" runat="server" GridViewID="gvOfficeCounters">
    </dx:ASPxGridViewExporter>
    <asp:SqlDataSource ID="sdsOfficeCounters" runat="server" ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
        ProviderName="System.Data.SqlClient" SelectCommand="SELECT * FROM vOfficeCounters ORDER BY TotalPreAssignedPositions DESC">
    </asp:SqlDataSource>
</asp:Content>
