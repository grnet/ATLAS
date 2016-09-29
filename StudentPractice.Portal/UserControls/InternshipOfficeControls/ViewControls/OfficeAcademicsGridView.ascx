<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OfficeAcademicsGridView.ascx.cs"
    Inherits="StudentPractice.Portal.UserControls.InternshipOfficeControls.ViewControls.OfficeAcademicsGridView" %>

<dx:ASPxGridView ID="gvAcademics" runat="server" AutoGenerateColumns="False" KeyFieldName="ID"
    EnableRowsCache="false" EnableCallBacks="true" Width="100%">
    <SettingsLoadingPanel Text="<%$ Resources:Grid, Loading %>" />
    <SettingsPager PageSize="10" Summary-Text="<%$ Resources:Grid, PagingAcademics %>" Summary-Position="Left" />
    <Styles>
        <Cell Font-Size="11px" />
    </Styles>
    <Templates>
        <EmptyDataRow>
            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Grid, AllAcademics %>" />
        </EmptyDataRow>
    </Templates>
    <Columns>
        <dx:GridViewDataTextColumn FieldName="Institution" Caption="<%$ Resources:Grid, GridCaption_Institution %>" CellStyle-HorizontalAlign="Left">
            <DataItemTemplate>
                <%# Eval("Institution")%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="School" Caption="<%$ Resources:Grid, GridCaption_School %>" CellStyle-HorizontalAlign="Left"
            SortIndex="0" SortOrder="Ascending">
            <DataItemTemplate>
                <%# Eval("School")%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Department" Caption="<%$ Resources:Grid, GridCaption_Department %>" CellStyle-HorizontalAlign="Left"
            SortIndex="1" SortOrder="Ascending">
            <DataItemTemplate>
                <%# Eval("Department")%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
    </Columns>
</dx:ASPxGridView>
