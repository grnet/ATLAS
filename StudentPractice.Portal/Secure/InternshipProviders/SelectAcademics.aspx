<%@ Page Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.InternshipProviders.SelectAcademics"
    CodeBehind="SelectAcademics.aspx.cs" Title="Προσθήκη Σχολών/Τμημάτων" %>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <style type="text/css">
        .dxgvSelectedRow
        {
            background-color: LightGreen;
            color: black;
        }
    </style>
    <div style="margin: 5px;">
        <asp:MultiView ID="mv" runat="server" ActiveViewIndex="0">
            <asp:View ID="vSelect" runat="server">
                <dx:ASPxGridView ID="gvAcademics" runat="server" AutoGenerateColumns="False" DataSourceID="odsAcademics"
                    EnableRowsCache="false" EnableCallBacks="true" Width="100%" KeyFieldName="ID"
                    OnDataBound="gvAcademics_Databound">
                    <SettingsLoadingPanel Text="<%$ Resources:Grid, Loading %>" />
                    <Settings ShowGroupPanel="false" ShowFilterRow="true" />
                    <SettingsBehavior AllowGroup="False" AllowSort="True" AutoFilterRowInputDelay="900" />
                    <SettingsPager PageSize="10" Summary-Text="<%$ Resources:Grid, PagingAcademics %>"
                        Summary-Position="Left" />
                    <Styles>
                        <Cell Font-Size="11px" />
                    </Styles>
                    <Templates>
                        <EmptyDataRow>
                            <asp:Literal runat="server" Text="<%$ Resources:Grid, NoResults %>" />
                        </EmptyDataRow>
                    </Templates>
                    <Columns>
                        <dx:GridViewCommandColumn ShowSelectCheckbox="True" Width="40px" Caption=" " />
                        <dx:GridViewDataTextColumn FieldName="Institution" Caption="<%$ Resources:Grid, GridCaption_Institution %>" SortIndex="0" SortOrder="Ascending">
                            <Settings AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="School" Caption="<%$ Resources:Grid, GridCaption_School %>" SortIndex="1" SortOrder="Ascending">
                            <Settings AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="Department" Caption="<%$ Resources:Grid, GridCaption_Department %>" SortIndex="2" SortOrder="Ascending">
                            <Settings AutoFilterCondition="Contains" />
                        </dx:GridViewDataTextColumn>
                    </Columns>
                </dx:ASPxGridView>
                <div style="text-align: left; margin-top: 10px">
                    <asp:LinkButton ID="btnContinue" runat="server" Text="<%$ Resources:GlobalProvider, Global_Continue %>" CssClass="icon-btn bg-next"
                        OnClick="btnContinue_Click" />
                    <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:GlobalProvider, Global_Cancel %>" CssClass="icon-btn bg-cancel"
                        OnClick="btnCancel_Click" />
                </div>
            </asp:View>
            <asp:View ID="vAccept" runat="server">
                <div style="text-align: left; margin-top: 10px">
                    <p>
                        <asp:Literal runat="server" Text="<%$ Resources:AddAcademics, DescriptionsMessage %>" />
                    </p>
                    <asp:LinkButton ID="btnSubmit" runat="server" Text="<%$ Resources:AddAcademics, DescriptionsAccept %>" CssClass="icon-btn bg-accept"
                        OnClick="btnSubmit_Click" />
                </div>
                <br />
                <table class="dv" style="width: 100%;">
                    <tr>
                        <td id="rulesArea" runat="server"></td>
                    </tr>
                </table>
            </asp:View>
        </asp:MultiView>
    </div>
    <asp:ObjectDataSource ID="odsAcademics" runat="server" SelectMethod="GetAllActive" TypeName="StudentPractice.Portal.DataSources.Academics" />
</asp:Content>
