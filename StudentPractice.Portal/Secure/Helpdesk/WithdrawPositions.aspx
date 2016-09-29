<%@ Page Title="Αφαίρεση θέσεων πρακτικής άσκησης" Language="C#" MasterPageFile="~/Secure/BackOffice.master"
    AutoEventWireup="true" CodeBehind="WithdrawPositions.aspx.cs" Inherits="StudentPractice.Portal.Secure.Helpdesk.WithdrawPositions" %>

<%@ Import Namespace="StudentPractice.BusinessModel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:LinkButton ID="cmdRefresh" runat="server" Style="display: none;" OnClick="cmdRefresh_Click" />
    <table style="width: 850px" class="dv">
        <tr>
            <th colspan="8" class="popupHeader">
                Φίλτρα Αναζήτησης
            </th>
        </tr>
        <tr>
            <th style="width: 120px">
                ID Φορέα:
            </th>
            <td style="width: 200px">
                <asp:TextBox ID="txtProviderID" runat="server" TabIndex="1" Width="95%" />
            </td>
            <th style="width: 120px">
                ID Group:
            </th>
            <td style="width: 200px">
                <asp:TextBox ID="txtGroupID" runat="server" TabIndex="5" Width="95%" />
            </td>
            <th style="width: 120px">Χώρα:
            </th>
            <td style="width: 200px">
                <asp:DropDownList ID="ddlCountry" runat="server" ClientIDMode="Static" Width="98%"
                    OnInit="ddlCountry_Init" DataTextField="Name" DataValueField="ID" />
            </td>
        </tr>
        <tr>
            <th style="width: 120px">
                Α.Φ.Μ. Φορέα:
            </th>
            <td style="width: 300px">
                <asp:TextBox ID="txtProviderAFM" runat="server" TabIndex="2" Width="95%" />
            </td>
            <th style="width: 120px">
                Αντικείμενο θέσης:
            </th>
            <td style="width: 300px">
                <asp:DropDownList ID="ddlPhysicalObject" runat="server" TabIndex="6" Width="100%"
                    OnInit="ddlPhysicalObject_Init" />
            </td>
            <th style="width: 120px">Νομός:
            </th>
            <td style="width: 300px">
                <asp:DropDownList ID="ddlPrefecture" runat="server" ClientIDMode="Static" Width="98%"
                    DataTextField="Name" DataValueField="ID" />
                <act:CascadingDropDown ID="cddPrefecture" runat="server" TargetControlID="ddlPrefecture"
                    ParentControlID="ddlCountry" Category="Prefectures" PromptText="-- επιλέξτε νομό --"
                    ServicePath="~/PortalServices/Services.asmx" ServiceMethod="GetPrefectures" LoadingText="Παρακαλω περιμένετε">
                </act:CascadingDropDown>
            </td>
        </tr>
        <tr>
            <th style="width: 120px">
                Ημ/νία καταχώρισης:
            </th>
            <td style="width: 300px">
                <asp:DropDownList ID="ddlSubmissionDate" runat="server" TabIndex="3" Width="100%">
                    <asp:ListItem Text="-- αδιάφορο --" Value="-1" Selected="True" />
                    <asp:ListItem Text="Τελευταίες 24 ώρες" Value="1" />
                    <asp:ListItem Text="Τελευταία εβδομάδα" Value="2" />
                    <asp:ListItem Text="Τελευταίος μήνας" Value="3" />
                    <asp:ListItem Text="Τελευταίοι 3 μήνες" Value="4" />
                </asp:DropDownList>
            </td>
            <th style="width: 120px">
                Κατάσταση:
            </th>
            <td style="width: 300px">
                <asp:DropDownList ID="ddlPositionGroupStatus" runat="server" Width="98%" TabIndex="1"
                    OnInit="ddlPositionGroupStatus_Init" />
            </td>
            <th style="width: 120px">
                Πόλη:
            </th>
            <td style="width: 300px">
                <asp:DropDownList ID="ddlCity" runat="server" ClientIDMode="Static" TabIndex="8"
                    Width="100%" DataTextField="Name" DataValueField="ID" />
                <act:CascadingDropDown ID="cddCity" runat="server" TargetControlID="ddlCity"
                    ParentControlID="ddlPrefecture" Category="Cities" PromptText="-- επιλέξτε πόλη --"
                    ServicePath="~/PortalServices/Services.asmx" ServiceMethod="GetCities" LoadingText="Παρακαλω περιμένετε">
                </act:CascadingDropDown>
            </td>
        </tr>
    </table>
    <div style="padding: 5px 0px 20px;">
        <asp:LinkButton ID="btnSearch" runat="server" Text="Αναζήτηση" OnClick="btnSearch_Click"
            CssClass="icon-btn bg-search" />
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <dx:ASPxGridView ID="gvPositionGroups" runat="server" AutoGenerateColumns="False"
                DataSourceID="odsPositionGroups" KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true"
                Width="100%" DataSourceForceStandardPaging="true" OnHtmlRowPrepared="gvPositionGroups_HtmlRowPrepared"
                OnCustomCallback="gvPositionGroups_CustomCallback" ClientInstanceName="gv">
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
                    <dx:GridViewDataTextColumn Caption="Κωδικός Group" HeaderStyle-HorizontalAlign="Center"
                        FieldName="ID" HeaderStyle-Wrap="true" CellStyle-HorizontalAlign="Center" Width="40px" SortIndex="0" SortOrder="Descending">
                        <DataItemTemplate>
                            <%# Eval("ID")%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Κωδικός Φορέα Υποδοχής" HeaderStyle-HorizontalAlign="Center"
                        FieldName="ProviderID" HeaderStyle-Wrap="true" CellStyle-HorizontalAlign="Center"
                        Width="40px">
                        <DataItemTemplate>
                            <%# Eval("ProviderID")%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Τίτλος" CellStyle-HorizontalAlign="Left" FieldName="Title">
                        <DataItemTemplate>
                            <%# Eval("Title")%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Φορέας Υποδοχής" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# GetProviderDetails((InternshipPositionGroup)Container.DataItem) %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Αριθμός Θέσεων" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# Eval("TotalPositions") %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Κατάσταση Group" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# GetGroupStatus((InternshipPositionGroup)Container.DataItem) %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Κατάσταση Θέσεων" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# GetPositionsStatus((InternshipPositionGroup)Container.DataItem) %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Πλήρη Στοιχεία" Width="100px">
                        <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                        <CellStyle HorizontalAlign="Center" />
                        <DataItemTemplate>
                            <a runat="server" style="text-decoration: none" href="javascript:void(0)"
                                onclick='<%# string.Format("popUp.show(\"ViewPosition.aspx?pID={0}\",\"Προβολή Θέσης\", cmdRefresh, 800, 700);", Eval("ID"))%>' >
                                <img src="/_img/iconView.png" alt="Προβολή Θέσης" />
                            </a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Ενέργειες" Width="100px">
                        <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                        <CellStyle HorizontalAlign="Center" />
                        <DataItemTemplate>
                            <a runat="server" href="javascript:void(0);" class="icon-btn bg-delete" 
                                onclick='<%# string.Format("return doAction(\"unpublish\", {0}, \"WithdrawPositions\");", Eval("ID"))%>'
                                visible='<%# CanUnPublish((InternshipPositionGroup)Container.DataItem) %>'>Απο-Δημοσίευση</a>

                            <a runat="server" href="javascript:void(0);" class="icon-btn bg-edit" 
                                onclick='<%# string.Format("return doAction(\"publish\", {0}, \"WithdrawPositions\");", Eval("ID"))%>'
                                visible='<%# CanPublish((InternshipPositionGroup)Container.DataItem) %>'>Δημοσίευση</a>

                            <a runat="server" href="javascript:void(0);" class="icon-btn bg-delete" 
                                onclick='<%# string.Format("return doAction(\"revoke\", {0}, \"WithdrawPositions\");", Eval("ID"))%>'
                                visible='<%# CanRevoke((InternshipPositionGroup)Container.DataItem) %>'>Απόσυρση</a>

                            <a runat="server" href="javascript:void(0);" class="icon-btn bg-edit" 
                                onclick='<%# string.Format("return doAction(\"rollback\", {0}, \"WithdrawPositions\");", Eval("ID"))%>'
                                visible='<%# CanRollback((InternshipPositionGroup)Container.DataItem) %>'>Επαναφορά</a>

                            <a runat="server" href="javascript:void(0);" class="icon-btn bg-edit" 
                                onclick='<%# string.Format("return doAction(\"rollbackpublish\", {0}, \"WithdrawPositions\");", Eval("ID"))%>'
                                visible='<%# CanRollbackNPublish((InternshipPositionGroup)Container.DataItem) %>'>Επαναφορά & Δημοσίευση</a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                </Columns>
            </dx:ASPxGridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ObjectDataSource ID="odsPositionGroups" runat="server" TypeName="StudentPractice.Portal.DataSources.PositionGroups"
        SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
        EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsPositionGroups_Selecting">
        <SelectParameters>
            <asp:Parameter Name="criteria" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <script type="text/javascript">
            function cmdRefresh() {
                <%= this.Page.ClientScript.GetPostBackEventReference(this.cmdRefresh, string.Empty) %>;
            }
    </script>
</asp:Content>
