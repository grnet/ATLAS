<%@ Page Language="C#" MasterPageFile="~/Secure/BackOffice.master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.Helpdesk.SearchPositions" Title="Θέσεις Πρακτικής Άσκησης" CodeBehind="SearchPositions.aspx.cs" %>

<%@ Register Src="~/UserControls/GenericControls/EmailForm.ascx" TagName="EmailForm"
    TagPrefix="my" %>

<%@ Import Namespace="StudentPractice.BusinessModel" %>
<%@ Import Namespace="StudentPractice.Portal" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .dxgvHeader td {
            font-size: 11px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width: 1200px" class="dv">
        <tr>
            <th colspan="10" class="popupHeader">Φίλτρα Αναζήτησης
            </th>
        </tr>
        <tr>
            <th style="width: 100px">ID Θέσης:
            </th>
            <td style="width: 200px">
                <asp:TextBox ID="txtPositionID" runat="server" TabIndex="7" Width="95%" />
            </td>
            <th style="width: 100px">Αντικείμενο θέσης:
            </th>
            <td style="width: 200px">
                <asp:DropDownList ID="ddlPhysicalObject" runat="server" TabIndex="7" Width="100%" OnInit="ddlPhysicalObject_Init" />
            </td>
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
        </tr>
        <tr>
            <th style="width: 100px">ID Group:
            </th>
            <td style="width: 200px">
                <asp:TextBox ID="txtGroupId" runat="server" TabIndex="6" Width="95%" />
            </td>
            <th style="width: 100px">Χώρα:
            </th>
            <td style="width: 200px">
                <asp:DropDownList ID="ddlCountry" runat="server" ClientIDMode="Static" Width="98%"
                    OnInit="ddlCountry_Init" DataTextField="Name" DataValueField="ID" />
            </td>
            <th style="width: 100px">Ίδρυμα:
            </th>
            <td style="width: 200px">
                <asp:DropDownList ID="ddlInstitution" runat="server" Width="98%" OnInit="ddlInstitution_Init" />
            </td>
            <th style="width: 100px">Τρόπος Χρηματοδότησης:</th>
            <td style="width: 200px">
                <asp:DropDownList ID="ddlFundingType" runat="server" ClientIDMode="Static" OnInit="ddlFundingType_Init" Width="98%" />
            </td>
        </tr>
        <tr>
            <th style="width: 100px">ID Φορέα: 
            </th>
            <td style="width: 200px">
                <asp:TextBox ID="txtProviderID" runat="server" TabIndex="1" Width="95%" />
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
            <th>Είδος Δημιουργίας Θέσης</th>
            <td>
                <asp:DropDownList ID="ddlCreationType" runat="server" TabIndex="3" Width="100%" OnInit="ddlCreationType_Init" />
            </td>
        </tr>
        <tr>
            <th style="width: 100px">ID Φοιτητή:</th>
            <td style="width: 200px">
                <asp:TextBox ID="txtStudentID" runat="server" TabIndex="1" Width="95%" />
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
            <th style="width: 100px">Αρ. Μητρώου: </th>
            <td style="width: 200px">
                <asp:TextBox ID="txtStudentNumber" runat="server" TabIndex="1" Width="95%" />
            </td>
            <th></th>
            <td></td>
        </tr>
    </table>
    <div style="padding: 5px 0px 20px;">
        <asp:LinkButton ID="btnSearch" runat="server" Text="Αναζήτηση" OnClick="btnSearch_Click"
            CssClass="icon-btn bg-search" />
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <dx:ASPxGridView ID="gvPositions" runat="server" AutoGenerateColumns="False"
                DataSourceID="odsPositions" KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true"
                Width="100%" DataSourceForceStandardPaging="true" OnHtmlRowPrepared="gvPositions_HtmlRowPrepared"
                OnCustomCallback="gvPositions_CustomCallback" ClientInstanceName="gv">
                <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
                <SettingsPager PageSize="10" Summary-Text="Σελίδα {0} από {1} ({2} Θέσεις Πρακτικής Άσκησης)"
                    Summary-Position="Left" />
                <Styles>
                    <Cell Font-Size="11px" />
                </Styles>
                <Templates>
                    <EmptyDataRow>
                        Δεν έχετε ακόμα δημιουργήσεις θέσεις πρακτικής άσκησης
                    </EmptyDataRow>
                </Templates>
                <Columns>
                    <dx:GridViewDataTextColumn FieldName="ID" Caption="Κωδικός" HeaderStyle-HorizontalAlign="Center"
                        CellStyle-HorizontalAlign="Center" Width="50px" SortIndex="0" SortOrder="Descending">
                        <DataItemTemplate>
                            <%# Eval("ID")%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="InternshipPositionGroup.ID" Caption="Κωδικός Group" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="True"
                        CellStyle-HorizontalAlign="Center" Width="50px">
                        <DataItemTemplate>
                            <%# Eval("InternshipPositionGroup.ID")%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Στοιχεία Δημοσίευσης" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# GetPublishDetails((InternshipPosition)Container.DataItem)%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Στοιχεία Φορέα" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# GetProviderDetails((InternshipPosition)Container.DataItem)%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Αντικείμενο Θέσης" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# ((InternshipPosition)Container.DataItem).InternshipPositionGroup.GetPhysicalObjectDetails() %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="InternshipPositionGroup.Title" Caption="Τίτλος" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# Eval("InternshipPositionGroup.Title")%>
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
                    <dx:GridViewDataTextColumn Caption="Πλήρη Στοιχεία" Width="100px">
                        <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                        <CellStyle HorizontalAlign="Center" />
                        <DataItemTemplate>
                            <a id="A1" runat="server" style="text-decoration: none" href="javascript:void(0)" 
                                onclick='<%# string.Format("popUp.show(\"ViewPositionDetails.aspx?pID={0}\",\"Προβολή Θέσης\", null, 800, 700);", Eval("ID"))%>' >
                                <img src="/_img/iconView.png" alt="Προβολή Θέσης" /></a>
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
</asp:Content>
