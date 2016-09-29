<%@ Page Title="Γραφεία Πρακτικής Άσκησης" Language="C#" MasterPageFile="~/Secure/BackOffice.master"
    AutoEventWireup="true" CodeBehind="InternshipOffices.aspx.cs" Inherits="StudentPractice.Portal.Secure.Reports.InternshipOffices" %>

<%@ Import Namespace="StudentPractice.BusinessModel" %>
<%@ Import Namespace="StudentPractice.Portal" %>

<asp:Content ID="cphHeadContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="cphMainContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:LinkButton ID="cmdRefresh" runat="server" Style="display: none;" OnClick="cmdRefresh_Click" />
    <table width="960px" class="dv">
        <tr>
            <th colspan="4" class="popupHeader">Φίλτρα Αναζήτησης
            </th>
        </tr>
        <tr>
            <th style="width: 100px">ID Γραφείου: 
            </th>
            <td style="width: 330px;">
                <asp:TextBox ID="txtOfficeID" runat="server" TabIndex="1" Width="95%" />
            </td>
            <th style="width: 140px">Αρ. Βεβαίωσης:
            </th>
            <td style="width: 330px">
                <asp:TextBox ID="txtCertificationNumber" runat="server" TabIndex="4" Width="95%" />
            </td>
        </tr>
        <tr>
            <th style="width: 100px">Πιστοποιημένος:
            </th>
            <td style="width: 330px">
                <asp:DropDownList ID="ddlVerificationStatus" runat="server" Width="100%" TabIndex="1">
                    <asp:ListItem Text="-- αδιάφορο --" Value="-1" Selected="True" />
                    <asp:ListItem Text="Όχι" Value="0" />
                    <asp:ListItem Text="Ναι" Value="1" />
                    <asp:ListItem Text="Απορρίφθηκε" Value="2" />
                </asp:DropDownList>
            </td>
            <th style="width: 140px">Ημ/νία Βεβαίωσης:
            </th>
            <td style="width: 330px">
                <dx:ASPxDateEdit ID="deCertificationDate" runat="server" TabIndex="5" Width="95%" />
            </td>
        </tr>
        <tr>
            <th style="width: 100px">E-mail:
            </th>
            <td style="width: 330px">
                <asp:TextBox ID="txtEmail" runat="server" TabIndex="6" Width="95%" />
            </td>
            <th style="width: 120px">Ίδρυμα:
            </th>
            <td style="width: 330px">
                <asp:DropDownList ID="ddlInstitution" runat="server" TabIndex="8" Width="100%" OnInit="ddlInstitution_Init" />
            </td>
        </tr>
        <tr>
            <th style="width: 120px">Είδος Γραφείου:
            </th>
            <td style="width: 330px">
                <asp:DropDownList ID="ddlOfficeType" runat="server" TabIndex="8" Width="100%" OnInit="ddlOfficeType_Init" />
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
        <asp:LinkButton ID="btnExportUsers" runat="server" Text="Εξαγωγή σε Excel Χρηστών Γραφείων" OnClick="btnExportUsers_Click"
            CssClass="icon-btn bg-excel" />
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <dx:ASPxGridView ID="gvOffices" runat="server" AutoGenerateColumns="False" DataSourceID="odsOffices"
                KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true" Width="100%"
                DataSourceForceStandardPaging="true" OnHtmlRowPrepared="gvOffices_HtmlRowPrepared">
                <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
                <SettingsPager PageSize="10" Summary-Text="Σελίδα {0} από {1} ({2} Γραφεία)" Summary-Position="Left" />
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
                            <%# ((DateTime)Eval("CreatedAt")).ToString("dd/MM/yyyy<br />HH:mm")%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" HeaderStyle-HorizontalAlign="Center"
                        CellStyle-HorizontalAlign="Center" Width="50px">
                        <DataItemTemplate>
                            <%# Eval("ID") %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Είδος Γραφείου" FieldName="OfficeType" Name="OfficeType"
                        CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# ((enOfficeType)Eval("OfficeType")).GetLabel() %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Ίδρυμα Γραφείου" Name="OfficeInstitute" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# ((InternshipOffice)Container.DataItem).GetOfficeInstitution() %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Τμήματα Γραφείου" Name="OfficeAcademics" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# GetOfficeAcademics((InternshipOffice)Container.DataItem) %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Στοιχεία γραφείου" Name="OfficeDetails" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# GetOfficeDetails((InternshipOffice)Container.DataItem)%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Στοιχεία Υπευθύνου" Name="OfficeAdmin" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# GetOfficeAdmin((InternshipOffice)Container.DataItem)%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Πιστοποιούσα Αρχή" Name="OfficeCertifier"
                        CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# GetOfficeCertifier((InternshipOffice)Container.DataItem)%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Στοιχεία Αναπληρωτή Υπευθύνου" Name="OfficeAdminAlt"
                        CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# GetOfficeAdminAlt((InternshipOffice)Container.DataItem)%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Στοιχεία Βεβαίωσης" Name="OfficeCertification"
                        CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# ((InternshipOffice)Container.DataItem).GetCertificationDetails() %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Πιστοποιημένος" Name="VerificationStatus"
                        CellStyle-HorizontalAlign="Center">
                        <DataItemTemplate>
                            <%# ((InternshipOffice)Container.DataItem).GetVerificationStatus() %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                </Columns>
            </dx:ASPxGridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ObjectDataSource ID="odsOffices" runat="server" TypeName="StudentPractice.Portal.DataSources.Offices"
        SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
        EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsOffices_Selecting">
        <SelectParameters>
            <asp:Parameter Name="criteria" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <dx:ASPxGridView ID="gvOfficesExport" runat="server" AutoGenerateColumns="False"
        DataSourceID="odsOffices" KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true"
        Width="100%" DataSourceForceStandardPaging="true" Visible="false">
        <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
        <SettingsPager PageSize="2" Summary-Text="Σελίδα {0} από {1} ({2} Φορείς)" Summary-Position="Left" />
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
                    <%# ((DateTime)Eval("CreatedAt")).ToString("dd/MM/yyyy<br />HH:mm")%>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" HeaderStyle-HorizontalAlign="Center"
                CellStyle-HorizontalAlign="Center" Width="50px">
                <DataItemTemplate>
                    <%# Eval("ID") %>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Είδος Γραφείου" FieldName="OfficeType" Name="OfficeType"
                CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# ((enOfficeType)Eval("OfficeType")).GetLabel() %>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Ίδρυμα Γραφείου" Name="OfficeInstitute" CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# ((InternshipOffice)Container.DataItem).GetOfficeInstitution() %>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Τμήματα Γραφείου" Name="OfficeAcademics" CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# GetOfficeAcademics((InternshipOffice)Container.DataItem) %>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Ημερομηνία Πιστοποίησης" Name="VerificationDate">
                <DataItemTemplate>
                    <%# GetVerificationDate((InternshipOffice)Container.DataItem) %>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Email γραφείου" Name="Email" FieldName="Email" CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# Eval("Email") %>
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
            <dx:GridViewDataTextColumn Caption="Περιφερειακή Ενότητα" Name="OfficePrefecture" CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# GetOfficePrefecture((InternshipOffice)Container.DataItem) %>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Καλλικρατικός Δήμος" Name="OfficeCity" CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# GetOfficeCity((InternshipOffice)Container.DataItem) %>
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
            <dx:GridViewDataTextColumn Caption="Πιστοποιούσα Αρχή" Name="OfficeCertifier"
                CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# GetOfficeCertifier((InternshipOffice)Container.DataItem)%>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Στοιχεία Βεβαίωσης" Name="OfficeCertification"
                CellStyle-HorizontalAlign="Left">
                <DataItemTemplate>
                    <%# ((InternshipOffice)Container.DataItem).GetCertificationDetails() %>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Πιστοποιημένος" Name="VerificationStatus"
                CellStyle-HorizontalAlign="Center">
                <DataItemTemplate>
                    <%# ((InternshipOffice)Container.DataItem).GetVerificationStatus() %>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Αριθμός Χρηστών Γραφείου" Name="OfficeUsersCount"
                CellStyle-HorizontalAlign="Center">
                <DataItemTemplate>
                    <%# GetOfficeUsersCount((InternshipOffice)Container.DataItem)%>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
        </Columns>
    </dx:ASPxGridView>
    <dx:ASPxGridViewExporter ID="gveIntershipOffices" runat="server" GridViewID="gvOfficesExport"
        OnRenderBrick="gveIntershipOffices_RenderBrick">
    </dx:ASPxGridViewExporter>

    <dx:ASPxGridView ID="gvOfficeUsersExport" runat="server" AutoGenerateColumns="False"
        KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true"
        Width="100%" Visible="false">
        <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
        <SettingsPager PageSize="2" Summary-Text="Σελίδα {0} από {1} ({2} Φορείς)" Summary-Position="Left" />
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
            <dx:GridViewDataTextColumn Caption="ID Παραρτήματος" FieldName="ID" Name="ID" />
            <dx:GridViewDataTextColumn Caption="Username Παραρτήματος" FieldName="UserName" Name="UserName" />
            <dx:GridViewDataTextColumn Caption="Ενεργός" FieldName="IsApproved" Name="IsApproved" />
            <dx:GridViewDataTextColumn Caption="Email Χρήστη" FieldName="ContactEmail" Name="ContactEmail" />
            <dx:GridViewDataTextColumn Caption="Ονοματεπώνυμο Χρήστη" FieldName="ContactName" Name="ContactName" />
            <dx:GridViewDataTextColumn Caption="Τηλέφωνο Χρήστη" FieldName="ContactPhone" Name="ContactPhone" />
            <dx:GridViewDataTextColumn Caption="Κινητό Τηλέφωνο Χρήστη" FieldName="ContactMobilePhone" Name="ContactMobilePhone" />
            <dx:GridViewDataTextColumn Caption="Πρόσβαση σε τμήματα του Γραφείου Πρακτικής" Name="Academics" />
            <dx:GridViewDataTextColumn Caption="ID Κεντρικού Γραφείου" FieldName="MasterAccountID" Name="MasterAccountID" />
            <dx:GridViewDataTextColumn Caption="Username Κεντρικού Γραφείου" FieldName="MasterAccount.UserName" Name="MasterAccount.UserName" />
            <dx:GridViewDataTextColumn Caption="E-mail Λογαριασμού" FieldName="Email" Name="Email" />
            <dx:GridViewDataTextColumn Caption="Ίδρυμα" Name="Institution" />
            <dx:GridViewDataTextColumn Caption="Είδος Κεντρικού ΓΠΑ" Name="MasterOfficeType" />
            <dx:GridViewDataTextColumn Caption="Ημ/νία Δημιουργίας" FieldName="CreatedAtDateOnly" Name="CreatedAtDateOnly" />
        </Columns>
    </dx:ASPxGridView>
    <dx:ASPxGridViewExporter ID="gveInternshipOfficeUsers" runat="server" GridViewID="gvOfficeUsersExport"
        OnRenderBrick="gveInternshipOfficeUsers_RenderBrick" />

    <script type="text/javascript">
        function cmdRefresh() {
            <%= this.Page.ClientScript.GetPostBackEventReference(this.cmdRefresh, string.Empty) %>;
        }
    </script>
</asp:Content>
