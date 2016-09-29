<%@ Page Title="Επικοινωνία με Γραφείο Αρωγής" Language="C#" MasterPageFile="~/Secure/SecurePages.Master" AutoEventWireup="true" CodeBehind="HelpdeskContact.aspx.cs" Inherits="StudentPractice.Portal.Secure.Common.HelpdeskContact" %>

<%@ Import Namespace="StudentPractice.BusinessModel" %>

<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <style type="text/css">
        .dxgvHeader td {
            font-size: 11px;
        }
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <asp:LinkButton ID="cmdRefresh" runat="server" Style="display: none;" OnClick="cmdRefresh_Click" />

    <div class="reminder">
        <asp:Literal runat="server" Text="<%$ Resources:HelpdeskContact, Reminder %>" />
    </div>

    <div style="padding: 10px 0px">
        <asp:HyperLink ID="lnkSubmitQuestion" runat="server" Text="<%$ Resources:HelpdeskContact, NewQuestion %>" href="javascript:void(0)" CssClass="icon-btn bg-addNewItem" />
    </div>

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <dx:ASPxGridView ID="gvIncidentReports" runat="server" AutoGenerateColumns="False"
                DataSourceID="odsIncidentReports" KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true"
                Width="100%" DataSourceForceStandardPaging="true">
                <SettingsLoadingPanel Text="<%$ Resources:Grid, Loading %>" />
                <SettingsPager PageSize="10" Summary-Text="<%$ Resources:Grid, PagingIncident %>" Summary-Position="Left" />
                <Styles>
                    <Header HorizontalAlign="Center" Wrap="True" />
                    <Cell HorizontalAlign="Left" Font-Size="11px" Wrap="True" />
                </Styles>
                <Templates>
                    <EmptyDataRow>
                        <asp:Literal runat="server" Text="<%$ Resources:Grid, NoResults %>" />
                    </EmptyDataRow>
                </Templates>
                <Columns>
                    <dx:GridViewDataTextColumn FieldName="CreatedAt" Name="CreatedAt" Caption="<%$ Resources:Grid, GridCaption_SentAt %>" SortIndex="0" SortOrder="Descending" Width="125px">
                        <CellStyle HorizontalAlign="Center" Wrap="False" />
                        <DataItemTemplate>
                            <%# ((DateTime)Eval("CreatedAt")).ToString("dd/MM/yyyy HH:mm")%><br />
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_IncidentType %>">
                        <DataItemTemplate>
                            <%# GetIncidentTypeDetails((IncidentReport)Container.DataItem)%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ReportText" Name="ReportText" Caption="<%$ Resources:Grid, GridCaption_IncidentText %>" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_IncidentLastAnswer %>">
                        <DataItemTemplate>
                            <%# GetLastAnswer((IncidentReport)Container.DataItem)%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_IncidentHistory %>" Name="ContactHistory" Width="50px">
                        <CellStyle HorizontalAlign="Center" />
                        <DataItemTemplate>
                            <a runat="server" style="text-decoration: none" href="javascript:void(0)" onclick="<%# GetContactHistoryLink((IncidentReport)Container.DataItem) %>">
                                <img src="/_img/iconViewDetails.png" runat="server" alt="<%$ Resources:HelpdeskContact, ContactHistory %>" />
                            </a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                </Columns>
            </dx:ASPxGridView>
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
