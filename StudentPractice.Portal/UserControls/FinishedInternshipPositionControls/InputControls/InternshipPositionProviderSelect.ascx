<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InternshipPositionProviderSelect.ascx.cs" Inherits="StudentPractice.Portal.UserControls.FinishedInternshipPositionControls.InputControls.InternshipPositionProviderSelect" %>

<br />
<asp:LinkButton ID="cmdRefresh" runat="server" Style="display: none;" OnClick="cmdRefresh_Click" />

<table width="100%" class="dv">
    <tr>
        <th colspan="6" class="popupHeader">Φίλτρα Αναζήτησης
        </th>
    </tr>
    <tr>
        <th style="width: 80px">Κωδικός Φορέα:
        </th>
        <td>
            <asp:TextBox ID="txtProviderID" runat="server" TabIndex="10" Width="90%" />
        </td>
        <th style="width: 50px">Α.Φ.Μ.:
        </th>
        <td>
            <asp:TextBox ID="txtProviderAFM" runat="server" TabIndex="10" Width="90%" />
        </td>
        <th style="width: 80px">Επωνυμία / Διακριτικός Τίτλος:
        </th>
        <td>
            <asp:TextBox ID="txtProviderName" runat="server" TabIndex="11" Width="90%" />
        </td>
    </tr>
</table>

<div style="padding: 5px 0px 10px;">
    <asp:LinkButton ID="btnSearch" runat="server" Text="Αναζήτηση" OnClick="btnSearch_Click"
        CssClass="icon-btn bg-search" />
</div>

<dx:ASPxGridView ID="gvProviders" runat="server" Visible="false" AutoGenerateColumns="False" DataSourceID="odsProviders"
    KeyFieldName="ID" EnableRowsCache="false" EnableCallBacks="true" Width="100%" DataSourceForceStandardPaging="true"
    OnDataBound="gvProviders_Databound">
    <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
    <SettingsBehavior AllowGroup="False" AllowSort="True" AutoFilterRowInputDelay="900"  AllowSelectSingleRowOnly="true"/>
    <SettingsPager PageSize="10" Summary-Text="Σελίδα {0} από {1} ({2} Φορείς)" Summary-Position="Left" />
    <Styles>
        <Cell Font-Size="11px" HorizontalAlign="Center" VerticalAlign="Middle" />
        <Header HorizontalAlign="Center" VerticalAlign="Middle" />
    </Styles>
    <Templates>
        <EmptyDataRow>
            Δεν βρέθηκαν Φορείς Υποδοχής με τα κριτήρια αναζήτησης. Σε περίπτωση που ο Φορέας δεν είναι εγγεγραμμένος, ενημερώστε τον να εγγραφεί στην δράση του Άτλαντα.
        </EmptyDataRow>
    </Templates>
    <Columns>
        <dx:GridViewCommandColumn ShowSelectCheckbox="True" Width="40px" Caption=" " />
        <dx:GridViewDataTextColumn FieldName="ID" Caption="Κωδικός" SortIndex="0" SortOrder="Ascending" Width="50px" />
        <dx:GridViewDataTextColumn FieldName="Name" Caption="Επωνυμία" SortIndex="1" SortOrder="Ascending" />
        <dx:GridViewDataTextColumn FieldName="TradeName" Caption="Διακριτικός Τίτλος" SortIndex="2" SortOrder="Ascending" />
        <dx:GridViewDataTextColumn FieldName="AFM" Caption="ΑΦΜ" SortIndex="3" />
    </Columns>
</dx:ASPxGridView>

<div id="lblNoSearchCriteria" runat="server" class="reminder">Δεν έχετε εισάγει κριτήρια αναζήτησης</div>

<asp:ObjectDataSource ID="odsProviders" runat="server" TypeName="StudentPractice.Portal.DataSources.Providers"
    SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
    EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsProviders_Selecting">
    <SelectParameters>
        <asp:Parameter Name="criteria" Type="Object" />
    </SelectParameters>
</asp:ObjectDataSource>
<script type="text/javascript">
    function cmdRefresh() {
        <%= this.Page.ClientScript.GetPostBackEventReference(this.cmdRefresh, string.Empty) %>;
    }
</script>
