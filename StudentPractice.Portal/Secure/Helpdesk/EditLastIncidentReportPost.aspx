<%@ Page Title="" Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true"
    CodeBehind="EditLastIncidentReportPost.aspx.cs" Inherits="StudentPractice.Portal.Secure.Helpdesk.EditLastIncidentReportPost" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <asp:MultiView ID="mv" runat="server">
        <asp:View ID="vEditPost" runat="server">
            <div style="margin: 10px;">
                <table style="width: 100%" class="dv">
                    <tr>
                        <th colspan="2" class="header">
                            &raquo; Επεξεργασία μηνύματος
                        </th>
                    </tr>
                    <tr>
                        <th style="width: 30%">
                            Τύπος Κλήσης:
                        </th>
                        <td>
                            <asp:DropDownList ID="ddlCallType" runat="server" OnInit="ddlCallType_Init" Width="460px" />
                            <asp:RequiredFieldValidator ID="rfvCallType" Display="Dynamic" runat="server" ControlToValidate="ddlCallType"
                                ErrorMessage="Το πεδίο 'Τύπος Κλήσης' είναι υποχρεωτικό"><img src="/_img/error.gif" title="Το πεδίο είναι υποχρεωτικό" /></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th style="width: 30%">
                            Κείμενο μηνύματος:
                        </th>
                        <td>
                            <asp:TextBox ID="txtPostText" runat="server" TextMode="MultiLine" Rows="4" Width="460px" />
                            <asp:RequiredFieldValidator ID="rfvPostText" Display="Dynamic" runat="server" ControlToValidate="txtPostText"
                                ErrorMessage="Το πεδίο 'Κείμενο μηνύματος' είναι υποχρεωτικό"><img src="/_img/error.gif" title="Το πεδίο είναι υποχρεωτικό" /></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
            </div>
            <table style="width: 100%">
                <tr>
                    <td colspan="2" style="text-align: left; padding-left: 7px;">
                        <asp:LinkButton ID="btnSubmit" runat="server" Text="Ενημέρωση" CssClass="icon-btn bg-accept"
                            OnClick="btnSubmit_Click" />
                        <asp:LinkButton ID="btnCancel" runat="server" Text="Ακύρωση" CssClass="icon-btn bg-cancel"
                            CausesValidation="false" OnClick="btnCancel_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblValidationErrors" runat="server" CssClass="error" />
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vReportLocked" runat="server">
            <div class="reminder">
                <asp:Label ID="lblReportLocked" runat="server" />
            </div>
            <table style="width: 100%">
                <tr>
                    <td colspan="2" style="text-align: left; padding-left: 7px;">
                        <asp:LinkButton ID="btnClose" runat="server" Text="Κλείσιμο" CssClass="icon-btn bg-cancel"
                            CausesValidation="false" OnClientClick="window.parent.popUp.hide();" />
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vCannotEdit" runat="server">
            <div class="reminder">
                Δεν μπρορείτε να επεξεργαστείτε το μηνύμα γιατί έχει ήδη σταλθεί στον χρήστη.
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
