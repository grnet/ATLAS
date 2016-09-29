<%@ Page Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.Common.SelectPhysicalObjects"
    CodeBehind="SelectPhysicalObjects.aspx.cs" Title="Προσθήκη Αντικειμένου Θέσης" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <style type="text/css">
        .dxgvSelectedRow
        {
            background-color: LightGreen;
            color: black;
        }
    </style>
    <div style="margin: 5px;">
        <asp:UpdatePanel ID="updPhysicalObject" runat="server">
            <ContentTemplate>
                <dx:ASPxGridView ID="gvPhysicalObjects" runat="server" AutoGenerateColumns="False"
                    DataSourceID="odsPhysicalObjects" EnableRowsCache="false" EnableCallBacks="true"
                    Width="100%" KeyFieldName="ID" OnDataBound="gvPhysicalObjects_Databound">
                    <SettingsLoadingPanel Text="<%$ Resources:Grid, Loading %>" />
                    <Settings ShowGroupPanel="false" ShowFilterRow="true" />
                    <SettingsBehavior AllowGroup="False" AllowSort="True" AutoFilterRowInputDelay="900" />
                    <SettingsPager PageSize="15" Summary-Text="<%$ Resources:Grid, PagingPhysicalObject %>"
                        Summary-Position="Left" />
                    <Styles>
                        <Cell Font-Size="11px" />
                    </Styles>
                    <Templates>
                        <EmptyDataRow>
                            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Grid, NoResults %>" />
                        </EmptyDataRow>
                    </Templates>
                    <Columns>
                        <dx:GridViewCommandColumn ShowSelectCheckbox="True" Width="40px" Caption=" " />
                        <dx:GridViewDataTextColumn FieldName="Name" Caption="<%$ Resources:Grid, GridCaption_Name %>" SortIndex="0" SortOrder="Ascending">
                            <Settings AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                    </Columns>
                </dx:ASPxGridView>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div style="text-align: left; margin-top: 10px">
            <asp:LinkButton ID="btnSubmit" runat="server" Text="<%$ Resources:GlobalProvider, Global_Save %>" CssClass="icon-btn bg-accept"
                OnClick="btnSubmit_Click" />
            <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:GlobalProvider, Global_Cancel %>" CssClass="icon-btn bg-cancel"
                OnClick="btnCancel_Click" />
        </div>
    </div>
    <asp:ObjectDataSource ID="odsPhysicalObjects" runat="server" SelectMethod="GetAll"
        TypeName="StudentPractice.Portal.DataSources.PhysicalObjects" />
</asp:Content>
