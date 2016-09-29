<%@ Page Language="C#" MasterPageFile="~/Secure/SecurePages.Master"
    AutoEventWireup="true" CodeBehind="OfficeDetails.aspx.cs" Inherits="StudentPractice.Portal.Secure.InternshipOffices.OfficeDetails"
    Title="Διαχείριση Στοιχείων Γραφείου Πρακτικής Άσκησης" %>

<%@ Register Src="~/UserControls/GenericControls/FlashMessage.ascx" TagName="FlashMessage" TagPrefix="my" %>
<%@ Register Src="~/UserControls/InternshipOfficeControls/InputControls/OfficeInput.ascx" TagName="OfficeInput" TagPrefix="my" %>
<%@ Register Src="~/UserControls/GenericControls/MemoEdit.ascx" TagName="MemoEdit" TagPrefix="my" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <style type="text/css">
        #email {
            width: 30.667em;
        }

        #mobilePhone {
            width: 320px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <script src="/_js/UserContactDetails.js" type="text/javascript"></script>
    <my:FlashMessage ID="fm" runat="server" CssClass="fade" />

    <dx:ASPxPageControl ID="dxTabs" runat="server" Width="100%">
        <TabPages>
            <dx:TabPage Text="Πρόσβαση σε Σχολές/Τμήματα" Name="tabOfficeAcademics">
                <ContentCollection>
                    <dx:ContentControl>
                        <asp:LinkButton ID="cmdRefresh" runat="server" Style="display: none;" OnClick="cmdRefresh_Click" />
                        <asp:MultiView ID="mvAcademics" runat="server">
                            <asp:View ID="vCanViewAllAcademics" runat="server">
                                <br />
                                <div class="reminder" style="font-weight: bold">
                                    Έχετε επιλέξει ότι το Γραφείο Πρακτικής Άσκησης που δημιουργήσατε αφορά ολόκληρο το Ίδρυμα.
                            <asp:PlaceHolder ID="phRestrictAcademics" runat="server">
                                <br />
                                Αν θέλετε να περιοριστεί η πρόσβαση σε συγκεκριμένες Σχολές/Τμήματα πατήστε το παρακάτω κουμπί.
                                <br />
                                <br />
                                <div style="clear: both; text-align: center">
                                    <a id="btnRestrictAcademics" runat="server" class="icon-btn bg-edit" href="javascript:void(0)"
                                        onclick="popUp.show('SelectOfficeAcademics.aspx','Προσθήκη Σχολών/Τμημάτων',cmdRefresh)">Περιορισμός σε συγκεκριμένες Σχολές/Τμήματα</a>
                                </div>
                            </asp:PlaceHolder>
                                </div>
                            </asp:View>
                            <asp:View ID="vCanViewCertainAcademics" runat="server">
                                <div class="reminder" style="font-weight: bold">
                                    <asp:Label ID="lblRepresentingAcademics" runat="server" />
                                </div>
                                <div style="padding: 20px 0px 10px;">
                                    <asp:LinkButton ID="btnAddAllAcademics" runat="server" CssClass="icon-btn bg-acceptAll" ValidationGroup="vgPosition"
                                        OnClick="btnAddAllAcademics_Click">Πρόσβαση σε όλες τις Σχολές/Τμήματα</asp:LinkButton>
                                    <a id="btnAddAcademics" runat="server" class="icon-btn bg-add" href="javascript:void(0)"
                                        onclick="popUp.show('SelectOfficeAcademics.aspx','Προσθήκη Σχολών/Τμημάτων',cmdRefresh)">Προσθήκη Σχολών/Τμημάτων</a>
                                </div>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <dx:ASPxGridView ID="gvAcademics" runat="server" AutoGenerateColumns="False" KeyFieldName="ID"
                                            EnableRowsCache="false" EnableCallBacks="true" Width="100%" OnCustomCallback="gvAcademics_CustomCallback"
                                            OnCustomDataCallback="gvAcademics_CustomDataCallback" ClientInstanceName="gv">
                                            <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
                                            <SettingsPager PageSize="10" Summary-Text="Σελίδα {0} από {1} ({2} Τμήματα)" Summary-Position="Left" />
                                            <Styles>
                                                <Cell Font-Size="11px" />
                                            </Styles>
                                            <Templates>
                                                <EmptyDataRow>
                                                    Δεν έχετε ακόμα επιλέξει το Τμήμα ή τα Τμήματα που εκπροσωπεί το Γραφείο Πρακτικής
                                                </EmptyDataRow>
                                            </Templates>
                                            <Columns>
                                                <dx:GridViewDataTextColumn FieldName="ID" Caption="Α/Α" HeaderStyle-HorizontalAlign="Center"
                                                    CellStyle-HorizontalAlign="Center" Width="50px">
                                                    <DataItemTemplate>
                                                        <%# Container.ItemIndex + 1 %>
                                                    </DataItemTemplate>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="Institution" Caption="Ίδρυμα" CellStyle-HorizontalAlign="Left">
                                                    <DataItemTemplate>
                                                        <%# Eval("Institution")%>
                                                    </DataItemTemplate>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="School" Caption="Σχολή" CellStyle-HorizontalAlign="Left"
                                                    SortIndex="0" SortOrder="Ascending">
                                                    <DataItemTemplate>
                                                        <%# Eval("School")%>
                                                    </DataItemTemplate>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="Department" Caption="Τμήμα" CellStyle-HorizontalAlign="Left"
                                                    SortIndex="1" SortOrder="Ascending">
                                                    <DataItemTemplate>
                                                        <%# Eval("Department")%>
                                                    </DataItemTemplate>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Ενέργειες" Width="100px">
                                                    <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                                                    <CellStyle HorizontalAlign="Center" />
                                                    <DataItemTemplate>
                                                        <a id="A1" runat="server" href="javascript:void(0);" class="icon-btn bg-delete" onclick='<%# string.Format("return doAction(\"delete\", {0}, \"OfficeDetails\");", Eval("ID"))%>'>Διαγραφή</a>
                                                    </DataItemTemplate>
                                                </dx:GridViewDataTextColumn>
                                            </Columns>
                                        </dx:ASPxGridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:View>
                        </asp:MultiView>
                        <script type="text/javascript">
                            function cmdRefresh() {
                                <%= this.Page.ClientScript.GetPostBackEventReference(this.cmdRefresh, string.Empty) %>;
                            }
                        </script>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>

            <dx:TabPage Text="Στοιχεία Γραφείου" Name="tabOffice">
                <ContentCollection>
                    <dx:ContentControl>
                        <div id="divAccountVerified" runat="server" class="reminder">
                            Επειδή ο λογαριασμός έχει πιστοποιηθεί δεν μπορείτε να τροποποιήσετε κάποια από
                    τα στοιχεία σας.<br />
                            Για την τροποποίησή τους παρακαλούμε επικοινωνήστε με το Γραφείο Αρωγής στο τηλέφωνο
                    215 215 7860
                        </div>
                        <asp:ValidationSummary ID="vdOffice" runat="server" ValidationGroup="vgOffice" />
                        <br />
                        <my:OfficeInput ID="ucOfficeInput" runat="server" ValidationGroup="vgOffice" />
                        <br />
                        <asp:LinkButton ID="btnUpdateOffice" runat="server" Text="Ενημέρωση Στοιχείων Γραφείου"
                            CssClass="icon-btn bg-accept" ValidationGroup="vgOffice" OnClick="btnUpdateOffice_Click" />
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>

            <dx:TabPage Text="Στοιχεία Χρήστη" Name="tabUser">
                <ContentCollection>
                    <dx:ContentControl>
                        <table style="width: 100%" class="dv">
                            <tr>
                                <th colspan="2" class="header">&raquo; Στοιχεία Χρήστη
                                </th>
                            </tr>
                            <tr>
                                <th>
                                    <label for="txtEmail">
                                        Όνομα Χρήστη:</label>
                                </th>
                                <td>
                                    <asp:Label ID="lblUserName" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <label for="txtEmail">
                                        E-mail:</label>
                                </th>
                                <td>
                                    <asp:TextBox ID="txtEmail" runat="server" Width="300px" Enabled="false" ClientIDMode="Static" />
                                    <a id="btnEmail" title="Αλλαγή E-mail" href="#" class="icon-btn bg-emailEdit">Αλλαγή E-mail</a>
                                    <div id="emailError" style="font-weight: bold" />
                                </td>
                            </tr>
                        </table>

                        <asp:LinkButton ID="btnSendEmailVerificationCode" runat="server" Text="Επαναποστολή E-mail Πιστοποίησης"
                            CssClass="icon-btn bg-email" ValidationGroup="vgContactInfo" OnClick="btnSendEmailVerificationCode_Click" Style="margin-top: 10px;" />

                        <asp:Label ID="lblContactInfoErrors" runat="server" Font-Bold="true" ForeColor="Red" Style="margin-top: 10px;" />
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>

            <dx:TabPage Text="Περιγραφή Πρακτικής Άσκησης Τμήματος" Name="tabPositionRules">
                <ContentCollection>
                    <dx:ContentControl>
                        <table class="dv" style="width: 100%">
                            <tr>
                                <th style="width: 130px">Επιλογή Τμήματος:
                                </th>
                                <td>
                                    <asp:DropDownList ID="ddlDepartment" runat="server" AutoPostBack="true" Width="60%" OnInit="ddlDepartment_Init" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged" />
                                    <asp:RequiredFieldValidator ID="rfvCountry" Display="Dynamic" runat="server" ControlToValidate="ddlDepartment" ValidationGroup="vgPositionRules"
                                        ErrorMessage="Το πεδίο 'Επιλογή Τμήματος' είναι υποχρεωτικό">
                            <img src="/_img/error.gif" class="errortip" title="Το πεδίο είναι υποχρεωτικό" />
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <th style="width: 130px">Περιγραφή:
                                </th>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanelPositionRules">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlDepartment" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <my:MemoEdit ID="ucPositionRules" runat="server" Rows="8" EnableCharCounter="true" IsRequired="true" ValidationGroup="vgPositionRules"
                                                CounterText="Στοιχεία για την Πρακτική Άσκηση του Τμήματος που επιθυμείτε να γνωστοποιήσετε στους Φορείς Υποδοχής
                             (π.χ. ελάχιστη διάρκεια, χρονικοί περιορισμοί, γενική περιγραφή) - ελεύθερο κείμενο μέχρι {0} χαρακτήρες, έχετε ήδη εισάγει {1} χαρακτήρες." />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:LinkButton ID="btnUpdatePositionRules" runat="server" Text="Ενημέρωση περιγραφής"
                            CssClass="icon-btn bg-accept" ValidationGroup="vgPositionRules" OnClick="btnUpdatePositionRules_Click" />
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
        </TabPages>
    </dx:ASPxPageControl>
</asp:Content>
