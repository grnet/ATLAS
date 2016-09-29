<%@ Page Language="C#" MasterPageFile="~/Secure/SecurePages.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.InternshipProviders.EditPreAssignedPosition"
    Title="Επεξεργασία Θέσης" CodeBehind="EditPreAssignedPosition.aspx.cs" %>

<%@ Register TagPrefix="ajaxControlToolkit" Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" %>
<%@ Register TagPrefix="my" Src="~/UserControls/InternshipPositionControls/ViewControls/PositionGroupView.ascx" TagName="PositionGroupView" %>

<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <link href="/_css/wizard.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <asp:LinkButton ID="cmdRefresh" runat="server" Style="display: none;" OnClick="cmdRefresh_Click" />

    <asp:MultiView ID="mvAcademics" runat="server">

        <asp:View ID="vVisibleToAllAcademics" runat="server">
            <br />
            <div class="reminder" style="font-weight: bold; text-align: left">
                <asp:Literal runat="server" Text="<%$ Resources:EditPreAssignedPosition, Reminder1 %>" />
                <br />
                <br />
                <asp:Literal runat="server" Text="<%$ Resources:EditPreAssignedPosition, Reminder2 %>" />
                <br />
                <br />
                <div style="clear: both; text-align: left">
                    <asp:LinkButton runat="server" CssClass="icon-btn bg-delete" OnClick="btnCancelUnPreAssignedPositions_Click" Text="<%$ Resources:EditPreAssignedPosition, RevokePosition %>" />
                </div>
            </div>
        </asp:View>

        <asp:View ID="vVisibleToCertainAcademics" runat="server">
            <br />
            <div class="reminder" style="font-weight: bold; text-align: left">
                <asp:Literal runat="server" Text="<%$ Resources:EditPreAssignedPosition, Reminder3 %>" />
                <br />
                <br />
                <asp:Literal runat="server" Text="<%$ Resources:EditPreAssignedPosition, Reminder2 %>" />
                <br />
                <br />
                <div style="clear: both; text-align: left">
                    <asp:LinkButton runat="server" CssClass="icon-btn bg-delete" OnClick="btnCancelUnPreAssignedPositions_Click" Text="<%$ Resources:EditPreAssignedPosition, RevokePosition %>" />
                </div>
            </div>
            <div style="padding: 30px 0px 10px;">
                <asp:LinkButton ID="btnAddAllAcademics" runat="server" CssClass="icon-btn bg-acceptAll" ValidationGroup="vgPosition" OnClick="btnAddAllAcademics_Click" Text="<%$ Resources:EditPreAssignedPosition, AvailableToAllAcademics %>" />
                <a id="btnAddAcademics" runat="server" class="icon-btn bg-add" href="javascript:void(0)">
                    <asp:Literal runat="server" Text="<%$ Resources:EditPreAssignedPosition, AddAcademics %>" />
                </a>
            </div>

            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <dx:ASPxGridView ID="gvAcademics" runat="server" AutoGenerateColumns="False" KeyFieldName="ID"
                        EnableRowsCache="false" EnableCallBacks="true" Width="100%">
                        <SettingsLoadingPanel Text="<%$ Resources:Grid, Loading %>" />
                        <SettingsPager PageSize="10" Summary-Text="<%$ Resources:Grid, PagingAcademics %>" Summary-Position="Left" />
                        <Styles>
                            <Cell Font-Size="11px" HorizontalAlign="Left" />
                        </Styles>
                        <Templates>
                            <EmptyDataRow>
                                <asp:Literal runat="server" Text="<%$ Resources:EditPreAssignedPosition, IsAvailableToAllAcademics %>" />
                            </EmptyDataRow>
                        </Templates>
                        <Columns>
                            <dx:GridViewDataTextColumn FieldName="ID" Caption="<%$ Resources:Grid, GridCaption_Incrimental %>" HeaderStyle-HorizontalAlign="Center"
                                CellStyle-HorizontalAlign="Center" Width="50px">
                                <DataItemTemplate>
                                    <%# Container.ItemIndex + 1 %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="Institution" Caption="<%$ Resources:Grid, GridCaption_Institution %>" SortIndex="0" SortOrder="Ascending">
                                <DataItemTemplate>
                                    <%# Eval("Institution")%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="School" Caption="<%$ Resources:Grid, GridCaption_School %>" SortIndex="1" SortOrder="Ascending">
                                <DataItemTemplate>
                                    <%# Eval("School")%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="Department" Caption="<%$ Resources:Grid, GridCaption_Department %>" SortIndex="2" SortOrder="Ascending">
                                <DataItemTemplate>
                                    <%# Eval("Department")%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_Rules %>" CellStyle-HorizontalAlign="Center"
                                CellStyle-VerticalAlign="Middle" HeaderStyle-Wrap="True" Name="PositionRules" Width="50px">
                                <DataItemTemplate>
                                    <%# GetAcademicRulesLink((StudentPractice.BusinessModel.Academic)Container.Grid.GetRow(Container.VisibleIndex)) %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:ASPxGridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:View>
    </asp:MultiView>
    <br />

    <table width="100%" class="dv">
        <tr>
            <th colspan="2" class="header">&raquo; 
                <asp:Literal runat="server" Text="<%$ Resources:EditPreAssignedPosition, ContactDetails %>" />
            </th>
        </tr>
        <tr>
            <th>
                <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, Supervisor %>" />
            </th>
            <td>
                <asp:TextBox ID="txtSupervisor" runat="server" MaxLength="500" ClientIDMode="Static" Width="60%" title="<%$ Resources:PositionInput, Supervisor %>" />
            </td>
        </tr>
        <tr>
            <th>
                <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, SupervisorEmail %>" />
            </th>
            <td>
                <asp:TextBox ID="txtSupervisorEmail" runat="server" MaxLength="256" ClientIDMode="Static" Width="60%"
                    onblur="$find('popSupervisorEmail').hidePopup();" title="<%$ Resources:PositionInput, SupervisorEmail %>" />
                <asp:RegularExpressionValidator ID="revSupervisorEmail" runat="server" Display="Dynamic" ControlToValidate="txtSupervisorEmail"
                    ValidationExpression="^([a-zA-Z0-9_\-])+(\.([a-zA-Z0-9_\-])+)*@((\[(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5]))\]))|((([a-zA-Z0-9])+(([\-])+([a-zA-Z0-9])+)*\.)+([a-zA-Z])+(([\-])+([a-zA-Z0-9])+)*))$"
                    ErrorMessage="<%$ Resources:PositionGroupInput, SupervisorEmailRegex %>">
                    <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:PositionGroupInput, SupervisorEmailRegex %>" />
                </asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <th>
                <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, ContactPhone %>" />
            </th>
            <td>
                <asp:TextBox ID="txtContactPhone" runat="server" MaxLength="10" ClientIDMode="Static"
                    Width="20%" title="<%$ Resources:PositionInput, ContactPhone %>" />
                <asp:RequiredFieldValidator ID="rfvContactPhone" Display="Dynamic" runat="server" ControlToValidate="txtContactPhone"
                    ValidationGroup="vgPosition" ErrorMessage="Το πεδίο 'Τηλέφωνο επικοινωνίας' είναι υποχρεωτικό">
                    <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:PositionGroupInput, ContactPhoneRequired %>" />
                </asp:RequiredFieldValidator>

                <asp:RegularExpressionValidator ID="revContactPhone" runat="server" ControlToValidate="txtContactPhone"
                    ValidationGroup="vgPosition" Display="Dynamic" ValidationExpression="^(2[0-9]{9})|(69[0-9]{8})$"
                    ErrorMessage="<%$ Resources:PositionGroupInput, ContactPhoneGrRegex %>">
                    <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:PositionGroupInput, ContactPhoneGrRegex %>" />
                </asp:RegularExpressionValidator>

            </td>
        </tr>
    </table>

    <br />

    <div style="clear: both; text-align: left">
        <asp:LinkButton ID="btnSubmit" runat="server" CssClass="icon-btn bg-accept" ValidationGroup="vgPosition"
            OnClick="btnSubmit_Click" Text="<%$ Resources:GlobalProvider, Global_Save %>"></asp:LinkButton>
        <asp:LinkButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" CausesValidation="false"
            CssClass="icon-btn bg-cancel" Text="<%$ Resources:GlobalProvider, Global_Cancel %>" />
    </div>

    <div style="margin-top: 20px">
        <my:PositionGroupView ID="ucPositionGroupView" runat="server" />
    </div>

    <script type="text/javascript">
        function cmdRefresh() {
            <%= this.Page.ClientScript.GetPostBackEventReference(this.cmdRefresh, string.Empty) %>;
        }
    </script>
</asp:Content>
