<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InternshipPositionPopup.aspx.cs"
    Inherits="StudentPractice.Portal.Secure.Reports.InternshipPopup" %>

<%@ Register Src="~/Secure/Reports/UserControls/InternshipPositionDetailsGridView.ascx"
    TagName="InternshipPositionGridView" TagPrefix="my" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link id="lnkMain" runat="server" href="~/_css/main.css" type="text/css" rel="stylesheet"
        rev="stylesheet" media="all" />
    <title>Στοιχεία Αιτήσεων</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="sm">
        <compositescript>
            <Scripts>
                <asp:ScriptReference Path="~/_js/jquery-1.9.1.min.js" />
                    <asp:ScriptReference Path="~/_js/jquery-migrate-1.1.1.min.js" />
                    <asp:ScriptReference Path="~/_js/Imis.Lib.js" />
                    <asp:ScriptReference Path="~/_js/popUp.js" />
                    <asp:ScriptReference Path="~/_js/SchoolSearch.js" />
                    <asp:ScriptReference Path="~/_js/DxExtensions.js" />
            </Scripts>
        </compositescript>
    </asp:ScriptManager>
    <dx:ASPxPopupControl runat="server" ID="dxpcPopup" AllowDragging="true" HeaderText="Αλλαγή Κωδικού Πρόσβασης"
        Height="300" Width="700" Modal="true" PopupHorizontalAlign="WindowCenter" ClientInstanceName="devExPopup"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton">
        <ClientSideEvents CloseUp="function(s,e){popUp.hide();}" />
    </dx:ASPxPopupControl>
    <my:InternshipPositionGridView ID="gvApplications" runat="server" DataSourceID="odsPositions"
        EnableExport="false" />
    <asp:ObjectDataSource ID="odsPositions" runat="server" TypeName="StudentPractice.Portal.DataSources.Positions"
        SelectMethod="FindWithCriteria" SelectCountMethod="CountWithCriteria"
        EnablePaging="true" SortParameterName="sortExpression" OnSelecting="odsPositions_Selecting">
        <SelectParameters>
            <asp:Parameter Name="criteria" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>
    </form>
</body>
</html>
