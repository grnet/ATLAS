<%@ Page Language="C#" MasterPageFile="~/Secure/SecurePages.Master" AutoEventWireup="true"
    Inherits="StudentPractice.Portal.Secure.Students.SearchPositions" Title="Αναζήτηση Θέσεων Πρακτικής Άσκησης"
    CodeBehind="SearchPositions.aspx.cs" %>

<%@ Import Namespace="StudentPractice.BusinessModel" %>
<%@ Import Namespace="StudentPractice.Portal" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphHead" runat="server">
    <style type="text/css">
        .dxgvHeader td {
            font-size: 11px;
        }

        .dvTable {
            width: 1160px !important;
        }

            .dvTable tr th {
                width: 140px !important;
            }

            .dvTable tr td {
                width: 240px !important;
            }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <asp:LinkButton ID="cmdRefresh" runat="server" Style="display: none;" OnClick="cmdRefresh_Click" />
    <table class="dv dvTable">
        <tr>
            <th colspan="6" class="popupHeader">Φίλτρα Αναζήτησης
            </th>
        </tr>
        <tr>
            <th class="first">Κωδικός Θέσης:</th>
            <td class="first">
                <asp:TextBox ID="txtGroupID" runat="server" Width="280px" />
            </td>

            <th class="second">Xώρα:</th>
            <td class="second">
                <asp:DropDownList ID="ddlCountry" CssClass="countrySelector" runat="server" ClientIDMode="Static" Width="98%"
                    OnInit="ddlCountry_Init" DataTextField="Name" DataValueField="ID" />
            </td>

            <th class="third">Φυσικό αντικείμενο:</th>
            <td class="third">
                <asp:DropDownList ID="ddlPhysicalObject" runat="server" Width="98%" OnInit="ddlPhysicalObject_Init" />
            </td>
        </tr>
        <tr>
            <th class="first">Τίτλος:</th>
            <td class="first">
                <asp:TextBox ID="txtTitle" runat="server" Width="280px" />
            </td>

            <th class="second ddls">
                <asp:Literal ID="ltrPrefecture" runat="server" Text="Περιφερειακή Eνότητα" />:
            </th>
            <td class="ddls second">
                <asp:DropDownList ID="ddlPrefecture" runat="server" ClientIDMode="Static" Width="98%"
                    DataTextField="Name" DataValueField="ID" />
                <act:CascadingDropDown ID="cddPrefecture" runat="server" TargetControlID="ddlPrefecture"
                    ParentControlID="ddlCountry" Category="Prefectures" PromptText="-- αδιάφορο --"
                    ServicePath="~/PortalServices/Services.asmx" ServiceMethod="GetPrefectures" LoadingText="Παρακαλω περιμένετε">
                </act:CascadingDropDown>
            </td>
            <th class="txts second">Πόλη:</th>
            <td class="txts second">
                <asp:TextBox ID="txtCity" runat="server" Width="92%" />
            </td>

            <th class="third">Φορέας Υποδοχής:</th>
            <td class="third">
                <asp:TextBox ID="txtProvider" runat="server" Width="98%" />
            </td>
        </tr>
        <tr>
            <th class="first">Θέσεις για όλα τα Τμήματα:</th>
            <td class="first">
                <asp:CheckBox ID="chbxIsVisibleToAllAcademics" runat="server" Checked="true" />
            </td>
            <th class="ddls second">
                <asp:Literal ID="ltrCity" runat="server" Text="Καλλικρατικός Δήμος" />:
            </th>
            <td class="ddls second">
                <asp:DropDownList ID="ddlCity" runat="server" ClientIDMode="Static" Width="98%" DataTextField="Name"
                    DataValueField="ID" />
                <act:CascadingDropDown ID="cddCity" runat="server" TargetControlID="ddlCity"
                    ParentControlID="ddlPrefecture" Category="Cities" PromptText="-- αδιάφορο --"
                    ServicePath="~/PortalServices/Services.asmx" ServiceMethod="GetCities" LoadingText="Παρακαλω περιμένετε">
                </act:CascadingDropDown>
            </td>

            <th class="txts second"></th>
            <td class="txts second"></td>

            <th class="third"></th>
            <td class="third"></td>
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
                Width="100%" DataSourceForceStandardPaging="true">
                <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
                <SettingsPager PageSize="10" Summary-Text="Σελίδα {0} από {1} ({2} Θέσεις Πρακτικής Άσκησης)"
                    Summary-Position="Left" />
                <SettingsBehavior AllowSort="false" />
                <Styles>
                    <Cell Font-Size="11px" />
                </Styles>
                <Templates>
                    <EmptyDataRow>
                        Δεν βρέθηκαν αποτελέσματα
                    </EmptyDataRow>
                </Templates>
                <Columns>
                    <dx:GridViewDataTextColumn CellStyle-HorizontalAlign="Center" Width="20px">
                        <DataItemTemplate>
                            <a runat="server" style="text-decoration: none" href="javascript:void(0)" tip="Προβολή Θέσης"
                                onclick='<%# string.Format("popUp.show(\"ViewPosition.aspx?pID={0}\", \"Προβολή Θέσης Πρακτικής Άσκησης\");", Eval("ID"))%>' >
                                <img src="/_img/iconView.png" alt="Προβολή Θέσης" />
                            </a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ID" Caption="Κωδικός" HeaderStyle-HorizontalAlign="Center"
                        CellStyle-HorizontalAlign="Center" Width="50px">
                        <DataItemTemplate>
                            <%# Eval("ID")%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="FirstPublishedAt" Caption="Ημ/νία Δημοσίευσης"
                        CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# ((InternshipPositionGroup)Container.DataItem).GetFirstPublishedAt() %>
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
                    <dx:GridViewDataTextColumn FieldName="Provider.Name" Caption="Φορέας Υποδοχής"
                        CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# GetProviderDetails((InternshipPositionGroup)Container.DataItem)%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Prefecture.Name" Caption="Τόπος Διεξαγωγής"
                        CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# ((InternshipPositionGroup)Container.DataItem).GetAddressDetails() %>
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
