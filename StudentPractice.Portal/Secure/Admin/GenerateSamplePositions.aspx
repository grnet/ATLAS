<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GenerateSamplePositions.aspx.cs" Inherits="StudentPractice.Portal.Secure.Admin.GenerateSamplePositions"
   MasterPageFile="~/Secure/BackOffice.master"Title="Κεντρική Σελίδα" %>

<%@ Import Namespace="StudentPractice.BusinessModel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="lblErrors" runat="server" Font-Bold="true" ForeColor="Red" />
    <table class="dv" style="width: 900px">
        <colgroup>
            <col style="width: 250px" />
        </colgroup>
        <tr>
            <th colspan="4" class="popupHeader">Παράμετροι
            </th>
        </tr>
        <tr>
            <th>Επιλογή ΦΥΠΑ:
            </th>
            <td>
                <asp:DropDownList ID="ddlProviders" runat="server" TabIndex="2" Width="400px" OnInit="ddlProviders_Init" />
            </td>
        </tr>
        <tr>
            <th>Αριθμός Γκρουπ Θέσεων :
            </th>
            <td>
                <asp:DropDownList ID="ddlPositionGroups" runat="server" TabIndex="2" Width="105px" OnInit="ddlPositionGroups_Init" />
            </td>
        </tr>
        <tr>
            <th>Αριθμός Θέσεων για κάθε Γκρουπ:
            </th>
            <td>
                <asp:TextBox ID="tbxPositions" runat="server" Width="100px" />
            </td>
        </tr>
        <tr>
            <th>Διαθέσιμο για όλα τα Τμήματα
            </th>
            <td>
                <asp:CheckBox ID="chk_IsAvailableByAllAcademics" runat="server" />
            </td>
        </tr>
        <tr>
            <th>Επιλογή Ολόκληρου Ιδρύματος
            </th>
            <td>
                <asp:DropDownList ID="ddlInstitutions" runat="server" TabIndex="2" Width="400px" OnInit="ddlInstitutions_Init" />
            </td>
        </tr>
    </table>
    <br />
    <asp:Button ID="btnGeneratePositions" runat="server" Text="Δημιουργία Θέσεων" OnClick="btnbtnGeneratePositions_Click" />
    <br />
    <br />
    <dx:ASPxGridView ID="gvAcademics" runat="server" DataSourceID="odsAcademics" AutoGenerateColumns="False"
        EnableRowsCache="false" EnableCallBacks="true"
        Width="900px" KeyFieldName="ID" OnDataBound="gvAcademics_DataBound">
        <SettingsBehavior AllowGroup="False" AllowSort="True" AutoFilterRowInputDelay="900" />
        <SettingsPager PageSize="30" Summary-Text="Σελίδα {0} από {1} ({2} Τμήματα)"
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
            <dx:GridViewDataTextColumn FieldName="Institution" Caption="Ίδρυμα" SortIndex="0" SortOrder="Ascending" />
            <dx:GridViewDataTextColumn FieldName="Department" Caption="Τμήμα" />
        </Columns>
    </dx:ASPxGridView>
    <asp:ObjectDataSource ID="odsAcademics" runat="server" SelectMethod="GetAll" TypeName="StudentPractice.Portal.DataSources.Academics" />



    <dx:ASPxGridView ID="gvGeneratedPosition" runat="server" AutoGenerateColumns="False"
        EnableRowsCache="false" EnableCallBacks="true"
        Width="100%" KeyFieldName="ID" Visible="false">
        <Columns>
            <dx:GridViewDataTextColumn Caption="ID Group" FieldName="ID" Name="GroupID">
                <DataItemTemplate>
                    <%# Eval("ID") %>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="ID Φορέα" FieldName="ProviderID" Name="ProviderID">
                <DataItemTemplate>
                    <%# Eval("ProviderID") %>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Επωνυμία Φορέα" FieldName="Provider.Name" Name="ProviderName">
                <DataItemTemplate>
                    <%# Eval("Provider.Name") %>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Σύνολο Θέσεων" FieldName="TotalPositions" Name="TotalPositions">
                <DataItemTemplate>
                    <%# Eval("TotalPositions") %>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Διάκρεια (εβδομάδες)" FieldName="Duration" Name="Duration">
                <DataItemTemplate>
                    <%# Eval("Duration")%>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Τίτλος - Περιγραφή" Name="TitleDescription">
                <DataItemTemplate>
                    <%# GetPositionName(Container.Grid.GetRow(Container.VisibleIndex) as InternshipPositionGroup) %>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Τοποθεσία" Name="CityPrefecture">
                <DataItemTemplate>
                    <%# GetCityPrefecture(Container.Grid.GetRow(Container.VisibleIndex) as InternshipPositionGroup)%>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Χρονικός Περιορισμός" Name="TimeConstrain">
                <DataItemTemplate>
                    <%# GetTimeConstrain(Container.Grid.GetRow(Container.VisibleIndex) as InternshipPositionGroup)%>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Είδος Θέσης" FieldName="PositionTypeInt"
                Name="PositionType">
                <DataItemTemplate>
                    <%# ((enPositionType)Eval("PositionType")).GetLabel()%>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Τηλέφωνο Επικοινωνίας Θέσης" Name="ContactPhone">
                <DataItemTemplate>
                    <%# Eval("ContactPhone")%>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Ίδρυμα/Τμήμα" Name="Academics" Width="250px">
                <DataItemTemplate>
                    <%# GetAcademics(Container.Grid.GetRow(Container.VisibleIndex) as InternshipPositionGroup)%>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Αντικείμενο Θέσης" Name="PhysicalObjects">
                <DataItemTemplate>
                    <%# GetPhysicalObjects(Container.Grid.GetRow(Container.VisibleIndex) as InternshipPositionGroup)%>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
        </Columns>
    </dx:ASPxGridView>
    <dx:ASPxGridViewExporter ID="gveGeneratedPosition" runat="server" GridViewID="gvGeneratedPosition"
        OnRenderBrick="gveIntershipPositionGroups_RenderBrick">
    </dx:ASPxGridViewExporter>
</asp:Content>
