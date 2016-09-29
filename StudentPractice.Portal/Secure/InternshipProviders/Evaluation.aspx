<%@ Page Title="" Language="C#" MasterPageFile="~/Secure/SecurePages.Master" AutoEventWireup="true" CodeBehind="Evaluation.aspx.cs" Inherits="StudentPractice.Portal.Secure.InternshipProviders.Evaluation" %>

<%@ Import Namespace="StudentPractice.BusinessModel" %>
<%@ Import Namespace="StudentPractice.Portal" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">

    <asp:MultiView ID="mvAccount" runat="server" ActiveViewIndex="0">
        <asp:View ID="vAccountNotVerified" runat="server">
            <div class="reminder">
                <asp:Label ID="lblVerificationError" runat="server" />
            </div>
        </asp:View>
        <asp:View ID="vAccountVerified" runat="server">
            <asp:LinkButton ID="cmdRefresh" runat="server" Style="display: none;" OnClick="cmdRefresh_Click" />
            <script type="text/javascript">
                function cmdRefresh() {
                    <%= this.Page.ClientScript.GetPostBackEventReference(this.cmdRefresh, string.Empty) %>;
                }
            </script>

            <dx:ASPxPageControl ID="dxTabs" runat="server" Width="100%">
                <TabPages>
                    <dx:TabPage Name="Offices" Text="<%$ Resources:Evaluation, TabOffices %>">
                        <ContentCollection>
                            <dx:ContentControl>

                                <table class="dv">
                                    <tr>
                                        <th colspan="2" class="popupHeader">
                                            <asp:Literal runat="server" Text="<%$ Resources:Filter, SearchFilters %>" />
                                        </th>
                                    </tr>
                                    <tr>
                                        <th style="width: 120px">
                                            <asp:Literal runat="server" Text="<%$ Resources:Filter, Institution %>" />
                                        </th>
                                        <td style="width: 330px">
                                            <asp:DropDownList ID="ddlInstitution" runat="server" TabIndex="10" Width="95%" OnInit="ddlInstitution_Init" />
                                        </td>
                                    </tr>
                                </table>

                                <div style="padding: 5px 0px 10px;">
                                    <asp:LinkButton ID="btnSearch" runat="server" Text="<%$ Resources:GlobalProvider, Button_Search %>" OnClick="btnSearch_Click"
                                        CssClass="icon-btn bg-search" />
                                    <%--<asp:LinkButton ID="btnExport" runat="server" Text="Εξαγωγή σε Excel" OnClick="btnExport_Click"
            CssClass="icon-btn bg-excel" />--%>
                                </div>

                                <dx:ASPxGridView ID="gvOffices" runat="server" AutoGenerateColumns="False" DataSourceID="odsOffices"
                                    KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true" Width="100%"
                                    DataSourceForceStandardPaging="true">
                                    <SettingsLoadingPanel Text="<%$ Resources:Grid, Loading %>" />
                                    <SettingsPager PageSize="10" Summary-Text="<%$ Resources:Grid, PagingOffices %>" Summary-Position="Left" />
                                    <Styles>
                                        <Cell Font-Size="11px" />
                                    </Styles>
                                    <Templates>
                                        <EmptyDataRow>
                                            <asp:Literal runat="server" Text="<%$ Resources:Grid, NoResults %>" />
                                        </EmptyDataRow>
                                    </Templates>
                                    <Columns>
                                        <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_OfficeType %>" FieldName="OfficeTypeInt" Name="OfficeType"
                                            CellStyle-HorizontalAlign="Left">
                                            <DataItemTemplate>
                                                <%# ((enOfficeType)Eval("OfficeType")).GetLabel() %>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_Office %>" CellStyle-HorizontalAlign="Left">
                                            <DataItemTemplate>
                                                <%# GetOfficeDetails((InternshipOffice)Container.DataItem)%>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Name="Evaluate" Caption="<%$ Resources:Grid, GridCaption_Evaluation %>" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <DataItemTemplate>
                                                <a id="A1" runat="server" href="javascript:void(0)" visible='<%# CanEvaluateOffice((InternshipOffice)Container.DataItem) %>'
                                                    onclick=<%# string.Format("popUp.show('/Secure/Common/Questionnaire.aspx?qType={0}&entityID={1}','{2}', cmdRefresh, 800, 610);", enQuestionnaireType.ProviderForOffice.GetValue(), Eval("ID"), Resources.Evaluation.Evaluate)%>>
                                                    <img src="/_img/iconEdit.png" runat="server" alt="<%$ Resources:Evaluation, Evaluate %>" title="<%$ Resources:Evaluation, Evaluate %>" />
                                                </a>
                                                <a id="A2" runat="server" href="javascript:void(0)" visible='<%# !CanEvaluateOffice((InternshipOffice)Container.DataItem) %>'
                                                    onclick=<%# string.Format("popUp.show('/Secure/Common/Questionnaire.aspx?qType={0}&entityID={1}','{2}', cmdRefresh, 800, 610);", enQuestionnaireType.ProviderForOffice.GetValue(), Eval("ID"), Resources.Evaluation.Evaluate)%>>
                                                    <img src="/_img/iconView.png" runat="server" alt="<%$ Resources:Evaluation, PreviewEvaluation %>" title="<%$ Resources:Evaluation, PreviewEvaluation %>" />
                                                </a>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                    </Columns>
                                </dx:ASPxGridView>

                                <asp:ObjectDataSource ID="odsOffices" runat="server" TypeName="StudentPractice.Portal.DataSources.Offices"
                                    SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria" EnablePaging="true"
                                    SortParameterName="sortExpression" OnSelecting="odsOffices_Selecting" OnSelected="odsOffices_Selected">
                                    <SelectParameters>
                                        <asp:Parameter Name="criteria" Type="Object" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>

                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>
                    <dx:TabPage Name="Students" Text="<%$ Resources:Evaluation, TabStudents %>">
                        <ContentCollection>
                            <dx:ContentControl>

                                <table class="dv">
                                    <tr>
                                        <th colspan="4" class="popupHeader">
                                            <asp:Literal runat="server" Text="<%$ Resources:Filter, SearchFilters %>" />
                                        </th>
                                    </tr>
                                    <tr>
                                        <th style="width: 120px">
                                            <asp:Literal runat="server" Text="<%$ Resources:Filter, Title %>" />
                                        </th>
                                        <td style="width: 230px">
                                            <asp:TextBox ID="txtTitle" runat="server" TabIndex="6" Width="95%" />
                                        </td>
                                        <th style="width: 120px">
                                            <asp:Literal runat="server" Text="<%$ Resources:Filter, PositionID %>" />
                                        </th>
                                        <td style="width: 230px">
                                            <asp:TextBox ID="txtPositionID" runat="server" TabIndex="7" Width="95%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th style="width: 120px">
                                            <asp:Literal runat="server" Text="<%$ Resources:Filter, StudentFirstName %>" />
                                        </th>
                                        <td style="width: 230px">
                                            <asp:TextBox ID="txtFirstName" runat="server" TabIndex="6" Width="95%" />
                                        </td>
                                        <th style="width: 120px">
                                            <asp:Literal runat="server" Text="<%$ Resources:Filter, StudentLastName %>" />
                                        </th>
                                        <td style="width: 230px">
                                            <asp:TextBox ID="txtLastName" runat="server" TabIndex="7" Width="95%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th style="width: 120px">
                                            <asp:Literal runat="server" Text="<%$ Resources:Filter, StudentNumber %>" />
                                        </th>
                                        <td style="width: 230px">
                                            <asp:TextBox ID="txtStudentNumber" runat="server" TabIndex="8" Width="95%" />
                                        </td>
                                        <th>&nbsp;</th>
                                        <th>&nbsp;</th>
                                    </tr>
                                </table>

                                <div style="padding: 5px 0px 10px;">
                                    <asp:LinkButton ID="btnSearchStudents" runat="server" Text="<%$ Resources:GlobalProvider, Button_Search %>" OnClick="btnSearchStudents_Click"
                                        CssClass="icon-btn bg-search" />
                                    <%--<asp:LinkButton ID="btnExport" runat="server" Text="Εξαγωγή σε Excel" OnClick="btnExport_Click"
            CssClass="icon-btn bg-excel" />--%>
                                </div>

                                <dx:ASPxGridView ID="gvPositions" runat="server" AutoGenerateColumns="False" DataSourceID="odsPositions"
                                    KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true" Width="100%"
                                    DataSourceForceStandardPaging="true">
                                    <SettingsLoadingPanel Text="<%$ Resources:Grid, Loading %>" />
                                    <SettingsPager PageSize="10" Summary-Text="<%$ Resources:Grid, Paging %>" Summary-Position="Left" />
                                    <Styles>
                                        <Header HorizontalAlign="Center" Wrap="True" />
                                        <Cell HorizontalAlign="Left" Wrap="True" Font-Size="11px" />
                                    </Styles>
                                    <Templates>
                                        <EmptyDataRow>
                                            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Grid, NoResults %>" />
                                        </EmptyDataRow>
                                    </Templates>
                                    <Columns>
                                        <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_ViewPosition %>" Width="40px" Name="ViewPosition" CellStyle-HorizontalAlign="Center">
                                            <DataItemTemplate>
                                                <a id="A4" runat="server" style="text-decoration: none" href="javascript:void(0)"
                                                    onclick=<%# string.Format("popUp.show('ViewCompletedPosition.aspx?pID={0}', '{1}');", Eval("ID"), Resources.Evaluation.ShowPosition)%>
                                                    tip="<%$ Resources:Grid, GridCaption_ViewPosition %>">
                                                    <img src="/_img/iconView.png" runat="server" alt="<%$ Resources:Grid, GridCaption_ViewPosition %>" />
                                                </a>
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
                                        <dx:GridViewDataTextColumn FieldName="InternshipPositionGroup.Title" Caption="<%$ Resources:Grid, GridCaption_Title %>" Width="450px">
                                            <DataItemTemplate>
                                                <%# Eval("InternshipPositionGroup.Title")%>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn HeaderStyle-Wrap="true" Width="300px" Caption="<%$ Resources:Grid, GridCaption_AssignedStudent %>">
                                            <DataItemTemplate>
                                                <%# GetStudentDetails((InternshipPosition)Container.DataItem)%>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Name="Evaluate" Caption="<%$ Resources:Grid, GridCaption_Evaluation %>" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="50px">
                                            <DataItemTemplate>
                                                <a id="A1" runat="server" href="javascript:void(0)" visible='<%# CanEvaluateStudent((InternshipPosition)Container.DataItem) %>'
                                                    onclick=<%# string.Format("popUp.show('/Secure/Common/Questionnaire.aspx?qType={0}&entityID={1}&positionID={2}','{3}', cmdRefresh, 800, 610);", enQuestionnaireType.ProviderForStudent.GetValue(), Eval("AssignedToStudentID"), Eval("ID"), Resources.Evaluation.Evaluate)%>>
                                                    <img src="/_img/iconEdit.png" runat="server" alt="<%$ Resources:Evaluation, Evaluate %>" title="<%$ Resources:Evaluation, Evaluate %>" />
                                                </a>
                                                <a id="A3" runat="server" href="javascript:void(0)" visible='<%# !CanEvaluateStudent((InternshipPosition)Container.DataItem) %>'
                                                    onclick=<%# string.Format("popUp.show('/Secure/Common/Questionnaire.aspx?qType={0}&entityID={1}&positionID={2}','{3}', cmdRefresh, 800, 610);", enQuestionnaireType.ProviderForStudent.GetValue(), Eval("AssignedToStudentID"), Eval("ID"), Resources.Evaluation.Evaluate)%>>
                                                    <img src="/_img/iconView.png" runat="server" alt="<%$ Resources:Evaluation, PreviewEvaluation %>" title="<%$ Resources:Evaluation, PreviewEvaluation %>" />
                                                </a>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                    </Columns>
                                </dx:ASPxGridView>

                                <asp:ObjectDataSource ID="odsPositions" runat="server" TypeName="StudentPractice.Portal.DataSources.Positions"
                                    SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria" EnablePaging="true"
                                    SortParameterName="sortExpression" OnSelecting="odsPositions_Selecting" OnSelected="odsPositions_Selected">
                                    <SelectParameters>
                                        <asp:Parameter Name="criteria" Type="Object" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>

                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>
                </TabPages>
            </dx:ASPxPageControl>
        </asp:View>
    </asp:MultiView>
</asp:Content>
