<%@ Page Title="Θέσεις Πρακτικής Άσκησης" Language="C#" MasterPageFile="~/Secure/BackOffice.master"
    AutoEventWireup="true" CodeBehind="InternshipPositions.aspx.cs" Inherits="StudentPractice.Portal.Secure.Reports.InternshipPositions" %>

<%@ Import Namespace="StudentPractice.BusinessModel" %>
<%@ Import Namespace="StudentPractice.Portal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:LinkButton ID="cmdRefresh" runat="server" Style="display: none;" OnClick="cmdRefresh_Click" />
    <table width="900px" class="dv">
        <tr>
            <th colspan="8" class="popupHeader">Φίλτρα Αναζήτησης
            </th>
        </tr>
        <tr>
            <th style="width: 100px">ID Φορέα: 
            </th>
            <td style="width: 200px">
                <asp:TextBox ID="txtProviderID" runat="server" TabIndex="1" Width="95%" />
            </td>
            <th style="width: 100px">ID Group:
            </th>
            <td style="width: 200px">
                <asp:TextBox ID="txtGroupId" runat="server" TabIndex="6" Width="95%" />
            </td>
            <th style="width: 100px">ID Θέσης:
            </th>
            <td style="width: 200px">
                <asp:TextBox ID="txtPositionID" runat="server" TabIndex="7" Width="95%" />
            </td>
        </tr>
        <tr>
            <th style="width: 100px">Ημ/νία καταχώρισης:
            </th>
            <td style="width: 200px">
                <asp:DropDownList ID="ddlSubmissionDate" runat="server" TabIndex="3" Width="100%">
                    <asp:ListItem Text="-- αδιάφορο --" Value="-1" Selected="True" />
                    <asp:ListItem Text="Τελευταίες 24 ώρες" Value="1" />
                    <asp:ListItem Text="Τελευταία εβδομάδα" Value="2" />
                    <asp:ListItem Text="Τελευταίος μήνας" Value="3" />
                    <asp:ListItem Text="Τελευταίοι 3 μήνες" Value="4" />
                </asp:DropDownList>
            </td>

            <th style="width: 100px">Κατάσταση Θέσης:
            </th>
            <td style="width: 200px">
                <asp:DropDownList ID="ddlPositionStatus" runat="server" TabIndex="4" Width="100%" OnInit="ddlPositionStatus_Init" />
            </td>

            <th style="width: 100px">Αντικείμενο θέσης:
            </th>
            <td style="width: 200px">
                <asp:DropDownList ID="ddlPhysicalObject" runat="server" TabIndex="7" Width="100%" OnInit="ddlPhysicalObject_Init" />
            </td>
        </tr>
        <tr>
            <th style="width: 100px">Χώρα:
            </th>
            <td style="width: 200px">
                <asp:DropDownList ID="ddlCountry" runat="server" ClientIDMode="Static" Width="98%"
                    OnInit="ddlCountry_Init" DataTextField="Name" DataValueField="ID" />
            </td>
            <th style="width: 100px">Περιφερειακή Ενότητα:
            </th>
            <td style="width: 200px">
                <asp:DropDownList ID="ddlPrefecture" runat="server" ClientIDMode="Static" TabIndex="8"
                    Width="100%" DataTextField="Name" DataValueField="ID" />
                <act:CascadingDropDown ID="cddPrefecture" runat="server" TargetControlID="ddlPrefecture"
                    ParentControlID="ddlCountry" Category="Prefectures" PromptText="-- αδιάφορο --"
                    ServicePath="~/PortalServices/Services.asmx" ServiceMethod="GetPrefectures" LoadingText="Παρακαλω περιμένετε">
                </act:CascadingDropDown>
            </td>
            <th style="width: 100px">Καλλικρατικός Δήμος:
            </th>
            <td style="width: 200px">
                <asp:DropDownList ID="ddlCity" runat="server" ClientIDMode="Static" TabIndex="9"
                    Width="100%" DataTextField="Name" DataValueField="ID" />
                <act:CascadingDropDown ID="cddCity" runat="server" TargetControlID="ddlCity"
                    ParentControlID="ddlPrefecture" Category="Cities" PromptText="-- αδιάφορο --"
                    ServicePath="~/PortalServices/Services.asmx" ServiceMethod="GetCities" LoadingText="Παρακαλω περιμένετε">
                </act:CascadingDropDown>
            </td>
        </tr>
        <tr>
            <th style="width: 100px;">Ημερομηνία Δημιουργίας από:
            </th>
            <td style="width: 200px;">
                <dx:ASPxDateEdit ID="deCreatedAtFrom" runat="server" Width="95%" />
            </td>
            <th style="width: 100px;">Ημερομηνία Δημιουργίας έως:
            </th>
            <td style="width: 200px;">
                <dx:ASPxDateEdit ID="deCreatedAtTo" runat="server" Width="95%" />
            </td>
            <th style="width: 100px">Είδος Δημιουργίας Θέσης:</th>
            <td style="width: 200px">
                <asp:DropDownList ID="ddlPositionCreationType" runat="server" Width="95%" OnInit="ddlPositionCreationType_Init" />
            </td>
        </tr>
        <tr>
            <th style="width: 100px">Ίδρυμα:
            </th>
            <td style="width: 200px">
                <asp:DropDownList ID="ddlInstitution" runat="server" Width="98%" OnInit="ddlInstitution_Init" />
            </td>
            <th style="width: 100px">Τμήμα:
            </th>
            <td style="width: 200px">
                <asp:DropDownList ID="ddlDepartment" runat="server" Width="98%" DataTextField="Name"
                    DataValueField="ID" />
                <act:CascadingDropDown ID="cddDepartment" runat="server" TargetControlID="ddlDepartment"
                    ParentControlID="ddlInstitution" Category="Academics" PromptText="-- αδιάφορο --"
                    ServicePath="~/PortalServices/Services.asmx" ServiceMethod="GetAcademics" LoadingText="Παρακαλω περιμένετε">
                </act:CascadingDropDown>
            </td>
            <th style="width: 100px">Τρόπος Χρηματοδότησης:</th>
            <td style="width: 200px">
                <asp:DropDownList ID="ddlFundingType" runat="server" ClientIDMode="Static" OnInit="ddlFundingType_Init" Width="98%" />
            </td>
        </tr>
    </table>
    <div style="padding: 5px 0px 20px;">
        <asp:LinkButton ID="btnSearch" runat="server" Text="Αναζήτηση" OnClick="btnSearch_Click"
            CssClass="icon-btn bg-search" />
        <asp:LinkButton ID="btnExport" runat="server" Text="Εξαγωγή με φίλτρα" OnClick="btnExport_Click"
            CssClass="icon-btn bg-excel" />
        <asp:LinkButton ID="btnExportAll" runat="server" Text="Εξαγωγή όλων" OnClick="btnExportAll_Click"
            CssClass="icon-btn bg-excel" />
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <dx:ASPxGridView ID="gvPositions" runat="server" AutoGenerateColumns="False" DataSourceID="odsPositions"
                KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true" Width="100%"
                DataSourceForceStandardPaging="true" OnHtmlRowPrepared="gvPositions_HtmlRowPrepared">
                <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
                <SettingsPager PageSize="10" Summary-Text="Σελίδα {0} από {1} ({2} Θέσεις Πρακτικής Άσκησης)"
                    Summary-Position="Left" />
                <Styles>
                    <Header Wrap="True" />
                    <Cell Font-Size="11px" Wrap="True" HorizontalAlign="Left" />
                </Styles>
                <Templates>
                    <EmptyDataRow>
                        Δεν βρέθηκαν αποτελέσματα
                    </EmptyDataRow>
                </Templates>
                <Columns>
                    <dx:GridViewDataTextColumn Caption="ID Group" FieldName="InternshipPositionGroup.ID" Name="GroupID">
                        <DataItemTemplate>
                            <%# Eval("InternshipPositionGroup.ID") %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="ID Θέσης" FieldName="ID" Name="ID">
                        <DataItemTemplate>
                            <%# Eval("ID") %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Ημ/νία Δημιουργίας" FieldName="CreatedAt" Name="CreatedAt">
                        <DataItemTemplate>
                            <%#((DateTime)Eval("CreatedAt")).ToString("dd/MM/yyyy HH:mm") %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Ημ/νία Δημοσίευσης" FieldName="InternshipPositionGroup.FirstPublishedAt" Name="FirstPublishedAt">
                        <DataItemTemplate>
                            <%# GetFirstPublishedAt((InternshipPosition)Container.DataItem) %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="ID Φορέα" FieldName="InternshipPositionGroup.ProviderID" Name="ProviderID">
                        <DataItemTemplate>
                            <%# Eval("InternshipPositionGroup.ProviderID") %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Επωνυμία Φορέα" FieldName="InternshipPositionGroup.Provider.Name" Name="ProviderName">
                        <DataItemTemplate>
                            <%# Eval("InternshipPositionGroup.Provider.Name") %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Διάρκεια (εβδομάδες)" FieldName="InternshipPositionGroup.Duration" Name="Duration">
                        <DataItemTemplate>
                            <%# Eval("InternshipPositionGroup.Duration")%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Τίτλος - Περιγραφή" FieldName="InternshipPositionGroup.Title" Name="TitleDescription">
                        <DataItemTemplate>
                            <%# ((InternshipPosition)Container.DataItem).GetPositionName() %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Τοποθεσία" Name="CityPrefecture">
                        <DataItemTemplate>
                            <%# ((InternshipPosition)Container.DataItem).GetAddressDetails() %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Χρονικός Περιορισμός" Name="TimeConstrain">
                        <DataItemTemplate>
                            <%# GetTimeConstrain((InternshipPosition)Container.DataItem)%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Είδος Θέσης" FieldName="InternshipPositionGroup.PositionTypeInt"
                        Name="PositionType">
                        <DataItemTemplate>
                            <%# ((enPositionType)Eval("InternshipPositionGroup.PositionType")).GetLabel()%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Στοιχεία επικοινωνίας θέσης" Name="ContanctDetails">
                        <DataItemTemplate>
                            <%# GetContanctDetails((InternshipPosition)Container.DataItem)%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Κατάσταση Group" FieldName="InternshipPositionGroup.PositionGroupStatusInt" Name="PositionGroupStatus">
                        <DataItemTemplate>
                            <%# GetPositionGroupStatus((InternshipPosition)Container.DataItem) %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Κατάσταση Θέσης" FieldName="PositionStatusInt" Name="PositionStatus">
                        <DataItemTemplate>
                            <%# GetPositionStatus((InternshipPosition)Container.DataItem) %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Στοιχεία Προδέσμευσης" FieldName="PreAssignedAt" Name="PreAssignInfo">
                        <DataItemTemplate>
                            <%# GetPreAssignInfo((InternshipPosition)Container.DataItem) %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Στοιχεία Αντιστοίχισης" FieldName="AssignedAt" Name="AssignInfo">
                        <DataItemTemplate>
                            <%# GetAssignInfo((InternshipPosition)Container.DataItem) %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Στοιχεία Εκτέλεσης" Name="ImplementationInfo">
                        <DataItemTemplate>
                            <%# GetImplementationInfo((InternshipPosition)Container.DataItem) %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Ημ/νία Ολοκλήρωσης" FieldName="CompletedAt" Name="CompletedAt">
                        <DataItemTemplate>
                            <%# GetCompletedAt((InternshipPosition)Container.DataItem) %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Είδος Δημιουργίας Θέσης" FieldName="PositionCreationTypeInt" Name="PositionCreationType">
                        <DataItemTemplate>
                            <%# ((InternshipPosition)Container.DataItem).InternshipPositionGroup.PositionCreationType.GetLabel() %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <%--<dx:GridViewDataTextColumn Caption="Ίδρυμα/Τμήμα" Name="Academics">
                        <DataItemTemplate>
                            <%# ((InternshipPosition)Container.DataItem).GetAcademics() %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>--%>
                    <dx:GridViewDataTextColumn Caption="Αντικείμενο Θέσης" Name="PhysicalObjects">
                        <DataItemTemplate>
                            <%# ((InternshipPosition)Container.DataItem).GetPhysicalObjectDetails() %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                </Columns>
            </dx:ASPxGridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <dx:ASPxGridView ID="gvPositionsExport" runat="server" Visible="false" AutoGenerateColumns="False"
        KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="false" Width="100%"
        DataSourceForceStandardPaging="false">
        <SettingsPager Mode="ShowAllRecords" />
        <Styles>
            <Header Wrap="True" />
            <Cell Font-Size="11px" Wrap="True" HorizontalAlign="Left" />
        </Styles>
        <Templates>
            <EmptyDataRow>
                Δεν βρέθηκαν αποτελέσματα
            </EmptyDataRow>
        </Templates>
        <Columns>
            <dx:GridViewDataTextColumn Caption="ID Group" FieldName="InternshipPositionGroup.ID" Name="GroupID" />
            <dx:GridViewDataTextColumn Caption="ID Θέσης" FieldName="ID" Name="ID" />
            <dx:GridViewDataTextColumn Caption="Ημ/νία Δημιουργίας" Name="CreatedAt" />
            <dx:GridViewDataTextColumn Caption="Ημ/νία Δημοσίευσης" Name="FirstPublishedAt" />
            <dx:GridViewDataTextColumn Caption="ID Φορέα" FieldName="InternshipPositionGroup.ProviderID" Name="ProviderID" />
            <dx:GridViewDataTextColumn Caption="Επωνυμία Φορέα" FieldName="InternshipPositionGroup.Provider.Name" Name="ProviderName" />
            <dx:GridViewDataTextColumn Caption="Διάρκεια (εβδομάδες)" FieldName="InternshipPositionGroup.Duration" Name="Duration" />
            <dx:GridViewDataTextColumn Caption="Τίτλος" FieldName="InternshipPositionGroup.Title" Name="Title" />
            <dx:GridViewDataTextColumn Caption="Περιγραφή" FieldName="InternshipPositionGroup.Description" Name="Description" />
            <dx:GridViewDataTextColumn Caption="Χώρα" Name="Country" />
            <dx:GridViewDataTextColumn Caption="Περιφερειακή Ενότητα" Name="Prefecture" />
            <dx:GridViewDataTextColumn Caption="Καλλικρατικός Δήμος" Name="City" />
            <dx:GridViewDataTextColumn Caption="Διαθέσιμη Χρονική Περίοδος" Name="TimeAvailable" />
            <dx:GridViewDataTextColumn Caption="Είδος Θέσης" Name="PositionType" />
            <dx:GridViewDataTextColumn Caption="Τηλέφωνο Θέσης" FieldName="InternshipPositionGroup.ContactPhone" Name="ContactPhone" />
            <dx:GridViewDataTextColumn Caption="Ον/μο Επόπτη" FieldName="InternshipPositionGroup.Supervisor" Name="Supervisor" />
            <dx:GridViewDataTextColumn Caption="E-mail Επόπτη" FieldName="InternshipPositionGroup.SupervisorEmail" Name="SupervisorEmail" />
            <dx:GridViewDataTextColumn Caption="Κατάσταση Group" Name="PositionGroupStatus" />
            <dx:GridViewDataTextColumn Caption="Κατάσταση Θέσης" Name="PositionStatus" />
            <dx:GridViewDataTextColumn Caption="Ημ/νία Προδέσμευσης" Name="PreAssignedAt" />
            <dx:GridViewDataTextColumn Caption="ID Γραφείου" FieldName="PreAssignedByOfficeID" Name="PreAssignedByOfficeID" />
            <dx:GridViewDataTextColumn Caption="Ίδρυμα Προδέσμευσης" Name="PreAssignedForAcademic.Institution" />
            <dx:GridViewDataTextColumn Caption="Τμήμα Προδέσμευσης" Name="PreAssignedForAcademic.Department" />
            <dx:GridViewDataTextColumn Caption="Ημ/νία Αντιστοίχισης" Name="AssignedAt" />
            <dx:GridViewDataTextColumn Caption="ID Φοιτητή" Name="AssignedToStudent.ID" />
            <dx:GridViewDataTextColumn Caption="Oν/μο Αντιστοιχισμένου Φοιτητή" Name="AssignedToStudent.ContactName" />
            <dx:GridViewDataTextColumn Caption="A.M. Φοιτητή" Name="AssignedToStudent.StudentNumber" />
            <dx:GridViewDataTextColumn Caption="12ψήφιος Ακαδημαϊκής Ταυτότητας" Name="AssignedToStudent.AcademicIDNumber" />
            <dx:GridViewDataTextColumn Caption="Ημ/νία Έναρξης Εκτέλεσης" Name="ImplementationStartDate" />
            <dx:GridViewDataTextColumn Caption="Ημ/νία Λήξης Εκτέλεσης" Name="ImplementationEndDate" />
            <dx:GridViewDataTextColumn Caption="Τρόπος Χρηματοδότησης" Name="FundingType" />
            <dx:GridViewDataTextColumn Caption="Ημ/νία Ολοκλήρωσης" Name="CompletedAt" />
            <dx:GridViewDataTextColumn Caption="Παρατηρήσεις ολοκλήρωσης" FieldName="CompletionComments" Name="CompletionComments" />
            <dx:GridViewDataTextColumn Caption="Αντικείμενο Θέσης" Name="PhysicalObjects" />
            <dx:GridViewDataTextColumn Caption="Είδος Δημιουργίας Θέσης" Name="PositionCreationType" />
        </Columns>
    </dx:ASPxGridView>
    <dx:ASPxGridViewExporter ID="gveIntershipPositions" runat="server" GridViewID="gvPositionsExport" OnRenderBrick="gveIntershipPositions_RenderBrick">
    </dx:ASPxGridViewExporter>
    <asp:ObjectDataSource ID="odsPositions" runat="server" TypeName="StudentPractice.Portal.DataSources.Positions"
        SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
        EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsPositions_Selecting">
        <SelectParameters>
            <asp:Parameter Name="criteria" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <%--    <asp:ObjectDataSource ID="odsPositionsExport" runat="server" TypeName="StudentPractice.Portal.DataSources.Positions"
        SelectMethod="FindInternshipPositionReport" SelectCountMethod="CountWithCriteria"
        EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsPositionsExport_Selecting">
        <SelectParameters>
            <asp:Parameter Name="criteria" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>--%>
    <script type="text/javascript">
        function cmdRefresh() {
            <%= this.Page.ClientScript.GetPostBackEventReference(this.cmdRefresh, string.Empty) %>;
        }
    </script>
</asp:Content>
