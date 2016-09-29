<%@ Page Title="Χρήστες Γραφείου" Language="C#" MasterPageFile="~/PopUp.Master"
    AutoEventWireup="true" CodeBehind="ViewOfficeUsers.aspx.cs" Inherits="StudentPractice.Portal.Secure.Helpdesk.ViewOfficeUsers" %>

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
            <dx:ASPxGridView ID="gvOfficeUsers" runat="server" AutoGenerateColumns="False"
                DataSourceID="odsOfficeUsers" KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true"
                Width="100%" DataSourceForceStandardPaging="true" OnHtmlRowPrepared="gvOfficeUsers_HtmlRowPrepared">
                <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
                <SettingsPager PageSize="5" Summary-Text="Σελίδα {0} από {1} ({2} Χρήστες Γραφείου)" Summary-Position="Left" />
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
                            <%# GetApprovalStatus((InternshipOffice)Container.DataItem)%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Στοιχεία Υπευθύνου" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# GetContactDetails((InternshipOffice)Container.DataItem)%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Στοιχεία Λογαριασμού" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# Eval("UserName")%><br />
                            <%# Eval("Email")%><br />
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Πρόσβαση σε τμήματα του Γραφείου Πρακτικής"
                        CellStyle-HorizontalAlign="Left" Width="200px">
                        <DataItemTemplate>
                            <%# GetOfficeAcademics((InternshipOffice)Container.DataItem)%>
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
    <asp:ObjectDataSource ID="odsOfficeUsers" runat="server" TypeName="StudentPractice.Portal.DataSources.Offices"
        SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
        EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsOfficeUsers_Selecting">
        <SelectParameters>
            <asp:Parameter Name="criteria" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
