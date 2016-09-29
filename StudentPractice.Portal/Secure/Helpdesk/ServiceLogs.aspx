<%@ Page Language="C#" MasterPageFile="~/Secure/BackOffice.master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.Helpdesk.ServiceLogs" Title="ServiceLogs" CodeBehind="ServiceLogs.aspx.cs" %>

<%@ Import Namespace="StudentPractice.BusinessModel" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .dxgvHeader td {
            font-size: 11px;
        }

        .dxeRadioButtonList td.dxe {
            padding: 2px 0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width: 900px" class="dv">
        <tr>
            <th colspan="8" class="popupHeader">Φίλτρα Αναζήτησης
            </th>
        </tr>
        <tr>
            <th style="width: 120px">ID Γραφείου:
            </th>
            <td style="width: 130px">
                <asp:TextBox ID="txtOfficeID" runat="server" TabIndex="1" Width="95%" />
            </td>
            <th style="width: 120px">Αποτέλεσμα εκτέλεσης:
            </th>
            <td style="width: 130px">
                <asp:DropDownList ID="ddlSuccess" runat="server" TabIndex="3" Width="100%">
                    <asp:ListItem Text="-- αδιάφορο --" Value="-1" Selected="True" />
                    <asp:ListItem Text="Επιτυχία" Value="1" />
                    <asp:ListItem Text="Αποτυχία" Value="2" />
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th style="width: 120px">Ημ/νία εκτέλεσης από:
            </th>
            <td style="width: 130px">
                <dx:ASPxDateEdit ID="deCalledAtFrom" runat="server" Width="95%" />
            </td>
            <th style="width: 120px">Ημ/νία εκτέλεσης έως:
            </th>
            <td style="width: 130px">
                <dx:ASPxDateEdit ID="deCalledAtTo" runat="server" Width="95%" />
            </td>
        </tr>
        <tr>
            <th style="width: 150px">Ημ/νία εκτέλεσης:
            </th>
            <td style="width: 150px">
                <asp:DropDownList ID="ddlSubmissionDate" runat="server" TabIndex="3" Width="100%">
                    <asp:ListItem Text="-- αδιάφορο --" Value="-1" Selected="True" />
                    <asp:ListItem Text="Τελευταίες 24 ώρες" Value="1" />
                    <asp:ListItem Text="Τελευταία εβδομάδα" Value="2" />
                    <asp:ListItem Text="Τελευταίος μήνας" Value="3" />
                    <asp:ListItem Text="Τελευταίοι 3 μήνες" Value="4" />
                </asp:DropDownList>
            </td>
            <th style="width: 150px">Όνομα Service:
            </th>
            <td style="width: 150px">
                <asp:DropDownList ID="ddlServiceName" runat="server" Width="100%">
                    <asp:ListItem Text="-- αδιάφορο --" Value="" Selected="True" />
                    <asp:ListItem Text="Login" Value="Login" />
                    <asp:ListItem Text="GetAvailablePositionGroups" Value="GetAvailablePositionGroups" />
                    <asp:ListItem Text="GetPositionGroupDetails" Value="GetPositionGroupDetails" />
                    <asp:ListItem Text="GetProviderDetails" Value="GetProviderDetails" />
                    <asp:ListItem Text="GetProvidersByAFM" Value="GetProvidersByAFM" />
                    <asp:ListItem Text="PreAssignPositions" Value="PreAssignPositions" />
                    <asp:ListItem Text="RollbackPreAssignment" Value="RollbackPreAssignment" />
                    <asp:ListItem Text="RollbackPreAssignmentInfo" Value="RollbackPreAssignmentInfo" />
                    <asp:ListItem Text="GetPreAssignedPositions" Value="GetPreAssignedPositions" />
                    <asp:ListItem Text="AssignStudent" Value="AssignStudent" />
                    <asp:ListItem Text="ChangeAssignedStudent" Value="ChangeAssignedStudent" />
                    <asp:ListItem Text="ChangeImplementationData" Value="ChangeImplementationData" />
                    <asp:ListItem Text="DeleteAssignment" Value="DeleteAssignment" />
                    <asp:ListItem Text="DeleteAssignmentInfo" Value="DeleteAssignmentInfo" />
                    <asp:ListItem Text="GetAssignedPositions" Value="GetAssignedPositions" />
                    <asp:ListItem Text="CompletePosition" Value="CompletePosition" />
                    <asp:ListItem Text="GetCompletedPositions" Value="GetCompletedPositions" />
                    <asp:ListItem Text="CancelPosition" Value="CancelPosition" />
                    <asp:ListItem Text="GetRegisteredStudents" Value="GetRegisteredStudents" />
                    <asp:ListItem Text="FindStudentWithAcademicIDNumber" Value="FindStudentWithAcademicIDNumber" />
                    <asp:ListItem Text="FindAcademicIDNumber" Value="FindAcademicIDNumber" />
                    <asp:ListItem Text="GetStudentDetails" Value="GetStudentDetails" />
                    <asp:ListItem Text="RegisterNewStudent" Value="RegisterNewStudent" />
                    <asp:ListItem Text="UpdateStudent" Value="UpdateStudent" />
                    <asp:ListItem Text="RegisterFinishedPosition" Value="RegisterFinishedPosition" />
                    <asp:ListItem Text="GetFinishedPositions" Value="GetFinishedPositions" />
                    <asp:ListItem Text="DeleteFinishedPosition" Value="DeleteFinishedPosition" />
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <div style="padding: 5px 0px 20px;">
        <asp:LinkButton ID="btnSearch" runat="server" Text="Αναζήτηση" OnClick="btnSearch_Click"
            CssClass="icon-btn bg-search" />
        <asp:LinkButton ID="btnExport" runat="server" Text="Εξαγωγή σε Excel" OnClick="btnExport_Click"
            CssClass="icon-btn bg-excel" />
    </div>
    <asp:LinkButton ID="cmdRefresh" runat="server" Style="display: none;" OnClick="cmdRefresh_Click" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <dx:ASPxGridView ID="gvLogs" runat="server" AutoGenerateColumns="False" DataSourceID="odsStudentPracticeApiLog"
                KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true" Width="100%"
                DataSourceForceStandardPaging="true" OnHtmlRowPrepared="gvLogs_HtmlRowPrepared">
                <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
                <SettingsPager PageSize="10" Summary-Text="Σελίδα {0} από {1} ({2} Logs )" Summary-Position="Left" />
                <Styles>
                    <Cell Font-Size="11px" />
                </Styles>
                <Templates>
                    <EmptyDataRow>
                        Δεν βρέθηκαν αποτελέσματα
                    </EmptyDataRow>
                </Templates>
                <Columns>
                    <dx:GridViewDataTextColumn FieldName="ID" Caption="Κωδικός" HeaderStyle-HorizontalAlign="Center" SortIndex="0" SortOrder="Descending"
                        CellStyle-HorizontalAlign="Center" Width="50px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Ημ/νία Δημιουργίας" FieldName="ServiceCalledAt" CellStyle-Wrap="False" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# ((DateTime)Eval("ServiceCalledAt")).ToString("dd/MM/yyyy HH:mm")%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Service" FieldName="ServiceMethodCalled" HeaderStyle-HorizontalAlign="Center"
                        CellStyle-HorizontalAlign="Center">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Αίτημα" FieldName="Request" HeaderStyle-HorizontalAlign="Center"
                        CellStyle-HorizontalAlign="Center">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="ID Γραφείου Πρακτικής" FieldName="ServiceCallerID" HeaderStyle-HorizontalAlign="Center"
                        CellStyle-HorizontalAlign="Center">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Όνομα Σφάλματος" FieldName="ErrorCode" HeaderStyle-HorizontalAlign="Center"
                        CellStyle-HorizontalAlign="Center">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="IP" FieldName="IP" HeaderStyle-HorizontalAlign="Center"
                        CellStyle-HorizontalAlign="Center">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Επιτυχία" FieldName="Success" HeaderStyle-HorizontalAlign="Center"
                        CellStyle-HorizontalAlign="Center">
                    </dx:GridViewDataTextColumn>
                </Columns>
            </dx:ASPxGridView>

        </ContentTemplate>
    </asp:UpdatePanel>
    <dx:ASPxGridViewExporter ID="gveLogs" runat="server" GridViewID="gvLogs" />
    <asp:ObjectDataSource ID="odsStudentPracticeApiLog" runat="server" TypeName="StudentPractice.Portal.DataSources.StudentPracticeApiLog" SelectMethod="FindWithCriteria"
        SelectCountMethod="CountWithCriteria" EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsStudentPracticeApiLog_Selecting">
        <SelectParameters>
            <asp:Parameter Name="criteria" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>

    <script type="text/javascript">
        function cmdRefresh() {
            <%= this.Page.ClientScript.GetPostBackEventReference(this.cmdRefresh, string.Empty) %>;
        }
    </script>
</asp:Content>
