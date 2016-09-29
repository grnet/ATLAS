<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IncidentReportsGridview.ascx.cs"
    Inherits="StudentPractice.Portal.Secure.Helpdesk.UserControls.IncidentReportsGridview" %>

<%@ Import Namespace="StudentPractice.BusinessModel" %>

<style type="text/css">
    .dxgvHeader td
    {
        font-size: 11px;
    }
    .img-btn
    {
        border: dashed 1px gray;
        padding: 2px 0px 4px 0px;
        outline: none;
    }
    .img-btn:hover
    {
        background-color: lightgray;
    }
</style>
<script type="text/javascript">
    var currentElement = null;
    function showPopup(elem, id, oldStatus) {
        var status = oldStatus;

        currentElement = $(elem);
        var updatedStatus = currentElement.find('img').attr('rel');
        if (updatedStatus && updatedStatus != '')
            status = updatedStatus;

        var txt = $('#popTemplate').html();
        var regex = null;
        if (status == 1)
            regex = /<option value="1">.+<\/option>/;
        else if (status == 2)
            regex = /<option value="2">.+<\/option>/;
        else if (status == 3)
            regex = /<option value="3">.+<\/option>/;
        txt = txt.replace(regex, '')
        $.prompt(txt, { speed: 'fast', callback: beginChangeStatus, show: 'dropIn', buttons: { 'Αλλαγή Κατάστασης': id, Ακύρωση: false} });
        return false;
    }

    function beginChangeStatus(v, m, f) {
        if (v == undefined || v == false)
            return;

        var userContext = {};
        userContext.doRefresh = isValueChanged;
        var newStatus = m.find('#newStatus').val();
        userContext.newStatus = newStatus;
        //PageMethods.ChangeStatus(v, newStatus, onCompleted, onFailed, userContext);
        StudentPractice.Portal.PortalServices.Services.ChangeIncidentReportStatus(v, newStatus, onCompleted, onFailed, userContext);
    }

    function onFailed(args) {

    }

    function onCompleted(args, userContext) {
        if (args != null) {
            if (args != "error") {

                if (userContext.doRefresh) {
                    cmdRefresh();
                }
                else {
                    currentElement.html(args.replace('>', 'rel="' + userContext.newStatus + '" >'));
                }
            }
            else {
                $.prompt('Η αλλαγή δεν πραγματοποιήθηκε. Παρακαλούμε δοκιμάστε ξανά αργότερα.');
            }
        }
        else {
            $.prompt('Η αλλαγή δεν πραγματοποιήθηκε. Παρακαλούμε δοκιμάστε ξανά αργότερα.');
        }
    }
    
</script>
<div style="display: none" id="popTemplate">
    Νέα Κατάσταση:
    <select id="newStatus" name="newStatus">
        <option value="1">Εκκρεμεί</option>
        <option value="2">Έχει απαντηθεί</option>
        <option value="3">Έχει κλείσει</option>
    </select>
</div>
<dx:ASPxGridView ID="gvIncidentReports" runat="server" AutoGenerateColumns="False"
    DataSourceID="odsIncidentReports" KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true"
    Width="100%" DataSourceForceStandardPaging="true" OnCustomCallback="gvIncidentReports_CustomCallback"
    OnCustomDataCallback="gvIncidentReports_CustomDataCallback" EnableViewState="false"
    ViewStateMode="Disabled">
    <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
    <SettingsPager PageSize="10" Summary-Text="Σελίδα {0} από {1} ({2} Αναφορές)" Summary-Position="Left" />
    <Styles>
        <Cell Font-Size="11px" />
    </Styles>
    <Templates>
        <EmptyDataRow>
            Δεν βρέθηκαν αποτελέσματα
        </EmptyDataRow>
    </Templates>
    <Columns>
        <dx:GridViewDataTextColumn FieldName="CreatedAt" Name="CreatedAt" Caption="Ημ/νία Αναφοράς"
            CellStyle-Wrap="False" CellStyle-HorizontalAlign="Left" Width="100px" SortIndex="0" SortOrder="Descending">
            <DataItemTemplate>
                <%# GetReportDetails((IncidentReport)Container.DataItem, false)%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Reporter.ReporterType" Name="Reporter.ReporterType"
            Caption="Στοιχεία Αναφοράς" CellStyle-HorizontalAlign="Left" Settings-AllowSort="False">
            <DataItemTemplate>
                <%# GetIncidentTypeDetails((IncidentReport)Container.DataItem)%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="ReporterName" Name="ReporterName" Caption="Στοιχεία Ατόμου Επικοινωνίας"
            CellStyle-HorizontalAlign="Left" Settings-AllowSort="False">
            <DataItemTemplate>
                <%# GetContactPersonDetails((Reporter)Eval("Reporter"))%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Name="SpecialDetailsOfReporter" Caption="Ειδικά Στοιχεία Αναφέροντος"
            CellStyle-HorizontalAlign="Left" Settings-AllowSort="False">
            <DataItemTemplate>
                <%# GetReporterDetails(Eval("Reporter"))%>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="ReportStatusInt" Name="ReportStatus" Caption="Κατάσταση"
            CellStyle-HorizontalAlign="Center" Settings-AllowSort="False">
            <DataItemTemplate>
                <a href="#" class="cmd-btn" onclick='return showPopup(this,<%# Eval("ID")+ ","+ (Eval("ReportStatus")!=null ? ((enReportStatus)Eval("ReportStatus")).ToString("D") : "")%>)'>
                    <%# Eval("ReportStatus")!=null ? ((enReportStatus)Eval("ReportStatus")).GetIcon() : ""%></a>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="ReportText" Name="ReportText" Caption="Κείμενο Αναφοράς"
            CellStyle-HorizontalAlign="Left" Settings-AllowSort="False" />
        <dx:GridViewDataTextColumn FieldName="LastPost.PostText" Name="LastPost.PostText"
            Caption="Τελευταία Απάντηση" CellStyle-HorizontalAlign="Left" Settings-AllowSort="False">
            <DataItemTemplate>
                <asp:PlaceHolder ID="phDispatchSentAt" runat="server" Visible='<%# Eval("LastPost.LastDispatch.DispatchSentBy") != null%>'>
                    <span style="font-size: 11px; font-weight: bold">Ημ/νία Αποστολής</span><br />
                    <%# Eval("LastPost.LastDispatch.DispatchSentBy") != null ? ((DateTime)Eval("LastPost.LastDispatch.DispatchSentAt")).ToString("dd/MM/yyyy HH:mm") : ""%><br />
                    Χρήστης:
                    <%# Eval("LastPost.LastDispatch.DispatchSentBy")%><br />
                    <br />
                    <%# Eval("LastPost.PostText")%>
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="phNoDispatchSent" runat="server" Visible='<%# Eval("LastPost.LastDispatch.DispatchSentBy") == null && Eval("LastPost.PostText") != null%>'>
                    <table>
                        <tr>
                            <td>
                                <a id="lnkEditLastIncidentReportPost" runat="server" href="javascript:void(0)" onclick=<%# string.Format("popUp.show('EditLastIncidentReportPost.aspx?irID={0}','Επεξεργασία Μηνύματος', cmdRefresh, 800, 600)", Eval("ID")) %>>
                                    <img src="/_img/iconReportEdit.png" alt="Επεξεργασία Μηνύματος" />
                                </a>
                            </td>
                            <td>
                                <%# Eval("LastPost.PostText") %>
                            </td>
                        </tr>
                    </table>
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="phNoAnswerExists" runat="server" Visible='<%# Eval("LastPost.PostText") == null%>'>
                    &nbsp; </asp:PlaceHolder>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="HandlerTypeInt" Name="HandlerType" Caption="Χειρισμός Από"
            HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" Width="100px" Settings-AllowSort="False">
            <DataItemTemplate>
                <table>
                    <tr>
                        <td>
                            <a id="lnkEditIncidentReportHandlerInput" runat="server" href="javascript:void(0)"
                                onclick=<%# string.Format("popUp.show('EditIncidentReportHandler.aspx?irID={0}','Επεξεργασία Χειριστή Συμβάντος', cmdRefresh, 800, 400)", Eval("ID")) %>>
                                <img src="/_img/iconReportEdit.png" alt="Επεξεργασία Χειριστή Συμβάντος" />
                            </a>
                        </td>
                        <td>
                            <%# GetHandlerDetails((IncidentReport)Container.DataItem)%>
                        </td>
                    </tr>
                </table>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn Name="Commands" CellStyle-Wrap="False" Settings-AllowSort="False">
        </dx:GridViewDataTextColumn>
    </Columns>
</dx:ASPxGridView>
<dx:ASPxGridViewExporter ID="gveIncidentReports" runat="server" GridViewID="gvIncidentReports"
    OnRenderBrick="gveIncidentReports_RenderBrick">
</dx:ASPxGridViewExporter>
<dx:ASPxPopupControl ID="dxpcPopup" runat="server" ClientInstanceName="devExPopup"
    Width="800" Height="610" Modal="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
    AllowDragging="true" CloseAction="CloseButton">
</dx:ASPxPopupControl>
