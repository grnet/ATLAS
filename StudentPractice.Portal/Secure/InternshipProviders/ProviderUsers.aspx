<%@ Page Language="C#" MasterPageFile="~/Secure/SecurePages.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.InternshipProviders.ProviderUsers"
    Title="Παραρτήματα Φορέα" CodeBehind="ProviderUsers.aspx.cs" %>

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
                <a id="btnAddProviderUser" runat="server" class="icon-btn bg-addNewItem" href="AddProviderUser.aspx">
                    <asp:Literal runat="server" Text="<%$ Resources:ProviderUser, AddUser %>" />
                </a>
            </div>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <dx:ASPxGridView ID="gvProviderUsers" runat="server" AutoGenerateColumns="False"
                        DataSourceID="odsProviderUsers" KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true"
                        Width="100%" DataSourceForceStandardPaging="true" OnCustomCallback="gvProviderUsers_CustomCallback"
                        OnCustomDataCallback="gvProviderUsers_CustomDataCallback" ClientInstanceName="gv">
                        <SettingsLoadingPanel Text="<%$ Resources:Grid, Loading %>" />
                        <SettingsPager PageSize="10" Summary-Text="<%$ Resources:Grid, PagingUsers %>" Summary-Position="Left" />
                        <Styles>
                            <Cell HorizontalAlign="Left" Font-Size="11px" />
                            <Header HorizontalAlign="Center" Wrap="true"/>
                        </Styles>
                        <Templates>
                            <EmptyDataRow>
                                <asp:Literal runat="server" Text="<%$ Resources:Grid, NoResults %>" />
                            </EmptyDataRow>
                        </Templates>
                        <Columns>

                            <dx:GridViewDataTextColumn Caption="Α/Α" Width="50px">
                                <CellStyle HorizontalAlign="Center" />
                                <DataItemTemplate>
                                    <%# Container.ItemIndex + 1 %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>

                            <dx:GridViewDataTextColumn FieldName="UserName" Name="UserName" Caption="<%$ Resources:Grid, GridCaption_UserAccountInfo %>" Width="200px">
                                <DataItemTemplate>
                                    <%# ((InternshipProvider)Container.DataItem).GetAccountDetails() %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>

                            <dx:GridViewDataTextColumn FieldName="ContactName" Name="Name" Caption="<%$ Resources:Grid, GridCaption_UserInfo %>" Width="200px">
                                <DataItemTemplate>
                                    <%# ((InternshipProvider)Container.DataItem).GetContactDetails() %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>

                            <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_EditUser %>" Width="100px">
                                <CellStyle HorizontalAlign="Center" />
                                <DataItemTemplate>
                                    <a runat="server" style="text-decoration: none" href='<%# string.Format("EditProviderUser.aspx?pID={0}", Eval("ID"))%>'
                                       tip="<%$ Resources:ProviderUser, EditUser %>" >
                                        <img runat="server" src="/_img/iconEdit.png" alt="<%$ Resources:ProviderUser, EditUser %>" />
                                    </a>
                                    
                                    <a runat="server" href="javascript:void(0);" style="text-decoration: none;"
                                        onclick='<%# string.Format("return doAction(\"lock\", {0}, \"ProviderUsers\");", Eval("ID"))%>'
                                        visible='<%# (bool)Eval("IsApproved") %>' tip="<%$ Resources:ProviderUser, DeactivateUser %>">
                                        <img runat="server" src="/_img/iconLock.png" alt="<%$ Resources:ProviderUser, DeactivateUser %>" />
                                    </a> 
                                    
                                    <a runat="server" href="javascript:void(0);" style="text-decoration: none;" 
                                        onclick='<%# string.Format("return doAction(\"unlock\", {0}, \"ProviderUsers\");", Eval("ID"))%>'
                                        visible='<%# !((bool)Eval("IsApproved")) %>' tip="<%$ Resources:ProviderUser, ReactivateUser %>">
                                        <img runat="server" src="/_img/iconUnLock.png" alt="<%$ Resources:ProviderUser, ReactivateUser %>" />
                                    </a> 
                                    
                                    <a runat="server"  href="javascript:void(0);" style="text-decoration: none;" 
                                       onclick='<%# string.Format("return doAction(\"delete\", {0}, \"ProviderUsers\");", Eval("ID"))%>' tip="<%$ Resources:ProviderUser, DeleteUser %>">
                                        <img runat="server" src="/_img/iconDelete.png" alt="<%$ Resources:ProviderUser, DeleteUser %>" />
                                    </a>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:ASPxGridView>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:ObjectDataSource ID="odsProviderUsers" runat="server" TypeName="StudentPractice.Portal.DataSources.Providers"
                SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
                EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsProviderUsers_Selecting">
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
