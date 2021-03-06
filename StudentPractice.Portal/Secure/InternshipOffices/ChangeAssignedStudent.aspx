﻿<%@ Page Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.InternshipOffices.ChangeAssignedStudent"
    CodeBehind="ChangeAssignedStudent.aspx.cs" Title="Επεξεργασία Στοιχείων Φοιτητή" %>

<%@ Register TagPrefix="my" Src="~/UserControls/StudentControls/InputControls/StudentInput.ascx" TagName="StudentInput" %>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <asp:MultiView ID="mvAccess" runat="server" ActiveViewIndex="1">

        <asp:View ID="vNoAccess" runat="server">
            <div class="reminder">
                <asp:Label ID="lblVerificationError" runat="server"
                    Text="To Γραφείο Πρακτικής δεν διαχειρίζεται το τμήμα του Φοιτητή που έχει επιλεγεί. Αν πιστεύετε ότι αυτό δεν είναι σωστό, παρακαλούμε επικοινωνήστε με Γραφείο Αρωγής Χρηστών για να διαπιστώσετε τι συμβαίνει." />
            </div>
        </asp:View>

        <asp:View ID="vHasAccess" runat="server">
            <div style="margin: 10px;">
                <my:StudentInput ID="ucStudentInput" runat="server" ValidationGroup="vgStudentInput" />
            </div>
            <table style="width: 100%">
                <tr>
                    <td colspan="2" style="text-align: left; padding-left: 7px;">
                        <asp:LinkButton ID="btnSubmit" runat="server" Text="Ενημέρωση" CssClass="icon-btn bg-accept"
                            OnClick="btnSubmit_Click" ValidationGroup="vgStudentInput" />
                        <asp:LinkButton ID="btnCancel" runat="server" Text="Ακύρωση" CssClass="icon-btn bg-cancel"
                            CausesValidation="false" OnClientClick="window.parent.popUp.hide();" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblValidationErrors" runat="server" CssClass="error" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
