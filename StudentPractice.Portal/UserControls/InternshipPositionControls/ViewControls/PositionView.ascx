<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PositionView.ascx.cs"
    Inherits="StudentPractice.Portal.UserControls.InternshipPositionControls.ViewControls.PositionView" %>

<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">&raquo; Γενικά Στοιχεία Θέσης
        </th>
    </tr>
    <tr id="trPositionID" runat="server">
        <th style="width: 180px">ID Θέσης:
        </th>
        <td>
            <asp:Label ID="lblPositionID" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
    <tr>
        <th>Τίτλος:
        </th>
        <td>
            <asp:Label ID="lblTitle" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
    <tr>
        <th>Περιγραφή:
        </th>
        <td>
            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="12" Width="70%"
                Enabled="false" />
        </td>
    </tr>
    <tr>
        <th>Διάρκεια Πρακτικής Άσκησης:
        </th>
        <td>
            <asp:Label ID="lblDuration" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
    <tr>
        <th style="width: 170px">Χώρα:
        </th>
        <td>
            <asp:Label ID="lblCountry" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
    <tr id="trPrefecture" runat="server">
        <th style="width: 170px">
            <asp:Literal ID="ltrPrefecture" runat="server" Text="Περιφερειακή Eνότητα"/>:
        </th>
        <td>
            <asp:Label ID="lblPrefecture" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
    <tr>
        <th style="width: 170px">
            <asp:Literal ID="ltrCity" runat="server" Text="Καλλικρατικός Δήμος"/>:
        </th>
        <td>
            <asp:Label ID="lblCity" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
    <tr>
        <th>Διαθέσιμη χρονική περίοδος για την εκτέλεση της ΠΑ:
        </th>
        <td>
            <asp:Label ID="lblNoTimeLimit" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
    <tr id="trStartDate" runat="server">
        <th>Ημ/νία εκτέλεσης (από):
        </th>
        <td>
            <asp:Label ID="lblStartDate" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
    <tr id="trEndDate" runat="server">
        <th>Ημ/νία εκτέλεσης (έως):
        </th>
        <td>
            <asp:Label ID="lblEndDate" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
    <tr>
        <th>Είδος θέσης:
        </th>
        <td>
            <asp:Label ID="lblPositionType" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
    <tr id="trSupervisor" runat="server" visible="false">
        <th>Ον/μο Επόπτη:
        </th>
        <td>
            <asp:Label ID="lblSupervisor" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
    <tr id="trSupervisorEmail" runat="server" visible="false">
        <th>E-mail Επόπτη:
        </th>
        <td>
            <asp:Label ID="lblSupervisorEmail" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
    <tr>
        <th>Τηλέφωνο επικοινωνίας:
        </th>
        <td>
            <asp:Label ID="lblContactPhone" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
</table>
<br />
<table width="100%" class="dv">
    <tr>
        <th class="header">&raquo; Αντικείμενο Θέσης
        </th>
    </tr>
    <tr>
        <th>
            <dx:ASPxGridView ID="gvPhysicalObjects" runat="server" AutoGenerateColumns="False"
                KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true" Width="100%">
                <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
                <Styles>
                    <Cell Font-Size="11px" />
                </Styles>
                <Columns>
                    <dx:GridViewDataTextColumn FieldName="ID" Caption="Α/Α" HeaderStyle-HorizontalAlign="Center"
                        CellStyle-HorizontalAlign="Center" Width="50px">
                        <DataItemTemplate>
                            <%# Container.ItemIndex + 1 %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Name" Caption="Ονομασία" CellStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <%# Eval("Name")%>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                </Columns>
            </dx:ASPxGridView>
        </th>
    </tr>
</table>
<br />
<table id="tbAcademics" runat="server" width="100%" class="dv">
    <tr>
        <th class="header">&raquo; Σχολές/Τμήματα για τα οποία είναι προσβάσιμη η θέση
        </th>
    </tr>
    <tr>
        <th>
            <asp:MultiView ID="mvAcademics" runat="server">
                <asp:View ID="vVisibleToAllAcademics" runat="server">
                    <div class="reminder" style="font-weight: bold; text-align: left">
                        Η θέση είναι προσβάσιμη από φοιτητές όλων των Σχολών/Τμημάτων.
                    </div>
                </asp:View>
                <asp:View ID="vVisibleToCertainAcademics" runat="server">
                    <dx:ASPxGridView ID="gvAcademics" runat="server" AutoGenerateColumns="False" KeyFieldName="ID"
                        EnableRowsCache="false" EnableCallBacks="true" Width="100%">
                        <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
                        <SettingsPager PageSize="10" Summary-Text="Σελίδα {0} από {1} ({2} Τμήματα)" Summary-Position="Left" />
                        <Styles>
                            <Cell Font-Size="11px" />
                        </Styles>
                        <Templates>
                            <EmptyDataRow>
                                To ΓΠΑ δεν εκπροσωπεί Τμήματα για τα οποία είναι διαθέσιμη η θέση.
                            </EmptyDataRow>
                        </Templates>
                        <Columns>
                            <dx:GridViewDataTextColumn FieldName="ID" Caption="Α/Α" HeaderStyle-HorizontalAlign="Center"
                                CellStyle-HorizontalAlign="Center" Width="50px">
                                <DataItemTemplate>
                                    <%# Container.ItemIndex + 1 %>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="Institution" Caption="Ίδρυμα" CellStyle-HorizontalAlign="Left"
                                SortIndex="0" SortOrder="Ascending">
                                <DataItemTemplate>
                                    <%# Eval("Institution")%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="School" Caption="Σχολή" CellStyle-HorizontalAlign="Left"
                                SortIndex="1" SortOrder="Ascending">
                                <DataItemTemplate>
                                    <%# Eval("School")%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="Department" Caption="Τμήμα" CellStyle-HorizontalAlign="Left"
                                SortIndex="2" SortOrder="Ascending">
                                <DataItemTemplate>
                                    <%# Eval("Department")%>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:ASPxGridView>
                </asp:View>
            </asp:MultiView>
        </th>
    </tr>
</table>
