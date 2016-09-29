<%@ Page Title="Στοιχεία Θέσεων Ανά Φορέα Υποδοχής" Language="C#" MasterPageFile="~/Secure/BackOffice.master"
    AutoEventWireup="true" CodeBehind="StatisticsByProvider.aspx.cs" Inherits="StudentPractice.Portal.Secure.Reports.StatisticsByProvider" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="padding: 5px 0 10px;">
        <asp:LinkButton ID="btnExport" runat="server" Text="Εξαγωγή σε Excel" OnClick="btnExport_Click"
            CssClass="icon-btn bg-excel" />
    </div>
    <dx:ASPxGridView ID="gvStatisticsByProvider" runat="server" AutoGenerateColumns="False" KeyFieldName="ProviderID"
        DataSourceID="sdsStatistics" EnableRowsCache="false">
        <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
        <SettingsPager PageSize="50" Summary-Text="Σελίδα {0} από {1} ({2} Φορείς Υποδοχής)"
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
            <dx:GridViewDataTextColumn Caption="ID Φορέα" FieldName="ProviderID" CellStyle-HorizontalAlign="Center" />
            <dx:GridViewDataTextColumn Caption="Επωνυμία" FieldName="ProviderName">
                <Settings AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Διακριτικός Τίτλος" FieldName="ProviderTradeName">
                <Settings AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Α.Φ.Μ." FieldName="ProviderAFM" />
            <dx:GridViewDataTextColumn Caption="Δ.Ο.Υ." FieldName="ProviderDOY" />
            <dx:GridViewDataTextColumn FieldName="CreatedPositions" Caption="Δημιουργημένες Θέσεις"
                Width="70px" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="True">
                <PropertiesTextEdit DisplayFormatString="{0:n0}" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="PublishedPositions" Caption="Δημοσιευμένες Θέσεις"
                Width="70px" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="True">
                <PropertiesTextEdit DisplayFormatString="{0:n0}" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="PreAssignedPositions" Caption="Προδεσμευμένες Θέσεις"
                Width="70px" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="True">
                <PropertiesTextEdit DisplayFormatString="{0:n0}" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="AssignedPositions" Caption="Αντιστοιχισμένες Θέσεις"
                Width="70px" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="True">
                <PropertiesTextEdit DisplayFormatString="{0:n0}" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="UnderImplementationPositions" Caption="Υπό διενέργεια Θέσεις"
                Width="100px" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="True">
                <PropertiesTextEdit DisplayFormatString="{0:n0}" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="CompletedPositions" Caption="Ολοκληρωμένες Θέσεις"
                Width="120px" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="True">
                <PropertiesTextEdit DisplayFormatString="{0:n0}" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="CompletedFromOfficePositions" Caption="Ολοκληρωμένες Θέσεις (από ΓΠΑ)"
                Width="120px" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="True">
                <PropertiesTextEdit DisplayFormatString="{0:n0}" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="CanceledPositions" Caption="Ακυρωμένες Θέσεις"
                Width="100px" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="True">
                <PropertiesTextEdit DisplayFormatString="{0:n0}" />
            </dx:GridViewDataTextColumn>
        </Columns>
        <TotalSummary>
            <dx:ASPxSummaryItem FieldName="CreatedPositions" SummaryType="Sum" DisplayFormat="{0:n0}" />
            <dx:ASPxSummaryItem FieldName="PublishedPositions" SummaryType="Sum" DisplayFormat="{0:n0}" />
            <dx:ASPxSummaryItem FieldName="PreAssignedPositions" SummaryType="Sum" DisplayFormat="{0:n0}" />
            <dx:ASPxSummaryItem FieldName="AssignedPositions" SummaryType="Sum" DisplayFormat="{0:n0}" />
            <dx:ASPxSummaryItem FieldName="UnderImplementationPositions" SummaryType="Sum"
                DisplayFormat="{0:n0}" />
            <dx:ASPxSummaryItem FieldName="CompletedPositions" SummaryType="Sum" DisplayFormat="{0:n0}" />
            <dx:ASPxSummaryItem FieldName="CompletedFromOfficePositions" SummaryType="Sum" DisplayFormat="{0:n0}" />
            <dx:ASPxSummaryItem FieldName="CanceledPositions" SummaryType="Sum" DisplayFormat="{0:n0}" />
        </TotalSummary>
    </dx:ASPxGridView>
    <dx:ASPxGridViewExporter ID="gveStatisticsByProvider" runat="server" GridViewID="gvStatisticsByProvider">
    </dx:ASPxGridViewExporter>
    <asp:SqlDataSource ID="sdsStatistics" runat="server" ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
        ProviderName="System.Data.SqlClient" SelectCommand="SELECT * FROM vStatisticsByProvider">
    </asp:SqlDataSource>
</asp:Content>
