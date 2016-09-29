<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InternshipPositionDetailsGridView.ascx.cs"
    Inherits="StudentPractice.Portal.Secure.Reports.UserControls.InternshipPositionDetailsGridView" %>

<%@ Import Namespace="StudentPractice.BusinessModel" %>
<%@ Import Namespace="StudentPractice.Portal" %>

<style type="text/css">
    .dxgvHeader td
    {
        font-size: 11px;
    }
    
    .dxgv
    {
        font-size: 11px;
    }
</style>
<asp:PlaceHolder ID="phExport" runat="server">
    <div style="padding: 5px 0px 10px;">
        <asp:LinkButton ID="btnExport" runat="server" Text="Εξαγωγή σε Excel" OnClick="btnExport_Click"
            CssClass="icon-btn bg-excel" Visible="false" />
    </div>
    <dx:ASPxGridViewExporter ID="gvePositions" runat="server" GridViewID="gvPositionsExport"
        OnRenderBrick="gvePositions_RenderBrick">
    </dx:ASPxGridViewExporter>
    <dx:ASPxGridView ID="gvPositionsExport" runat="server" AutoGenerateColumns="False"
        EnableRowsCache="false" EnableCallBacks="true" Width="100%" DataSourceForceStandardPaging="true"
        Visible="false">
        <Columns>
            <dx:GridViewDataTextColumn Caption="Κωδικός Θέσης" FieldName="ID" Name="ID" />
            <dx:GridViewDataTextColumn Caption="Τίτλος Θέσης" Name="PositionTitle" />
            <dx:GridViewDataTextColumn Caption="Φυσικό Αντικείμενο" Name="PhysicalObjects" />
            <dx:GridViewDataTextColumn Caption="Στοιχεία ΦΥΠΑ" Name="ProviderDetails" />
            <dx:GridViewDataTextColumn Caption="Στοιχεία ΓΠΑ" Name="OfficeDetails" />
            <dx:GridViewDataTextColumn Caption="Τμήμα Προδέσμευσης" Name="PreAssignedAcademinDetails"/>
            <dx:GridViewDataTextColumn Caption="Στοιχεία Αντιστοιχισμένου Φοιτητή" Name="AssignedStudentDetails" />
            <dx:GridViewDataTextColumn Caption="Ακαδημαϊκά Στοιχεία Αντιστοιχισμένου Φοιτητή" Name="AssignedStudentAcademicDetails" />
            <dx:GridViewDataTextColumn Caption="Στοιχεία Εκτέλεσης Πρακτικής Άσκησης" Name="PositionDuration" />
            <dx:GridViewDataTextColumn Caption="Κατάσταση Θέσης" FieldName="PositionStatusInt" Name="PositionStatus" />
        </Columns>
    </dx:ASPxGridView>
</asp:PlaceHolder>
<dx:ASPxGridView ID="gvPositions" runat="server" AutoGenerateColumns="False" DataSourceID="odsPositions"
    KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true" Width="100%"
    DataSourceForceStandardPaging="true" OnHtmlRowPrepared="gvPositions_HtmlRowPrepared">
    <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
    <SettingsPager PageSize="10" Summary-Text="Σελίδα {0} από {1} ({2} Θέσεις Πρακτικής Άσκησης)"
        Summary-Position="Left" />
    <Styles>
        <Header Wrap="True" />
        <Cell Font-Size="11px" Wrap="True" />
    </Styles>
    <Templates>
        <EmptyDataRow>
            Δεν βρέθηκαν αποτελέσματα
        </EmptyDataRow>
    </Templates>
    <Columns>
        <dx:GridViewDataTextColumn Caption="Κωδικός Θέσης" FieldName="ID" Name="ID">
            <DataItemTemplate>
                <%# Eval("ID") %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Τίτλος Θέσης" Name="PositionTitle">
            <DataItemTemplate>
                <%# Eval("InternshipPositionGroup.Title")%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Φυσικό Αντικείμενο" Name="PhysicalObjects">
            <DataItemTemplate>
                <%# ((InternshipPosition)Container.DataItem).GetPhysicalObjectDetails() %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Στοιχεία ΦΥΠΑ" Name="ProviderDetails" >
            <DataItemTemplate>
                <%# GetProviderDetails((InternshipPosition) Container.DataItem) %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Στοιχεία ΓΠΑ" Name="OfficeDetails">
            <DataItemTemplate>
                <%# GetOfficeDetails((InternshipPosition)Container.DataItem)%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Τμήμα Προδέσμευσης" Name="PreAssignedAcademinDetails">
            <DataItemTemplate>
                <%# GetPreAssignedForAcademic((InternshipPosition)Container.DataItem)%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Στοιχεία Αντιστοιχισμένου Φοιτητή" Name="AssignedStudentDetails">
            <DataItemTemplate>
                <%# GetAssignedStudentDetails((InternshipPosition)Container.DataItem)%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Ακαδημαϊκά Στοιχεία Αντιστοιχισμένου Φοιτητή" Name="AssignedStudentAcademicDetails" >
            <DataItemTemplate>
                <%# GetAssignedStudentAcademicDetails((InternshipPosition)Container.DataItem)%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
         <dx:GridViewDataTextColumn Caption="Στοιχεία Εκτέλεσης Πρακτικής Άσκησης" Name="PositionDuration">
            <DataItemTemplate>
                <%# GetPositionDuration((InternshipPosition)Container.DataItem)%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Κατάσταση Θέσης" FieldName="PositionStatusInt" Name="PositionStatus">
            <DataItemTemplate>
                <%# GetPositionStatus((InternshipPosition)Container.DataItem)%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
    </Columns>
</dx:ASPxGridView>
