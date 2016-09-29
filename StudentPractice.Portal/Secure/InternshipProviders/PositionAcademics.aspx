<%@ Page Language="C#" MasterPageFile="~/Secure/SecurePages.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.InternshipProviders.PositionAcademics"
    Title="Προβολή Θέσης από Σχολές/Τμήματα" CodeBehind="PositionAcademics.aspx.cs" %>

<%@ Register TagPrefix="my" Src="~/UserControls/GenericControls/AcademicPositionRulesPopup.ascx" TagName="AcademicPositionRulesPopup" %>

<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <link href="/_css/wizard.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function termsAccepted() {
            <%= this.Page.ClientScript.GetPostBackEventReference(this.btnTermsAccepted, string.Empty) %>;
        }

        var termsPopup = null;
        var requestAcceptanceWhenLoaded = false;
        $(function () {
            Sys.Application.add_load(function () {
                termsPopup = $find('<%= this.ucAcademicPositionRulesPopup.ClientID %>');
                termsPopup.onTermsAccepted(termsAccepted);
                if (requestAcceptanceWhenLoaded) {
                    termsPopup.set_requireTermsAcceptance(true);
                    termsPopup.render();
                    termsPopup.show();
                }
            });

            $('.btn-academicRules').live('click', function () {
                var id = $(this).attr('data-aid');
                termsPopup.set_requireTermsAcceptance(false);
                termsPopup.render();
                termsPopup.show([id]);
            });
        });

        function requestTermsAcceptance() {
            requestAcceptanceWhenLoaded = true;
        }

    </script>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphBeforeHeaderTitle" runat="server">
    <asp:PlaceHolder ID="phSteps" runat="server">
        <ul id="mainNav" runat="server" clientidmode="Static" class="fourStep">
            <li class="done">
                <a id="btnPositionDetails" runat="server" title="<%$ Resources:PositionWizard, Step1Title %>">
                    <em>
                        <asp:Literal runat="server" Text="<%$ Resources:PositionWizard, Step1 %>" /></em>
                    <span>
                        <asp:Literal runat="server" Text="<%$ Resources:PositionWizard, Step1Title %>" /></span>
                </a>
            </li>

            <li class="lastDone">
                <a id="btnPositionPhysicalObject" runat="server" title="<%$ Resources:PositionWizard, Step2Title %>">
                    <em>
                        <asp:Literal runat="server" Text="<%$ Resources:PositionWizard, Step2 %>" /></em>
                    <span>
                        <asp:Literal runat="server" Text="<%$ Resources:PositionWizard, Step2Title %>" /></span>
                </a>
            </li>

            <li class="current">
                <a runat="server" title="<%$ Resources:PositionWizard, Step3Title %>">
                    <em>
                        <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:PositionWizard, Step3 %>" /></em>
                    <span>
                        <asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:PositionWizard, Step3Title %>" /></span>
                </a>
            </li>

            <li class="preview">
                <a runat="server" title="<%$ Resources:PositionWizard, Step4Title %>">
                    <em>
                        <asp:Literal runat="server" Text="<%$ Resources:PositionWizard, Step4 %>" /></em>
                    <span>
                        <asp:Literal runat="server" Text="<%$ Resources:PositionWizard, Step4Title %>" /></span>
                </a>
            </li>
        </ul>
    </asp:PlaceHolder>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <asp:LinkButton ID="cmdRefresh" runat="server" Style="display: none;" OnClick="cmdRefresh_Click" />
    <asp:LinkButton ID="btnTermsAccepted" runat="server" OnClick="btnTermsAccepted_Click" Style="display: none;" />

    <asp:MultiView ID="mvAcademics" runat="server">

        <asp:View ID="vVisibleToAllAcademics" runat="server">
            <br />
            <div class="reminder" style="font-weight: bold; text-align: left">
                <asp:Literal runat="server" Text="<%$ Resources:PositionPages, PositionAcademics_VisibleToAllAcademics1 %>" />
                <br />
                <br />

                <asp:Literal runat="server" Text="<%$ Resources:PositionPages, PositionAcademics_VisibleToAllAcademics2 %>" />
                <br />
                <br />

                <asp:Literal runat="server" Text="<%$ Resources:PositionPages, PositionAcademics_VisibleToAllAcademics3 %>" />
                <br />
                <br />

                <div style="clear: both; text-align: left">
                    <a id="btnRestrictAcademics" runat="server" class="icon-btn bg-edit" href="javascript:void(0)">
                        <asp:Literal runat="server" Text="<%$ Resources:PositionPages, PositionAcademics_RestrictAcademics %>" />
                    </a>
                </div>
            </div>
        </asp:View>

        <asp:View ID="vVisibleToCertainAcademics" runat="server">
            <div class="sub-description" style="margin-top: 10px">
                <span style="font-size: 11px; font-weight: bold">
                    <asp:Literal runat="server" Text="<%$ Resources:PositionPages, PositionAcademics_SelectAcademics %>" />
                </span>
            </div>

            <div style="padding: 5px 0px 10px;">
                <a id="btnAddAcademics" runat="server" class="icon-btn bg-add" href="javascript:void(0)">
                    <asp:Literal runat="server" Text="<%$ Resources:PositionPages, PositionAcademics_AddAcademics %>" />
                </a>

                <asp:LinkButton ID="btnAddAllAcademics" runat="server" OnClick="btnAddAllAcademics_Click" Text="<%$ Resources:PositionPages, PositionAcademics_VisibleToAll %>" />
            </div>

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <dx:ASPxGridView ID="gvAcademics" runat="server" AutoGenerateColumns="False" KeyFieldName="ID"
                        EnableRowsCache="false" EnableCallBacks="true" Width="100%" OnCustomCallback="gvAcademics_CustomCallback"
                        OnCustomDataCallback="gvAcademics_CustomDataCallback" ClientInstanceName="gv">
                        <SettingsLoadingPanel Text="<%$ Resources:Grid, Loading %>" />
                        <SettingsPager PageSize="10" Summary-Text="<%$ Resources:Grid, PagingAcademics %>" Summary-Position="Left" />
                        <Styles>
                            <Cell Font-Size="11px" />
                        </Styles>
                        <Templates>
                            <EmptyDataRow>
                                <asp:Literal runat="server" Text="<%$ Resources:Grid, NoAcademics %>" />
                            </EmptyDataRow>
                        </Templates>
                        <Columns>
                            <dx:GridViewDataTextColumn FieldName="ID" Caption="<%$ Resources:Grid, GridCaption_Incrimental %>" HeaderStyle-HorizontalAlign="Center"
                                CellStyle-HorizontalAlign="Center" Width="50px">
                                <DataItemTemplate>
                                    <%# Container.ItemIndex + 1 %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>

                            <dx:GridViewDataTextColumn FieldName="Institution" Caption="<%$ Resources:Grid, GridCaption_Institution %>" CellStyle-HorizontalAlign="Left"
                                SortIndex="0" SortOrder="Ascending">
                                <DataItemTemplate>
                                    <%# Eval("Institution")%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>

                            <dx:GridViewDataTextColumn FieldName="School" Caption="<%$ Resources:Grid, GridCaption_School %>" CellStyle-HorizontalAlign="Left"
                                SortIndex="1" SortOrder="Ascending">
                                <DataItemTemplate>
                                    <%# Eval("School")%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>

                            <dx:GridViewDataTextColumn FieldName="Department" Caption="<%$ Resources:Grid, GridCaption_Department %>" CellStyle-HorizontalAlign="Left"
                                SortIndex="2" SortOrder="Ascending">
                                <DataItemTemplate>
                                    <%# Eval("Department")%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>

                            <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_Rules %>" CellStyle-HorizontalAlign="Center" 
                                CellStyle-VerticalAlign="Middle" HeaderStyle-Wrap="True" Name="PositionRules" Width="50px">
                                <DataItemTemplate>
                                    <%# GetAcademicRulesLink((StudentPractice.BusinessModel.Academic)Container.Grid.GetRow(Container.VisibleIndex)) %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>

                            <dx:GridViewDataTextColumn Caption="<%$ Resources:Grid, GridCaption_Actions %>" Width="100px">
                                <HeaderStyle HorizontalAlign="Center" Wrap="true" />
                                <CellStyle HorizontalAlign="Center" />
                                <DataItemTemplate>
                                    <a runat="server" href="javascript:void(0);" class="icon-btn bg-delete" onclick='<%# string.Format("return doAction(\"delete\", {0}, \"PositionAcademics\");", Eval("ID"))%>'>
                                        <asp:Literal runat="server" Text="<%$ Resources:GlobalProvider, Global_Delete %>" />
                                    </a>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>

                        </Columns>
                    </dx:ASPxGridView>
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
            <asp:Label ID="lblErrors" runat="server" Font-Bold="true" ForeColor="Red" />
        </asp:View>

    </asp:MultiView>

    <div style="clear: both; text-align: left; margin-top: 30px; margin-bottom: 15px;">
        <asp:LinkButton ID="btnSubmit" runat="server" CssClass="icon-btn bg-accept" ValidationGroup="vgPosition"
            OnClick="btnSubmit_Click" Text="<%$ Resources:GlobalProvider, Global_SaveNContinue %>" />
        <asp:LinkButton ID="btnCancel" runat="server" CssClass="icon-btn bg-cancel" CausesValidation="false"
            OnClick="btnCancel_Click" Text="<%$ Resources:GlobalProvider, Global_Previous %>" />
    </div>

    <my:AcademicPositionRulesPopup ID="ucAcademicPositionRulesPopup" runat="server" RequireTermsAcceptance="true" Width="800" Height="610" />
    <script type="text/javascript">
        function cmdRefresh() {
            <%= this.Page.ClientScript.GetPostBackEventReference(this.cmdRefresh, string.Empty) %>;
        }
    </script>
</asp:Content>
