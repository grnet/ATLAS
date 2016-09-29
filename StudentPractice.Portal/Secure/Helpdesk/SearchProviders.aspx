<%@ Page Language="C#" MasterPageFile="~/Secure/BackOffice.master" AutoEventWireup="true"
    Inherits="StudentPractice.Portal.Secure.Helpdesk.SearchProviders" Title="Πιστοποίηση Φορέων Υποδοχής Πρακτικής Άσκησης"
    CodeBehind="SearchProviders.aspx.cs" %>

<%@ Register Src="~/UserControls/GenericControls/EmailForm.ascx" TagName="EmailForm"
    TagPrefix="my" %>

<%@ Import Namespace="StudentPractice.BusinessModel" %>
<%@ Import Namespace="StudentPractice.Portal" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .dxgvHeader td
        {
            font-size: 11px;
        }
        .dxeRadioButtonList td.dxe
        {
            padding: 2px 0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:LinkButton ID="cmdRefresh" runat="server" Style="display: none;" OnClick="cmdRefresh_Click" />
    <table style="width: 970px" class="dv">
        <tr>
            <th colspan="6" class="popupHeader">
                Φίλτρα Αναζήτησης
            </th>
        </tr>
        <tr>
            <th style="width: 100px">
                Πιστοποιημένος:
            </th>
            <td style="width: 150px">
                <asp:DropDownList ID="ddlVerificationStatus" runat="server" Width="100%" TabIndex="1">
                    <asp:ListItem Text="-- αδιάφορο --" Value="-1" Selected="True" />
                    <asp:ListItem Text="Όχι" Value="0" />
                    <asp:ListItem Text="Ναι" Value="1" />
                    <asp:ListItem Text="Δεν μπορεί να πιστοποιηθεί" Value="2" />
                </asp:DropDownList>
            </td>
            <th style="width: 140px">
                Αρ. Βεβαίωσης:
            </th>
            <td style="width: 150px">
                <asp:TextBox ID="txtCertificationNumber" runat="server" TabIndex="5" Width="95%" />
            </td>
            <th style="width: 100px">
                Είδος Φορέα:
            </th>
            <td style="width: 200px">
                <asp:DropDownList ID="ddlProviderType" runat="server" Width="100%" TabIndex="9" OnInit="ddlProviderType_Init" />
            </td>
        </tr>
        <tr>
            <th style="width: 100px">
                Ενεργός:
            </th>
            <td style="width: 150px">
                <asp:DropDownList ID="ddlApprovalStatus" runat="server" Width="100%" TabIndex="2">
                    <asp:ListItem Text="-- αδιάφορο --" Value="-1" Selected="True" />
                    <asp:ListItem Text="Όχι" Value="0" />
                    <asp:ListItem Text="Ναι" Value="1" />
                </asp:DropDownList>
            </td>
            <th style="width: 140px">
                Ημ/νία Βεβαίωσης:
            </th>
            <td style="width: 200px">
                <dx:ASPxDateEdit ID="deCertificationDate" runat="server" TabIndex="6" Width="98%" />
            </td>
            <th style="width: 100px">
                Α.Φ.Μ.:
            </th>
            <td style="width: 200px">
                <asp:TextBox ID="txtProviderAFM" runat="server" TabIndex="10" Width="95%" />
            </td>
        </tr>
        <tr>
            <th style="width: 100px">
                Username:
            </th>
            <td style="width: 200px">
                <asp:TextBox ID="txtUserName" runat="server" TabIndex="3" Width="95%" />
            </td>
            <th style="width: 100px">
                Τύπος Φορέα:
            </th>
            <td style="width: 200px">
                <asp:DropDownList ID="ddlAccountType" runat="server" Width="100%" TabIndex="7">
                    <asp:ListItem Text="-- αδιάφορο --" Value="-1" Selected="True" />
                    <asp:ListItem Text="Κεντρικός Φορέας" Value="1" />
                    <asp:ListItem Text="Παράρτημα" Value="2" />
                </asp:DropDownList>
            </td>
            <th style="width: 100px">
                Επωνυμία:
            </th>
            <td style="width: 200px">
                <asp:TextBox ID="txtProviderName" runat="server" TabIndex="11" Width="95%" />
            </td>
        </tr>
        <tr>
            <th style="width: 100px">
                E-mail:
            </th>
            <td style="width: 200px">
                <asp:TextBox ID="txtEmail" runat="server" TabIndex="4" Width="95%" />
            </td>
            <th style="width: 100px">
                ID Φορέα:
            </th>
            <td style="width: 200px">
                <asp:TextBox ID="txtProviderID" runat="server" TabIndex="8" Width="95%" />
            </td>
            <th style="width: 100px;">
                Χώρα: 
            </th>
            <td style="width: 200px">
                <asp:DropDownList ID="ddlCountry" runat="server" Width="100%" TabIndex="13" OnInit="ddlCountry_Init" />
            </td>
        </tr>
    </table>
    <div style="padding: 5px 0px 10px;">
        <asp:LinkButton ID="btnSearch" runat="server" Text="Αναζήτηση" OnClick="btnSearch_Click"
            CssClass="icon-btn bg-search" />
        <%--<asp:LinkButton ID="btnExport" runat="server" Text="Εξαγωγή σε Excel" OnClick="btnExport_Click"
            CssClass="icon-btn bg-excel" />--%>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <dx:ASPxGridView ID="gvProviders" runat="server" AutoGenerateColumns="False" DataSourceID="odsProviders"
                KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true" Width="100%"
                DataSourceForceStandardPaging="true" OnHtmlRowPrepared="gvProviders_HtmlRowPrepared">
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
                    <dx:GridViewDataTextColumn Caption="Ημ/νία Δημιουργίας" FieldName="CreatedAt"
                        Name="CreatedAt" CellStyle-Wrap="False" CellStyle-HorizontalAlign="Left" SortIndex="0"
                        SortOrder="Descending" Width="110px">
                        <DataItemTemplate>
                            <%# ((DateTime)Eval("CreatedAt")).ToString("dd/MM/yyyy HH:mm")%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" HeaderStyle-HorizontalAlign="Center"
                        CellStyle-HorizontalAlign="Center" Width="50px">
                        <DataItemTemplate>
                            <%# Eval("ID") %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Τύπος" FieldName="IsMasterAccount" Name="IsMasterAccount"
                        CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# ((bool)Eval("IsMasterAccount")) ? "Κεντρικός Φορέας" : "Παράρτημα" %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Κατηγορία" FieldName="ProviderTypeInt" Name="ProviderType"
                        CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# ((enProviderType)Eval("ProviderType")).GetLabel()%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Στοιχεία Φορέα" FieldName="Name" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# GetProviderDetails((InternshipProvider)Container.DataItem)%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Στοιχεία Λογαριασμού" FieldName="UserName" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# Eval("UserName")%><br />
                            <%# Eval("Email")%><br />
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Επεξεργασία" HeaderStyle-Wrap="True" HeaderStyle-HorizontalAlign="Center"
                        CellStyle-HorizontalAlign="Center" Width="50">
                        <DataItemTemplate>
                            <a id="lnkViewAccountDetails" runat="server" style="text-decoration: none" href="javascript:void(0)"
                                onclick='<%# string.Format("popUp.show(\"ViewAccountDetails.aspx?rid={0}&t={1}\", \"Στοιχεία Λογαριασμού\", null, 700, 250);", Eval("ID"), (enReporterType.InternshipProvider).ToString("D"))%>' >
                                <img src="/_img/iconUserEdit.png" alt="Στοιχεία Λογαριασμού" />
                            </a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Στοιχεία Βεβαίωσης" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# ((InternshipProvider)Container.DataItem).GetCertificationDetails() %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Πλήρη Στοιχεία" Width="50px">
                        <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                        <CellStyle HorizontalAlign="Center" />
                        <DataItemTemplate>
                            <a runat="server" style="text-decoration: none" href="javascript:void(0)"
                                 onclick='<%# string.Format("popUp.show(\"VerifyProvider.aspx?pID={0}\",\"Προβολή Στοιχείων Φορέα Υποδοχής\",cmdRefresh, 800, 700);", Eval("ID"))%>' >
                                <img src="/_img/iconViewDetails.png" alt="Προβολή Στοιχείων" />
                            </a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Αλλαγή Στοιχείων" Width="50px">
                        <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                        <CellStyle HorizontalAlign="Center" />
                        <DataItemTemplate>
                            <a runat="server" style="text-decoration: none" href="javascript:void(0)" visible='<%# CanEditProvider((InternshipProvider)Container.DataItem) %>'
                                onclick='<%# string.Format("popUp.show(\"EditProvider.aspx?pID={0}\",\"Αλλαγή Στοιχείων Φορέα Υποδοχής\",cmdRefresh, 800, 700);", Eval("ID"))%>' >
                                <img src="/_img/iconReportEdit.png" alt="Αλλαγή Στοιχείων Φορέα Υποδοχής" />
                            </a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Παραρτήματα Φορέα" Width="50px">
                        <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                        <CellStyle HorizontalAlign="Center" />
                        <DataItemTemplate>
                            <a runat="server" style="text-decoration: none" href="javascript:void(0)" visible='<%# CanShowProviderUsers((InternshipProvider)Container.DataItem) %>'
                                onclick='<%# string.Format("popUp.show(\"ViewProviderUsers.aspx?pID={0}\",\"Προβολή Παραρτημάτων\",cmdRefresh, 800, 500);", Eval("ID"))%>'>
                                <img src="/_img/iconUsers.png" alt="Προβολή Παραρτημάτων" />
                            </a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Θέσεις Πρακτικής" Width="50px">
                        <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                        <CellStyle HorizontalAlign="Center" />
                        <DataItemTemplate>
                            <a runat="server" style="text-decoration: none" href='<%# GetInternshipPositionsLink((InternshipProvider)Container.DataItem) %>'>
                                <img src="/_img/iconViewDetails.png" alt="Θέσεις Πρακτικής" />
                            </a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Ιστορικό Πιστοποίησης" Width="50px">
                        <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                        <CellStyle HorizontalAlign="Center" />
                        <DataItemTemplate>
                            <a runat="server" style="text-decoration: none" href="javascript:void(0)" visible='<%# CanViewVerificationLogs((InternshipProvider)Container.DataItem) %>'
                                onclick='<%# string.Format("popUp.show(\"ViewVerificationLogs.aspx?rID={0}\",\"Ιστορικό Πιστοποίησης\",cmdRefresh, 800, 500);", Eval("ID"))%>' >
                                <img src="/_img/iconViewDetails.png" alt="Ιστορικό Πιστοποίησης" />
                            </a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Προβολή Συμβάντων" Width="50px">
                        <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                        <CellStyle HorizontalAlign="Center" />
                        <DataItemTemplate>
                            <a runat="server" style="text-decoration: none" href='<%# GetSearchIncidentReportLink((InternshipProvider)Container.DataItem) %>'>
                                <img src="/_img/iconView.png" alt="Προβολή Συμβάντων" />
                            </a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Αναφορά Συμβάντος" Width="50px">
                        <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                        <CellStyle HorizontalAlign="Center" />
                        <DataItemTemplate>
                            <a runat="server" style="text-decoration: none" href="javascript:void(0)" 
                                onclick='<%# string.Format("popUp.show(\"ReportIncident.aspx?rID={0}\",\"Αναφορά Συμβάντος\", cmdRefresh, 800, 610);", Eval("ID"))%>' >
                                <img src="/_img/iconAddNewItem.png" alt="Προσθήκη Συμβάντος" />
                            </a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                </Columns>
            </dx:ASPxGridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ObjectDataSource ID="odsProviders" runat="server" TypeName="StudentPractice.Portal.DataSources.Providers"
        SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
        EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsProviders_Selecting"
        OnSelected="odsProviders_Selected">
        <SelectParameters>
            <asp:Parameter Name="criteria" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <script type="text/javascript">
        function cmdRefresh() {
            <%= this.Page.ClientScript.GetPostBackEventReference(this.cmdRefresh, string.Empty) %>;
        }
    </script>
    <br />
    <br />
    <my:EmailForm ID="EmailForm1" runat="server" OnEmailSending="OnEmailSending" />
</asp:Content>
