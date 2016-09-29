<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HandledStudentsGridView.ascx.cs"
    Inherits="StudentPractice.Portal.UserControls.InternshipPositionControls.ViewControls.HandledStudentsGridView" %>

<%@ Import Namespace="StudentPractice.BusinessModel" %>
<dx:ASPxGridView ID="gvPositions" runat="server" AutoGenerateColumns="False" KeyFieldName="ID"
    EnableRowsCache="false" EnableCallBacks="true" Width="100%" OnHtmlRowPrepared="gvPositions_HtmlRowPrepared"
    DataSourceForceStandardPaging="true">
    <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
    <SettingsPager PageSize="10" Summary-Text="Σελίδα {0} από {1} ({2} Φοιτητές)" Summary-Position="Left" />
    <Styles>
        <Cell Font-Size="11px" Wrap="True" HorizontalAlign="Left" />
    </Styles>
    <Templates>
        <EmptyDataRow>
            Δεν βρέθηκαν αποτελέσματα
        </EmptyDataRow>
    </Templates>
    <Columns>
        <dx:GridViewDataTextColumn Caption="Ημ/νία Αντιστοίχισης" FieldName="AssignedAt"
            Name="AssignedAt" CellStyle-Wrap="False" HeaderStyle-Wrap="True" SortIndex="0"
            SortOrder="Descending">
            <DataItemTemplate>
                <%# ((DateTime)Eval("AssignedAt")).ToString("dd/MM/yyyy") %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="ID Φοιτητή" FieldName="AssignedToStudentID">
            <DataItemTemplate>
                <%# Eval("AssignedToStudentID") %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="ID Γραφείου" FieldName="PreAssignedByMasterAccountID">
            <DataItemTemplate>
                <%# Eval("PreAssignedByMasterAccountID")%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Κατάσταση Θέσης" FieldName="PositionStatusInt">
            <DataItemTemplate>
                <%# ((enPositionStatus)Eval("PositionStatus")).GetLabel() %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Στοιχεία Φοιτητή" Name="StudentDetails">
            <DataItemTemplate>
                <%# GetStudentDetails((InternshipPosition)Container.DataItem) %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Στοιχεία Τμήματος Φοιτητή" Name="StudentAcademicDetails">
            <DataItemTemplate>
                <%# GetStudentAcademicDetails((InternshipPosition)Container.DataItem) %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Στοιχεία Γραφείου" Name="OfficeDetails">
            <DataItemTemplate>
                <%# GetOfficeDetails((InternshipPosition)Container.DataItem) %>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Caption="Πλήρη Στοιχεία" Name="FullDetails" HeaderStyle-Wrap="True"
            CellStyle-HorizontalAlign="Center">
            <DataItemTemplate>
                <a runat="server" style="text-decoration: none" href="javascript:void(0)" onclick=<%# string.Format("popUp.show('ViewPositionDetails.aspx?pID={0}','Πλήρη Στοιχεία', null, 800, 600);", Eval("ID"))%>>
                    <img src="/_img/iconViewDetails.png" alt="Προβολή Θέσης Πρακτικής" />
                </a>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
    </Columns>
</dx:ASPxGridView>
