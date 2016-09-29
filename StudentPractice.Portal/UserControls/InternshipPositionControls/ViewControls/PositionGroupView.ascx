<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PositionGroupView.ascx.cs"
    Inherits="StudentPractice.Portal.UserControls.InternshipPositionControls.ViewControls.PositionGroupView" %>

<%@ Register TagPrefix="my" Src="~/UserControls/GenericControls/AcademicPositionRulesPopup.ascx" TagName="AcademicPositionRulesPopup" %>

<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">&raquo; 
            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:PositionGroupInput, Header %>" />
        </th>
    </tr>

    <tr>
        <th style="width: 180px">
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, Title %>" />
        </th>
        <td>
            <asp:Label ID="lblTitle" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>

    <tr id="trTotalPositions" runat="server">
        <th style="width: 180px">
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, TotalPositions %>" />
        </th>
        <td>
            <asp:Label ID="lblTotalPositions" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>

    <tr id="trPreAssignedPositions" runat="server">
        <th style="width: 180px">
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, PreassignedPositions %>" />
        </th>
        <td>
            <asp:Label ID="lblPreAssignedPositions" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>

    <tr>
        <th style="width: 180px">
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, Description %>" />
        </th>
        <td>
            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="12" Width="70%" Enabled="false" />
        </td>
    </tr>

    <tr>
        <th style="width: 180px">
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, Duration %>" />
        </th>
        <td>
            <asp:Label ID="lblDuration" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>

    <tr>
        <th style="width: 170px">
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, Country %>" />
        </th>
        <td>
            <asp:Label ID="lblCountry" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>

    <tr id="trPrefecture" runat="server">
        <th style="width: 180px">
            <asp:Literal ID="litPrefecture" runat="server" />
        </th>
        <td>
            <asp:Label ID="lblPrefecture" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>

    <tr>
        <th style="width: 180px">
            <asp:Literal ID="litCity" runat="server" />
        </th>
        <td>
            <asp:Label ID="lblCity" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>

    <tr>
        <th style="width: 180px">
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, TimeLimit %>" />
        </th>
        <td>
            <asp:Label ID="lblNoTimeLimit" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>

    <tr id="trStartDate" runat="server">
        <th style="width: 180px">
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, StartDate %>" />
        </th>
        <td>
            <asp:Label ID="lblStartDate" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
    <tr id="trEndDate" runat="server">
        <th style="width: 180px">
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, EndDate %>" />
        </th>
        <td>
            <asp:Label ID="lblEndDate" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
    <tr>
        <th style="width: 180px">
             <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, PositionType %>" />
        </th>
        <td>
            <asp:Label ID="lblPositionType" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
    <tr id="trSupervisor" runat="server" visible="false">
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, Supervisor %>" />
        </th>
        <td>
            <asp:Label ID="lblSupervisor" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
    <tr id="trSupervisorEmail" runat="server" visible="false">
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, SupervisorEmail %>" />
        </th>
        <td>
            <asp:Label ID="lblSupervisorEmail" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
    <tr>
        <th style="width: 180px">
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, ContactPhone %>" />
        </th>
        <td>
            <asp:Label ID="lblContactPhone" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
</table>
<br />

<table width="100%" class="dv">
    <tr>
        <th class="header">&raquo; 
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, PhysicanObject %>" />
        </th>
    </tr>
    <tr>
        <th>
            <dx:ASPxGridView ID="gvPhysicalObjects" runat="server" AutoGenerateColumns="False"
                KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true" Width="100%">
                <SettingsLoadingPanel Text="<%$ Resources:Grid, Loading %>" />
                <Styles>
                    <Cell Font-Size="11px" />
                </Styles>
                <Columns>
                    <dx:GridViewDataTextColumn FieldName="ID" Caption="<%$ Resources:Grid, GridCaption_Incrimental %>" HeaderStyle-HorizontalAlign="Center"
                        CellStyle-HorizontalAlign="Center" Width="50px">
                        <DataItemTemplate>
                            <%# Container.ItemIndex + 1 %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Name" Caption="<%$ Resources:Grid, GridCaption_Name %>" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# Eval("Name")%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                </Columns>
            </dx:ASPxGridView>
        </th>
    </tr>
</table>
<br />

<table id="tbAcademics" runat="server" width="100%" class="dv">
    <tr>
        <th class="header">&raquo;
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, Academics %>" /> 
        </th>
    </tr>
    <tr>
        <th>
            <asp:MultiView ID="mvAcademics" runat="server">

                <asp:View ID="vVisibleToAllAcademics" runat="server">
                    <div class="reminder" style="font-weight: bold; text-align: left">
                       <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, AcademicsAll %>" /> 
                    </div>
                </asp:View>

                <asp:View ID="vVisibleToCertainAcademics" runat="server">
                    <dx:ASPxGridView ID="gvAcademics" runat="server" AutoGenerateColumns="False" KeyFieldName="ID"
                        EnableRowsCache="false" EnableCallBacks="true" Width="100%">
                        <SettingsLoadingPanel Text="<%$ Resources:Grid, Loading %>" />
                        <SettingsPager PageSize="10" Summary-Text="<%$ Resources:Grid, PagingAcademics %>" Summary-Position="Left" />
                        <Styles>
                            <Cell Font-Size="11px" />
                        </Styles>
                        <Templates>
                            <EmptyDataRow>
                                <asp:Literal runat="server" Text="<%$ Resources:Grid, NoGPAAcademics %>" />
                            </EmptyDataRow>
                        </Templates>
                        <Columns>
                            <dx:GridViewDataTextColumn FieldName="ID" Caption="<%$ Resources:Grid, GridCaption_Incrimental %>" HeaderStyle-HorizontalAlign="Center"
                                CellStyle-HorizontalAlign="Center" Width="50px">
                                <DataItemTemplate>
                                    <%# Container.ItemIndex + 1 %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="Institution" Caption="<%$ Resources:Grid, GridCaption_Institution %>" CellStyle-HorizontalAlign="Left"
                                SortIndex="0" SortOrder="Ascending">
                                <DataItemTemplate>
                                    <%# Eval("Institution")%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="School" Caption="<%$ Resources:Grid, GridCaption_School %>" CellStyle-HorizontalAlign="Left"
                                SortIndex="1" SortOrder="Ascending">
                                <DataItemTemplate>
                                    <%# Eval("School")%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="Department" Caption="<%$ Resources:Grid, GridCaption_Department %>" CellStyle-HorizontalAlign="Left"
                                SortIndex="2" SortOrder="Ascending">
                                <DataItemTemplate>
                                    <%# Eval("Department")%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_Rules %>" CellStyle-HorizontalAlign="Center" Name="PositionRules" Width="30px">
                                <DataItemTemplate>
                                    <%# GetAcademicRulesLink((StudentPractice.BusinessModel.Academic)Container.Grid.GetRow(Container.VisibleIndex)) %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:ASPxGridView>

                    <my:AcademicPositionRulesPopup ID="ucAcademicPositionRulesPopup" runat="server" RequireTermsAcceptance="false" />
                    <script type="text/javascript">
                        var termsPopup = null;
                        Sys.Application.add_load(function () {
                            termsPopup = $find('<%= this.ucAcademicPositionRulesPopup.ClientID %>');
                        });

                        $(function () {
                            $('.btn-academicRules').live('click', function () {
                                var id = $(this).attr('data-aid');
                                termsPopup.show([id]);
                            });
                        });
                    </script>
                </asp:View>
            </asp:MultiView>
        </th>
    </tr>
</table>
