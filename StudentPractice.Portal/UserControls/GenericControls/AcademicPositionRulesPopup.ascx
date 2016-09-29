<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AcademicPositionRulesPopup.ascx.cs" Inherits="StudentPractice.Portal.UserControls.GenericControls.AcademicPositionRulesPopup" %>


<div id="<%= ClientID %>"></div>
<dx:ASPxPopupControl ID="dxpcPopup" runat="server" Width="800" Height="610" Modal="true" HeaderText="Περιγραφές Τμημάτων"
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AllowDragging="true" CloseAction="CloseButton">
    <ContentCollection>
        <dx:PopupControlContentControl>
            <div style="text-align: left; margin-top: 10px" id="acceptTermsArea" runat="server">
                <p>
                    Ακολουθούν οι περιγραφές που έχουν δηλώσει τα Τμήματα που επιλέξατε για την Πρακτική Άσκηση των φοιτητών τους. 
                        Η θέση πρακτικής θα πρέπει να έχει λάβει υπόψη τους όρους που υπάρχουν στις περιγραφές αυτές.
                </p>
                <a href="javascript:void(0);" id="btnSubmit" runat="server" class="icon-btn bg-accept">Έχω διαβάσει τις περιγραφές και τις έχω λάβει υπόψη</a>
            </div>
            <div style="text-align: left; margin-top: 10px" id="cancelArea" runat="server">
                <a href="javascript:void(0);" id="btnCancel" runat="server" class="icon-btn bg-cancel">Κλείσιμο</a>
            </div>
            <br />
            <table class="dv" style="width: 100%;">
                <tr>
                    <td id="rulesArea" runat="server"></td>
                </tr>
            </table>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
