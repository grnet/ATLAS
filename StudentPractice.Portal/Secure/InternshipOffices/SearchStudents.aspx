<%@ Page Language="C#" MasterPageFile="~/Secure/SecurePages.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.InternshipOffices.SearchStudents"
    Title="Φοιτητές Γραφείου Πρακτικής Άσκησης" CodeBehind="SearchStudents.aspx.cs" %>

<%@ Register Src="~/UserControls/GenericControls/FlashMessage.ascx" TagName="FlashMessage" TagPrefix="my" %>
<%@ Import Namespace="StudentPractice.BusinessModel" %>
<%@ Import Namespace="StudentPractice.Portal" %>

<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <style type="text/css">
        .dxgvHeader td {
            font-size: 11px;
        }

        .dvTable {
            width: 1160px !important;
        }

            .dvTable tr th {
                width: 210px !important;
            }


            .dvTable tr td {
                width: 360px !important;
            }
    </style>
    <% if (DesignMode)
       { %>
    <script type="text/javascript" src="../../_js/ASPxScriptIntelliSense.js"></script>
    <% } %>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <asp:LinkButton ID="cmdRefresh" runat="server" Style="display: none;" OnClick="cmdRefresh_Click" />

    <asp:MultiView ID="mvAccount" runat="server" ActiveViewIndex="0">

        <asp:View ID="vAccountNotVerified" runat="server">
            <div class="reminder">
                <asp:Label ID="lblVerificationError" runat="server" />
            </div>
        </asp:View>

        <asp:View ID="vAccountVerified" runat="server">
            <table class="dv dvTable">
                <tr>
                    <th colspan="4" class="popupHeader">Φίλτρα Αναζήτησης
                    </th>
                </tr>
                <tr>
                    <th class="first">Όνομα:</th>
                    <td class="first">
                        <asp:TextBox ID="txtFirstName" runat="server" TabIndex="6" Width="95%" />
                    </td>

                    <th class="second">Τμήμα:</th>
                    <td class="second">
                        <asp:DropDownList ID="ddlDepartment" runat="server" Width="98%" OnInit="ddlDepartment_Init" />
                    </td>
                </tr>
                <tr>
                    <th class="first">Επώνυμο:</th>
                    <td class="first">
                        <asp:TextBox ID="txtLastName" runat="server" TabIndex="7" Width="95%" />
                    </td>

                    <th class="second">Αρ. Μητρώου:</th>
                    <td class="second">
                        <asp:TextBox ID="txtStudentNumber" runat="server" TabIndex="8" Width="95%" />
                    </td>
                </tr>
                <tr>
                    <th class="first">ID Φοιτητή:</th>
                    <td class="first">
                        <asp:TextBox ID="txtStudentID" runat="server" TabIndex="5" Width="95%" />
                    </td>

                    <th class="second"></th>
                    <td class="second"></td>
                </tr>
            </table>

            <div style="padding: 10px 0px 10px;">
                <asp:LinkButton ID="btnSearch" runat="server" Text="Αναζήτηση" OnClick="btnSearch_Click" CssClass="icon-btn bg-search" />
            </div>

            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <dx:ASPxGridView ID="gvStudents" runat="server" AutoGenerateColumns="False"
                        DataSourceID="odsStudents" KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true"
                        Width="100%" DataSourceForceStandardPaging="true" OnHtmlRowPrepared="gvStudents_HtmlRowPrepared">
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
                            <dx:GridViewDataTextColumn Caption="ID Φοιτητή" FieldName="ID" Name="ID" SortOrder="Ascending" Width="60px">
                                <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                <CellStyle HorizontalAlign="Center" />
                                <DataItemTemplate>
                                    <%# Eval("ID") %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Στοιχεία Φοιτητή" FieldName="OriginalFirstName" Name="OriginalFirstName">
                                <DataItemTemplate>
                                    <%# GetStudentDetails((Student)Container.DataItem)%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Στοιχεία Τμήματος" FieldName="Academic.DepartmentInGreek" Name="AcademicID">
                                <DataItemTemplate>
                                    <%# GetAcademicDetails((Student)Container.DataItem)%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="PositionCount" Caption="Θέσεις Πρακτικής" Width="50px">
                                <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                <CellStyle HorizontalAlign="Center" />
                                <DataItemTemplate>
                                    <b>
                                        <a runat="server" style="text-decoration: none" visible='<%# HasPositions((Student)Container.DataItem) %>'
                                            href='<%# string.Format("StudentPositions.aspx?sID={0}", Eval("ID")) %>'>
                                            <%# string.Format("{0:n0}", Eval("PositionCount")) %>
                                        </a>
                                    </b>
                                    <span runat="server" visible='<%# !HasPositions((Student)Container.DataItem) %>'>0</span>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:ASPxGridView>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:ObjectDataSource ID="odsStudents" runat="server" TypeName="StudentPractice.Portal.DataSources.Students"
                SelectMethod="FindWithOfficePositionCount" SelectCountMethod="CountWithCriteria"
                EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsStudents_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="criteria" Type="Object" />
                </SelectParameters>
            </asp:ObjectDataSource>

        </asp:View>
    </asp:MultiView>

    <script type="text/javascript">
        function cmdRefresh() {
            <%= this.Page.ClientScript.GetPostBackEventReference(this.cmdRefresh, string.Empty) %>;
        }
    </script>
</asp:Content>
