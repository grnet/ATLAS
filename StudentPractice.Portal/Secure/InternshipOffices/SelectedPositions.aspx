<%@ Page Language="C#" MasterPageFile="~/Secure/SecurePages.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.InternshipOffices.SelectedPositions" Title="Επιλεγμένες Θέσεις Πρακτικής Άσκησης" CodeBehind="SelectedPositions.aspx.cs" %>

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
    <% if (DesignMode)
       { %>
    <script type="text/javascript" src="../../_js/ASPxScriptIntelliSense.js"></script>
    <% } %>
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
            <table class="dv dvTable">
                <tr>
                    <th colspan="6" class="popupHeader">Φίλτρα Αναζήτησης
                    </th>
                </tr>
                <tr>
                    <th class="first">Κωδικός Group:</th>
                    <td class="first">
                        <asp:TextBox ID="txtGroupID" runat="server" Width="94%" />
                    </td>

                    <th class="second">Χώρα:</th>
                    <td class="second">
                        <asp:DropDownList ID="ddlCountry" CssClass="countrySelector" runat="server" ClientIDMode="Static" Width="98%"
                            OnInit="ddlCountry_Init" DataTextField="Name" DataValueField="ID" />
                    </td>

                    <th class="third">Όνομα:</th>
                    <td class="third">
                        <asp:TextBox ID="txtFirstName" runat="server" Width="94%" />
                    </td>
                </tr>
                <tr>
                    <th class="first">Κωδικός Θέσης:</th>
                    <td class="first">
                        <asp:TextBox ID="txtPositionID" runat="server" Width="94%" />
                    </td>

                    <th class="ddls second">Περιφερειακή Ενότητα:</th>
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

                    <th class="third">Επώνυμο:</th>
                    <td class="third">
                        <asp:TextBox ID="txtLastName" runat="server" Width="94%" />
                    </td>
                </tr>
                <tr>
                    <th class="first">Φορέας Υποδοχής:</th>
                    <td class="first">
                        <asp:TextBox ID="txtProvider" runat="server" Width="94%" />
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


                    <th class="third">Αρ. Μητρώου:</th>
                    <td class="third">
                        <asp:TextBox ID="txtStudentNumber" runat="server" Width="94%" />
                    </td>
                </tr>
                <tr>
                    <th class="first">Τίτλος:</th>
                    <td class="first">
                        <asp:TextBox ID="txtTitle" runat="server" Width="94%" />
                    </td>

                    <th class="second">Ημ/νία δημοσίευσης:</th>
                    <td class="second">
                        <asp:DropDownList ID="ddlFirstPublishedAt" runat="server" Width="98%">
                            <asp:ListItem Text="-- αδιάφορο --" Value="-1" Selected="True" />
                            <asp:ListItem Text="Τελευταίες 24 ώρες" Value="1" />
                            <asp:ListItem Text="Τελευταία εβδομάδα" Value="2" />
                            <asp:ListItem Text="Τελευταίος μήνας" Value="3" />
                        </asp:DropDownList>
                    </td>

                    <th class="third">Τμήμα:</th>
                    <td class="third">
                        <asp:DropDownList ID="ddlDepartment" runat="server" Width="98%" OnInit="ddlDepartment_Init" />
                    </td>
                </tr>
                <tr>
                    <th class="first">ΑΦΜ Φορέα Υποδοχής:</th>
                    <td class="first">
                        <asp:TextBox ID="txtProviderAFM" runat="server" Width="94%" />
                    </td>

                    <th class="second">Φυσικό αντικείμενο:</th>
                    <td class="second">
                        <asp:DropDownList ID="ddlPhysicalObject" runat="server" Width="98%" OnInit="ddlPhysicalObject_Init" />
                    </td>

                    <th class="third">Κατάσταση Θέσης:</th>
                    <td class="third">
                        <asp:DropDownList ID="ddlPositionStatus" runat="server" Width="98%" OnInit="ddlPositionStatus_Init" />
                    </td>
                </tr>
                <tr>
                    <th class="first">Τρόπος δημιουργίας</th>
                    <td class="first">
                        <asp:DropDownList ID="ddlCreationType" runat="server" TabIndex="3" Width="98%" OnInit="ddlCreationType_Init" />
                    </td>
                    <%--<th class="second">Τρόπος Χρηματοδότησης:</th>
                    <td class="second">
                        <asp:DropDownList ID="ddlFundingType" runat="server" ClientIDMode="Static" OnInit="ddlFundingType_Init" Width="98%" />
                    </td>--%>
                    <th class="second"></th>
                    <th class="second"></th>
                    <th class="third"></th>
                    <th class="third"></th>
                </tr>
            </table>
            <div style="padding: 20px 0px 20px;">
                <asp:LinkButton ID="btnSearch" runat="server" Text="Αναζήτηση" OnClick="btnSearch_Click"
                    CssClass="icon-btn bg-search" />
                <asp:HyperLink ID="HyperLink1" runat="server" Text="Προσθήκη Ολοκληρωμένης Θέσης" class="icon-btn bg-addNewItem"
                    NavigateUrl="FinishedPositionProvider.aspx" />
                <asp:LinkButton ID="LinkButton2" runat="server" Text="Εξαγωγή σε Excel" OnClick="btnExport_Click" CssClass="icon-btn bg-excel" />
                <a class="icon-btn bg-colourExplanation" style="float: right;" href="javascript:void(0)" onclick="window.open('/ColourExplanation.aspx','colourExplanation','toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=500, height=250'); return false;">Επεξήγηση Χρωμάτων</a>
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <my:FlashMessage ID="fm" runat="server" CssClass="fade" />
                    <dx:ASPxGridView ID="gvPositions" runat="server" AutoGenerateColumns="False" DataSourceID="odsPositions"
                        KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true" Width="100%"
                        DataSourceForceStandardPaging="true" OnHtmlRowPrepared="gvPositions_HtmlRowPrepared"
                        OnCustomCallback="gvPositions_CustomCallback" ClientInstanceName="gv">
                        <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
                        <SettingsPager PageSize="10" Summary-Text="Σελίδα {0} από {1} ({2} Θέσεις Πρακτικής Άσκησης)"
                            Summary-Position="Left" />
                        <Styles>
                            <Header HorizontalAlign="Center" Wrap="True" />
                            <Cell HorizontalAlign="Left" Wrap="True" Font-Size="11px" />
                        </Styles>
                        <Templates>
                            <EmptyDataRow>
                                Δεν βρέθηκαν θέσεις πρακτικής άσκησης
                            </EmptyDataRow>
                        </Templates>
                        <Columns>
                            <dx:GridViewDataTextColumn CellStyle-HorizontalAlign="Center" CellStyle-Wrap="False">
                                <DataItemTemplate>
                                    <a runat="server" href="javascript:void(0);" style="text-decoration: none" onclick='<%# string.Format("return doAction(\"rollbackpreassignment\", {0}, \"SelectedPositions\");", Eval("ID"))%>'
                                        visible='<%# CanRollbackPreAssignment((InternshipPosition)Container.DataItem) && RollbackPreAssignmentWithoutPenalty((InternshipPosition)Container.DataItem) %>'
                                        tip="Αποδέσμευση">
                                        <img src="/_img/iconUndo.png" alt="Αποδέσμευση" title="Αποδέσμευση" />
                                    </a>

                                    <a runat="server" href="javascript:void(0);" style="text-decoration: none" onclick='<%# string.Format("return doAction(\"rollbackpreassignment2\", {0}, \"SelectedPositions\");", Eval("ID"))%>'
                                        visible='<%# CanRollbackPreAssignment((InternshipPosition)Container.DataItem) && RollbackPreAssignmentWithPenalty((InternshipPosition)Container.DataItem) %>'
                                        tip="Αποδέσμευση">
                                        <img src="/_img/iconUndo.png" alt="Αποδέσμευση" title="Αποδέσμευση" />
                                    </a>

                                    <a runat="server" href="javascript:void(0);" style="text-decoration: none" onclick='<%# string.Format("return doAction(\"rollbackcompletion\", {0}, \"SelectedPositions\");", Eval("ID"))%>'
                                        visible='<%# CanRollbackCompletion((InternshipPosition)Container.DataItem) %>'
                                        tip="Επαναφορά σε 'Υπό Διενέργεια'">
                                        <img src="/_img/iconUndo.png" alt="Επαναφορά σε Υπό Διενέργεια" title="Επαναφορά σε 'Υπό Διενέργεια'" />
                                    </a>

                                    <a runat="server" href="javascript:void(0);" style="text-decoration: none" onclick='<%# string.Format("return doAction(\"rollbackcancellation\", {0}, \"SelectedPositions\");", Eval("ID"))%>'
                                        visible='<%# CanRollbackCancellation((InternshipPosition)Container.DataItem) %>'
                                        tip="Επαναφορά σε Υπό Διενέργεια">
                                        <img src="/_img/iconUndo.png" alt="Επαναφορά σε Υπό Διενέργεια" title="Επαναφορά σε 'Υπό Διενέργεια'" />
                                    </a>

                                    <a runat="server" style="text-decoration: none" href='<%# string.Format("AssignPosition.aspx?pID={0}", Eval("ID"))%>'
                                        visible="<%# CanEditAssignment((InternshipPosition)Container.DataItem) %>"
                                        tip="Επεξεργασία">
                                        <img src="/_img/iconEdit.png" alt="Επεξεργασία Στοιχείων Αντιστοίχισης" title="Επεξεργασία" />
                                    </a>

                                    <a runat="server" style="text-decoration: none" href="javascript:void(0)"
                                        onclick=<%# string.Format("popUp.show('ViewPosition.aspx?pID={0}', 'Προβολή Θέσης Πρακτικής Άσκησης');", Eval("ID"))%>
                                        tip="Προβολή Θέσης">
                                        <img src="/_img/iconView.png" alt="Προβολή Θέσης" />
                                    </a>

                                    <a runat="server" style="text-decoration: none"
                                        href='<%# string.Format("FinishedPositionProvider.aspx?gID={0}", Eval("GroupID")) %>' visible='<%# CanEdit((InternshipPosition)Container.DataItem) %>'
                                        tip="Επεξεργασία Θέσης">
                                        <img src="/_img/iconEdit.png" alt="Επεξεργασία Θέσης" />
                                    </a>

                                    <a runat="server" href="javascript:void(0);" style="text-decoration: none" onclick='<%# string.Format("return doAction(\"deletefinishedposition\", {0}, \"SelectedPositions\");", Eval("ID"))%>'
                                        visible='<%# CanDelete((InternshipPosition)Container.DataItem) %>' title="Διαγραφή Θέσης"
                                        tip="Διαγραφή Θέσης">
                                        <img src="/_img/iconDelete.png" alt="Διαγραφή Θέσης" title="Διαγραφή Θέσης" />
                                    </a>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Κωδικός Θέσης" Width="40px" FieldName="ID">
                                <CellStyle HorizontalAlign="Center" />
                                <DataItemTemplate>
                                    <%# Eval("ID")%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Κωδικός Group" Width="40px" FieldName="GroupID">
                                <CellStyle HorizontalAlign="Center" />
                                <DataItemTemplate>
                                    <%# Eval("GroupID")%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Αντικείμενο Θέσης">
                                <DataItemTemplate>
                                    <%# ((InternshipPosition)Container.DataItem).GetPhysicalObjectDetails() %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Τίτλος" FieldName="InternshipPositionGroup.Title">
                                <DataItemTemplate>
                                    <%# Eval("InternshipPositionGroup.Title")%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Φορέας Υποδοχής" FieldName="InternshipPositionGroup.Provider.Name">
                                <DataItemTemplate>
                                    <%# ((InternshipPosition)Container.DataItem).InternshipPositionGroup.GetProviderDetails() %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Τόπος Διεξαγωγής" FieldName="InternshipPositionGroup.Country.NameInGreek" Width="150px">
                                <DataItemTemplate>
                                    <%# ((InternshipPosition)Container.DataItem).GetAddressDetails() %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Προδέσμευση για Τμήμα / Αντιστοιχισμένος Φοιτητής" Width="150px">
                                <DataItemTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <%# GetStudentDetails((InternshipPosition)Container.DataItem) %>
                                            </td>
                                            <td style="vertical-align: bottom">
                                                <a runat="server" href="javascript:void(0);" onclick='<%# string.Format("return doAction(\"deleteassignment\", {0}, \"SelectedPositions\");", Eval("ID"))%>'
                                                    visible='<%# CanDeleteAssignment((InternshipPosition)Container.DataItem) && DeleteAssignmentWithoutPenalty((InternshipPosition)Container.DataItem) %>'
                                                    title="Διαγραφή Αντιστοίχισης" style="text-decoration: none">
                                                    <img src="/_img/iconDelete.png" alt="Διαγραφή Αντιστοίχισης" title="Διαγραφή Αντιστοίχισης" /></a>
                                                <a runat="server" href="javascript:void(0);" onclick='<%# string.Format("return doAction(\"deleteassignment2\", {0}, \"SelectedPositions\");", Eval("ID"))%>'
                                                    visible='<%# CanDeleteAssignment((InternshipPosition)Container.DataItem) && DeleteAssignmentWithPenalty((InternshipPosition)Container.DataItem) %>'
                                                    title="Διαγραφή Αντιστοίχισης" style="text-decoration: none">
                                                    <img src="/_img/iconDelete.png" alt="Διαγραφή Αντιστοίχισης" title="Διαγραφή Αντιστοίχισης" /></a>
                                            </td>
                                        </tr>
                                    </table>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Μέρες που μπορεί να μείνει προδεσμευμένη" Width="50px" FieldName="DaysLeftForAssignment">
                                <CellStyle HorizontalAlign="Center" />
                                <DataItemTemplate>
                                    <%# ((InternshipPosition)Container.DataItem).GetDaysLeftForAssignment() %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Ενέργειες" Width="100px">
                                <CellStyle HorizontalAlign="Center" />
                                <DataItemTemplate>
                                    <a runat="server" class="btn" href='<%# string.Format("AssignPosition.aspx?pID={0}", Eval("ID"))%>'
                                        visible="<%# CanAssign((InternshipPosition)Container.DataItem) %>">Αντιστοίχιση</a>
                                    <a runat="server" class="btn" href="javascript:void(0)" onclick=<%# string.Format("popUp.show('CompletePosition.aspx?pID={0}', 'Ολοκλήρωση Θέσης Πρακτικής Άσκησης', cmdRefresh, 800, 610);", Eval("ID"))%>
                                        visible="<%# CanCompleteImplementation((InternshipPosition)Container.DataItem) %>">Ολοκλήρωση Θέσης </a>
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
                    <dx:GridViewDataTextColumn Caption="Κωδικός Group" FieldName="InternshipPositionGroup.ID" Name="GroupID" />
                    <dx:GridViewDataTextColumn Caption="Κωδικός Θέσης" FieldName="ID" Name="ID" />
                    <dx:GridViewDataTextColumn Caption="Τίτλος" FieldName="InternshipPositionGroup.Title" Name="Title" />
                    <dx:GridViewDataTextColumn Caption="Περιγραφή" FieldName="InternshipPositionGroup.Description" Name="Description" />
                    <dx:GridViewDataTextColumn Caption="Αντικείμενο Θέσης" Name="PhysicalObjects" />
                    <dx:GridViewDataTextColumn Caption="Φορέας Υποδοχής" FieldName="InternshipPositionGroup.Provider.Name" Name="ProviderName" />
                    <dx:GridViewDataTextColumn Caption="Διακριτικός Τίτλος" FieldName="InternshipPositionGroup.Provider.TradeName" Name="ProviderTradeName" />
                    <dx:GridViewDataTextColumn Caption="ΑΦΜ" FieldName="InternshipPositionGroup.Provider.AFM" Name="ProviderAFM" />
                    <dx:GridViewDataTextColumn Caption="ΔΟΥ" FieldName="InternshipPositionGroup.Provider.DOY" Name="ProviderDOY" />
                    <dx:GridViewDataTextColumn Caption="Είδος Φορέα" FieldName="Provider.ProviderType" Name="ProviderType" />
                    <dx:GridViewDataTextColumn Caption="Χώρα" Name="Country" />
                    <dx:GridViewDataTextColumn Caption="Περιφερειακή Ενότητα" Name="Prefecture" />
                    <dx:GridViewDataTextColumn Caption="Καλλικρατικός Δήμος" Name="City" />
                    <dx:GridViewDataTextColumn Caption="Κατάσταση Θέσης" Name="PositionStatus" />
                    <dx:GridViewDataTextColumn Caption="Είδος Θέσης" Name="PositionType" />
                    <dx:GridViewDataTextColumn Caption="Ημ/νία Προδέσμευσης" Name="PreAssignedAt" />
                    <dx:GridViewDataTextColumn Caption="Τμήμα Προδέσμευσης" Name="PreAssignedForAcademic.Department" />
                    <dx:GridViewDataTextColumn Caption="Ημ/νία Αντιστοίχισης" Name="AssignedAt" />
                    <dx:GridViewDataTextColumn Caption="Oν/μο Αντιστοιχισμένου Φοιτητή" Name="AssignedToStudent.ContactName" />
                    <dx:GridViewDataTextColumn Caption="A.M. Φοιτητή" Name="AssignedToStudent.StudentNumber" />
                    <dx:GridViewDataTextColumn Caption="Ημ/νία Έναρξης Εκτέλεσης" Name="ImplementationStartDate" />
                    <dx:GridViewDataTextColumn Caption="Ημ/νία Λήξης Εκτέλεσης" Name="ImplementationEndDate" />
                    <%--<dx:GridViewDataTextColumn Caption="Τρόπος Χρηματοδότησης" Name="FundingType" />--%>
                    <dx:GridViewDataTextColumn Caption="Ημ/νία Ολοκλήρωσης" Name="CompletedAt" />
                    <dx:GridViewDataTextColumn Caption="Παρατηρήσεις ολοκλήρωσης" FieldName="CompletionComments" Name="CompletionComments" />
                    <dx:GridViewDataTextColumn Caption="Τηλέφωνο Θέσης" FieldName="InternshipPositionGroup.ContactPhone" Name="ContactPhone" />
                    <dx:GridViewDataTextColumn Caption="Ον/μο Υπευθύνου Φορέα Υποδοχής" FieldName="InternshipPositionGroup.Provider.ContactName" Name="InternshipPositionGroup.Provider.ContactName" />
                    <dx:GridViewDataTextColumn Caption="Τηλέφωνο Υπευθύνου Φορέα Υποδοχής" FieldName="InternshipPositionGroup.Provider.ContactPhone" Name="InternshipPositionGroup.Provider.ContactPhone" />
                    <dx:GridViewDataTextColumn Caption="Ε-mail Υπευθύνου Φορέα Υποδοχής" FieldName="InternshipPositionGroup.Provider.ContactEmail" Name="InternshipPositionGroup.Provider.ContactEmail" />
                    <dx:GridViewDataTextColumn Caption="Ονοματεπώνυμο Επόπτη" FieldName="InternshipPositionGroup.Supervisor" Name="Supervisor" />
                    <dx:GridViewDataTextColumn Caption="E-mail Επόπτη" FieldName="InternshipPositionGroup.SupervisorEmail" Name="SupervisorEmail" />
                    <dx:GridViewDataTextColumn Caption="Τρόπος Δημιουργίας" Name="PositionCreationType" />
                    <dx:GridViewDataTextColumn Caption="Χρήστης ΓΠΑ" Name="OfficeUser" />
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
            <script type="text/javascript">
                function cmdRefresh() {
                    <%= this.Page.ClientScript.GetPostBackEventReference(this.cmdRefresh, string.Empty) %>;
                }
            </script>
        </asp:View>
    </asp:MultiView>
</asp:Content>
