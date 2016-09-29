<%@ Page Language="C#" MasterPageFile="~/Secure/BackOffice.master" AutoEventWireup="true"
    Inherits="StudentPractice.Portal.Secure.Helpdesk.SearchOffices" Title="Πιστοποίηση Γραφείων Πρακτικής Άσκησης"
    CodeBehind="SearchOffices.aspx.cs" %>

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
    <table style="width: 960px" class="dv" id="tblFilters" runat="server">
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
                <asp:TextBox ID="txtCertificationNumber" runat="server" TabIndex="4" Width="95%" />
            </td>
            <th style="width: 120px">
                ID Γραφείου
            </th>
            <td>
                <asp:TextBox ID="txtOfficeID" runat="server" TabIndex="5" Width="95%" />
            </td>
            <%--<th style="width: 90px">
                Τύπος:
            </th>
            <td style="width: 200px">
                <asp:DropDownList ID="ddlAccountType" runat="server" Width="100%" TabIndex="7">
                    <asp:ListItem Text="-- αδιάφορο --" Value="-1" />
                    <asp:ListItem Text="Master Account" Value="1" Selected="True" />
                    <asp:ListItem Text="User Account" Value="2" />
                </asp:DropDownList>
            </td>--%>
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
                <dx:ASPxDateEdit ID="deCertificationDate" runat="server" TabIndex="6" Width="95%" />
            </td>
            <th style="width: 120px">
                Είδος Γραφείου:
            </th>
            <td style="width: 330px">
                <asp:DropDownList ID="ddlOfficeType" runat="server" TabIndex="7" Width="100%" OnInit="ddlOfficeType_Init" />
            </td>
        </tr>
        <tr>
            <th style="width: 100px">
                Username:
            </th>
            <td style="width: 200px">
                <asp:TextBox ID="txtUserName" runat="server" TabIndex="8" Width="95%" />
            </td>
            <th style="width: 100px">
                E-mail:
            </th>
            <td style="width: 200px">
                <asp:TextBox ID="txtEmail" runat="server" TabIndex="9" Width="95%" />
            </td>
            <th style="width: 120px">
                Ίδρυμα:
            </th>
            <td style="width: 330px">
                <asp:DropDownList ID="ddlInstitution" runat="server" TabIndex="10" Width="100%" OnInit="ddlInstitution_Init" />
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
            <dx:ASPxGridView ID="gvOffices" runat="server" AutoGenerateColumns="False" DataSourceID="odsOffices"
                KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true" Width="100%"
                DataSourceForceStandardPaging="true" OnHtmlRowPrepared="gvOffices_HtmlRowPrepared">
                <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
                <SettingsPager PageSize="10" Summary-Text="Σελίδα {0} από {1} ({2} Γραφεία)" Summary-Position="Left" />
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
                    <dx:GridViewDataTextColumn Caption="Είδος Γραφείου" FieldName="OfficeTypeInt" Name="OfficeType"
                        CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# ((enOfficeType)Eval("OfficeType")).GetLabel() %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Στοιχεία Γραφείου" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <asp:PlaceHolder runat="server" Visible="<%#CanEditOfficeAcademics((InternshipOffice)Container.DataItem) == true%>">
                                <table>
                                    <tr>
                                        <td>
                                            <a runat="server" style="text-decoration: none" href="javascript:void(0)" title="Περιορισμός σε συγκεκριμένες Σχολές/Τμήματα"
                                                onclick='<%# string.Format("popUp.show(\"EditOfficeAcademics.aspx?oID={0}\",\"Περιορισμός σε συγκεκριμένες Σχολές/Τμήματα\",cmdRefresh, 800, 600);", Eval("ID"))%>' >
                                                <img src="/_img/iconReportEdit.png" alt="Περιορισμός σε συγκεκριμένες Σχολές/Τμήματα" />
                                            </a>
                                        </td>
                                        <td>
                                            <%# GetOfficeDetails((InternshipOffice)Container.DataItem)%>
                                        </td>
                                    </tr>
                                </table>
                            </asp:PlaceHolder>
                            <asp:PlaceHolder runat="server" Visible="<%#CanEditOfficeAcademics((InternshipOffice)Container.DataItem) == false%>">
                                <%# GetOfficeDetails((InternshipOffice)Container.DataItem)%>
                            </asp:PlaceHolder>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Στοιχεία Λογαριασμού" FieldName="UserName" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# Eval("UserName")%><br />
                            <%# Eval("Email")%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Επεξεργασία" HeaderStyle-Wrap="True" HeaderStyle-HorizontalAlign="Center"
                        CellStyle-HorizontalAlign="Center" Width="50px">
                        <DataItemTemplate>
                            <a runat="server" style="text-decoration: none" href="javascript:void(0)" 
                                onclick='<%# string.Format("popUp.show(\"ViewAccountDetails.aspx?rid={0}&t={1}\", \"Στοιχεία Λογαριασμού\", 400, 300);", Eval("ID"), (enReporterType.InternshipOffice).ToString("D"))%>' >
                                <img src="/_img/iconUserEdit.png" alt="Στοιχεία Λογαριασμού" />
                            </a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Στοιχεία Βεβαίωσης" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# ((InternshipOffice)Container.DataItem).GetCertificationDetails() %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Πλήρη Στοιχεία" Width="50px">
                        <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                        <CellStyle HorizontalAlign="Center" />
                        <DataItemTemplate>
                            <a runat="server" style="text-decoration: none" href="javascript:void(0)" 
                                onclick='<%# string.Format("popUp.show(\"VerifyOffice.aspx?sID={0}\",\"Προβολή Στοιχείων Γραφείου\",cmdRefresh, 800, 600);", Eval("ID"))%>' >
                                <img src="/_img/iconViewDetails.png" alt="Προβολή Στοιχείων" />
                            </a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Αλλαγή Στοιχείων" Width="50px">
                        <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                        <CellStyle HorizontalAlign="Center" />
                        <DataItemTemplate>
                            <a runat="server" style="text-decoration: none" href="javascript:void(0)" visible='<%# CanEditOffice((InternshipOffice)Container.DataItem) %>'
                                onclick='<%# string.Format("popUp.show(\"EditOffice.aspx?sID={0}\",\"Αλλαγή Στοιχείων Γραφείου\",cmdRefresh, 800, 600);", Eval("ID"))%>' >
                                <img src="/_img/iconReportEdit.png" alt="Αλλαγή Στοιχείων Γραμματείας" />
                            </a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Χρήστες Γραφείου" Width="50px">
                        <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                        <CellStyle HorizontalAlign="Center" />
                        <DataItemTemplate>
                            <a runat="server" style="text-decoration: none" href="javascript:void(0)" visible='<%# CanShowOfficeUsers((InternshipOffice)Container.DataItem) %>'
                                onclick='<%# string.Format("popUp.show(\"ViewOfficeUsers.aspx?oID={0}\",\"Προβολή Χρηστών\",cmdRefresh, 800, 600);", Eval("ID"))%>' >
                                <img src="/_img/iconUsers.png" alt="Προβολή Παραρτημάτων" />
                            </a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Φοιτητές" Width="50px">
                        <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                        <CellStyle HorizontalAlign="Center" />
                        <DataItemTemplate>
                            <a id="A1" runat="server" style="text-decoration: none" href='<%# string.Format("SearchOfficeStudents.aspx?oID={0}", Eval("ID"))%>'>
                                <img src="/_img/iconViewDetails.png" alt="Φοιτητές" />
                            </a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Ιστορικό Πιστοποίησης" Width="50px">
                        <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                        <CellStyle HorizontalAlign="Center" />
                        <DataItemTemplate>
                            <a runat="server" style="text-decoration: none" href="javascript:void(0)" visible='<%# CanViewVerificationLogs((InternshipOffice)Container.DataItem) %>'
                                onclick='<%# string.Format("popUp.show(\"ViewVerificationLogs.aspx?rID={0}\",\"Ιστορικό Πιστοποίησης\",cmdRefresh, 800, 600);", Eval("ID"))%>' >
                                <img src="/_img/iconViewDetails.png" alt="Ιστορικό Πιστοποίησης" />
                            </a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Προβολή Συμβάντων" Width="50px">
                        <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                        <CellStyle HorizontalAlign="Center" />
                        <DataItemTemplate>
                            <a runat="server" style="text-decoration: none" href='<%# GetSearchIncidentReportLink((InternshipOffice)Container.DataItem) %>'>
                                <img src="/_img/iconView.png" alt="Προβολή Συμβάντων" />
                            </a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Αναφορά Συμβάντος" Width="50px">
                        <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                        <CellStyle HorizontalAlign="Center" />
                        <DataItemTemplate>
                            <a runat="server" style="text-decoration: none" href="javascript:void(0)" 
                                onclick='<%# string.Format("popUp.show(\"ReportIncident.aspx?rID={0}\",\"Αναφορά Συμβάντος\", cmdRefresh, 900, 610);", Eval("ID"))%>' >
                                <img src="/_img/iconAddNewItem.png" alt="Προσθήκη Συμβάντος" />
                            </a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                </Columns>
            </dx:ASPxGridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ObjectDataSource ID="odsOffices" runat="server" TypeName="StudentPractice.Portal.DataSources.Offices"
        SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
        EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsOffices_Selecting"
        OnSelected="odsOffices_Selected">
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
