<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssignStudent.ascx.cs" Inherits="StudentPractice.Portal.UserControls.InternshipOfficeControls.InputControls.AssignStudent" %>

<%@ Register TagPrefix="my" TagName="SearchAndRegisterStudent" Src="~/UserControls/GenericControls/SearchAndRegisterStudent.ascx" %>
<%@ Register TagPrefix="my" TagName="PositionView" Src="~/UserControls/InternshipPositionControls/ViewControls/PositionView.ascx" %>
<%@ Register TagPrefix="my" TagName="StudentView" Src="~/UserControls/StudentControls/ViewControls/StudentView.ascx" %>
<%@ Register TagPrefix="my" TagName="ImplementationView" Src="~/UserControls/InternshipPositionControls/ViewControls/ImplementationView.ascx" %>
<%@ Import Namespace="StudentPractice.BusinessModel" %>

<asp:LinkButton ID="cmdRefresh" runat="server" Style="display: none;" OnClick="cmdRefresh_Click" />
<asp:MultiView ID="mvAssignPosition" runat="server" ActiveViewIndex="0">
    <asp:View ID="vViewPosition" runat="server">
        <asp:MultiView ID="mvSelectStudent" runat="server">
            <asp:View ID="vStudentNotSelected" runat="server">
                <div class="reminder" style="font-weight: bold">
                    Δεν έχετε ακόμα αντιστοιχίσει τη Θέση Πρακτικής Άσκησης σε φοιτητή
                        <br />
                    <br />
                    <div style="clear: both; text-align: center">
                        <asp:LinkButton ID="btnSelectStudent" runat="server" Text="Αντιστοίχιση σε Φοιτητή"
                            CssClass="icon-btn bg-accept" OnClick="btnSelectStudent_Click" />
                    </div>
                </div>
            </asp:View>
            <asp:View ID="vStudentSelected" runat="server">
                <div class="reminder" style="font-weight: bold;">
                    Η αντιστοίχιση με το φοιτητή έχει πραγματοποιηθεί.
                        <br />
                    <br />
                    <div style="clear: both; text-align: center">
                        <asp:LinkButton ID="btnFinishPosition" runat="server" Text="Καταχώριση Θέσης" CssClass="icon-btn bg-accept" 
                            OnClick="btnFinishPosition_Click" />
                        <asp:LinkButton ID="btnChangeStudent" runat="server" Text="Αλλαγή Επιλεγμένου Φοιτητή"
                            CssClass="icon-btn bg-edit" OnClick="btnChangeStudent_Click" />
                        <asp:LinkButton ID="btnChangeImplementationData" runat="server" Text="Αλλαγή Στοιχείων Εκτέλεσης Πρακτικής Άσκησης"
                            CssClass="icon-btn bg-edit" href="javascript:void(0)" />
                    </div>
                </div>
                <my:StudentView ID="ucStudentView" runat="server" />
                <br />
                <my:ImplementationView ID="ucImplementationView" runat="server" />
            </asp:View>
        </asp:MultiView>
        <table width="100%" cellspacing="0" style="margin-top: 10px;">
            <tr>
                <td style="vertical-align: top; width: 60%">
                    <my:PositionView ID="ucPositionView" runat="server" />
                </td>
            </tr>
        </table>
    </asp:View>
    <asp:View ID="vSelectStudent" runat="server">
        <my:SearchAndRegisterStudent ID="ucSearchAndRegisterStudent" runat="server"
            AllowStudentAssignment="true" ShowCancelButtons="true" ConfirmStudentRegistration="false" AllowAllInstitutions="false"
            OnInitAcademics="ucSearchAndRegisterStudent_InitAcademics" OnStudentAssigned="ucSearchAndRegisterStudent_StudentAssigned"
            OnCancel="ucSearchAndRegisterStudent_Cancel" />
    </asp:View>
</asp:MultiView>
<script type="text/javascript">
    function cmdRefresh() {
        <%= this.Page.ClientScript.GetPostBackEventReference(this.cmdRefresh, string.Empty) %>;
    }
</script>
