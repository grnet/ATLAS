<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmailForm.ascx.cs" Inherits="StudentPractice.Portal.UserControls.GenericControls.EmailForm" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxControlToolkit" %>

<script type="text/javascript">
    function retFalse() {
        return false;
    }
    function toggleEmailArea(btnClicked) {
        $('#emailArea').slideDown();
        $('#btnEmailForm').addClass('btn-mask');
        $('#btnEmailForm').unbind('click', toggleEmailArea);
        $('#btnEmailForm').bind('click', retFalse);
        if (btnClicked)
            $('#emailFormMessage').hide();

        if ($('#btnSendEmailCancel').css('display') == 'none')
            $('#btnSendEmailCancel').fadeIn();
        else
            $('#btnSendEmailCancel').fadeOut();
        return false;

    }
    $(function () {
        $('.tb').focus(function () { $(this).addClass('focused') });
        $('.tb').blur(function () { $(this).removeClass('focused') });
        $('#btnEmailForm').bind('click', [true], toggleEmailArea);
        $('#btnSendEmailCancel').bind('click', cancelEmail);
        if (typeof (__EmailFormExpanded) !== 'undefined') {
            toggleEmailArea();
            $('#btnSendEmailCancel').show();
        }
        if ($('#lblEmailFormMessage').text() == '')
            $('#emailFormMessage').hide();
    });

    function cancelEmail() {
        $('#emailArea').slideUp();
        $('#btnEmailForm').removeClass('btn-mask');
        $('#btnEmailForm').bind('click', toggleEmailArea);
        $('#btnEmailForm').unbind('click', retFalse);
        $('#emailArea input,#emailArea textarea').val('');
        $('#cbxTestSend').attr('checked', false);
        $('#btnSendEmailCancel').fadeOut();
        return false;
    }
</script>

<a id="btnEmailForm" href="#" class="icon-btn bg-viewDetails" style="margin-right: 4px;">Μαζική Αποστολή Email</a>
<a id="btnSendEmailCancel" href="#" class="icon-btn" style="display: none">Ακύρωση</a>

<div id="emailArea" style="display: none" runat="server" clientidmode="Static">
    <br />
    <table style="width: 100%">
        <tr>
            <td style="width: 90px">
                <asp:Label ID="Label1" Text="Τίτλος" runat="server" AssociatedControlID="txtSubject" />
            </td>
            <td>
                <asp:TextBox CssClass="tb" runat="server" ID="txtSubject" Width="95%" />

                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Το πεδίο 'Κείμενο' είναι υποχρεωτικό"
                    ControlToValidate="txtSubject" SetFocusOnError="true" runat="server" Display="Dynamic" ValidationGroup="vgSendEmail">
                     <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο είναι υποχρεωτικό" />
                </asp:RequiredFieldValidator>

            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label2" Text="Κείμενο" runat="server" AssociatedControlID="txtBody" />
            </td>
            <td>
                <asp:TextBox CssClass="tb" ID="txtBody" runat="server" TextMode="MultiLine" Width="95%" Rows="15" />

                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Το πεδίο 'Κείμενο' είναι υποχρεωτικό"
                    ControlToValidate="txtBody" SetFocusOnError="true" runat="server" Display="Dynamic" ValidationGroup="vgSendEmail">
                     <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο είναι υποχρεωτικό" />
                </asp:RequiredFieldValidator>

            </td>
        </tr>
        <tr>
            <td style="width: 90px">
                <asp:Label ID="Label3" Text="Επιπλέον E-mail" runat="server" AssociatedControlID="txtSubject" />
            </td>
            <td>
                <asp:TextBox CssClass="tb" runat="server" ID="txtAdditionalEmail" Width="95%" />

                <asp:RegularExpressionValidator ID="revEmail" Display="Dynamic" ControlToValidate="txtAdditionalEmail"
                    runat="server" ValidationExpression="^([a-zA-Z0-9_\-])+(\.([a-zA-Z0-9_\-])+)*@((\[(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5]))\]))|((([a-zA-Z0-9])+(([\-])+([a-zA-Z0-9])+)*\.)+([a-zA-Z])+(([\-])+([a-zA-Z0-9])+)*))$"
                    ValidationGroup="vgSendEmail" ErrorMessage="Το E-mail δεν είναι έγκυρο">
                     <img src="/_img/error.gif" class="errortip" runat="server" title="Το E-mail δεν είναι έγκυρο" />
                </asp:RegularExpressionValidator>

            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:CheckBox Text="Δοκιμαστική αποστολή στον τρέχοντα συνδεδεμένο χρήστη" ID="cbxTestSend" ClientIDMode="Static" runat="server" />
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:LinkButton ID="btnSendEmail" OnClick="btnSendEmail_Click" Text="Αποστολή Email"
                    ValidationGroup="vgSendEmail" CssClass="icon-btn" OnClientClick="if(Page_ClientValidate('vgSendEmail')){return confirm('Θέλετε να πραγματοποιήσετε την μαζική αποστολή;')}else{return false;}"
                    runat="server" />
            </td>
        </tr>
    </table>
    <br />
</div>

<br />

<div id="emailFormMessage" style="margin-top: 10px;">
    <asp:Label ID="lblEmailFormMessage" CssClass="bg-info" Style="background-repeat: no-repeat; padding-left: 20px;" ClientIDMode="Static" runat="server" />
</div>
