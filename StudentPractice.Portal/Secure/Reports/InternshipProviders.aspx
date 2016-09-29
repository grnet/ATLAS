<%@ Page Language="C#" MasterPageFile="~/Secure/BackOffice.master" AutoEventWireup="true"
    CodeBehind="InternshipProviders.aspx.cs" Inherits="StudentPractice.Portal.Secure.Reports.InternshipProviders"
    Title="Φορείς Υποδοχής Πρακτικής Άσκησης" %>

<%@ Import Namespace="StudentPractice.BusinessModel" %>
<%@ Import Namespace="StudentPractice.Portal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:LinkButton ID="cmdRefresh" runat="server" Style="display: none;" OnClick="cmdRefresh_Click" />
    <table width="970px" class="dv">
        <tr>
            <th colspan="6" class="popupHeader">Φίλτρα Αναζήτησης
            </th>
        </tr>
        <tr>
            <th style="width: 100px">ID Φορέα:
            </th>
            <td style="width: 200px">
                <asp:TextBox ID="txtProviderID" runat="server" TabIndex="8" Width="95%" />
            </td>
            <th style="width: 100px">Τύπος Φορέα:
            </th>
            <td style="width: 200px">
                <asp:DropDownList ID="ddlAccountType" runat="server" Width="100%" TabIndex="7">
                    <asp:ListItem Text="-- αδιάφορο --" Value="-1" Selected="True" />
                    <asp:ListItem Text="Κεντρικός Φορέας" Value="1" />
                    <asp:ListItem Text="Παράρτημα" Value="2" />
                </asp:DropDownList>
            </td>
            <th style="width: 100px">Είδος Φορέα:
            </th>
            <td style="width: 200px">
                <asp:DropDownList ID="ddlProviderType" runat="server" Width="100%" TabIndex="9" OnInit="ddlProviderType_Init" />
            </td>
        </tr>
        <tr>
            <th style="width: 100px">E-mail:
            </th>
            <td style="width: 200px">
                <asp:TextBox ID="txtEmail" runat="server" TabIndex="4" Width="95%" />
            </td>
            <th style="width: 100px">Α.Φ.Μ.:
            </th>
            <td style="width: 200px">
                <asp:TextBox ID="txtProviderAFM" runat="server" TabIndex="10" Width="95%" />
            </td>
            <th style="width: 100px">Επωνυμία:
            </th>
            <td style="width: 200px">
                <asp:TextBox ID="txtProviderName" runat="server" TabIndex="11" Width="95%" />
            </td>
        </tr>
        <tr>
            <th style="width: 100px">Πιστοποιημένος:
            </th>
            <td style="width: 150px">
                <asp:DropDownList ID="ddlVerificationStatus" runat="server" Width="100%" TabIndex="12">
                    <asp:ListItem Text="-- αδιάφορο --" Value="-1" Selected="True" />
                    <asp:ListItem Text="Όχι" Value="0" />
                    <asp:ListItem Text="Ναι" Value="1" />
                    <asp:ListItem Text="Δεν μπορεί να πιστοποιηθεί" Value="2" />
                </asp:DropDownList>
            </td>
            <th style="width: 100px">Χώρα: 
            </th>
            <td style="width: 150px">
                <asp:DropDownList ID="ddlCountry" runat="server" Width="100%" TabIndex="13" OnInit="ddlCountry_Init" />
            </td>
            <th></th>
            <th></th>
        </tr>
    </table>
    <div style="padding: 5px 0px 10px;">
        <asp:LinkButton ID="btnSearch" runat="server" Text="Αναζήτηση" OnClick="btnSearch_Click"
            CssClass="icon-btn bg-search" />
        <asp:LinkButton ID="btnExport" runat="server" Text="Εξαγωγή σε Excel" OnClick="btnExport_Click"
            CssClass="icon-btn bg-excel" />
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <dx:ASPxGridView ID="gvProviders" runat="server" AutoGenerateColumns="False" DataSourceID="odsProviders"
                KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true" Width="100%"
                DataSourceForceStandardPaging="true" OnHtmlRowPrepared="gvProviders_HtmlRowPrepared">
                <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
                <SettingsPager PageSize="10" Summary-Text="Σελίδα {0} από {1} ({2} Φορείς)" Summary-Position="Left" />
                <Styles>
                    <Cell Font-Size="11px" Wrap="True" />
                    <Header Wrap="True" />
                </Styles>
                <Templates>
                    <EmptyDataRow>
                        Δεν βρέθηκαν αποτελέσματα
                    </EmptyDataRow>
                </Templates>
                <Columns>
                    <dx:GridViewDataTextColumn Caption="Ημ/νία Δημιουργίας" FieldName="CreatedAt"
                        Name="CreatedAt" CellStyle-Wrap="False" CellStyle-HorizontalAlign="Left" SortIndex="0"
                        SortOrder="Descending">
                        <DataItemTemplate>
                            <%# ((DateTime)Eval("CreatedAt")).ToString("dd/MM/yyyy")%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" HeaderStyle-HorizontalAlign="Center"
                        CellStyle-HorizontalAlign="Center" Width="50px">
                        <DataItemTemplate>
                            <%# Eval("ID") %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Στοιχεία Φορέα" Name="ProviderDetails" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# GetProviderDetails((InternshipProvider)Container.DataItem)%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Τύπος" FieldName="IsMasterAccount" Name="IsMasterAccount"
                        CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# ((bool)Eval("IsMasterAccount")) ? "Κεντρικός Φορέας" : "Παράρτημα" %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Κατηγορία" Name="ProviderType" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# ((enProviderType)Eval("ProviderType")).GetLabel()%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Πεδίο Δραστηριότητας" Name="PrimaryActivity"
                        CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# ((InternshipProvider)Container.DataItem).GetPrimaryActivity() %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Αριθμός απασχολούμενων" FieldName="NumberOfEmployees"
                        Name="NumberOfEmployees" CellStyle-HorizontalAlign="Center">
                        <DataItemTemplate>
                            <%# Eval("NumberOfEmployees") %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Στοιχεία Διεύθυνσης" Name="AddressDetails" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# GetAddressDetails((InternshipProvider)Container.DataItem)%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Στοιχεία Επικοινωνίας" Name="ContantDetails" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# GetContantDetails((InternshipProvider)Container.DataItem)%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Στοιχεία Νομίμου Εκπροσώπου" Name="LegalPerson" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# GetLegalPersonDetails((InternshipProvider)Container.DataItem) %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Στοιχεία Υπευθύνου" Name="ResponsiblePerson" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# GetResponsiblePersonDetails((InternshipProvider)Container.DataItem)%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Στοιχεία Αναπληρωτή Υπευθύνου" Name="AlternateResponsiblePerson" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# GetAlternateResponsiblePersonDetails((InternshipProvider)Container.DataItem)%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Πιστοποιημένος" Name="VerificationStatus"
                        CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# ((InternshipProvider)Container.DataItem).GetVerificationStatus() %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                </Columns>
            </dx:ASPxGridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ObjectDataSource ID="odsProviders" runat="server" TypeName="StudentPractice.Portal.DataSources.Providers"
        SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
        EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsProviders_Selecting">
        <SelectParameters>
            <asp:Parameter Name="criteria" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <dx:ASPxGridView ID="gvProvidersExport" runat="server" AutoGenerateColumns="False"
        DataSourceID="odsProviders" KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true"
        Width="100%" DataSourceForceStandardPaging="true" Visible="false">
        <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
        <SettingsPager PageSize="10" Summary-Text="Σελίδα {0} από {1} ({2} Φορείς)" Summary-Position="Left" />
        <Styles>
            <Cell Font-Size="11px" Wrap="True" />
            <Header Wrap="True" />
        </Styles>
        <Templates>
            <EmptyDataRow>
                Δεν βρέθηκαν αποτελέσματα
            </EmptyDataRow>
        </Templates>
        <Columns>
            <dx:GridViewDataTextColumn Caption="Ημ/νία Δημιουργίας" FieldName="CreatedAt"
                Name="CreatedAt" CellStyle-Wrap="False" CellStyle-HorizontalAlign="Left" SortIndex="0"
                SortOrder="Descending">
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
            <dx:GridViewDataTextColumn Caption="ΕΠΩΝΥΜΙΑ" Name="Name" FieldName="Name" CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# Eval("Name")%>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="ΔΙΑΚΡΙΤΙΚΟΣ ΤΙΤΛΟΣ" Name="TradeName" FieldName="TradeName" CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# Eval("TradeName")%>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="ΑΦΜ" Name="AFM" FieldName="AFM" CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# Eval("AFM")%>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="ΔΟΥ" Name="DOY" FieldName="DOY" CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# Eval("DOY")%>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Τύπος" FieldName="IsMasterAccount" Name="IsMasterAccount"
                CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# ((bool)Eval("IsMasterAccount")) ? "Κεντρικός Φορέας" : "Παράρτημα" %>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Κατηγορία" Name="ProviderType" CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# ((enProviderType)Eval("ProviderType")).GetLabel()%>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Πεδίο Δραστηριότητας" Name="PrimaryActivity"
                CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# ((InternshipProvider)Container.DataItem).GetPrimaryActivity() %>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Αριθμός απασχολούμενων" FieldName="NumberOfEmployees"
                Name="NumberOfEmployees" CellStyle-HorizontalAlign="Center">
                <DataItemTemplate>
                    <%# Eval("NumberOfEmployees") %>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Ημερομηνία Πιστοποίησης" Name="VerificationDate">
                <DataItemTemplate>
                    <%# ((InternshipProvider)Container.DataItem).GetVerificationDate() %>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Διεύθυνση (Οδός και Αριθμός)" Name="Address" FieldName="Address" CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# Eval("Address")%>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Τ.Κ." Name="ZipCode" FieldName="ZipCode" CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# Eval("ZipCode")%>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Χώρα" Name="Country" CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# ((InternshipProvider)Container.DataItem).GetProviderCountry() %>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Περιφερειακή Ενότητα" Name="ProviderPrefecture" CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# ((InternshipProvider)Container.DataItem).GetProviderPrefecture() %>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Καλλικρατικός Δήμος" Name="ProviderCity" CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# ((InternshipProvider)Container.DataItem).GetProviderCity() %>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Τηλέφωνο (σταθερό)" Name="ProviderPhone" FieldName="ProviderPhone" CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# Eval("ProviderPhone")%>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Email" Name="ProviderEmail" FieldName="ProviderEmail" CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# Eval("ProviderEmail")%>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Φαξ" Name="ProviderFax" FieldName="ProviderFax" CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# Eval("ProviderFax")%>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Ιστοσελίδα" Name="ProviderURL" FieldName="ProviderURL" CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# Eval("ProviderURL")%>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Ονοματεπώνυμο Νομίμου Εκπροσώπου" Name="LegalPersonName" FieldName="LegalPersonName" CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# Eval("LegalPersonName")%>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Τηλέφωνο Νομίμου Εκπροσώπου" Name="LegalPersonPhone" FieldName="LegalPersonPhone" CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# Eval("LegalPersonPhone")%>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Email Νομίμου Εκπροσώπου" Name="LegalPersonEmail" FieldName="LegalPersonEmail" CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# Eval("LegalPersonEmail")%>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Αστυνομική Ταυτότητα Νομίμου Εκπροσώπου" Name="LegalPersonIdentification" CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# GetLegalPersonIdentification((InternshipProvider)Container.DataItem) %>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Ονοματεπώνυμο Υπευθύνου" Name="ContactName" FieldName="ContactName" CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# Eval("ContactName")%>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Τηλέφωνο (σταθερό) Υπευθύνου" Name="ContactPhone" FieldName="ContactPhone" CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# Eval("ContactPhone")%>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Τηλέφωνο (κινητό) Υπευθύνου" Name="ContactMobilePhone" FieldName="ContactMobilePhone" CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# Eval("ContactMobilePhone")%>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Email Υπευθύνου" Name="ContactEmail" FieldName="ContactEmail" CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# Eval("ContactEmail")%>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Ονοματεπώνυμο Αναπληρωτή Υπευθύνου" Name="AlternateContactName" FieldName="AlternateContactName" CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# Eval("AlternateContactName")%>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Τηλέφωνο (σταθερό) Αναπληρωτή Υπευθύνου" Name="AlternateontactPhone" FieldName="AlternateContactPhone" CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# Eval("AlternateContactPhone")%>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Τηλέφωνο (κινητό) Αναπληρωτή Υπευθύνου" Name="AlternateContactMobilePhone" FieldName="AlternateContactMobilePhone" CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# Eval("AlternateContactMobilePhone")%>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Email Αναπληρωτή Υπευθύνου" Name="AlternateContactEmail" FieldName="AlternateContactEmail" CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# Eval("AlternateContactEmail")%>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Πιστοποιημένος" Name="VerificationStatus"
                CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# ((InternshipProvider)Container.DataItem).GetVerificationStatus() %>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
        </Columns>
    </dx:ASPxGridView>
    <dx:ASPxGridViewExporter ID="gveIntershipProviders" runat="server" GridViewID="gvProvidersExport"
        OnRenderBrick="gveIntershipProviders_RenderBrick">
    </dx:ASPxGridViewExporter>
    <script type="text/javascript">
        function cmdRefresh() {
            <%= this.Page.ClientScript.GetPostBackEventReference(this.cmdRefresh, string.Empty) %>;
        }
    </script>
</asp:Content>
