<%@ Page Language="C#" MasterPageFile="~/Secure/SecurePages.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.InternshipProviders.InternshipPositions" 
    Title="Διαχείριση Θέσεων Πρακτικής Άσκησης" CodeBehind="InternshipPositions.aspx.cs" %>

<%@ Register Src="~/UserControls/GenericControls/FlashMessage.ascx" TagName="FlashMessage" TagPrefix="my" %>
<%@ Import Namespace="StudentPractice.BusinessModel" %>
<%@ Import Namespace="StudentPractice.Portal" %>

<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <style type="text/css">
        .dxgvHeader td {
            font-size: 11px;
        }

        .dvTable {
            width: 1160px !important;
        }

            .dvTable tr th {
                width: 140px !important;
            }

            .dvTable tr td {
                width: 240px !important;
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
            <my:FlashMessage ID="fm" runat="server" CssClass="fade" />
            <table class="dv dvTable">
                <tr>
                    <th colspan="6" class="popupHeader">
                        <asp:Literal runat="server" Text="<%$ Resources:Filter, SearchFilters %>" />
                    </th>
                </tr>
                <tr>
                    <th class="first">
                        <asp:Literal runat="server" Text="<%$ Resources:Filter, GroupID %>" />
                    </th>
                    <td class="first">
                        <asp:TextBox ID="txtGroupID" runat="server" Width="92%" />
                    </td>

                    <th class="second">
                        <asp:Literal runat="server" Text="<%$ Resources:Filter, Country %>" />
                    </th>
                    <td class="second">
                        <asp:DropDownList ID="ddlCountry" CssClass="countrySelector" runat="server" ClientIDMode="Static" Width="98%"
                            OnInit="ddlCountry_Init" DataTextField="Name" DataValueField="ID" ViewStateMode="Disabled" />
                    </td>

                    <th class="third">
                        <asp:Literal runat="server" Text="<%$ Resources:Filter, Institution %>" />
                    </th>
                    <td class="third">
                        <asp:DropDownList ID="ddlInstitution" runat="server" Width="98%" OnInit="ddlInstitution_Init" />
                    </td>
                </tr>
                <tr>
                    <th class="first">
                        <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Filter, Status %>" />
                    </th>
                    <td class="first">
                        <asp:DropDownList ID="ddlPositionGroupStatus" runat="server" Width="98%" TabIndex="1" OnInit="ddlPositionGroupStatus_Init" />
                    </td>

                    <th class="ddls second">
                        <asp:Literal runat="server" Text="<%$ Resources:Filter, Prefecture %>" />
                    </th>
                    <td class="ddls second">
                        <asp:DropDownList ID="ddlPrefecture" runat="server" ClientIDMode="Static" Width="98%"
                            DataTextField="Name" DataValueField="ID" />
                        <act:CascadingDropDown ID="cddPrefecture" runat="server" TargetControlID="ddlPrefecture"
                            ParentControlID="ddlCountry" Category="Prefectures" PromptText="<%$ Resources:GlobalProvider, DropDownIndifferent %>"
                            ServicePath="~/PortalServices/Services.asmx" ServiceMethod="GetPrefectures" LoadingText="Παρακαλω περιμένετε">
                        </act:CascadingDropDown>
                    </td>

                    <th class="second txts">
                        <asp:Literal runat="server" Text="<%$ Resources:Filter, City %>" />
                    </th>
                    <td class="second txts">
                        <asp:TextBox ID="txtCity" runat="server" Width="92%" />
                    </td>

                    <th class="third">
                        <asp:Literal runat="server" Text="<%$ Resources:Filter, Department %>" />
                    </th>
                    <td class="third">
                        <asp:DropDownList ID="ddlDepartment" runat="server" Width="98%" DataTextField="Name"
                            DataValueField="ID" />
                        <act:CascadingDropDown ID="cddDepartment" runat="server" TargetControlID="ddlDepartment"
                            ParentControlID="ddlInstitution" Category="Academics" PromptText="<%$ Resources:GlobalProvider, DropDownIndifferent %>"
                            ServicePath="~/PortalServices/Services.asmx" ServiceMethod="GetActiveAcademics" LoadingText="Παρακαλω περιμένετε">
                        </act:CascadingDropDown>
                    </td>
                </tr>
                <tr>
                    <th class="first">
                        <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Filter, PhysicalObject %>" />
                    </th>
                    <td class="first">
                        <asp:DropDownList ID="ddlPhysicalObject" runat="server" Width="98%" OnInit="ddlPhysicalObject_Init" />
                    </td>

                    <th class="ddls second">
                        <asp:Literal runat="server" Text="<%$ Resources:Filter, KaliClity %>" />
                    </th>
                    <td class="ddls second">
                        <asp:DropDownList ID="ddlCity" runat="server" ClientIDMode="Static" Width="98%" DataTextField="Name"
                            DataValueField="ID" />
                        <act:CascadingDropDown ID="cddCity" runat="server" TargetControlID="ddlCity"
                            ParentControlID="ddlPrefecture" Category="Cities" PromptText="<%$ Resources:GlobalProvider, DropDownIndifferent %>"
                            ServicePath="~/PortalServices/Services.asmx" ServiceMethod="GetCities" LoadingText="Παρακαλω περιμένετε">
                        </act:CascadingDropDown>
                    </td>
                    <th class="txts second"></th>
                    <td class="txts second"></td>


                    <th class="third">
                        <asp:Literal runat="server" Text="<%$ Resources:Filter, CreationType %>" />
                    </th>
                    <td class="third">
                        <asp:DropDownList ID="ddlCreationType" runat="server" TabIndex="3" Width="98%" OnInit="ddlCreationType_Init" />
                    </td>
                </tr>
                <tr>
                    <th class="first" colspan="2">
                        <asp:Literal runat="server" Text="<%$ Resources:Filter, Revoked %>" />
                    </th>
                    <td class="first" colspan="2">
                        <asp:CheckBox ID="chbxShowRevokedPositions" runat="server" />
                    </td>
                </tr>
            </table>
            <div style="padding: 5px 0px 20px;">
                <asp:LinkButton ID="btnSearch" runat="server" Text="<%$ Resources:GlobalProvider, Button_Search %>" OnClick="btnSearch_Click" CssClass="icon-btn bg-search" />
                <asp:HyperLink runat="server" Text="<%$ Resources:GlobalProvider, Button_AddPosition %>" class="icon-btn bg-addNewItem" NavigateUrl="PositionDetails.aspx" />
                <asp:LinkButton ID="btnExport" runat="server" Text="<%$ Resources:GlobalProvider, Button_Export %>" OnClick="btnExport_Click" CssClass="icon-btn bg-excel" />
                <asp:LinkButton ID="btnExportUsers" runat="server" Text="<%$ Resources:GlobalProvider, Button_ExportUsers %>" OnClick="btnExportUsers_Click" CssClass="icon-btn bg-excel" />
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <dx:ASPxGridView ID="gvPositionGroups" ClientInstanceName="gv" runat="server"
                        AutoGenerateColumns="False" DataSourceID="odsPositionGroups" KeyFieldName="ID"
                        EnableRowsCache="false" EnableCallBacks="true" Width="100%" DataSourceForceStandardPaging="true"
                        OnHtmlRowPrepared="gvPositionGroups_HtmlRowPrepared" OnCustomCallback="gvPositionGroups_CustomCallback"
                        OnCustomDataCallback="gvPositionGroups_CustomDataCallback">
                        <SettingsLoadingPanel Text="<%$ Resources:Grid, Loading %>" />
                        <SettingsPager PageSize="10" Summary-Text="<%$ Resources:Grid, Paging %>" Summary-Position="Left" />
                        <Styles>
                            <Header HorizontalAlign="Center" Wrap="True" />
                            <Cell HorizontalAlign="Left" Wrap="True" Font-Size="11px" />
                        </Styles>
                        <Templates>
                            <EmptyDataRow>
                                <asp:Literal runat="server" Text="<%$ Resources:Grid, NoPositions %>" />
                            </EmptyDataRow>
                        </Templates>
                        <Columns>
                            <dx:GridViewDataTextColumn Width="100px">
                                <CellStyle HorizontalAlign="Center" />
                                <DataItemTemplate>
                                    <a runat="server" href="javascript:void(0);" class="btn"
                                        onclick='<%# string.Format("return doAction(\"edit\", {0}, \"InternshipPositions\");", Eval("ID"))%>'
                                        visible='<%# NeedsEdit((InternshipPositionGroup)Container.DataItem) %>'>
                                        <asp:Literal runat="server" Text="<%$ Resources:InternshipPositions, Action_Edit %>" />
                                    </a>
                                    <a runat="server" href="javascript:void(0);" class="btn"
                                        onclick='<%# string.Format("return doAction(\"publish\", {0}, \"InternshipPositions\");", Eval("ID"))%>'
                                        visible='<%# CanPublishGroup((InternshipPositionGroup)Container.DataItem) %>'>
                                        <asp:Literal runat="server" Text="<%$ Resources:InternshipPositions, Action_Publish %>" />
                                    </a>
                                    <a runat="server" href="javascript:void(0);" class="btn"
                                        onclick='<%# string.Format("return doAction(\"unpublish\", {0}, \"InternshipPositions\");", Eval("ID"))%>'
                                        visible='<%# CanUnPublishGroup((InternshipPositionGroup)Container.DataItem) %>'>
                                        <asp:Literal runat="server" Text="<%$ Resources:InternshipPositions, Action_Unpublish %>" />
                                    </a>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="ID" Caption="<%$ Resources:Grid, GridCaption_ID %>" Width="50px" 
                                SortIndex="0" SortOrder="Descending">
                                <CellStyle HorizontalAlign="Center" />
                                <DataItemTemplate>
                                    <%# Eval("ID")%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_PhysicalObject %>">
                                <DataItemTemplate>
                                    <%# ((InternshipPositionGroup)Container.DataItem).GetPhysicalObjectDetails() %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="Title" Caption="<%$ Resources:Grid, GridCaption_Title %>">
                                <DataItemTemplate>
                                    <%# Eval("Title")%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="Country.NameInGreek" Caption="<%$ Resources:Grid, GridCaption_Address %>" Width="150px">
                                <DataItemTemplate>
                                    <%# ((InternshipPositionGroup)Container.DataItem).GetAddressDetails() %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="TotalPositions" Caption="<%$ Resources:Grid, GridCaption_TotalPositions %>" Width="50px">
                                <CellStyle HorizontalAlign="Center" />
                                <DataItemTemplate>
                                    <%# ((InternshipPositionGroup)Container.DataItem).GetTotalPositions() %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="PreAssignedPositions" Caption="<%$ Resources:Grid, GridCaption_PreassignedPositions %>" Width="50px">
                                <CellStyle HorizontalAlign="Center" />
                                <DataItemTemplate>
                                    <%# GetPreAssignedPositions((InternshipPositionGroup)Container.DataItem)%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_Actions %>" Width="100px">
                                <CellStyle HorizontalAlign="Center" />
                                <DataItemTemplate>
                                    <a runat="server" href="javascript:void(0)"
                                        onclick=<%# string.Format("popUp.show('ViewPosition.aspx?gID={0}','{1}', null, 800, 610);", Eval("ID"), Resources.InternshipPositions.Action_Preview)%> 
                                        tip="<%$ Resources:InternshipPositions, Action_Preview %>">
                                        <asp:Image runat="server" ImageUrl="~/_img/iconView.png" AlternateText="<%$ Resources:InternshipPositions, Action_Preview %>" />
                                    </a>
                                    <a runat="server" href='<%# string.Format("PositionDetails.aspx?gID={0}", Eval("ID")) %>'
                                        visible="<%# CanEditGroup((InternshipPositionGroup)Container.DataItem) %>"
                                        tip="<%$ Resources:InternshipPositions, Action_Edit %>">
                                        <asp:Image runat="server" ImageUrl="~/_img/iconEdit.png" AlternateText="<%$ Resources:InternshipPositions, Action_Edit %>" />
                                    </a>
                                    <a runat="server" href="javascript:void(0);"
                                        onclick='<%# string.Format("return doAction(\"delete\", {0}, \"InternshipPositions\");", Eval("ID"))%>'
                                        visible='<%# CanDeleteGroup((InternshipPositionGroup)Container.DataItem) %>'
                                        tip="<%$ Resources:InternshipPositions, Action_Delete %>">
                                        <asp:Image runat="server" ImageUrl="~/_img/iconDelete.png" AlternateText="<%$ Resources:InternshipPositions, Action_Delete %>" />
                                    </a>
                                    <a runat="server" href="javascript:void(0);"
                                        onclick='<%# string.Format("return doAction(\"cancel\", {0}, \"InternshipPositions\");", Eval("ID"))%>'
                                        visible='<%# CanCancelGroup((InternshipPositionGroup)Container.DataItem) %>'
                                        tip="<%$ Resources:InternshipPositions, Action_Revoke %>">
                                        <asp:Image runat="server" ImageUrl="~/_img/iconDelete.png" AlternateText="<%$ Resources:InternshipPositions, Action_Revoke %>" />
                                    </a>
                                    <a runat="server" href="javascript:void(0);" 
                                        onclick='<%# string.Format("return doAction(\"clone\", {0}, \"InternshipPositions\");", Eval("ID"))%>'
                                        visible='<%# CanCloneGroup((InternshipPositionGroup)Container.DataItem) %>'
                                        tip="<%$ Resources:InternshipPositions, Action_Clone %>">
                                        <asp:Image runat="server" ImageUrl="~/_img/iconClone.png" AlternateText="<%$ Resources:InternshipPositions, Action_Clone %>" />
                                    </a>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:ASPxGridView>
                </ContentTemplate>
            </asp:UpdatePanel>
            <dx:ASPxGridView ID="gvPositionGroupsExport" runat="server" Visible="false" AutoGenerateColumns="False"
                KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="false" Width="100%"
                DataSourceForceStandardPaging="false">
                <SettingsPager Mode="ShowAllRecords" />
                <Styles>
                    <Header Wrap="True" />
                    <Cell Font-Size="11px" Wrap="True" HorizontalAlign="Left" />
                </Styles>
                <Templates>
                    <EmptyDataRow>
                        <asp:Literal runat="server" Text="<%$ Resources:Grid, NoPositions %>" />
                    </EmptyDataRow>
                </Templates>
                <Columns>
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_ID %>" FieldName="ID" Name="GroupID" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_User %>" Name="User" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_Provider %>" FieldName="ProviderID" Name="Provider" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_Title %>" FieldName="Title" Name="Title" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_Description %>" FieldName="Description" Name="Description" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_Duration %>" FieldName="Duration" Name="Duration" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_Country %>" Name="Country" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_Prefecture %>" Name="Prefecture" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_City %>" Name="City" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_GroupStatus %>" Name="PositionGroupStatus" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_TotalPositions %>" FieldName="TotalPositions" Name="TotalPositions" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_PreassignedPositions %>" FieldName="PreAssignedPositions" Name="PreAssignedPositions" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_TimeAvailable %>" Name="TimeAvailable" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_PositionType %>" Name="PositionType" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_ContactPhone %>" FieldName="ContactPhone" Name="ContactPhone" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_Supervisor %>" FieldName="Supervisor" Name="Supervisor" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_SupervisorEmail %>" FieldName="SupervisorEmail" Name="SupervisorEmail" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_PhysicalObject %>" Name="PhysicalObjects" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_InstitutionsAccess %>" Name="Institutions" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_DepartmentsAccess %>" Name="Departments" />
                </Columns>
            </dx:ASPxGridView>
            <dx:ASPxGridViewExporter ID="gveIntershipPositionGroups" runat="server" GridViewID="gvPositionGroupsExport" OnRenderBrick="gveIntershipPositionGroups_RenderBrick">
            </dx:ASPxGridViewExporter>
            <asp:ObjectDataSource ID="odsPositionGroups" runat="server" TypeName="StudentPractice.Portal.DataSources.PositionGroups"
                SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
                EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsPositionGroups_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="criteria" Type="Object" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </asp:View>
    </asp:MultiView>
</asp:Content>
