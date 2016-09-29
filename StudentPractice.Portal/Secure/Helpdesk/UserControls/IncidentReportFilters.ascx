<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IncidentReportFilters.ascx.cs"
    Inherits="StudentPractice.Portal.Secure.Helpdesk.UserControls.IncidentReportFilters" %>

<table id="tbFilters" runat="server" cellspacing="0">
    <tr>
        <td style="vertical-align: top">
            <table width="500px" class="dv">
                <tr>
                    <th colspan="4" class="popupHeader">
                        Φίλτρα Αναζήτησης
                    </th>
                </tr>
                <tr runat="server" visible="false">
                    <th style="width: 115px">
                        Χειρισμός Από:
                    </th>
                    <td style="width: 120px">
                        <asp:DropDownList ID="ddlHandlerType" runat="server" Width="115px" TabIndex="1" OnInit="ddlHandlerType_Init"
                            ClientIDMode="Static" />
                    </td>
                    <th style="width: 90px">
                        Επικοινωνία:
                    </th>
                    <td style="width: 120px">
                        <asp:DropDownList ID="ddlHandlerStatus" runat="server" Width="115px" TabIndex="6"
                            OnInit="ddlHandlerStatus_Init" />
                    </td>
                </tr>
                <tr>
                    <th style="width: 115px">
                        ID Αναφοράς:
                    </th>
                    <td style="width: 120px">
                        <asp:TextBox ID="txtIncidentReportID" runat="server" Width="115px" />
                    </td>
                    <th style="width: 90px" />
                    <th style="width: 120px" />
                </tr>
                <tr>
                    <th style="width: 115px">
                        Κατάσταση:
                    </th>
                    <td style="width: 120px">
                        <asp:DropDownList ID="ddlReportStatus" runat="server" Width="115px" TabIndex="2"
                            OnInit="ddlReportStatus_Init" ClientIDMode="Static" />
                    </td>
                    <th style="width: 90px">
                        Ημ/νία (από):
                    </th>
                    <td style="width: 120px">
                        <dx:ASPxDateEdit ID="deIncidentReportDateFrom" runat="server" TabIndex="7" Width="115px" />
                    </td>
                </tr>
                <tr>
                    <th style="width: 115px">
                        Πηγή αναφοράς:
                    </th>
                    <td style="width: 120px">
                        <asp:DropDownList ID="ddlReporterType" runat="server" TabIndex="3" OnInit="ddlReporterType_Init"
                            Width="115px" />
                    </td>
                    <th style="width: 90px">
                        Ημ/νία (έως):
                    </th>
                    <td style="width: 120px">
                        <dx:ASPxDateEdit ID="deIncidentReportDateTo" runat="server" TabIndex="8" Width="115px" />
                    </td>
                </tr>
                <tr>
                    <th style="width: 115px">
                        Είδος αναφοράς:
                    </th>
                    <td colspan="3" style="width: 350px">
                        <asp:DropDownList ID="ddlIncidentType" runat="server" TabIndex="4" Width="355px"
                            DataTextField="Name" DataValueField="ID" />
                        <act:CascadingDropDown ID="cddIncidentType" runat="server" TargetControlID="ddlIncidentType"
                            ParentControlID="ddlReporterType" Category="IncidentTypes" PromptText="-- επιλέξτε πηγή αναφοράς --"
                            ServicePath="~/PortalServices/Services.asmx" ServiceMethod="GetIncidentTypes"
                            LoadingText="Παρακαλω περιμένετε">
                        </act:CascadingDropDown>
                    </td>
                </tr>
                <tr>
                    <th style="width: 115px">
                        Τελ. απάντηση από:
                    </th>
                    <td colspan="3" style="width: 350px">
                        <asp:DropDownList ID="ddlUpdatedBy" runat="server" TabIndex="5" Width="345px" DataTextField="Name"
                            DataValueField="ID" OnInit="ddlUpdatedBy_Init" />
                    </td>
                </tr>
            </table>
        </td>
        <td id="tdHelpdeskReports" runat="server" style="vertical-align: top">
            <table width="300px" class="dv">
                <tr>
                    <th colspan="2" class="popupHeader">
                        Για τηλεφωνικές αναφορές
                    </th>
                </tr>
                <tr>
                    <th style="width: 165px">
                        Τύπος Κλήσης:
                    </th>
                    <td style="width: 120px">
                        <asp:DropDownList ID="ddlCallType" runat="server" Width="100%" TabIndex="9" OnInit="ddlCallType_Init" />
                    </td>
                </tr>
                <tr>
                    <th style="width: 165px">
                        Καταγραφή από:
                    </th>
                    <td style="width: 120px">
                        <asp:DropDownList ID="ddlReportedBy" runat="server" TabIndex="10" OnInit="ddlReportedBy_Init"
                            Width="100%" />
                    </td>
                </tr>
                <tr>
                    <th style="width: 165px">
                        Τηλέφωνο:
                    </th>
                    <td style="width: 120px">
                        <asp:TextBox ID="txtReporterPhone" runat="server" TabIndex="11" />
                    </td>
                </tr>
                <tr>
                    <th style="width: 165px">
                        E-mail:
                    </th>
                    <td style="width: 120px">
                        <asp:TextBox ID="txtReporterEmail" runat="server" TabIndex="12" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
