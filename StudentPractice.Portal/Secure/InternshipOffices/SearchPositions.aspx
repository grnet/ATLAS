<%@ Page Language="C#" MasterPageFile="~/Secure/SecurePages.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.InternshipOffices.SearchPositions"
    Title="Αναζήτηση Θέσεων Πρακτικής Άσκησης" CodeBehind="SearchPositions.aspx.cs" %>

<%@ Register TagPrefix="my" Src="~/UserControls/GenericControls/FlashMessage.ascx" TagName="FlashMessage" %>
<%@ Import Namespace="StudentPractice.BusinessModel" %>
<%@ Import Namespace="StudentPractice.Portal" %>

<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%= txtProvider.ClientID %>').autocomplete({
                source: _providers,
                minLength: 3
            });
        });
    </script>
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

<asp:Content ContentPlaceHolderID="cphMain" runat="server">

    <asp:MultiView ID="mvAccount" runat="server" ActiveViewIndex="0">

        <asp:View ID="vAccountNotVerified" runat="server">
            <div class="reminder">
                <asp:Label ID="lblVerificationError" runat="server" />
            </div>
        </asp:View>

        <asp:View ID="vAccountVerified" runat="server">
            <asp:LinkButton ID="cmdRefresh" runat="server" Style="display: none;" OnClick="cmdRefresh_Click" />
            <my:FlashMessage ID="fm" runat="server" CssClass="fade" />
            <table class="dv dvTable">
                <tr>
                    <th colspan="6" class="popupHeader">Φίλτρα Αναζήτησης</th>
                </tr>
                <tr>
                    <th class="first">Κωδικός Θέσης:</th>
                    <td class="first">
                        <asp:TextBox ID="txtGroupID" runat="server" Width="94%" />
                    </td>

                    <th class="second">Χώρα:</th>
                    <td class="second">
                        <asp:DropDownList ID="ddlCountry" CssClass="countrySelector" runat="server" ClientIDMode="Static" Width="98%"
                            OnInit="ddlCountry_Init" DataTextField="Name" DataValueField="ID" />
                    </td>

                    <th class="third">Φορέας Υποδοχής:</th>
                    <td class="third">
                        <asp:TextBox ID="txtProvider" runat="server" Width="94%" />
                    </td>
                </tr>
                <tr>
                    <th class="first">Τίτλος:</th>
                    <td class="first">
                        <asp:TextBox ID="txtTitle" runat="server" Width="94%" />
                    </td>


                    <th class="ddls second">Περιφερειακή ενότητα:</th>
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
                        <asp:TextBox ID="txtCity" runat="server" Width="94%" />
                    </td>


                    <th class="third">ΑΦΜ Φορέα Υποδοχής:</th>
                    <td class="third">
                        <asp:TextBox ID="txtProviderAFM" runat="server" Width="94%" />
                    </td>
                </tr>
                <tr>
                    <th class="first">Τμήμα:</th>
                    <td class="first">
                        <asp:DropDownList ID="ddlDepartment" runat="server" Width="98%" OnInit="ddlDepartment_Init" />
                    </td>


                    <th class="ddls second">Καλλικρατικός Δήμος:</th>
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


                    <th class="third">Ημ/νία δημοσίευσης:</th>
                    <td class="third">
                        <asp:DropDownList ID="ddlFirstPublishedAt" runat="server" Width="98%">
                            <asp:ListItem Text="-- αδιάφορο --" Value="-1" Selected="True" />
                            <asp:ListItem Text="Τελευταίες 24 ώρες" Value="1" />
                            <asp:ListItem Text="Τελευταία εβδομάδα" Value="2" />
                            <asp:ListItem Text="Τελευταίος μήνας" Value="3" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th class="first">Θέσεις για όλα τα Τμήματα:</th>
                    <td class="first">
                        <asp:CheckBox ID="chbxIsVisibleToAllAcademics" runat="server" Checked="true" />
                    </td>

                    <th class="second">Φυσικό αντικείμενο:</th>
                    <td class="second">
                        <asp:DropDownList ID="ddlPhysicalObject" runat="server" Width="98%" OnInit="ddlPhysicalObject_Init" />
                    </td>

                    <th class="third"></th>
                    <td class="third"></td>
                </tr>
            </table>
            <div style="padding: 5px 0px 20px;">
                <asp:LinkButton ID="btnSearch" runat="server" Text="Αναζήτηση" OnClick="btnSearch_Click"
                    CssClass="icon-btn bg-search" />
                <asp:LinkButton ID="btnExport" runat="server" Text="Εξαγωγή σε Excel" OnClick="btnExport_Click"
                    CssClass="icon-btn bg-excel" />
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <dx:ASPxGridView ID="gvPositionGroups" runat="server" AutoGenerateColumns="False"
                        DataSourceID="odsPositionGroups" KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true"
                        Width="100%" DataSourceForceStandardPaging="true">
                        <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
                        <SettingsPager PageSize="10" Summary-Text="Σελίδα {0} από {1} ({2} Θέσεις Πρακτικής Άσκησης)"
                            Summary-Position="Left" />
                        <Styles>
                            <Header HorizontalAlign="Center" Wrap="True" />
                            <Cell Font-Size="11px" HorizontalAlign="Left" Wrap="True" />
                        </Styles>
                        <Templates>
                            <EmptyDataRow>
                                Δεν βρέθηκαν αποτελέσματα
                            </EmptyDataRow>
                        </Templates>
                        <Columns>
                            <dx:GridViewDataTextColumn CellStyle-HorizontalAlign="Center" Width="20px">
                                <CellStyle Wrap="False" />
                                <DataItemTemplate>
                                    <a runat="server" style="text-decoration: none" href="javascript:void(0)" tip="Προβολή Θέσης"
                                        onclick='<%# string.Format("popUp.show(\"ViewPositionGroup.aspx?pID={0}\", \"Προβολή Θέσης Πρακτικής Άσκησης\");", Eval("ID"))%>'>
                                        <img src="/_img/iconView.png" alt="Προβολή Θέσης" />
                                    </a>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Ημ/νία Δημοσίευσης" FieldName="LastPublishedAt" SortOrder="Descending" Width="125px">
                                <CellStyle HorizontalAlign="Center" />
                                <DataItemTemplate>
                                    <%# ((InternshipPositionGroup)Container.DataItem).GetLastPublishedAt() %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Κωδικός" FieldName="ID" CellStyle-HorizontalAlign="Center" Width="70px">
                                <DataItemTemplate>
                                    <%# Eval("ID")%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Αντικείμενο Θέσης">
                                <DataItemTemplate>
                                    <%# ((InternshipPositionGroup)Container.DataItem).GetPhysicalObjectDetails() %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Τίτλος" FieldName="Title">
                                <DataItemTemplate>
                                    <%# Eval("Title")%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Φορέας Υποδοχής" FieldName="Provider.Name">
                                <DataItemTemplate>
                                    <%#  ((InternshipPositionGroup)Container.DataItem).GetProviderDetails() %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Τόπος Διεξαγωγής" FieldName="Country.NameInGreek">
                                <DataItemTemplate>
                                    <%# ((InternshipPositionGroup)Container.DataItem).GetAddressDetails() %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Διαθέσιμες Θέσεις" FieldName="AvailablePositions" Width="70px">
                                <CellStyle HorizontalAlign="Center" />
                                <DataItemTemplate>
                                    <%# Eval("AvailablePositions") %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Ενέργειες" Width="100px">
                                <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                                <CellStyle HorizontalAlign="Center" />
                                <DataItemTemplate>
                                    <a runat="server" class="btn" href="javascript:void(0)" onclick=<%# string.Format("popUp.show('PreAssignPosition.aspx?gID={0}','Προδέσμευση Θέσης Πρακτικής Άσκησης',cmdRefresh, 800, 610);", Eval("ID"))%>>Προδέσμευση</a>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:ASPxGridView>
                </ContentTemplate>
            </asp:UpdatePanel>
            <dx:ASPxGridView ID="gvPositionGroupsExport" runat="server" Visible="false" AutoGenerateColumns="False"
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
                    <dx:GridViewDataTextColumn Caption="Κωδικός Group" FieldName="ID" Name="GroupID" />
                    <dx:GridViewDataTextColumn Caption="Τίτλος" FieldName="Title" Name="Title" />
                    <dx:GridViewDataTextColumn Caption="Περιγραφή" FieldName="Description" Name="Description" />
                    <dx:GridViewDataTextColumn Caption="Αντικείμενο Θέσης" Name="PhysicalObjects" />
                    <dx:GridViewDataTextColumn Caption="Φορέας Υποδοχής" FieldName="Provider.Name" Name="ProviderName" />
                    <dx:GridViewDataTextColumn Caption="Διακριτικός Τίτλος" FieldName="Provider.TradeName" Name="ProviderTradeName" />
                    <dx:GridViewDataTextColumn Caption="ΑΦΜ" FieldName="Provider.AFM" Name="ProviderAFM" />
                    <dx:GridViewDataTextColumn Caption="Είδος Φορέα" FieldName="Provider.ProviderType" Name="ProviderType" />
                    <dx:GridViewDataTextColumn Caption="Τηλέφωνο Θέσης" FieldName="ContactPhone" Name="ContactPhone" />
                    <dx:GridViewDataTextColumn Caption="Ον/μο Υπευθύνου Φορέα Υποδοχής" FieldName="Provider.ContactName" Name="Provider.ContactName" />
                    <dx:GridViewDataTextColumn Caption="Τηλέφωνο Υπευθύνου Φορέα Υποδοχής" FieldName="Provider.ContactPhone" Name="Provider.ContactPhone" />
                    <dx:GridViewDataTextColumn Caption="E-mail Υπευθύνου Φορέα Υποδοχής" FieldName="Provider.ContactEmail" Name="Provider.ContactEmail" />
                    <dx:GridViewDataTextColumn Caption="Διάρκεια (εβδομάδες)" FieldName="Duration" Name="Duration" />
                    <dx:GridViewDataTextColumn Caption="Χώρα" Name="Country" />
                    <dx:GridViewDataTextColumn Caption="Περιφερειακή Ενότητα" Name="Prefecture" />
                    <dx:GridViewDataTextColumn Caption="Καλλικρατικός Δήμος" Name="City" />
                    <dx:GridViewDataTextColumn Caption="Ημ/νία Δημοσίευσης" Name="FirstPublishedAt" />
                    <dx:GridViewDataTextColumn Caption="Αριθμός θέσεων" FieldName="TotalPositions" Name="TotalPositions" />
                    <dx:GridViewDataTextColumn Caption="Διαθέσιμη Χρονική Περίοδος" Name="TimeAvailable" />
                    <dx:GridViewDataTextColumn Caption="Είδος Θέσης" Name="PositionType" />
                    <dx:GridViewDataTextColumn Caption="Ονοματεπώνυμο Επόπτη" FieldName="Supervisor" Name="Supervisor" />
                    <dx:GridViewDataTextColumn Caption="E-mail Επόπτη" FieldName="SupervisorEmail" Name="SupervisorEmail" />
                    <dx:GridViewDataTextColumn Caption="Τμήμα για το οποίο είναι προσβάσιμη η θέση" Name="Departments" />
                </Columns>
            </dx:ASPxGridView>
            <dx:ASPxGridViewExporter ID="gveIntershipPositionGroups" runat="server" GridViewID="gvPositionGroupsExport" OnRenderBrick="gveIntershipPositionGroups_RenderBrick">
            </dx:ASPxGridViewExporter>
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
        </asp:View>
    </asp:MultiView>
</asp:Content>
