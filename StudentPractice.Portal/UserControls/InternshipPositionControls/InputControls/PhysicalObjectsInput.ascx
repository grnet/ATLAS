<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhysicalObjectsInput.ascx.cs" Inherits="StudentPractice.Portal.UserControls.InternshipPositionControls.InputControls.PhysicalObjectsInput" %>


<asp:LinkButton ID="cmdRefresh" runat="server" Style="display: none;" OnClick="cmdRefresh_Click" />
<div class="sub-description" style="margin-top: 10px">
    <span style="font-size: 11px; font-weight: bold">
        <asp:Literal runat="server" Text="<%$ Resources:PositionPages, PositionPhysicalObject_Info %>" />
    </span>
</div>
<div style="padding: 5px 0px 10px;">
    <a id="btnAddPhysicalObject" runat="server" class="icon-btn bg-add" href="javascript:void(0)">
        <asp:Literal runat="server" Text="<%$ Resources:PositionPages, PositionPhysicalObject_Add %>" />
    </a>
</div>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <dx:ASPxGridView ID="gvPhysicalObjects" runat="server" AutoGenerateColumns="False"
            KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true" Width="50%"
            OnCustomCallback="gvPhysicalObjects_CustomCallback" ClientInstanceName="gv">
            <SettingsLoadingPanel Text="<%$ Resources:Grid, Loading %>" />
            <Styles>
                <Cell Font-Size="11px" />
            </Styles>
            <Templates>
                <EmptyDataRow>
                    <asp:Literal runat="server" Text="<%$ Resources:Grid, NoPhysicalObject %>" />
                </EmptyDataRow>
            </Templates>
            <Columns>
                <dx:GridViewDataTextColumn FieldName="ID" Caption="<%$ Resources:Grid, GridCaption_Incrimental %>" HeaderStyle-HorizontalAlign="Center"
                    CellStyle-HorizontalAlign="Center" Width="50px">
                    <DataItemTemplate>
                        <%# Container.ItemIndex + 1 %>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Name" Caption="<%$ Resources:Grid, GridCaption_Name %>" CellStyle-HorizontalAlign="Left">
                    <DataItemTemplate>
                        <%# Eval("Name")%>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_Actions %>" Width="100px">
                    <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                    <CellStyle HorizontalAlign="Center" />
                    <DataItemTemplate>
                        <a runat="server" href="javascript:void(0);" class="icon-btn bg-delete"
                            onclick='<%# string.Format("return doAction(\"delete\", {0}, \"PositionPhysicalObject\");", Eval("ID"))%>'>
                             <asp:Literal runat="server" Text="<%$ Resources:GlobalProvider, Global_Delete %>" />
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
            </Columns>
        </dx:ASPxGridView>
    </ContentTemplate>
</asp:UpdatePanel>

<div style="clear: both; text-align: left; margin-top:15px; margin-bottom:15px;">
    <asp:LinkButton ID="btnSubmit" runat="server" CssClass="icon-btn bg-accept" ValidationGroup="vgPosition"
            OnClick="btnSubmit_Click" Text="<%$ Resources:GlobalProvider, Global_SaveNContinue %>" />
    <asp:LinkButton ID="btnCancel" runat="server" CssClass="icon-btn bg-cancel" CausesValidation="false"
            OnClick="btnCancel_Click" Text="<%$ Resources:GlobalProvider, Global_Previous %>" />
</div>

<asp:Label ID="lblErrors" runat="server" Font-Bold="true" ForeColor="Red" />
<script type="text/javascript">
    function cmdRefresh() {
        <%= this.Page.ClientScript.GetPostBackEventReference(this.cmdRefresh, string.Empty) %>;
        }
</script>
