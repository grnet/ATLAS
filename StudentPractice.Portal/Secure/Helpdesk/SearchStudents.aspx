<%@ Page Language="C#" MasterPageFile="~/Secure/BackOffice.master" AutoEventWireup="true"
    Inherits="StudentPractice.Portal.Secure.Helpdesk.SearchStudents" Title="Αναζήτηση Φοιτητών"
    CodeBehind="SearchStudents.aspx.cs" %>

<%@ Register Src="~/UserControls/GenericControls/EmailForm.ascx" TagName="EmailForm"
    TagPrefix="my" %>
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
    <asp:LinkButton ID="cmdRefresh" runat="server" Style="display: none;" OnClick="cmdRefresh_Click" />
    <table cellspacing="0">
        <tr>
            <td style="vertical-align: top">
                <table style="width: 600px" class="dv">
                    <tr>
                        <th colspan="4" class="popupHeader">Φίλτρα Αναζήτησης
                        </th>
                    </tr>
                    <tr>
                        <th style="width: 100px">Όνομα:
                        </th>
                        <td style="width: 150px">
                            <asp:TextBox ID="txtFirstName" runat="server" TabIndex="6" Width="95%" />
                        </td>
                        <th style="width: 100px">E-mail:
                        </th>
                        <td style="width: 150px">
                            <asp:TextBox ID="txtContactEmail" runat="server" TabIndex="3" Width="95%" />
                        </td>
                    </tr>
                    <tr>
                        <th style="width: 100px">Επώνυμο:
                        </th>
                        <td style="width: 150px">
                            <asp:TextBox ID="txtLastName" runat="server" TabIndex="7" Width="95%" />
                        </td>
                        <th style="width: 100px">Κινητό:
                        </th>
                        <td style="width: 150px">
                            <asp:TextBox ID="txtContactMobilePhone" runat="server" MaxLength="10" TabIndex="4"
                                Width="95%" />
                        </td>
                    </tr>
                    <tr>
                        <th style="width: 100px">Αρ. Μητρώου:
                        </th>
                        <td style="width: 150px">
                            <asp:TextBox ID="txtStudentNumber" runat="server" TabIndex="8" Width="95%" />
                        </td>
                        <th style="width: 100px">ID Φοιτητή:
                        </th>
                        <td style="width: 150px">
                            <asp:TextBox ID="txtStudentID" runat="server" TabIndex="5" Width="95%" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="vertical-align: top">
                <table style="width: 450px" class="dv">
                    <tr>
                        <th colspan="2" class="popupHeader">Επιλογή Σχολής/Τμήματος
                        </th>
                    </tr>
                    <tr>
                        <th style="width: 90px">Ίδρυμα:
                        </th>
                        <td colspan="3" style="width: 330px">
                            <asp:TextBox ID="txtInstitutionName" runat="server" TabIndex="11" Width="280px" />
                        </td>
                    </tr>
                    <tr>
                        <th style="width: 90px">Σχολή:
                        </th>
                        <td style="width: 330px">
                            <asp:TextBox ID="txtSchoolName" runat="server" TabIndex="12" Width="280px" />
                            <a href="javascript:void(0);" id="lnkSelectSchool" onclick='popUp.show("../../Common/SchoolSelectPopup.aspx", "Επιλογή Σχολής", null , 550, 550);'>
                                <img runat="server" src="~/_img/iconSelectSchool.png" alt="Επιλογή Σχολής" title="Επιλογή Σχολής" />
                            </a> 

                            <a href="javascript:void(0);" id="lnkRemoveSchoolSelection" onclick='return hd.removeSchoolSelection();" style="display: none;'>
                                <img runat="server" src="~/_img/iconRemoveSchool.png" alt="Αφαίρεση Σχολής" title="Αφαίρεση Σχολής" />
                            </a>
                            <asp:HiddenField ID="hfSchoolCode" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <th style="width: 90px">Τμήμα:
                        </th>
                        <td style="width: 330px">
                            <asp:TextBox ID="txtDepartmentName" runat="server" TabIndex="13" Width="280px" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div style="padding: 5px 0px 10px;">
        <asp:LinkButton ID="btnSearch" runat="server" Text="Αναζήτηση" OnClick="btnSearch_Click"
            CssClass="icon-btn bg-search" />
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <dx:ASPxGridView ID="gvStudents" runat="server" AutoGenerateColumns="False" DataSourceID="odsStudents" 
                KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true" Width="100%" 
                DataSourceForceStandardPaging="true" OnHtmlRowPrepared="gvStudents_HtmlRowPrepared">
                <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
                <SettingsPager PageSize="10" Summary-Text="Σελίδα {0} από {1} ({2} Φοιτητές)" Summary-Position="Left" />
                <Styles>
                    <Cell Font-Size="11px" />
                </Styles>
                <Templates>
                    <EmptyDataRow>
                        Δεν βρέθηκαν αποτελέσματα
                    </EmptyDataRow>
                </Templates>
                <Columns>
                    <dx:GridViewDataTextColumn Caption="Ημ/νία Δημιουργίας" FieldName="CreatedAt" Name="CreatedAt" SortIndex="0"
                        SortOrder="Descending" Width="90px">
                        <CellStyle HorizontalAlign="Left" Wrap="False" />
                        <DataItemTemplate>
                            <%# ((DateTime)Eval("CreatedAt")).ToString("dd/MM/yyyy HH:mm")%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="ID Φοιτητή" FieldName="ID" Name="ID" Width="50px">
                        <HeaderStyle HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Center" />
                        <DataItemTemplate>
                            <%# Eval("ID") %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Στοιχεία Φοιτητή" HeaderStyle-HorizontalAlign="Left"
                        CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# GetStudentDetails((Student)Container.DataItem)%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Στοιχεία Επικοινωνίας" HeaderStyle-HorizontalAlign="Left"
                        CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# GetContactDetails((Student)Container.DataItem)%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Στοιχεία Τμήματος" HeaderStyle-HorizontalAlign="Left"
                        CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# GetAcademicDetails((Student)Container.DataItem)%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="PositionCount" Caption="Θέσεις Πρακτικής" Width="50px">
                        <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                        <CellStyle HorizontalAlign="Center" />
                        <DataItemTemplate>
                            <a runat="server" style="text-decoration: none" visible='<%# HasPositions((Student)Container.DataItem) %>'
                                href='<%# string.Format("~/Secure/Helpdesk/SearchPositions.aspx?sID={0}", Eval("ID")) %>'>
                                <%# string.Format("{0:n0}", Eval("PositionCount")) %>
                            </a>
                            <span runat="server" visible='<%# !HasPositions((Student)Container.DataItem) %>'>0</span>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Προβολή Συμβάντων" Width="50px">
                        <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                        <CellStyle HorizontalAlign="Center" />
                        <DataItemTemplate>
                            <a runat="server" style="text-decoration: none" href='<%# GetSearchIncidentReportLink((Student)Container.DataItem) %>'>
                                <img src="/_img/iconView.png" alt="Προβολή Συμβάντων" />
                            </a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Αναφορά Συμβάντος" Width="50px">
                        <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                        <CellStyle HorizontalAlign="Center" />
                        <DataItemTemplate>
                            <a runat="server" style="text-decoration: none" href="javascript:void(0)" 
                                onclick='<%# string.Format("popUp.show(\"ReportIncident.aspx?rID={0}\",\"Αναφορά Συμβάντος\", cmdRefresh, 900, 680);", Eval("ID"))%>' >
                                <img src="/_img/iconAddNewItem.png" alt="Προσθήκη Συμβάντος" />
                            </a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                </Columns>
            </dx:ASPxGridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ObjectDataSource ID="odsStudents" runat="server" TypeName="StudentPractice.Portal.DataSources.Students"
        SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
        EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsStudents_Selecting">
        <SelectParameters>
            <asp:Parameter Name="criteria" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <script type="text/javascript">
        function cmdRefresh() {
            <%= this.Page.ClientScript.GetPostBackEventReference(this.cmdRefresh, string.Empty) %>;
        }
    </script>
    <br />
    <br />
    <%--<my:EmailForm ID="EmailForm" runat="server" OnEmailSending="OnEmailSending" Visible="false"/>--%>
</asp:Content>
