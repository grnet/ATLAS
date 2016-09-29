<%@ Page Language="C#" MasterPageFile="~/Secure/SecurePages.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.InternshipOffices.StudentPositions"
    Title="Θέσεις Πρακτικής Άσκησης Φοιτητών" CodeBehind="StudentPositions.aspx.cs" %>

<%@ Register Src="~/UserControls/GenericControls/FlashMessage.ascx" TagName="FlashMessage" TagPrefix="my" %>
<%@ Import Namespace="StudentPractice.BusinessModel" %>
<%@ Import Namespace="StudentPractice.Portal" %>

<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <style type="text/css">
        .dxgvHeader td {
            font-size: 11px;
        }
    </style>
    <% if (DesignMode)
       { %>
    <script type="text/javascript" src="../../_js/ASPxScriptIntelliSense.js"></script>
    <% } %>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <asp:MultiView ID="mvAccount" runat="server" ActiveViewIndex="0">
        <asp:View ID="vAccountNotVerified" runat="server">
            <div class="reminder">
                <asp:Label ID="lblVerificationError" runat="server" Text="Δεν μπορείτε να αναζητήσετε τις δείτε πρακτικής άσκησης των φοιτητών."/>
            </div>
        </asp:View>
        <asp:View ID="vAccountVerified" runat="server">
            <asp:LinkButton ID="cmdRefresh" runat="server" Style="display: none;" OnClick="cmdRefresh_Click" />
            <div style="padding: 0px 0px 20px;">
                <asp:LinkButton ID="btnReturn" runat="server" Text="Επιστροφή" OnClick="btnReturn_Click" CssClass="icon-btn bg-search" />
                <a class="icon-btn bg-colourExplanation" href="javascript:void(0)" onclick="window.open('/ColourExplanation.aspx','colourExplanation','toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=500, height=250'); return false;">Επεξήγηση Χρωμάτων</a>
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <my:FlashMessage ID="fm" runat="server" CssClass="fade" />
                    <dx:ASPxGridView ID="gvPositions" runat="server" AutoGenerateColumns="False" DataSourceID="odsPositions"
                        KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true" Width="100%"
                        DataSourceForceStandardPaging="true" OnHtmlRowPrepared="gvPositions_HtmlRowPrepared" ClientInstanceName="gv">
                        <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
                        <SettingsPager PageSize="10" Summary-Text="Σελίδα {0} από {1} ({2} Θέσεις Πρακτικής Άσκησης)"
                            Summary-Position="Left" />
                        <Styles>
                            <Cell Font-Size="11px" />
                        </Styles>
                        <Templates>
                            <EmptyDataRow>
                                Δεν βρέθηκαν θέσεις πρακτικής άσκησης
                            </EmptyDataRow>
                        </Templates>
                        <Columns>
                            <dx:GridViewDataTextColumn CellStyle-HorizontalAlign="Center" Width="40px">
                                <DataItemTemplate>
                                    <a runat="server" style="text-decoration: none" href="javascript:void(0)" onclick=<%# string.Format("popUp.show('ViewPosition.aspx?pID={0}', 'Προβολή Θέσης Πρακτικής Άσκησης');", Eval("ID"))%>>
                                        <img src="/_img/iconView.png" alt="Προβολή Θέσης" /></a>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Κωδικός Group" HeaderStyle-HorizontalAlign="Center"
                                HeaderStyle-Wrap="true" CellStyle-HorizontalAlign="Center" Width="40px">
                                <DataItemTemplate>
                                    <%# Eval("GroupID")%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Κωδικός Θέσης" HeaderStyle-HorizontalAlign="Center"
                                HeaderStyle-Wrap="true" CellStyle-HorizontalAlign="Center" Width="40px">
                                <DataItemTemplate>
                                    <%# Eval("ID")%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Αντικείμενο Θέσης" CellStyle-HorizontalAlign="Left">
                                <DataItemTemplate>
                                    <%# ((InternshipPosition)Container.DataItem).GetPhysicalObjectDetails() %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Τίτλος" CellStyle-HorizontalAlign="Left">
                                <DataItemTemplate>
                                    <%# Eval("InternshipPositionGroup.Title")%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Φορέας Υποδοχής" CellStyle-HorizontalAlign="Left">
                                <DataItemTemplate>
                                    <%# GetProviderDetails((InternshipPosition)Container.DataItem) %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Τόπος Διεξαγωγής" CellStyle-HorizontalAlign="Left">
                                <DataItemTemplate>
                                    <%# ((InternshipPosition)Container.DataItem).GetAddressDetails() %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Προδέσμευση για Τμήμα / Αντιστοιχισμένος Φοιτητής"
                                HeaderStyle-Wrap="true" Width="150px">
                                <DataItemTemplate>
                                    <%# GetStudentDetails((InternshipPosition)Container.DataItem) %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>                                                     
                        </Columns>
                    </dx:ASPxGridView>
                </ContentTemplate>
            </asp:UpdatePanel>
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
