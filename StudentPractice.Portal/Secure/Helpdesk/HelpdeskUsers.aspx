<%@ Page Language="C#" MasterPageFile="~/Secure/BackOffice.master" AutoEventWireup="true"
    Inherits="StudentPractice.Portal.Secure.Helpdesk.HelpdeskUsers" Title="Χρήστες Helpdesk"
    CodeBehind="HelpdeskUsers.aspx.cs" %>

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
        <a runat="server" class="icon-btn bg-addNewItem" href="javascript:void(0)" onclick='popUp.show("AddHelpdeskUser.aspx","Δημιουργία Χρήστη",cmdRefresh, 800, 400)'>
            Δημιουργία Χρήστη</a>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <dx:ASPxGridView ID="gvHelpdeskUsers" runat="server" AutoGenerateColumns="False"
                DataSourceID="odsHelpdeskUsers" KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true"
                Width="100%" DataSourceForceStandardPaging="true" OnCustomCallback="gvHelpdeskUsers_CustomCallback"
                OnCustomDataCallback="gvHelpdeskUsers_CustomDataCallback" ClientInstanceName="gv">
                <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
                <SettingsPager PageSize="10" Summary-Text="Σελίδα {0} από {1} ({2} Χρήστες)" Summary-Position="Left" />
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
                    <dx:GridViewDataTextColumn FieldName="UserName" Name="UserName" Caption="Στοιχεία Λογαριασμού"
                        CellStyle-HorizontalAlign="Left" Width="200px">
                        <DataItemTemplate>
                            <%# GetAccountDetails((HelpdeskUser)Container.DataItem)%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ContactName" Name="Name" Caption="Στοιχεία Χρήστη"
                        CellStyle-HorizontalAlign="Left" Width="200px">
                        <DataItemTemplate>
                            <%# GetContactDetails((HelpdeskUser)Container.DataItem)%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Συμβάντα που έχει απαντήσει" CellStyle-HorizontalAlign="Left"
                        Width="150px">
                        <DataItemTemplate>
                            <%# GetAnsweredIncidentReports((HelpdeskUser)Container.DataItem)%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Επεξεργασία Χρήστη" Width="100px">
                        <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                        <CellStyle HorizontalAlign="Center" />
                        <DataItemTemplate>
                            <a runat="server" style="text-decoration: none" href="javascript:void(0)" onclick='<%# string.Format("popUp.show(\"EditHelpdeskUser.aspx?sID={0}\", \"Επεξεργασία Χρήστη\", cmdRefresh);", Eval("ID"))%>'>
                                <img src="/_img/iconEdit.png" alt="Επεξεργασία Χρήστη" />
                            </a><a runat="server" href="javascript:void(0);" style="text-decoration: none;" onclick='<%# string.Format("return doAction(\"lock\", {0}, \"HelpDeskUsers\");", Eval("ID"))%>'
                                visible='<%# (bool)Eval("IsApproved") %>' title="Απενεργοποίηση Χρήστη">
                                <img src="/_img/iconLock.png" alt="Απενεργοποίηση Χρήστη" /></a> <a runat="server"
                                    href="javascript:void(0);" style="text-decoration: none;" onclick='<%# string.Format("return doAction(\"unlock\", {0}, \"HelpDeskUsers\");", Eval("ID"))%>'
                                    visible='<%# !((bool)Eval("IsApproved")) %>' title="Επανενεργοποίηση Χρήστη">
                                    <img src="/_img/iconUnLock.png" alt="Επανενεργοποίηση Χρήστη" /></a> <a runat="server"
                                        href="javascript:void(0);" style="text-decoration: none;" onclick='<%# string.Format("return doAction(\"delete\", {0}, \"HelpDeskUsers\");", Eval("ID"))%>'
                                        title="Διαγραφή Χρήστη">
                                        <img src="/_img/iconDelete.png" alt="Διαγραφή Χρήστη" /></a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                </Columns>
            </dx:ASPxGridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ObjectDataSource ID="odsHelpdeskUsers" runat="server" TypeName="StudentPractice.Portal.DataSources.HelpdeskUsers"
        SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
        EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsHelpdeskUsers_Selecting">
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
