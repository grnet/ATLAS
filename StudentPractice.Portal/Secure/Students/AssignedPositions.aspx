<%@ Page Language="C#" MasterPageFile="~/Secure/SecurePages.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.Students.AssignedPositions"
    CodeBehind="AssignedPositions.aspx.cs" Title="Θέσεις Πρακτικής Άσκησης" %>

<%@ Import Namespace="StudentPractice.BusinessModel" %>
<%@ Import Namespace="StudentPractice.Portal" %>

<asp:Content ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <div style="margin: 0px 10px 10px">
        <div style="padding: 0px 0px 20px;">
            <a runat="server" class="icon-btn bg-colourExplanation" href="javascript:void(0)" onclick="window.open('/ColourExplanation.aspx','colourExplanation','toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=500, height=250'); return false;">Επεξήγηση Χρωμάτων</a>
        </div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <dx:ASPxGridView ID="gvPositions" runat="server" AutoGenerateColumns="False"
                    DataSourceID="odsPositions" KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true"
                    Width="100%" DataSourceForceStandardPaging="true" OnHtmlRowPrepared="gvPositions_HtmlRowPrepared"
                    OnCustomCallback="gvPositions_CustomCallback" ClientInstanceName="gv">
                    <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
                    <SettingsPager PageSize="10" Summary-Text="Σελίδα {0} από {1} ({2} Θέσεις Πρακτικής Άσκησης)"
                        Summary-Position="Left" />
                    <Styles>
                        <Header HorizontalAlign="Center" />
                        <Cell Font-Size="11px" />
                    </Styles>
                    <Templates>
                        <EmptyDataRow>
                            Δεν έχετε ακόμα αντιστοιχιστεί σε κάποια θέση Πρακτικής Άσκησης
                        </EmptyDataRow>
                    </Templates>
                    <Columns>
                        <dx:GridViewDataTextColumn FieldName="ID" Caption="Κωδικός" HeaderStyle-HorizontalAlign="Center"
                            CellStyle-HorizontalAlign="Center" Width="50px" SortIndex="0" SortOrder="Descending">
                            <DataItemTemplate>
                                <%# Eval("ID")%>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="InternshipPositionGroup.Provider.Name" Caption="Στοιχεία Φορέα">
                            <DataItemTemplate>
                                <%# GetProviderDetails((InternshipPosition)Container.DataItem)%>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Αντικείμενο Θέσης" CellStyle-HorizontalAlign="Center">
                            <DataItemTemplate>
                                <%# ((InternshipPosition)Container.DataItem).InternshipPositionGroup.GetPhysicalObjectDetails() %>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="InternshipPositionGroup.Title" Caption="Τίτλος" CellStyle-HorizontalAlign="Left">
                            <DataItemTemplate>
                                <%# Eval("InternshipPositionGroup.Title")%>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Κατάσταση Θέσης" FieldName="PositionStatusInt" Name="PositionStatus" CellStyle-HorizontalAlign="Center">
                            <DataItemTemplate>
                                <%# GetPositionStatus((InternshipPosition)Container.DataItem) %>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Στοιχεία Εκτέλεσης" Name="ImplementationInfo" CellStyle-HorizontalAlign="Center" CellStyle-Wrap="False">
                            <DataItemTemplate>
                                <%# GetImplementationInfo((InternshipPosition)Container.DataItem) %>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Ημ/νία Ολοκλήρωσης" FieldName="CompletedAt" Name="CompletedAt" CellStyle-HorizontalAlign="Center">
                            <DataItemTemplate>
                                <%# GetCompletedAt((InternshipPosition)Container.DataItem) %>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Πλήρη Στοιχεία" Width="100px">
                            <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                            <CellStyle HorizontalAlign="Center" />
                            <DataItemTemplate>
                                <a runat="server" style="text-decoration: none" href="javascript:void(0)" tip="Προβολή Θέσης"
                                    onclick=<%# string.Format("popUp.show('ViewAssignedPositionDetails.aspx?pID={0}','Προβολή Θέσης', null, 800, 700);", Eval("ID"))%>>
                                    <img src="/_img/iconView.png" alt="Προβολή Θέσης" /></a>
                                <a runat="server" style="text-decoration: none" href="javascript:void(0)" tip="Προβολή Βεβαίωσης" visible='<%# ((enPositionStatus)Eval("PositionStatus")) == enPositionStatus.Completed %>'
                                    onclick=<%# string.Format("popUp.show('ViewPositionCert.aspx?pID={0}','Προβολή Βεβαίωσης', null, 1000, 750);", Eval("ID"))%>>
                                    <img src="/_img/iconViewDetails.png" alt="Προβολή Βεβαίωσης" /></a>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Name="EvaluateOffice" Caption="Αξιολόγηση Γραφείου" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="50px">
                            <DataItemTemplate>
                                <a id="A1" runat="server" href="javascript:void(0)" visible='<%# ((enPositionStatus)Eval("PositionStatus")) == enPositionStatus.Completed && CanEvaluateOffice((InternshipPosition)Container.DataItem) %>'
                                    onclick=<%# string.Format("popUp.show('/Secure/Common/Questionnaire.aspx?qType={0}&entityID={1}&positionID={2}','Αξιολόγηση', cmdRefresh, 800, 610);", enQuestionnaireType.StudentForOffice.GetValue(), Eval("PreAssignedByMasterAccountID"), Eval("ID"))%>>
                                    <img src="/_img/iconEdit.png" alt="Αξιολόγηση" title="Αξιολόγηση" />
                                </a>
                                <a id="A3" runat="server" href="javascript:void(0)" visible='<%# ((enPositionStatus)Eval("PositionStatus")) == enPositionStatus.Completed && !CanEvaluateOffice((InternshipPosition)Container.DataItem) %>'
                                    onclick=<%# string.Format("popUp.show('/Secure/Common/Questionnaire.aspx?qType={0}&entityID={1}&positionID={2}','Αξιολόγηση', cmdRefresh, 800, 610);", enQuestionnaireType.StudentForOffice.GetValue(), Eval("PreAssignedByMasterAccountID"), Eval("ID"))%>>
                                    <img src="/_img/iconView.png" alt="Προεπισκόπηση" title="Προεπισκόπηση" />
                                </a>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Name="EvaluateProvider" Caption="Αξιολόγηση Φορέα" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="50px">
                            <DataItemTemplate>
                                <a id="A2" runat="server" href="javascript:void(0)" visible='<%# ((enPositionStatus)Eval("PositionStatus")) == enPositionStatus.Completed && CanEvaluateProvider((InternshipPosition)Container.DataItem) %>'
                                    onclick=<%# string.Format("popUp.show('/Secure/Common/Questionnaire.aspx?qType={0}&entityID={1}&positionID={2}','Αξιολόγηση', cmdRefresh, 800, 610);", enQuestionnaireType.StudentForProvider.GetValue(), Eval("InternshipPositionGroup.ProviderID"), Eval("ID"))%>>
                                    <img src="/_img/iconEdit.png" alt="Αξιολόγηση" title="Αξιολόγηση" />
                                </a>
                                <a id="A4" runat="server" href="javascript:void(0)" visible='<%# ((enPositionStatus)Eval("PositionStatus")) == enPositionStatus.Completed && !CanEvaluateProvider((InternshipPosition)Container.DataItem) %>'
                                    onclick=<%# string.Format("popUp.show('/Secure/Common/Questionnaire.aspx?qType={0}&entityID={1}&positionID={2}','Αξιολόγηση', cmdRefresh, 800, 610);", enQuestionnaireType.StudentForProvider.GetValue(), Eval("InternshipPositionGroup.ProviderID"), Eval("ID"))%>>
                                    <img src="/_img/iconView.png" alt="Προεπισκόπηση" title="Προεπισκόπηση" />
                                </a>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                    </Columns>
                </dx:ASPxGridView>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:ObjectDataSource ID="odsPositions" runat="server" TypeName="StudentPractice.Portal.DataSources.Positions"
            SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria" EnablePaging="true"
            SortParameterName="sortExpression" OnSelecting="odsPositions_Selecting" OnSelected="odsPositions_Selected">
            <SelectParameters>
                <asp:Parameter Name="criteria" Type="Object" />
            </SelectParameters>
        </asp:ObjectDataSource>

        <asp:LinkButton ID="cmdRefresh" runat="server" Style="display: none;" OnClick="cmdRefresh_Click" />
        <script type="text/javascript">
            function cmdRefresh() {
                <%= this.Page.ClientScript.GetPostBackEventReference(this.cmdRefresh, string.Empty) %>;
            }
        </script>
    </div>
</asp:Content>
