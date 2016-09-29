<%@ Page Title="Περιορισμός σε συγκεκριμένες Σχολές/Τμήματα" Language="C#" MasterPageFile="~/PopUp.Master"
    AutoEventWireup="true" CodeBehind="EditOfficeAcademics.aspx.cs" Inherits="StudentPractice.Portal.Secure.Helpdesk.EditOfficeAcademics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <style type="text/css">
        .dxgvSelectedRow
        {
            background-color: LightGreen;
            color: black;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <div style="margin: 5px;">
        <asp:MultiView ID="mv" runat="server">
            <asp:View ID="vGrid" runat="server">
                <asp:UpdatePanel ID="updAcademic" runat="server">
                    <ContentTemplate>
                        <dx:ASPxGridView ID="gvAcademics" runat="server" AutoGenerateColumns="False" EnableRowsCache="false"
                            EnableCallBacks="true" Width="100%" KeyFieldName="ID" OnDataBound="gvAcademics_Databound">
                            <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
                            <Settings ShowGroupPanel="false" ShowFilterRow="true" />
                            <SettingsBehavior AllowGroup="False" AllowSort="True" AutoFilterRowInputDelay="900" />
                            <SettingsPager PageSize="10" Summary-Text="Σελίδα {0} από {1} ({2} Σχολές/Τμήματα)"
                                Summary-Position="Left" />
                            <Styles>
                                <Cell Font-Size="11px" />
                            </Styles>
                            <Templates>
                                <EmptyDataRow>
                                    Δεν βρέθηκαν αποτελέσματα
                                </EmptyDataRow>
                            </Templates>
                            <Columns>
                                <dx:GridViewCommandColumn ShowSelectCheckbox="True" Width="40px" Caption=" " />
                                <dx:GridViewDataTextColumn FieldName="Institution" Caption="Ίδρυμα" SortIndex="0"
                                    SortOrder="Ascending">
                                    <Settings AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="School" Caption="Σχολή" SortIndex="1" SortOrder="Ascending">
                                    <Settings AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="Department" Caption="Τμήμα" SortIndex="2"
                                    SortOrder="Ascending">
                                    <Settings AutoFilterCondition="Contains" />
                                </dx:GridViewDataTextColumn>
                            </Columns>
                        </dx:ASPxGridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div style="text-align: left; margin-top: 10px">
                    <asp:LinkButton ID="btnSubmit" runat="server" Text="Αποθήκευση" CssClass="icon-btn bg-accept"
                        OnClick="btnSubmit_Click" />
                    <asp:LinkButton ID="btnCancel" runat="server" Text="Ακύρωση" CssClass="icon-btn bg-cancel"
                        OnClick="btnCancel_Click" />
                </div>
            </asp:View>
            <asp:View ID="vCannotEdit" runat="server">
                <div style="margin: 10px; color: Red;" class="reminder">
                    <asp:Label ID="lblErrors" runat="server" />
                </div>
                <div style="text-align: left; margin-top: 10px">
                    <asp:LinkButton ID="btnClose" runat="server" Text="Κλείσιμο" CssClass="icon-btn bg-cancel"
                        OnClick="btnCancel_Click" />
                </div>
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>
