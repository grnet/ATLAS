<%@ Page Language="C#" MasterPageFile="~/Secure/SecurePages.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.InternshipOffices.OfficeUsers"
    Title="Χρήστες Γραφείου" CodeBehind="OfficeUsers.aspx.cs" %>

<%@ Import Namespace="StudentPractice.BusinessModel" %>
<%@ Import Namespace="StudentPractice.Portal" %>

<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <style type="text/css">
        .dxgvHeader td {
            font-size: 11px;
        }
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <asp:MultiView ID="mvAccount" runat="server" ActiveViewIndex="0">

        <asp:View ID="vAccountNotVerified" runat="server">
            <div class="reminder">
                <asp:Label ID="lblVerificationError" runat="server" />
            </div>
        </asp:View>

        <asp:View ID="vAccountVerified" runat="server">
            <asp:LinkButton ID="cmdRefresh" runat="server" Style="display: none;" OnClick="cmdRefresh_Click" />
            <div style="padding: 5px 0px 10px;">
                <a runat="server" class="icon-btn bg-addNewItem" href="AddOfficeUser.aspx">Δημιουργία
                    Χρήστη</a>
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <dx:ASPxGridView ID="gvOfficeUsers" runat="server" AutoGenerateColumns="False"
                        DataSourceID="odsOfficeUsers" KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true"
                        Width="100%" DataSourceForceStandardPaging="true" OnCustomCallback="gvOfficeUsers_CustomCallback"
                        OnCustomDataCallback="gvOfficeUsers_CustomDataCallback" ClientInstanceName="gv">
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
                                    <%# ((InternshipOffice)Container.DataItem).GetAccountDetails() %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="ContactName" Name="Name" Caption="Στοιχεία Χρήστη"
                                CellStyle-HorizontalAlign="Left" Width="200px">
                                <DataItemTemplate>
                                    <%# ((InternshipOffice)Container.DataItem).GetContactDetails() %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Πρόσβαση σε τμήματα του Γραφείου Πρακτικής"
                                CellStyle-HorizontalAlign="Left" Width="200px">
                                <DataItemTemplate>
                                    <%# ((InternshipOffice)Container.DataItem).GetOfficeAcademics(Entity.Academics.Count) %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Περιορισμός Πρόσβασης" Width="100px">
                                <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                                <CellStyle HorizontalAlign="Center" />
                                <DataItemTemplate>
                                    <a runat="server" style="text-decoration: none" href="javascript:void(0)" tip="Επιλογή Τμημάτων"
                                        onclick='<%# string.Format("popUp.show(\"OfficeAcademicsAuthorization.aspx?oID={0}\", \"Επιλογή Τμημάτων\", cmdRefresh, 800, 660);", Eval("ID"))%>'>
                                        <img src="/_img/iconReportEdit.png" alt="Επιλογή Τμημάτων" />
                                    </a>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Επεξεργασία Χρήστη" Width="100px">
                                <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                                <CellStyle HorizontalAlign="Center" />
                                <DataItemTemplate>
                                    <a runat="server" style="text-decoration: none" href='<%# string.Format("EditOfficeUser.aspx?sID={0}", Eval("ID"))%>'
                                        tip="Επεξεργασία Χρήστη">
                                        <img src="/_img/iconEdit.png" alt="Επεξεργασία Χρήστη" />
                                    </a>

                                    <a runat="server" href="javascript:void(0);" style="text-decoration: none;" onclick='<%# string.Format("return doAction(\"lock\", {0}, \"OfficeUsers\");", Eval("ID"))%>'
                                        visible='<%# (bool)Eval("IsApproved") %>' tip="Απενεργοποίηση Χρήστη">
                                        <img src="/_img/iconLock.png" alt="Απενεργοποίηση Χρήστη" />
                                    </a>

                                    <a runat="server" href="javascript:void(0);" style="text-decoration: none;" onclick='<%# string.Format("return doAction(\"unlock\", {0}, \"OfficeUsers\");", Eval("ID"))%>'
                                        visible='<%# !((bool)Eval("IsApproved")) %>' tip="Επανενεργοποίηση Χρήστη">
                                        <img src="/_img/iconUnLock.png" alt="Επανενεργοποίηση Χρήστη" />
                                    </a>

                                    <a runat="server" href="javascript:void(0);" style="text-decoration: none;" onclick='<%# string.Format("return doAction(\"delete\", {0}, \"OfficeUsers\");", Eval("ID"))%>'
                                        tip="Διαγραφή Χρήστη">
                                        <img src="/_img/iconDelete.png" alt="Διαγραφή Χρήστη" />
                                    </a>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:ASPxGridView>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:ObjectDataSource ID="odsOfficeUsers" runat="server" TypeName="StudentPractice.Portal.DataSources.Offices"
                SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
                EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsOfficeUsers_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="criteria" Type="Object" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <script type="text/javascript">
                function cmdRefresh() {
                    <%= this.Page.ClientScript.GetPostBackEventReference(this.cmdRefresh, string.Empty) %>;
                }
            </script>
        </asp:View>
    </asp:MultiView>
</asp:Content>
