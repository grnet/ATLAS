<%@ Page Title="Ιστορικό Πιστοποίησης" Language="C#" MasterPageFile="~/PopUp.Master"
    AutoEventWireup="true" CodeBehind="ViewVerificationLogs.aspx.cs" Inherits="StudentPractice.Portal.Secure.Helpdesk.ViewVerificationLogs" %>

<%@ Import Namespace="StudentPractice.BusinessModel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <style type="text/css">
        .dxgvHeader td
        {
            font-size: 11px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <dx:ASPxGridView ID="gvVerificationLogs" runat="server" AutoGenerateColumns="False"
                DataSourceID="odsVerificationLogs" KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true"
                Width="100%" DataSourceForceStandardPaging="true">
                <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
                <SettingsPager PageSize="5" Summary-Text="Σελίδα {0} από {1} ({2} Εγγραφές)" Summary-Position="Left" />
                <Styles>
                    <Cell Font-Size="11px" />
                </Styles>
                <Templates>
                    <EmptyDataRow>
                        Δεν βρέθηκαν αποτελέσματα
                    </EmptyDataRow>
                </Templates>
                <Columns>
                    <dx:GridViewDataTextColumn Caption="Ημ/νία" FieldName="CreatedAt" SortIndex="0" SortOrder="Descending">
                        <DataItemTemplate>
                            <%# ((DateTime)Eval("CreatedAt")).ToString("dd/MM/yyy HH:mm") %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Ενέργεια">
                        <DataItemTemplate>
                            <%# GetAction((VerificationLog)Container.DataItem) %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Χρήστης που έκανε την ενέργεια" FieldName="CreatedBy" HeaderStyle-Wrap="True">
                        <DataItemTemplate>
                            <%# Eval("CreatedBy") %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                </Columns>
            </dx:ASPxGridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table style="width: 100%">
        <tr>
            <td colspan="2" style="text-align: center;">
                <asp:LinkButton ID="btnCancel" runat="server" Text="Κλείσιμο" CssClass="icon-btn bg-cancel"
                    CausesValidation="false" OnClientClick="window.parent.popUp.hide();" />
            </td>
        </tr>
    </table>
    <asp:ObjectDataSource ID="odsVerificationLogs" runat="server" TypeName="StudentPractice.Portal.DataSources.VerificationLogs"
        SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
        EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsVerificationLogs_Selecting">
        <SelectParameters>
            <asp:Parameter Name="criteria" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
