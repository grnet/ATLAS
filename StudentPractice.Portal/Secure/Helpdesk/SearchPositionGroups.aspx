<%@ Page Language="C#" MasterPageFile="~/Secure/BackOffice.master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.Helpdesk.SearchPositionGroups" Title="Γκρουπ Θέσεων Πρακτικής Άσκησης" CodeBehind="SearchPositionGroups.aspx.cs" %>

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
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width: 900px" class="dv">
        <tr>
            <th colspan="8" class="popupHeader">Φίλτρα Αναζήτησης
            </th>
        </tr>
        <tr>
            <th style="width: 120px">ID Φορέα:
            </th>
            <td style="width: 130px">
                <asp:TextBox ID="txtProviderID" runat="server" TabIndex="1" Width="95%" />
            </td>
            <th style="width: 120px">Α.Φ.Μ. Φορέα:
            </th>
            <td style="width: 130px">
                <asp:TextBox ID="txtProviderAFM" runat="server" TabIndex="2" Width="95%" />
            </td>
            <th style="width: 120px">Χώρα:
            </th>
            <td style="width: 130px">
                <asp:DropDownList ID="ddlCountry" runat="server" ClientIDMode="Static" Width="98%"
                    OnInit="ddlCountry_Init" DataTextField="Name" DataValueField="ID" />
            </td>
        </tr>
        <tr>
            <th style="width: 100px">ID Group:
            </th>
            <td style="width: 200px">
                <asp:TextBox ID="txtGroupID" runat="server" TabIndex="5" Width="95%" />
            </td>
            <th style="width: 120px">Κατάσταση:
            </th>
            <td style="width: 130px">
                <asp:DropDownList ID="ddlPositionGroupStatus" runat="server" Width="98%" TabIndex="1"
                    OnInit="ddlPositionGroupStatus_Init" />
            </td>
            <th style="width: 100px">Νομός:
            </th>
            <td style="width: 200px">
                <asp:DropDownList ID="ddlPrefecture" runat="server" ClientIDMode="Static" Width="98%"
                    DataTextField="Name" DataValueField="ID" />
                <act:CascadingDropDown ID="cddPrefecture" runat="server" TargetControlID="ddlPrefecture"
                    ParentControlID="ddlCountry" Category="Prefectures" PromptText="-- επιλέξτε νομό --"
                    ServicePath="~/PortalServices/Services.asmx" ServiceMethod="GetPrefectures" LoadingText="Παρακαλω περιμένετε">
                </act:CascadingDropDown>
            </td>
        </tr>
        <tr>
            <th style="width: 100px">Αντικείμενο θέσης:
            </th>
            <td style="width: 200px">
                <asp:DropDownList ID="ddlPhysicalObject" runat="server" TabIndex="6" Width="100%"
                    OnInit="ddlPhysicalObject_Init" />
            </td>
            <th style="width: 120px">Τρόπος δημιουργίας:
            </th>
            <td style="width: 130px">
                <asp:DropDownList ID="ddlCreationType" runat="server" TabIndex="3" Width="100%">
                    <asp:ListItem Text="-- αδιάφορο --" Value="-1" Selected="True" />
                    <asp:ListItem Text="Από Φορέα Υποδοχής" Value="1" />
                    <asp:ListItem Text="Απο Γραφείο Πρακτικής" Value="2" />
                </asp:DropDownList>
            </td>
            <th style="width: 100px">Πόλη:
            </th>
            <td style="width: 200px">
                <asp:DropDownList ID="ddlCity" runat="server" ClientIDMode="Static" TabIndex="8"
                    Width="100%" DataTextField="Name" DataValueField="ID" />
                <act:CascadingDropDown ID="cddCity" runat="server" TargetControlID="ddlCity"
                    ParentControlID="ddlPrefecture" Category="Cities" PromptText="-- επιλέξτε πόλη --"
                    ServicePath="~/PortalServices/Services.asmx" ServiceMethod="GetCities" LoadingText="Παρακαλω περιμένετε">
                </act:CascadingDropDown>
            </td>
        </tr>
        <tr>
            <th style="width: 120px">Ημ/νία καταχώρισης:
            </th>
            <td style="width: 130px">
                <asp:DropDownList ID="ddlSubmissionDate" runat="server" TabIndex="3" Width="100%">
                    <asp:ListItem Text="-- αδιάφορο --" Value="-1" Selected="True" />
                    <asp:ListItem Text="Τελευταίες 24 ώρες" Value="1" />
                    <asp:ListItem Text="Τελευταία εβδομάδα" Value="2" />
                    <asp:ListItem Text="Τελευταίος μήνας" Value="3" />
                    <asp:ListItem Text="Τελευταίοι 3 μήνες" Value="4" />
                </asp:DropDownList>
            </td>
            <th></th>
            <th></th>
            <th></th>
            <th></th>
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
                        Δεν έχετε ακόμα δημιουργήσεις θέσεις πρακτικής άσκησης
                    </EmptyDataRow>
                </Templates>
                <Columns>
                    <dx:GridViewDataTextColumn Width="100px">
                        <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                        <CellStyle HorizontalAlign="Center" />
                        <DataItemTemplate>
                            <a runat="server" href="javascript:void(0);" class="icon-btn bg-undo"
                                onclick='<%# string.Format("return doAction(\"undeletegroup\", {0}, \"SearchPositionGroups\");", Eval("ID"))%>'
                                visible='<%# CanUnDeleteGroup((InternshipPositionGroup)Container.DataItem) %>'>Επαναφορά</a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ID" Caption="Κωδικός" HeaderStyle-HorizontalAlign="Center"
                        CellStyle-HorizontalAlign="Center" Width="50px" SortIndex="0" SortOrder="Descending">
                        <DataItemTemplate>
                            <%# Eval("ID")%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Στοιχεία Δημοσίευσης" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# GetPublishDetails((InternshipPositionGroup)Container.DataItem)%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Στοιχεία Φορέα" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# GetProviderDetails((InternshipPositionGroup)Container.DataItem)%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Αντικείμενο Θέσης" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# ((InternshipPositionGroup)Container.DataItem).GetPhysicalObjectDetails() %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Title" Caption="Τίτλος" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# Eval("Title")%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Τόπος Διεξαγωγής" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# ((InternshipPositionGroup)Container.DataItem).GetAddressDetails() %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Αρ. Θέσεων" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# ((InternshipPositionGroup)Container.DataItem).GetTotalPositions() %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Πλήρη Στοιχεία" Width="100px">
                        <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                        <CellStyle HorizontalAlign="Center" />
                        <DataItemTemplate>
                            <a runat="server" style="text-decoration: none" href="javascript:void(0)" 
                                onclick='<%# string.Format("popUp.show(\"ViewPosition.aspx?pID={0}\",\"Προβολή Θέσης\", null, 800, 700);", Eval("ID"))%>' >
                                <img src="/_img/iconView.png" alt="Προβολή Θέσης" /></a>
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
</asp:Content>
