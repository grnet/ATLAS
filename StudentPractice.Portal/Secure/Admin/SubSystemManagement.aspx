<%@ Page Language="C#"MasterPageFile="~/Secure/BackOffice.master"AutoEventWireup="true"
    Inherits="StudentPractice.Portal.Secure.Admin.SubSystemManagement" Title="Διαχείριση Υποσυστημάτων"
    CodeBehind="SubSystemManagement.aspx.cs" %>

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
    <asp:LinkButton ID="cmdRefresh" runat="server" Style="display: none;" OnClick="cmdRefresh_Click" />
    <div style="padding: 5px 0px 10px;">
        <a id="lnkCreateSubSystem" runat="server" class="icon-btn bg-addNewItem" href="javascript:void(0)"
            onclick="popUp.show('CreateSubSystem.aspx','Δημιουργία Υποσυστήματος',cmdRefresh)">
            Δημιουργία Υποσυστήματος</a>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <dx:ASPxGridView ID="gvSubSystems" runat="server" AutoGenerateColumns="False"
                DataSourceID="odsSubSystems" KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true"
                Width="100%" DataSourceForceStandardPaging="true" OnRowCommand="gvSubSystems_RowCommand">
                <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
                <SettingsPager PageSize="10" Summary-Text="Σελίδα {0} από {1} ({2} Αναφορές)" Summary-Position="Left" />
                <Styles>
                    <Cell Font-Size="11px" />
                </Styles>
                <Templates>
                    <EmptyDataRow>
                        Δεν βρέθηκαν αποτελέσματα
                    </EmptyDataRow>
                </Templates>
                <Columns>
                    <dx:GridViewDataTextColumn Caption="Α/Α" HeaderStyle-HorizontalAlign="Center"
                        CellStyle-HorizontalAlign="Center" Width="50px">
                        <DataItemTemplate>
                            <%# Container.ItemIndex + 1 %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Name" Name="Name" Caption="Όνομα" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# Eval("Name")%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Τύποι Αναφερόντων" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# GetReporterTypes((SubSystem)Container.DataItem) %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Συμβάντα που έχουν αναφερθεί" HeaderStyle-HorizontalAlign="Center"
                        CellStyle-HorizontalAlign="Center">
                        <DataItemTemplate>
                            <%# GetIncidentCount((SubSystem)Container.DataItem)%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Επεξεργασία Υποσυστήματος" Width="50px">
                        <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                        <CellStyle HorizontalAlign="Center" />
                        <DataItemTemplate>
                            <a id="lnkEditSubSystem" runat="server" style="text-decoration: none" href="javascript:void(0)"
                                onclick=<%# string.Format("popUp.show('EditSubSystem.aspx?sID={0}', 'Επεξεργασία Υποσυστήματος', cmdRefresh, 800, 610);", Eval("ID"))%>>
                                <img src="/_img/iconEdit.png" alt="Επεξεργασία Υποσυστήματος" />
                            </a>
                            <asp:LinkButton ID="lnkDeleteSubSystem" runat="server" CommandName="DeleteSubSystem"
                                CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Είστε σίγουροι ότι θέλετε να διαγράψετε το συγκρεκριμένο υποσύστημα;');"
                                Style="text-decoration: none;" ToolTip="Διαγραφή Υποσυστήματος" Visible='<%# GetIncidentCount((SubSystem)Container.DataItem) == 0 %>'>
                                    <img src="/_img/iconDelete.png" alt="Διαγραφή Υποσυστήματος" /></asp:LinkButton>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                </Columns>
            </dx:ASPxGridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ObjectDataSource ID="odsSubSystems" runat="server" TypeName="StudentPractice.Portal.DataSources.SubSystems"
        SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria" EnablePaging="true"
        SortParameterName="sortExpression" OnSelecting="odsSubSystems_Selecting">
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
