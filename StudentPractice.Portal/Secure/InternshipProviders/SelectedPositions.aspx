<%@ Page Language="C#" MasterPageFile="~/Secure/SecurePages.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.InternshipProviders.SelectedPositions" 
    Title="Επιλεγμένες Θέσεις Πρακτικής Άσκησης" CodeBehind="SelectedPositions.aspx.cs" %>

<%@ Register TagPrefix="my" Src="~/UserControls/GenericControls/FlashMessage.ascx" TagName="FlashMessage" %>
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
            <asp:LinkButton ID="cmdRefresh" runat="server" Style="display: none;" OnClick="cmdRefresh_Click" />
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
                        <asp:TextBox ID="txtGroupID" runat="server" Width="94%" />
                    </td>

                    <th class="second">
                        <asp:Literal runat="server" Text="<%$ Resources:Filter, PhysicalObject %>" />
                    </th>
                    <td class="second">
                        <asp:DropDownList ID="ddlPhysicalObject" runat="server" Width="98%" OnInit="ddlPhysicalObject_Init" />
                    </td>

                    <th class="third">
                        <asp:Literal runat="server" Text="<%$ Resources:Filter, StudentFirstName %>" />
                    </th>
                    <td class="third">
                        <asp:TextBox ID="txtFirstName" runat="server" Width="94%" />
                    </td>
                </tr>
                <tr>
                    <th class="first">
                        <asp:Literal runat="server" Text="<%$ Resources:Filter, PositionID %>" />
                    </th>
                    <td class="first">
                        <asp:TextBox ID="txtPositionID" runat="server" Width="94%" />
                    </td>

                    <th class="second">
                        <asp:Literal runat="server" Text="<%$ Resources:Filter, Country %>" />
                    </th>
                    <td class="second">
                        <asp:DropDownList ID="ddlCountry" CssClass="countrySelector" runat="server" ClientIDMode="Static" Width="98%"
                            OnInit="ddlCountry_Init" DataTextField="Name" DataValueField="ID" />
                    </td>

                    <th class="third">
                        <asp:Literal runat="server" Text="<%$ Resources:Filter, StudentLastName %>" />
                    </th>
                    <td class="third">
                        <asp:TextBox ID="txtLastName" runat="server" Width="94%" />
                    </td>
                </tr>
                <tr>
                    <th class="first">
                        <asp:Literal runat="server" Text="<%$ Resources:Filter, Title %>" />
                    </th>
                    <td class="first">
                        <asp:TextBox ID="txtTitle" runat="server" Width="94%" />
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
                    <th class="txts second">
                        <asp:Literal runat="server" Text="<%$ Resources:Filter, City %>" />
                    </th>
                    <td class="txts second">
                        <asp:TextBox ID="txtCity" runat="server" Width="94%" />
                    </td>


                    <th class="third">
                        <asp:Literal runat="server" Text="<%$ Resources:Filter, PressignedInstitution %>" />
                    </th>
                    <td class="third">
                        <asp:DropDownList ID="ddlInstitution" runat="server" Width="98%" OnInit="ddlInstitution_Init" />
                    </td>
                </tr>
                <tr>
                    <th class="first">
                        <asp:Literal runat="server" Text="<%$ Resources:Filter, PositionStatus %>" />
                    </th>
                    <td class="first">
                        <asp:DropDownList ID="ddlPositionStatus" runat="server" Width="98%" OnInit="ddlPositionStatus_Init" />
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
                        <asp:Literal runat="server" Text="<%$ Resources:Filter, PressignedDepartment %>" />
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
                    <th class="first" colspan="2">
                        <asp:Literal runat="server" Text="<%$ Resources:Filter, HideCompleted %>" />
                    </th>
                    <td class="first" colspan="2">
                        <asp:CheckBox ID="chbxHideCompletedPositions" runat="server" />
                    </td>
                </tr>
            </table>
            <div style="padding: 5px 0px 20px;">
                <asp:LinkButton ID="btnSearch" runat="server" Text="<%$ Resources:GlobalProvider, Button_Search %>" OnClick="btnSearch_Click" CssClass="icon-btn bg-search" />
                <a runat="server" class="icon-btn bg-colourExplanation" href="javascript:void(0)" onclick="window.open('/ColourExplanation.aspx','colourExplanation','toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=500, height=250'); return false;">
                    <asp:Literal runat="server" Text="<%$ Resources:GlobalProvider, Button_Color %>" />
                </a>
                <asp:LinkButton ID="btnExport" runat="server" Text="<%$ Resources:GlobalProvider, Button_Export %>" OnClick="btnExport_Click" CssClass="icon-btn bg-excel" />
                <asp:LinkButton ID="btnExportUsers" runat="server" Text="<%$ Resources:GlobalProvider, Button_ExportUsers %>" OnClick="btnExportUsers_Click" CssClass="icon-btn bg-excel" />
            </div>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <my:FlashMessage ID="fm" runat="server" CssClass="fade" />
                    <dx:ASPxGridView ID="gvPositions" runat="server" AutoGenerateColumns="False" DataSourceID="odsPositions"
                        KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true" Width="100%"
                        DataSourceForceStandardPaging="true" OnHtmlRowPrepared="gvPositions_HtmlRowPrepared">
                        <SettingsLoadingPanel Text="<%$ Resources:Grid, Loading %>" />
                        <SettingsPager PageSize="10" Summary-Text="<%$ Resources:Grid, Paging %>" Summary-Position="Left" />
                        <Styles>
                            <Header HorizontalAlign="Center" Wrap="True" />
                            <Cell HorizontalAlign="Left" Wrap="True" Font-Size="11px" />
                        </Styles>
                        <Templates>
                            <EmptyDataRow>
                                <asp:Literal runat="server" Text="<%$ Resources:Grid, NoResults %>" />
                            </EmptyDataRow>
                        </Templates>
                        <Columns>
                            <dx:GridViewDataTextColumn FieldName="UpdatedAt" Caption="<%$ Resources:Grid, GridCaption_UpdatedAt %>" Width="50px"
                                SortIndex="0" SortOrder="Descending">
                                <CellStyle HorizontalAlign="Left" />
                                <HeaderStyle Wrap="True" />
                                <DataItemTemplate>
                                    <%# ((DateTime)Eval("UpdatedAt")).ToString("dd/MM/yyyy") %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="GroupID" Caption="<%$ Resources:Grid, GridCaption_GroupID %>" Width="50px">
                                <CellStyle HorizontalAlign="Center" />
                                <DataItemTemplate>
                                    <%# Eval("GroupID")%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="ID" Caption="<%$ Resources:Grid, GridCaption_PositionID %>" Width="50px">
                                <CellStyle HorizontalAlign="Center" />
                                <DataItemTemplate>
                                    <%# Eval("ID")%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_PhysicalObject %>">
                                <DataItemTemplate>
                                    <%# ((InternshipPosition)Container.DataItem).GetPhysicalObjectDetails() %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="InternshipPositionGroup.Title" Caption="<%$ Resources:Grid, GridCaption_Title %>">
                                <DataItemTemplate>
                                    <%# Eval("InternshipPositionGroup.Title")%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="InternshipPositionGroup.Country.NameInGreek" Caption="<%$ Resources:Grid, GridCaption_Address %>">
                                <DataItemTemplate>
                                    <%# ((InternshipPosition)Container.DataItem).GetAddressDetails() %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn CellStyle-HorizontalAlign="Left" Width="125px">
                                <HeaderCaptionTemplate>
                                    <asp:Literal runat="server" Text="<%$ Resources:Grid, GridCaption_ImplementationDetails %>" />
                                    <img src="/_img/iconInformation.png" runat="server" class="tooltip" alt="Info" tip="<%$ Resources:Grid, GridCaption_ImplementationDetailsTip %>" />
                                </HeaderCaptionTemplate>
                                <DataItemTemplate>
                                    <%# ((InternshipPosition)Container.DataItem).GetImplementationDetails() %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn CellStyle-HorizontalAlign="Left" Caption="<%$ Resources:Grid, GridCaption_Office %>">
                                <DataItemTemplate>
                                    <%# GetOfficeDetails((InternshipPosition)Container.DataItem)%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn HeaderStyle-Wrap="true" Width="150px" Caption="<%$ Resources:Grid, GridCaption_PreassignedFor %>">
                                <DataItemTemplate>
                                    <%# GetStudentDetails((InternshipPosition)Container.DataItem)%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:ASPxGridView>
                </ContentTemplate>
            </asp:UpdatePanel>

            <dx:ASPxGridView ID="gvPositionsExport" runat="server" Visible="false" AutoGenerateColumns="False"
                KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="false" Width="100%"
                DataSourceForceStandardPaging="false">
                <SettingsPager Mode="ShowAllRecords" />
                <Styles>
                    <Header Wrap="True" />
                    <Cell Font-Size="11px" Wrap="True" HorizontalAlign="Left" />
                </Styles>
                <Templates>
                    <EmptyDataRow>
                        <asp:Literal runat="server" Text="<%$ Resources:Grid, NoResults %>" />
                    </EmptyDataRow>
                </Templates>
                <Columns>
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_GroupID %>" FieldName="InternshipPositionGroup.ID" Name="GroupID" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_PositionID %>" FieldName="ID" Name="ID" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_User %>" Name="User" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_Provider %>" FieldName="InternshipPositionGroup.ProviderID" Name="Provider" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_Title %>" FieldName="InternshipPositionGroup.Title" Name="Title" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_Description %>" FieldName="InternshipPositionGroup.Description" Name="Description" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_PhysicalObject %>" Name="PhysicalObjects" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_Duration %>" FieldName="InternshipPositionGroup.Duration" Name="Duration" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_Country %>" Name="Country" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_Prefecture %>" Name="Prefecture" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_City %>" Name="City" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_PositionType %>" Name="PositionType" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_PositionStatus %>" Name="PositionStatus" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_InstitutionPreAssign %>" Name="PreAssignedForAcademic.Institution" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_DepartmentPreAssign %>" Name="PreAssignedForAcademic.Department" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_MasterAccountName %>" Name="PreAssignedByMasterAccount.ContactName" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_MasterAccountPhone %>" Name="PreAssignedByMasterAccount.ContactPhone" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_MasterAccountEmail %>" Name="PreAssignedByMasterAccount.ContactEmail" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_StudentName %>" Name="AssignedToStudent.ContactName" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_StudentPhone %>" Name="AssignedToStudent.ContactMobilePhone" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_ImplementationStartDate %>" Name="ImplementationStartDate" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_ImplementationEndDate %>" Name="ImplementationEndDate" />
                    <%--<dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_FundingType %>" Name="FundingType" />--%>
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_GroupPhone %>" FieldName="InternshipPositionGroup.ContactPhone" Name="ContactPhone" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_Supervisor %>" FieldName="InternshipPositionGroup.Supervisor" Name="Supervisor" />
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_SupervisorEmail %>" FieldName="InternshipPositionGroup.SupervisorEmail" Name="SupervisorEmail" />
                </Columns>
            </dx:ASPxGridView>

            <dx:ASPxGridViewExporter ID="gveIntershipPositions" runat="server" GridViewID="gvPositionsExport" OnRenderBrick="gveIntershipPositions_RenderBrick">
            </dx:ASPxGridViewExporter>

            <asp:ObjectDataSource ID="odsPositions" runat="server" TypeName="StudentPractice.Portal.DataSources.Positions"
                SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
                EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsPositions_Selecting">
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
