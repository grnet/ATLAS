<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchAndRegisterStudent.ascx.cs" Inherits="StudentPractice.Portal.UserControls.GenericControls.SearchAndRegisterStudent" %>

<%@ Register Src="~/UserControls/GenericControls/FlashMessage.ascx" TagName="FlashMessage" TagPrefix="my" %>
<%@ Import Namespace="StudentPractice.BusinessModel" %>

<dx:ASPxPageControl ID="dxPageControl" runat="server" Width="100%">
    <TabPages>
        <dx:TabPage Text="Αναζήτηση με Αριθμό Μητρώου" Name="SearchByStudentNumber">
            <ContentCollection>
                <dx:ContentControl>
                    <my:FlashMessage ID="fmStudentNumberSearch" runat="server" SessionKey="flashStudentNumber" />
                    <table style="width: 700px" class="dv">
                        <tr>
                            <th colspan="2" class="popupHeader" style="color: #444444;">Αναζήτηση φοιτητή με τον Αριθμό Μητρώου</th>
                        </tr>
                        <asp:PlaceHolder ID="phInstitutionSearch" runat="server" Visible="false">
                            <tr>
                                <th style="width: 190px">Ίδρυμα
                                </th>
                                <td>
                                    <asp:DropDownList ID="ddlInstitution" runat="server" Width="98%" OnInit="ddlInstitution_Init" />
                                </td>
                            </tr>
                            <tr>
                                <th style="width: 190px">Τμήμα
                                </th>
                                <td>
                                    <asp:DropDownList ID="ddlDepartmentByInstSearch" runat="server" Width="85%" ValidationGroup="StudentNumberSearch" DataTextField="Name"
                                        DataValueField="ID" />

                                    <act:CascadingDropDown ID="cddDepartment" runat="server" TargetControlID="ddlDepartmentByInstSearch"
                                        ParentControlID="ddlInstitution" Category="Academics" PromptText="-- αδιάφορο --"
                                        ServicePath="~/PortalServices/Services.asmx" ServiceMethod="GetAcademics" LoadingText="Παρακαλω περιμένετε">
                                    </act:CascadingDropDown>

                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic" ValidationGroup="StudentNumberSearch"
                                        ControlToValidate="ddlDepartmentByInstSearch" ErrorMessage="Πρέπει να επιλέξετε το τμήμα">
                                        <img src="/_img/error.gif" class="errortip" title="Πρέπει να επιλέξετε το τμήμα" />
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="phAcademicSearch" runat="server" Visible="false">
                            <tr>
                                <th style="width: 190px">Τμήμα
                                </th>
                                <td>
                                    <asp:Literal ID="litDepartment" runat="server" />
                                    <asp:DropDownList ID="ddlDepartmentSearch" runat="server" OnInit="ddlDepartment_Init" Width="85%" ValidationGroup="StudentNumberSearch" />
                                    <asp:RequiredFieldValidator ID="rfvDepartmentDDL" runat="server" Display="Dynamic" ValidationGroup="StudentNumberSearch"
                                        ControlToValidate="ddlDepartmentSearch" ErrorMessage="Πρέπει να επιλέξετε το τμήμα">
                                        <img src="/_img/error.gif" class="errortip" title="Πρέπει να επιλέξετε το τμήμα" />
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </asp:PlaceHolder>
                        <tr>
                            <th style="width: 190px">Αριθμός Μητρώου
                            </th>
                            <td>
                                <asp:TextBox ID="txtStudentNumberSearch" runat="server" MaxLength="50" Width="85%" ValidationGroup="StudentNumberSearch" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ValidationGroup="StudentNumberSearch"
                                    ControlToValidate="txtStudentNumberSearch" ErrorMessage="Πρέπει να εισάγετε τον αριθμό μητρώου">
                                    <img src="/_img/error.gif" class="errortip" title="Πρέπει να εισάγετε τον αριθμό μητρώου" />
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                    <div style="padding: 5px 0px 10px;">
                        <asp:LinkButton ID="btnPreviewByStudentNumber" runat="server" Text="Αναζήτηση" ValidationGroup="StudentNumberSearch"
                            OnClick="btnPreviewByStudentNumber_Click" CssClass="icon-btn bg-search" Visible="false" Font-Bold="true"  />

                        <asp:LinkButton ID="btnSearchByStudentNumber" runat="server" Text="Αναζήτηση" ValidationGroup="StudentNumberSearch"
                            OnClick="btnSearchByStudentNumber_Click" CssClass="icon-btn bg-search" />                          

                        <asp:LinkButton ID="btnCancelByStudentNumber" runat="server" Text="Ακύρωση" OnClick="btnCancel_Click"
                            CssClass="icon-btn bg-cancel" />
                    </div>

                    <br />
                    <asp:Literal ID="ltrStudentNumber" runat="server"/>
                    <dx:ASPxGridView ID="gvStudentsByStudentNumber" runat="server" AutoGenerateColumns="False" DataSourceID="odsStudents" Visible="false"
                        KeyFieldName="ID" EnableRowsCache="false" DataSourceForceStandardPaging="true"
                        EnableCallBacks="false" Width="100%" OnHtmlDataCellPrepared="gvStudents_HtmlDataCellPrepared"
                        OnHtmlRowPrepared="gvStudents_HtmlRowPrepared">
                        <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
                        <SettingsPager PageSize="10" Summary-Text="Σελίδα {0} από {1} ({2} Φοιτητές)" Summary-Position="Left" />
                        <Templates>
                            <EmptyDataRow>
                                Δεν βρέθηκαν αποτελέσματα
                            </EmptyDataRow>
                        </Templates>
                        <Columns>
                            <dx:GridViewDataTextColumn Name="Actions" CellStyle-HorizontalAlign="Center">
                                <DataItemTemplate>
                                    <asp:LinkButton ID="btnAddImplementationData" runat="server" class="icon-btn bg-accept" href="javascript:void(0)" Text="Επιλογή" />
                                    <asp:LinkButton ID="btnChangeAssignedStudent" runat="server" class="icon-btn bg-accept" href="javascript:void(0)" Text="Επιλογή" />
                                    <asp:Image ID="tipAlreadyAssignedStudent" runat="server" CssClass="tooltip" ImageUrl="~/_img/iconHelp.png"
                                        tip="Ο φοιτητής είναι ήδη αντιστοιχισμένος σε θέση πρακτικής άσκησης" />
                                    <asp:Image ID="tipStudentAcademicNotSelectable" runat="server" CssClass="tooltip" ImageUrl="~/_img/iconHelp.png"
                                        tip="Ο φοιτητής ανήκει σε Τμήμα για το οποίο έχει δηλωθεί από το Φορέα Υποδοχής ότι η θέση δεν είναι διαθέσιμη" />
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Όνομα" CellStyle-HorizontalAlign="Left" HeaderStyle-Font-Bold="true">
                                <DataItemTemplate>
                                    <%# Eval("GreekFirstName") ?? Eval("OriginalFirstName") %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Επώνυμο" CellStyle-HorizontalAlign="Left"
                                HeaderStyle-Font-Bold="true">
                                <DataItemTemplate>
                                    <%# Eval("GreekLastName") ?? Eval("OriginalLastName") %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Αρ. Μητρώου" CellStyle-HorizontalAlign="Left"
                                HeaderStyle-Font-Bold="true">
                                <DataItemTemplate>
                                    <%# GetStudentNumber((Student)Container.DataItem) %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Αρ. Ακαδημαϊκής Ταυτότητας" CellStyle-HorizontalAlign="Left" HeaderStyle-Font-Bold="true">
                                <DataItemTemplate>
                                    <%# Eval("AcademicIDNumber") %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Ίδρυμα" Name="Institution" CellStyle-HorizontalAlign="Left" HeaderStyle-Font-Bold="true">
                                <DataItemTemplate>
                                    <%# GetInstitutionDetails((Student)Container.DataItem)%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Τμήμα" CellStyle-HorizontalAlign="Left" HeaderStyle-Font-Bold="true">
                                <DataItemTemplate>
                                    <%# GetDepartmentDetails((Student)Container.DataItem)%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:ASPxGridView>
                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Αναζήτηση με Κωδικό Ακ. Ταυτότητας" Name="SearchByAcademicID">
            <ContentCollection>
                <dx:ContentControl>
                    <my:FlashMessage ID="fmAcademicNumberSearch" runat="server" SessionKey="flashAcademicNumber" />
                    <table style="width: 700px" class="dv">
                        <tr>
                            <th colspan="2" class="popupHeader" style="color: #444444;">Αναζήτηση φοιτητή με τον Κωδικό Ταυτότητας
                            </th>
                        </tr>
                        <tr>
                            <th style="width: 190px">12ψήφιος Κωδικός Ταυτότητας:
                            </th>
                            <td style="width: 140px">
                                <asp:TextBox ID="txtAcademicIDNumberSearch" runat="server" MaxLength="16" Width="85%" ValidationGroup="AcademicIDNumberSearch" />
                                <asp:RequiredFieldValidator ID="rfvAcademicIDNumber" runat="server" Display="Dynamic" ValidationGroup="AcademicIDNumberSearch"
                                    ControlToValidate="txtAcademicIDNumberSearch" ErrorMessage="Πρέπει να εισάγετε το 12ψήφιο Κωδικό της Ακαδημαϊκής Ταυτότητας">
                                    <img src="/_img/error.gif" class="errortip" title="Πρέπει να εισάγετε το 12ψήφιο Κωδικό της Ακαδημαϊκής Ταυτότητας" />
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                    <div style="padding: 5px 0px 10px;">
                         <asp:LinkButton ID="btnPreviewByAcademicIDNumber" runat="server" Text="Αναζήτηση" ValidationGroup="AcademicIDNumberSearch"
                            OnClick="btnPreviewByAcademicIDNumber_Click" CssClass="icon-btn bg-search" Visible="false"  Font-Bold="true"  />

                        <asp:LinkButton ID="btnSearchByAcademicIDNumber" runat="server" Text="Αναζήτηση" ValidationGroup="AcademicIDNumberSearch"
                            OnClick="btnSearchByAcademicIDNumber_Click" CssClass="icon-btn bg-search" />

                        <asp:LinkButton ID="btnCancelByAcademicIDNumber" runat="server" Text="Ακύρωση" OnClick="btnCancel_Click"
                            CssClass="icon-btn bg-cancel" />
                    </div>
                    <br />
                    <asp:Literal ID="ltrAcademicID" runat="server" />
                    <dx:ASPxGridView ID="gvStudentsByAcademicID" runat="server" AutoGenerateColumns="False" DataSourceID="odsStudents" Visible="false"
                        KeyFieldName="ID" EnableRowsCache="false" DataSourceForceStandardPaging="true"
                        EnableCallBacks="false" Width="100%" OnHtmlDataCellPrepared="gvStudents_HtmlDataCellPrepared"
                        OnHtmlRowPrepared="gvStudents_HtmlRowPrepared">
                        <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
                        <SettingsPager PageSize="10" Summary-Text="Σελίδα {0} από {1} ({2} Φοιτητές)" Summary-Position="Left" />
                        <Templates>
                            <EmptyDataRow>
                                Δεν βρέθηκαν αποτελέσματα
                            </EmptyDataRow>
                        </Templates>
                        <Columns>
                            <dx:GridViewDataTextColumn Name="Actions" CellStyle-HorizontalAlign="Center">
                                <DataItemTemplate>
                                    <asp:LinkButton ID="btnAddImplementationData" runat="server" class="icon-btn bg-accept" href="javascript:void(0)" Text="Επιλογή" />
                                    <asp:LinkButton ID="btnChangeAssignedStudent" runat="server" class="icon-btn bg-accept" href="javascript:void(0)" Text="Επιλογή" />
                                    <asp:Image ID="tipAlreadyAssignedStudent" runat="server" CssClass="tooltip" ImageUrl="~/_img/iconHelp.png" tip="Ο φοιτητής είναι ήδη αντιστοιχισμένος σε θέση πρακτικής άσκησης" />
                                    <asp:Image ID="tipStudentAcademicNotSelectable" runat="server" CssClass="tooltip" ImageUrl="~/_img/iconHelp.png" tip="Ο φοιτητής ανήκει σε Τμήμα για το οποίο έχει δηλωθεί από το Φορέα Υποδοχής ότι η θέση δεν είναι διαθέσιμη" />
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Όνομα" CellStyle-HorizontalAlign="Left" HeaderStyle-Font-Bold="true">
                                <DataItemTemplate>
                                    <%# Eval("GreekFirstName") ?? Eval("OriginalFirstName") %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Επώνυμο" CellStyle-HorizontalAlign="Left"
                                HeaderStyle-Font-Bold="true">
                                <DataItemTemplate>
                                    <%# Eval("GreekLastName") ?? Eval("OriginalLastName") %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Αρ. Μητρώου" CellStyle-HorizontalAlign="Left"
                                HeaderStyle-Font-Bold="true">
                                <DataItemTemplate>
                                    <%# GetStudentNumber((Student)Container.DataItem) %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Αρ. Ακαδημαϊκής Ταυτότητας" CellStyle-HorizontalAlign="Left" HeaderStyle-Font-Bold="true">
                                <DataItemTemplate>
                                    <%# Eval("AcademicIDNumber") %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Ίδρυμα" Name="Institution" CellStyle-HorizontalAlign="Left" HeaderStyle-Font-Bold="true">
                                <DataItemTemplate>
                                    <%# GetInstitutionDetails((Student)Container.DataItem)%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Τμήμα" CellStyle-HorizontalAlign="Left" HeaderStyle-Font-Bold="true">
                                <DataItemTemplate>
                                    <%# GetDepartmentDetails((Student)Container.DataItem)%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:ASPxGridView>
                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>

        <dx:TabPage Text="Αναζήτηση με Ονοματεπώνυμο" Name="SearchByName">
            <ContentCollection>
                <dx:ContentControl>
                    <div class="reminder">Εμφανίζονται οι φοιτητές που έχουν εγγραφεί στο σύστημα ΑΤΛΑΣ. Εάν δεν βρίσκετε εδώ το φοιτητή, προτείνεται να τον αναζητήσετε με βάση τον αριθμό μητρώου ή τον 12-ψήφιο της ακαδημαϊκής ταυτότητας.</div>
                    <br />
                    <table width="700px" class="dv">
                        <tr>
                            <th colspan="4" class="popupHeader" scope="row" id="filter" style="color: #444444;">Φίλτρα Αναζήτησης Φοιτητή
                            </th>
                        </tr>
                        <tr>
                            <th style="width: 50px">Επώνυμο:
                            </th>
                            <td style="width: 100px">
                                <asp:TextBox ID="txtLastName" runat="server" TabIndex="2" Width="95%" />
                            </td>
                            <th style="width: 50px">Όνομα:
                            </th>
                            <td style="width: 100px">
                                <asp:TextBox ID="txtFirstName" runat="server" TabIndex="1" Width="95%" />
                            </td>
                        </tr>
                    </table>
                    <div style="padding: 5px 0px 10px;">
                        <asp:LinkButton ID="btnSearch" runat="server" Text="Αναζήτηση" OnClick="btnSearch_Click"
                            CssClass="icon-btn bg-search" />
                        <asp:LinkButton ID="btnCancel" runat="server" Text="Ακύρωση" OnClick="btnCancel_Click"
                            CssClass="icon-btn bg-cancel" />
                    </div>

                    <br />                    
                    <dx:ASPxGridView ID="gvStudents" runat="server" AutoGenerateColumns="False" DataSourceID="odsStudents" Visible="false"
                        KeyFieldName="ID" EnableRowsCache="false" DataSourceForceStandardPaging="true"
                        EnableCallBacks="false" Width="100%" OnHtmlDataCellPrepared="gvStudents_HtmlDataCellPrepared"
                        OnHtmlRowPrepared="gvStudents_HtmlRowPrepared">
                        <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
                        <SettingsPager PageSize="10" Summary-Text="Σελίδα {0} από {1} ({2} Φοιτητές)" Summary-Position="Left" />
                        <Templates>
                            <EmptyDataRow>
                                Δεν βρέθηκαν αποτελέσματα
                            </EmptyDataRow>
                        </Templates>
                        <Columns>
                            <dx:GridViewDataTextColumn Name="Actions" CellStyle-HorizontalAlign="Center">
                                <DataItemTemplate>
                                    <asp:LinkButton ID="btnAddImplementationData" runat="server" class="icon-btn bg-accept" href="javascript:void(0)" Text="Επιλογή" />
                                    <asp:LinkButton ID="btnChangeAssignedStudent" runat="server" class="icon-btn bg-accept" href="javascript:void(0)" Text="Επιλογή" />
                                    <asp:Image ID="tipAlreadyAssignedStudent" runat="server" CssClass="tooltip" ImageUrl="~/_img/iconHelp.png"
                                        tip="Ο φοιτητής είναι ήδη αντιστοιχισμένος σε θέση πρακτικής άσκησης" />
                                    <asp:Image ID="tipStudentAcademicNotSelectable" runat="server" CssClass="tooltip" ImageUrl="~/_img/iconHelp.png"
                                        tip="Ο φοιτητής ανήκει σε Τμήμα για το οποίο έχει δηλωθεί από το Φορέα Υποδοχής ότι η θέση δεν είναι διαθέσιμη" />
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Όνομα" CellStyle-HorizontalAlign="Left" HeaderStyle-Font-Bold="true">
                                <DataItemTemplate>
                                    <%# Eval("GreekFirstName") ?? Eval("OriginalFirstName") %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Επώνυμο" CellStyle-HorizontalAlign="Left"
                                HeaderStyle-Font-Bold="true">
                                <DataItemTemplate>
                                    <%# Eval("GreekLastName") ?? Eval("OriginalLastName") %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Αρ. Μητρώου" CellStyle-HorizontalAlign="Left"
                                HeaderStyle-Font-Bold="true">
                                <DataItemTemplate>
                                    <%# GetStudentNumber((Student)Container.DataItem) %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Αρ. Ακαδημαϊκής Ταυτότητας" CellStyle-HorizontalAlign="Left" HeaderStyle-Font-Bold="true">
                                <DataItemTemplate>
                                    <%# Eval("AcademicIDNumber") %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Ίδρυμα" Name="Institution" CellStyle-HorizontalAlign="Left" HeaderStyle-Font-Bold="true">
                                <DataItemTemplate>
                                    <%# GetInstitutionDetails((Student)Container.DataItem)%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Τμήμα" CellStyle-HorizontalAlign="Left" HeaderStyle-Font-Bold="true">
                                <DataItemTemplate>
                                    <%# GetDepartmentDetails((Student)Container.DataItem)%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:ASPxGridView>

                </dx:ContentControl>
            </ContentCollection>
        </dx:TabPage>
    </TabPages>
</dx:ASPxPageControl>
<asp:ObjectDataSource ID="odsStudents" runat="server" TypeName="StudentPractice.Portal.DataSources.Students"
    SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
    EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsStudents_Selecting">
    <SelectParameters>
        <asp:Parameter Name="criteria" Type="Object" />
    </SelectParameters>
</asp:ObjectDataSource>
