<%@ Page Title="" Language="C#" MasterPageFile="~/Secure/SecurePages.Master" AutoEventWireup="true" CodeBehind="Evaluation.aspx.cs" Inherits="StudentPractice.Portal.Secure.InternshipOffices.Evaluation" %>

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
                    <dx:TabPage Name="Providers" Text="Φορείς Υποδοχής">
                        <ContentCollection>
                            <dx:ContentControl>

                                <table class="dv">
                                    <tr>
                                        <th colspan="4" class="popupHeader">Φίλτρα Αναζήτησης
                                        </th>
                                    </tr>
                                    <tr>
                                        <th style="width: 100px">Επωνυμία:
                                        </th>
                                        <td style="width: 200px">
                                            <asp:TextBox ID="txtProviderName" runat="server" TabIndex="11" Width="95%" />
                                        </td>
                                        <th style="width: 100px">Α.Φ.Μ.:
                                        </th>
                                        <td style="width: 200px">
                                            <asp:TextBox ID="txtProviderAFM" runat="server" TabIndex="10" Width="95%" />
                                        </td>
                                    </tr>
                                </table>
                                <div style="padding: 5px 0px 10px;">
                                    <asp:LinkButton ID="btnSearch" runat="server" Text="Αναζήτηση" OnClick="btnSearch_Click"
                                        CssClass="icon-btn bg-search" />
                                    <%--<asp:LinkButton ID="btnExport" runat="server" Text="Εξαγωγή σε Excel" OnClick="btnExport_Click"
            CssClass="icon-btn bg-excel" />--%>
                                </div>

                                <dx:ASPxGridView ID="gvProviders" runat="server" AutoGenerateColumns="False" DataSourceID="odsProviders"
                                    KeyFieldName="ID" EnableRowsCache="false" Width="100%"
                                    DataSourceForceStandardPaging="true">
                                    <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
                                    <SettingsPager PageSize="10" Summary-Text="Σελίδα {0} από {1} ({2} Φορείς)" Summary-Position="Left" />
                                    <Styles>
                                        <Cell Font-Size="11px" />
                                    </Styles>
                                    <Templates>
                                        <EmptyDataRow>
                                            Δεν βρέθηκαν αποτελέσματα
                                        </EmptyDataRow>
                                    </Templates>
                                    <Columns>
                                        <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" HeaderStyle-HorizontalAlign="Center"
                                            CellStyle-HorizontalAlign="Center" Width="50px" SortIndex="0" SortOrder="Descending">
                                            <DataItemTemplate>
                                                <%# Eval("ID") %>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Στοιχεία Φορέα" FieldName="Name" CellStyle-HorizontalAlign="Left">
                                            <DataItemTemplate>
                                                <%# GetProviderDetails((InternshipProvider)Container.DataItem)%>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Στοιχεία Επικοινωνίας" Name="ContactDetails" FieldName="ContactEmail" CellStyle-HorizontalAlign="Left">
                                            <DataItemTemplate>
                                                <%# GetProviderContactDetails((InternshipProvider)Container.DataItem)%>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Name="Evaluate" Caption="Αξιολόγηση" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <DataItemTemplate>
                                                <a id="A1" runat="server" href="javascript:void(0)" visible='<%# CanEvaluateProvider((InternshipProvider)Container.DataItem) %>'
                                                    onclick=<%# string.Format("popUp.show('/Secure/Common/Questionnaire.aspx?qType={0}&entityID={1}','Αξιολόγηση', cmdRefresh, 800, 610);", enQuestionnaireType.OfficeForProvider.GetValue(), Eval("ID"))%>>
                                                    <img src="/_img/iconEdit.png" alt="Αξιολόγηση" title="Αξιολόγηση" />
                                                </a>
                                                <a id="A2" runat="server" href="javascript:void(0)" visible='<%# !CanEvaluateProvider((InternshipProvider)Container.DataItem) %>'
                                                    onclick=<%# string.Format("popUp.show('/Secure/Common/Questionnaire.aspx?qType={0}&entityID={1}','Αξιολόγηση', cmdRefresh, 800, 610);", enQuestionnaireType.OfficeForProvider.GetValue(), Eval("ID"))%>>
                                                    <img src="/_img/iconView.png" alt="Προεπισκόπηση" title="Προεπισκόπηση" />
                                                </a>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                    </Columns>
                                </dx:ASPxGridView>

                                <asp:ObjectDataSource ID="odsProviders" runat="server" TypeName="StudentPractice.Portal.DataSources.Providers"
                                    SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria" EnablePaging="true"
                                    SortParameterName="sortExpression" OnSelecting="odsProviders_Selecting" OnSelected="odsProviders_Selected">
                                    <SelectParameters>
                                        <asp:Parameter Name="criteria" Type="Object" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>


                            </dx:ContentControl>
                        </ContentCollection>
                    </dx:TabPage>
                    <dx:TabPage Name="Students" Text="Φοιτητές">
                        <ContentCollection>
                            <dx:ContentControl>

                                <table class="dv">
                                    <tr>
                                        <th colspan="4" class="popupHeader">Φίλτρα Αναζήτησης
                                        </th>
                                    </tr>
                                    <tr>
                                        <th style="width: 120px">Τίτλος:
                                        </th>
                                        <td style="width: 230px">
                                            <asp:TextBox ID="txtTitle" runat="server" TabIndex="6" Width="95%" />
                                        </td>
                                        <th style="width: 120px">ID Θέσης:
                                        </th>
                                        <td style="width: 230px">
                                            <asp:TextBox ID="txtPositionID" runat="server" TabIndex="7" Width="95%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th style="width: 120px">Όνομα:
                                        </th>
                                        <td style="width: 230px">
                                            <asp:TextBox ID="txtFirstName" runat="server" TabIndex="6" Width="95%" />
                                        </td>
                                        <th style="width: 120px">Επώνυμο:
                                        </th>
                                        <td style="width: 230px">
                                            <asp:TextBox ID="txtLastName" runat="server" TabIndex="7" Width="95%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <th style="width: 120px">Αρ. Μητρώου:
                                        </th>
                                        <td style="width: 230px">
                                            <asp:TextBox ID="txtStudentNumber" runat="server" TabIndex="8" Width="95%" />
                                        </td>
                                        <th>&nbsp;</th>
                                        <th>&nbsp;</th>
                                    </tr>
                                </table>

                                <div style="padding: 5px 0px 10px;">
                                    <asp:LinkButton ID="btnSearchStudents" runat="server" Text="Αναζήτηση" OnClick="btnSearchStudents_Click"
                                        CssClass="icon-btn bg-search" />
                                    <%--<asp:LinkButton ID="btnExport" runat="server" Text="Εξαγωγή σε Excel" OnClick="btnExport_Click"
            CssClass="icon-btn bg-excel" />--%>
                                </div>

                                <dx:ASPxGridView ID="gvPositions" runat="server" AutoGenerateColumns="False" DataSourceID="odsPositions"
                                    KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true" Width="100%"
                                    DataSourceForceStandardPaging="true">
                                    <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
                                    <SettingsPager PageSize="10" Summary-Text="Σελίδα {0} από {1} ({2} Θέσεις Πρακτικής Άσκησης)"
                                        Summary-Position="Left" />
                                    <Styles>
                                        <Header HorizontalAlign="Center" Wrap="True" />
                                        <Cell HorizontalAlign="Left" Wrap="True" Font-Size="11px" />
                                    </Styles>
                                    <Templates>
                                        <EmptyDataRow>
                                            Δεν βρέθηκαν θέσεις πρακτικής άσκησης
                                        </EmptyDataRow>
                                    </Templates>
                                    <Columns>
                                        <dx:GridViewDataTextColumn Caption="Προβολή Θέσης" Width="40px" Name="ViewPosition" CellStyle-HorizontalAlign="Center">
                                            <DataItemTemplate>
                                                <a id="A4" runat="server" style="text-decoration: none" href="javascript:void(0)"
                                                    onclick=<%# string.Format("popUp.show('ViewPosition.aspx?pID={0}', 'Προβολή Θέσης Πρακτικής Άσκησης');", Eval("ID"))%>
                                                    tip="Προβολή Θέσης">
                                                    <img src="/_img/iconView.png" alt="Προβολή Θέσης" />
                                                </a>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Κωδικός Θέσης" Width="40px" FieldName="ID">
                                            <CellStyle HorizontalAlign="Center" />
                                            <DataItemTemplate>
                                                <%# Eval("ID")%>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Κωδικός Group" Width="40px" FieldName="GroupID">
                                            <CellStyle HorizontalAlign="Center" />
                                            <DataItemTemplate>
                                                <%# Eval("GroupID")%>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Τίτλος" FieldName="InternshipPositionGroup.Title">
                                            <DataItemTemplate>
                                                <%# Eval("InternshipPositionGroup.Title")%>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Φορέας Υποδοχής" FieldName="InternshipPositionGroup.Provider.Name">
                                            <DataItemTemplate>
                                                <%# ((InternshipPosition)Container.DataItem).InternshipPositionGroup.GetProviderDetails() %>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn HeaderStyle-Wrap="true" Width="300px" Caption="Αντιστοιχισμένος Φοιτητής">
                                            <DataItemTemplate>
                                                <%# GetStudentDetails((InternshipPosition)Container.DataItem)%>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Name="Evaluate" Caption="Αξιολόγηση" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="50px">
                                            <DataItemTemplate>
                                                <a id="A1" runat="server" href="javascript:void(0)" visible='<%# CanEvaluateStudent((InternshipPosition)Container.DataItem) %>'
                                                    onclick=<%# string.Format("popUp.show('/Secure/Common/Questionnaire.aspx?qType={0}&entityID={1}&positionID={2}','Αξιολόγηση', cmdRefresh, 800, 610);", enQuestionnaireType.OfficeForStudent.GetValue(), Eval("AssignedToStudentID"), Eval("ID"))%>>
                                                    <img src="/_img/iconEdit.png" alt="Αξιολόγηση" title="Αξιολόγηση" />
                                                </a>
                                                <a id="A3" runat="server" href="javascript:void(0)" visible='<%# !CanEvaluateStudent((InternshipPosition)Container.DataItem) %>'
                                                    onclick=<%# string.Format("popUp.show('/Secure/Common/Questionnaire.aspx?qType={0}&entityID={1}&positionID={2}','Αξιολόγηση', cmdRefresh, 800, 610);", enQuestionnaireType.OfficeForStudent.GetValue(), Eval("AssignedToStudentID"), Eval("ID"))%>>
                                                    <img src="/_img/iconView.png" alt="Προεπισκόπηση" title="Προεπισκόπηση" />
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
