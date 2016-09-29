<%@ Page Language="C#" MasterPageFile="~/Secure/BackOffice.master" AutoEventWireup="true"
    Inherits="StudentPractice.Portal.Secure.Helpdesk.SearchAcademics" Title="Αναζήτηση Τμημάτων"
    CodeBehind="SearchAcademics.aspx.cs" %>

<%@ Register TagPrefix="my" TagName="AcademicPositionRulesPopup" Src="~/UserControls/GenericControls/AcademicPositionRulesPopup.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .dxgvHeader td {
            font-size: 11px;
        }

        .dxeRadioButtonList td.dxe {
            padding: 2px 0;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            var termsPopup = null;
            Sys.Application.add_load(function () 
            {
                termsPopup = $find('<%= this.ucAcademicPositionRulesPopup.ClientID %>');
            });
            $('.btn-academicRules').live('click', function () {
                 var id = $(this).attr('data-aid');
                 termsPopup.set_requireTermsAcceptance(false);
                 termsPopup.render();
                 termsPopup.show([id]);
             });
         });
    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:LinkButton ID="cmdRefresh" runat="server" Style="display: none;" OnClick="cmdRefresh_Click" />
    <table style="width: 670px" class="dv">
        <tr>
            <th colspan="4" class="popupHeader">Φίλτρα Αναζήτησης
            </th>
        </tr>
        <tr>
            <th style="width: 80px">Ίδρυμα:
            </th>
            <td style="width: 400px">
                <asp:DropDownList ID="ddlInstitution" runat="server" TabIndex="1" Width="100%" OnInit="ddlInstitution_Init" />
            </td>
            <th style="width: 270px">Μέγιστος Αριθμός Προδεσμεύσεων:
            </th>
            <td style="width: 130px">
                <asp:TextBox ID="txtMaxAllowedPreAssignedPositions" runat="server" TabIndex="3" Width="95%" />
            </td>
        </tr>
        <tr>
            <th style="width: 80px">Τμήμα:
            </th>
            <td style="width: 400px">
                <asp:DropDownList ID="ddlDepartment" runat="server" ClientIDMode="Static" Width="100%"
                    DataTextField="Name" DataValueField="ID" />
                <act:CascadingDropDown ID="cddDepartment" runat="server" TargetControlID="ddlDepartment"
                    ParentControlID="ddlInstitution" Category="Departments" PromptText="-- επιλέξτε τμήμα --"
                    ServicePath="~/PortalServices/Services.asmx" ServiceMethod="GetDepartments" LoadingText="Παρακαλω περιμένετε">
                </act:CascadingDropDown>
            </td>
            <th style="width: 270px">Αριθμός Προδεσμεύσεων:
            </th>
            <td style="width: 130px">
                <asp:TextBox ID="txtPreAssignedPositions" runat="server" TabIndex="4" Width="95%" />
            </td>
        </tr>
    </table>
    <div style="padding: 5px 0px 10px;">
        <asp:LinkButton ID="btnSearch" runat="server" Text="Αναζήτηση" OnClick="btnSearch_Click"
            CssClass="icon-btn bg-search" />
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <dx:ASPxGridView ID="gvAcademics" runat="server" AutoGenerateColumns="False" DataSourceID="odsAcademics"
                KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true" Width="95%"
                DataSourceForceStandardPaging="true">
                <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
                <SettingsPager PageSize="20" Summary-Text="Σελίδα {0} από {1} ({2} Τμήματα)" Summary-Position="Left" />
                <Styles>
                    <Cell Font-Size="11px" />
                </Styles>
                <Templates>
                    <EmptyDataRow>
                        Δεν βρέθηκαν αποτελέσματα
                    </EmptyDataRow>
                </Templates>
                <Columns>
                    <dx:GridViewDataTextColumn FieldName="ID" VisibleIndex="0" Visible="false">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="InstitutionInGreek" VisibleIndex="1" Caption="Ίδρυμα"
                        SortIndex="0" SortOrder="Ascending">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="SchoolInGreek" VisibleIndex="2" Caption="Σχολή">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="DepartmentInGreek" VisibleIndex="3" Caption="Τμήμα">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="MaxAllowedPreAssignedPositions" Caption="Μέγιστος Αριθμός Προδεσμεύσεων"
                        HeaderStyle-Wrap="True" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center"
                        Width="100px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="PreAssignedPositions" Caption="Αριθμός Προδεσμεύσεων"
                        HeaderStyle-Wrap="True" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center"
                        Width="100px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Επεξεργασία" HeaderStyle-HorizontalAlign="Center"
                        CellStyle-HorizontalAlign="Center" Width="50px">
                        <DataItemTemplate>
                            <a runat="server" style="text-decoration: none" href="javascript:void(0)" onclick='<%# string.Format("popUp.show(\"EditAcademic.aspx?aID={0}\", \"Επεξεργασία Τμήματος\", cmdRefresh, 900, 400);", Eval("ID"))%>' >
                                <img src="/_img/iconEdit.png" alt="Επεξεργασία Τμήματος" />
                            </a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Περιγραφή Πρακτικής Άσκησης Τμημάτων" CellStyle-HorizontalAlign="Center" Name="PositionRules" Width="30px">
                        <DataItemTemplate>
                            <%# GetAcademicRulesLink((StudentPractice.BusinessModel.Academic)Container.Grid.GetRow(Container.VisibleIndex)) %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                </Columns>
            </dx:ASPxGridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ObjectDataSource ID="odsAcademics" runat="server" TypeName="StudentPractice.Portal.DataSources.Academics"
        SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
        EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsAcademics_Selecting">
        <SelectParameters>
            <asp:Parameter Name="criteria" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>
     <my:AcademicPositionRulesPopup ID="ucAcademicPositionRulesPopup" runat="server" RequireTermsAcceptance="true"  Width="800" Height="610"/>
    <script type="text/javascript">
        function cmdRefresh() {
            <%= this.Page.ClientScript.GetPostBackEventReference(this.cmdRefresh, string.Empty) %>;
        }
    </script>
</asp:Content>
