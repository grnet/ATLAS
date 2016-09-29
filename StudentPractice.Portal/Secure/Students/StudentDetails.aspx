<%@ Page Language="C#" Title="Διαχείριση Λογαριασμού" MasterPageFile="~/Secure/SecurePages.Master" AutoEventWireup="true" CodeBehind="StudentDetails.aspx.cs" Inherits="StudentPractice.Portal.Secure.Students.StudentDetails" %>

<%@ Register TagPrefix="my" Src="~/UserControls/GenericControls/FlashMessage.ascx" TagName="FlashMessage" %>
<%@ Register TagPrefix="my" Src="~/UserControls/StudentControls/ViewControls/StudentView.ascx" TagName="StudentView" %>
<%@ Register TagPrefix="my" Src="~/UserControls/StudentControls/InputControls/StudentInput.ascx" TagName="StudentInput" %>

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
            <dx:TabPage Text="Στοιχεία Φοιτητή" Name="tabStudent">
                <ContentCollection>
                    <dx:ContentControl>
                        <asp:MultiView ID="mvStudent" runat="server" ActiveViewIndex="0">
                            <asp:View ID="vStudentView" runat="server">
                                <my:StudentView ID="ucStudentView" runat="server" />
                                <br />
                                <asp:LinkButton ID="btnChangeStudentInfo" runat="server" Text="Αλλαγή Στοιχείων Ον/μου"
                                    CssClass="icon-btn bg-edit" OnClick="btnChangeStudentInfo_Click" />
                                <br />
                                <br />
                                <asp:Label ID="lblChangeStudentInfoError" runat="server" Font-Bold="true" ForeColor="Red" Visible="false"
                                    Text="Η αλλαγή των στοιχείων του Ον/μου δεν μπορεί να πραγματοποιηθεί γιατί έχετε αντιστοιχιστεί σε θέση Πρακτικής Άσκησης από το Γραφείο Πρακτικής του Ιδρύματός σας. Παρακαλούμε επικοινωνήστε με το Γραφείο Πρακτικής για την εκτέλεση της αλλαγής που επιθυμείτε." />
                            </asp:View>
                            <asp:View ID="vStudentInput" runat="server">
                                <my:StudentInput ID="ucStudentInput" runat="server" ValidationGroup="vgStudentInput" />
                                <br />
                                <asp:LinkButton ID="btnUpdateStudentInfo" runat="server" Text="Αποθήκευση Στοιχείων"
                                    CssClass="icon-btn bg-save" OnClick="btnUpdateStudentInfo_Click" ValidationGroup="vgStudentInput" />
                            </asp:View>
                        </asp:MultiView>
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>

            <dx:TabPage Text="Στοιχεία Λογαριασμού / Επικοινωνίας" Name="tabUser">
                <ContentCollection>
                    <dx:ContentControl>
                        <table style="width: 100%;" class="dv">
                            <tr>
                                <th colspan="2" class="header">&raquo; Στοιχεία Επικοινωνίας
                                </th>
                            </tr>
                            <tr>
                                <th style="width: 20%">
                                    <label for="txtEmail">
                                        E-mail:</label>
                                </th>
                                <td>
                                    <asp:TextBox ID="txtEmail" runat="server" Width="300px" Enabled="false" ClientIDMode="Static" />
                                    <a id="btnEmail" title="Αλλαγή E-mail" href="#" class="icon-btn bg-edit">Αλλαγή E-mail</a>
                                    <span id="emailError" />
                                </td>
                            </tr>
                            <tr>
                                <th style="width: 20%">
                                    <label for="txtMobilePhone">
                                        Κινητό:</label>
                                </th>
                                <td>
                                    <asp:TextBox ID="txtMobilePhone" runat="server" Width="100px" Enabled="false" ClientIDMode="Static" />
                                    <a id="btnMobilePhone" runat="server" title="Αλλαγή Κινητού" clientidmode="Static"
                                        href="#" class="icon-btn bg-edit">Αλλαγή Κινητού</a> <span>&nbsp;</span>
                                    <div id="mobilePhoneError" style="font-weight: bold;" />
                                </td>
                            </tr>
                        </table>

                        <asp:LinkButton ID="btnSendEmailVerificationCode" runat="server" Text="Επαναποστολή E-mail Πιστοποίησης"
                            CssClass="icon-btn bg-email" ValidationGroup="vgContactInfo" OnClick="btnSendEmailVerificationCode_Click" Style="margin-top: 10px;" />

                        <asp:Label ID="lblContactInfoErrors" runat="server" Font-Bold="true" ForeColor="Red" Style="margin-top: 10px;" />
                    </dx:ContentControl>
                </ContentCollection>
            </dx:TabPage>
        </TabPages>
    </dx:ASPxPageControl>
</asp:Content>
