<%@ Page Title="Φοιτητές ΓΠΑ" Language="C#" MasterPageFile="~/Secure/BackOffice.master"
    AutoEventWireup="true" CodeBehind="SearchOfficeStudents.aspx.cs" Inherits="StudentPractice.Portal.Secure.Helpdesk.SearchOfficeStudents" %>

<%@ Register Src="~/UserControls/InternshipPositionControls/ViewControls/HandledStudentsGridView.ascx"
    TagName="HandledStudentsGridView" TagPrefix="my" %>
<%@ Import Namespace="StudentPractice.BusinessModel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .dxgvHeader td
        {
            font-size: 11px;
        }
        .dxeRadioButtonList td.dxe
        {
            padding: 2px 0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:LinkButton ID="cmdRefresh" runat="server" Style="display: none;" OnClick="cmdRefresh_Click" />
    <table style="width: 960px" class="dv">
        <tr>
            <th colspan="6" class="popupHeader">
                Φίλτρα Αναζήτησης
            </th>
        </tr>
        <tr>
            <th style="width: 140px">
                Όνομα Φοιτητή:
            </th>
            <td style="width: 150px">
                <asp:TextBox ID="txtStudentName" runat="server" TabIndex="1" Width="95%" />
            </td>
            <th style="width: 140px">
                Επώνυμο Φοιτητή:
            </th>
            <td style="width: 150px">
                <asp:TextBox ID="txtStudentLastName" runat="server" TabIndex="2" Width="95%" />
            </td>
            <th style="width: 140px">
                Αρ. Μητρώου Φοιτητή:
            </th>
            <td>
                <asp:TextBox ID="txtStudentNumber" runat="server" TabIndex="3" Width="95%" />
            </td>
        </tr>
        <tr>
            <th style="width: 140px">
                ID Φοιτητή:
            </th>
            <td style="width: 150px">
                <asp:TextBox ID="txtStudentID" runat="server" TabIndex="4" Width="95%" />
            </td>
            <th style="width: 140px">
                ID Γραφείου:
            </th>
            <td style="width: 200px">
                <asp:TextBox ID="txtOfficeID" runat="server" TabIndex="5" Width="95%" />
            </td>
            <th style="width: 140px">
                Κατάσταση Θέσης:
            </th>
            <td style="width: 330px">
                <asp:DropDownList ID="ddlPositionStatus" runat="server" TabIndex="6" Width="100%" OnInit="ddlPositionStatus_Init" />
            </td>
        </tr>
    </table>
    <div style="padding: 5px 0px 10px;">
        <asp:LinkButton ID="btnSearch" runat="server" Text="Αναζήτηση" OnClick="btnSearch_Click"
            CssClass="icon-btn bg-search" />
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <my:HandledStudentsGridView ID="ucHandledStudentsGridView" runat="server" DataSourceID="odsPositions" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ObjectDataSource ID="odsPositions" runat="server" TypeName="StudentPractice.Portal.DataSources.Positions"
        SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
        EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsPositions_Selecting">
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
