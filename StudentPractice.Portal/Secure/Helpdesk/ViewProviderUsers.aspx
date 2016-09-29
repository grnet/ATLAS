<%@ Page Language="C#" MasterPageFile="~/Popup.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.Helpdesk.ViewProviderUsers"
    Title="Παραρτήματα Φορέα" CodeBehind="ViewProviderUsers.aspx.cs" %>

<%@ Import Namespace="StudentPractice.BusinessModel" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphHead" runat="server">
    <style type="text/css">
        .dxgvHeader td
        {
            font-size: 11px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <dx:ASPxGridView ID="gvProviderUsers" runat="server" AutoGenerateColumns="False"
                DataSourceID="odsProviderUsers" KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true"
                Width="100%" DataSourceForceStandardPaging="true" OnHtmlRowPrepared="gvProviderUsers_HtmlRowPrepared">
                <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
                <SettingsPager PageSize="5" Summary-Text="Σελίδα {0} από {1} ({2} Παραρτήματα)" Summary-Position="Left" />
                <Styles>
                    <Cell Font-Size="11px" />
                </Styles>
                <Templates>
                    <EmptyDataRow>
                        Δεν βρέθηκαν αποτελέσματα
                    </EmptyDataRow>
                </Templates>
                <Columns>
                    <dx:GridViewDataTextColumn Caption="Α/Α" HeaderStyle-HorizontalAlign="Center"
                        CellStyle-HorizontalAlign="Center" Width="50px">
                        <DataItemTemplate>
                            <%# Container.ItemIndex + 1 %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Κατάσταση" FieldName="VerificationStatus"
                        HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" Width="50px">
                        <DataItemTemplate>
                            <%# GetApprovalStatus((InternshipProvider)Container.DataItem)%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Κατηγορία" FieldName="ProviderType" Name="ProviderType"
                        CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# ((enProviderType)Eval("ProviderType")).GetLabel() %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Στοιχεία Φορέα" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# GetProviderDetails((InternshipProvider)Container.DataItem) %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Στοιχεία Υπευθύνου" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# GetContactDetails((InternshipProvider)Container.DataItem) %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Στοιχεία Λογαριασμού" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# Eval("UserName")%><br />
                            <%# Eval("Email")%>
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
    <asp:ObjectDataSource ID="odsProviderUsers" runat="server" TypeName="StudentPractice.Portal.DataSources.Providers"
        SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
        EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsProviderUsers_Selecting">
        <SelectParameters>
            <asp:Parameter Name="criteria" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
