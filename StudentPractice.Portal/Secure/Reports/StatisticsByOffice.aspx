<%@ Page Title="Στοιχεία Θέσεων Ανά Γραφείο Πρακτικής Άσκσησης" Language="C#" MasterPageFile="~/Secure/BackOffice.master"
    AutoEventWireup="true" CodeBehind="StatisticsByOffice.aspx.cs" Inherits="StudentPractice.Portal.Secure.Reports.StatisticsByOffice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="padding: 5px 0 10px;">
        <asp:LinkButton ID="btnExport" runat="server" Text="Εξαγωγή σε Excel" OnClick="btnExport_Click"
            CssClass="icon-btn bg-excel" />
    </div>
    <dx:ASPxGridView ID="gvStatisticsByOffice" runat="server" AutoGenerateColumns="False" KeyFieldName="OfficeID"
        DataSourceID="sdsStatistics" EnableRowsCache="false" EnableCallBacks="true">
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
            <dx:GridViewDataTextColumn FieldName="OfficeID" Caption="ID Γραφείου" HeaderStyle-HorizontalAlign="Center" />
            <dx:GridViewDataTextColumn FieldName="OfficeType" Caption="Είδος Γραφείου">
                <Settings AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Institution" Caption="Ίδρυμα">
                <Settings AutoFilterCondition="Contains" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Academics" Caption="Τμήμα(τα)">
                <Settings AutoFilterCondition="Contains" />
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
            <dx:ASPxSummaryItem FieldName="PreAssignedPositions" SummaryType="Sum" DisplayFormat="{0:n0}" />
            <dx:ASPxSummaryItem FieldName="AssignedPositions" SummaryType="Sum" DisplayFormat="{0:n0}" />
            <dx:ASPxSummaryItem FieldName="UnderImplementationPositions" SummaryType="Sum"
                DisplayFormat="{0:n0}" />
            <dx:ASPxSummaryItem FieldName="CompletedPositions" SummaryType="Sum" DisplayFormat="{0:n0}" />
            <dx:ASPxSummaryItem FieldName="CompletedFromOfficePositions" SummaryType="Sum" DisplayFormat="{0:n0}" />
            <dx:ASPxSummaryItem FieldName="CanceledPositions" SummaryType="Sum" DisplayFormat="{0:n0}" />
        </TotalSummary>
    </dx:ASPxGridView>
    <dx:ASPxGridViewExporter ID="gveStatisticsByOffice" runat="server" GridViewID="gvStatisticsByOffice">
    </dx:ASPxGridViewExporter>
    <asp:SqlDataSource ID="sdsStatistics" runat="server" ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
        ProviderName="System.Data.SqlClient" SelectCommand="SELECT * FROM vStatisticsByOffice">
    </asp:SqlDataSource>
</asp:Content>
