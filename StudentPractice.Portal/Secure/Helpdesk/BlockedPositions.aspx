<%@ Page Language="C#" MasterPageFile="~/Secure/BackOffice.master" AutoEventWireup="true"
    Inherits="StudentPractice.Portal.Secure.Helpdesk.BlockedPositions" Title="Μπλοκαρισμένες Θέσεις Πρακτικής Άσκησης"
    CodeBehind="BlockedPositions.aspx.cs" %>

<%@ Import Namespace="StudentPractice.BusinessModel" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .dxgvHeader td
        {
            font-size: 11px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <table style="width: 960px" class="dv">
        <tr>
            <th colspan="4" class="popupHeader">Φίλτρα Αναζήτησης
            </th>
        </tr>
        <tr>
            <th style="width: 120px">ID Γραφείου:
            </th>
            <td>
                <asp:TextBox ID="txtOfficeID" runat="server" TabIndex="5" Width="95%" />
            </td>
            <th style="width: 120px">ID Group Θέσης:
            </th>
            <td>
                <asp:TextBox ID="txtGroupID" runat="server" TabIndex="5" Width="95%" />
            </td>
        </tr>
        <tr>
            <th style="width: 120px">ID Φορέα:
            </th>
            <td>
                <asp:TextBox ID="txtProviderID" runat="server" TabIndex="5" Width="95%" />
            </td>
            <th style="width: 120px">Ίδρυμα:
            </th>
            <td style="width: 330px">
                <asp:DropDownList ID="ddlInstitution" runat="server" TabIndex="10" Width="100%" OnInit="ddlInstitution_Init" />
            </td>
        </tr>
    </table>
    <div style="padding: 5px 0px 10px;">
        <asp:LinkButton ID="btnSearch" runat="server" Text="Αναζήτηση" OnClick="btnSearch_Click"
            CssClass="icon-btn bg-search" />
        <asp:LinkButton ID="btnExport" runat="server" Text="Εξαγωγή σε Excel" OnClick="btnExport_Click"
            CssClass="icon-btn bg-excel" />
    </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <dx:ASPxGridView ID="gvBlockedPositionGroups" runat="server" AutoGenerateColumns="False"
                DataSourceID="odsBlockedPositionGroups" KeyFieldName="ID" EnableRowsCache="false"
                EnableCallBacks="true" Width="100%" DataSourceForceStandardPaging="true"
                ClientInstanceName="gv" OnCustomCallback="gvBlockedPositionGroups_CustomCallback">
                <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
                <SettingsPager PageSize="10" Summary-Text="Σελίδα {0} από {1} ({2} Μπλοκαρισμένες Θέσεις Πρακτικής Άσκησης)"
                    Summary-Position="Left" />
                <Styles>
                    <Cell Font-Size="11px" />
                </Styles>
                <Templates>
                    <EmptyDataRow>
                        Δεν υπάρχουν μπλοκαρισμένες Θέσεις Πρακτικής Ασκησης
                    </EmptyDataRow>
                </Templates>
                <Columns>
                    <dx:GridViewDataTextColumn Caption="Αφαίρεση Ποινής" Width="100px">
                        <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                        <CellStyle HorizontalAlign="Center" />
                        <DataItemTemplate>
                            &nbsp;
                            <a runat="server" href="javascript:void(0);" style="text-decoration: none;"
                                onclick='<%# string.Format("return doAction(\"deleteblockedpositiongroup\", {0}, \"BlockedPositions\");", Eval("ID"))%>'
                                visible='<%# CanDeleteBlock((BlockedPositionGroup)Container.DataItem) %>' title="Αφαίρεση Ποινής">
                                <img src="/_img/iconDelete.png" alt="Αφαίρεση Ποινής" /></a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="GroupID" Caption="Κωδικός Group" HeaderStyle-HorizontalAlign="Center"
                        CellStyle-HorizontalAlign="Center" Width="50px">
                        <DataItemTemplate>
                            <%# Eval("GroupID")%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Στοιχεία Μπλοκαρισμένου MasterAccount" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# GetBlockedOfficeDetails((BlockedPositionGroup)Container.DataItem)%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="BlockingReasonInt" Caption="Αιτία Μπλοκαρίσματος"
                        CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# ((enBlockingReason)Eval("BlockingReason")).GetLabel()%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="DaysLeft" Caption="Ημέρες Μπλοκαρίσματος"
                        CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# Eval("DaysLeft") %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                </Columns>
            </dx:ASPxGridView>
        </ContentTemplate>
    </asp:UpdatePanel>


    <dx:ASPxGridView ID="gvBlockedPositionsExport" runat="server" AutoGenerateColumns="False"
        DataSourceID="odsBlockedPositionGroups" KeyFieldName="ID" EnableRowsCache="false"
        EnableCallBacks="true" Width="100%" DataSourceForceStandardPaging="true" Visible="false">
        <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
        <SettingsPager PageSize="2" Summary-Text="Σελίδα {0} από {1} ({2} Φορείς)" Summary-Position="Left" />
        <Styles>
            <Cell Font-Size="11px" Wrap="True" />
            <Header Wrap="True" />
        </Styles>
        <Columns>
            <dx:GridViewDataTextColumn Caption="Ίδρυμα" Name="Institution" FieldName="MasterAccount.InstitutionID" />
            <dx:GridViewDataTextColumn Caption="Κωδικός Group" Name="GroupID" FieldName="GroupID" />
            <dx:GridViewDataTextColumn Caption="Κωδικός Φορέα" Name="ProviderID" FieldName="InternshipPositionGroup.ProviderID" />
            <dx:GridViewDataTextColumn Caption="Κωδικός ΓΠΑ που προξένησε την εφαρμογή της ποινής" Name="MasterAccountID" FieldName="MasterAccountID" />
            <dx:GridViewDataTextColumn Caption="Αιτία μπλοκαρίσματος" Name="BlockingReasonInt" FieldName="BlockingReasonInt" />
            <dx:GridViewDataTextColumn Caption="Ημέρες μπλοκαρίσματος" Name="DaysLeft" FieldName="DaysLeft" />
        </Columns>
    </dx:ASPxGridView>

    <dx:ASPxGridViewExporter ID="gveBlockedPositionsExporter" runat="server" GridViewID="gvBlockedPositionsExport" OnRenderBrick="gveBlockedPositionsExporter_RenderBrick" />

    <asp:ObjectDataSource ID="odsBlockedPositionGroups" runat="server" TypeName="StudentPractice.Portal.DataSources.BlockedPositionGroups"
        SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
        EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsBlockedPositionGroups_Selecting">
        <SelectParameters>
            <asp:Parameter Name="criteria" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>

