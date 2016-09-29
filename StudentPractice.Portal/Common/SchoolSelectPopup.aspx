<%@ Page Language="C#" MasterPageFile="~/PopUpPublic.Master" AutoEventWireup="true" CodeBehind="SchoolSelectPopup.aspx.cs" Inherits="HelpDesk.Portal.Common.SchoolSelectPopup" Title="Επιλογή Σχολής" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div>
        <dx:ASPxGridView ID="gv" ClientInstanceName="gv" runat="server" Width="98%" 
            AutoGenerateColumns="False" DataSourceID="odsAcademics" KeyFieldName="ID" 
            AccessibilityCompliant="True" AccessKey="G" 
            Font-Names="Verdana,Tahoma,Arial,Sans-Serif" Font-Size="1em" 
            ForeColor="#716162">
            <Settings ShowGroupPanel="false" ShowFilterRow="true" />
            <SettingsBehavior AllowFocusedRow="True" AllowDragDrop="False" AllowGroup="False" AllowSort="True" AutoFilterRowInputDelay="900" />
            <SettingsPager PageSize="10" />
            <Styles Cell-Font-Size="XX-Small" />
            <Columns>
                <dx:GridViewDataTextColumn FieldName="ID" VisibleIndex="0" Visible="false">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Institution" VisibleIndex="1" Caption="Ίδρυμα" SortIndex="0" SortOrder="Ascending">
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="School" VisibleIndex="2" Caption="Σχολή">
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Department" VisibleIndex="3" Caption="Τμήμα">
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Address" VisibleIndex="4" Visible="false">
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ZipCode" VisibleIndex="5" Visible="false">
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="PrefectureID" Visible="false">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="CityID" Visible="false">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="CityName" VisibleIndex="6" Visible="false" UnboundType="String">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="PrefectureName" VisibleIndex="7" Visible="false" UnboundType="String">
                </dx:GridViewDataTextColumn>
            </Columns>
        </dx:ASPxGridView>
        <asp:ObjectDataSource ID="odsAcademics" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetAll" TypeName="StudentPractice.Portal.DataSources.Academics" />
        <div style="text-align: right; padding: .5em 0 .5em .5em;">
            <asp:LinkButton ID="btnSubmit" runat="server" CssClass="icon-btn" Text="<%$ Resources:GlobalProvider, Global_Select %>" OnClientClick="return onSchoolSelected();" />
            <asp:LinkButton ID="btnCancel" runat="server" CssClass="icon-btn" Text="<%$ Resources:GlobalProvider, Global_Close %>" OnClientClick="window.parent.popUp.hide();" />
        </div>
    </div>
</asp:Content>
