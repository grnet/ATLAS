<%@ Page Language="C#" MasterPageFile="~/Secure/BackOffice.master" AutoEventWireup="true"CodeBehind="Default.aspx.cs" Inherits="StudentPractice.Portal.Secure.Reports.Default" Title="Αναφορές" %>

<%@ Import Namespace="StudentPractice.BusinessModel" %>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Repeater runat="server" ID="rptStatistics" DataSourceID="sdsStatistics">
        <ItemTemplate>
            <h1>Γενικά Στατιστικά
            </h1>
            <p class="importantStatistic">
                Εγγεγραμμένοι Φορείς Υποδοχής :
                <asp:Label ID="Label1" runat="server" Font-Bold="true"><%# Eval("TotalInternshipProviders", "{0:n0}").ToString().Replace(",", ".")%></asp:Label>
            </p>
            <p class="importantStatistic">
                Πιστοποιημένοι Φορείς Υποδοχής :
                <asp:Label ID="Label2" runat="server" Font-Bold="true"><%# Eval("TotalInternshipProviders_Verified", "{0:n0}").ToString().Replace(",", ".")%></asp:Label>
            </p>
            <p class="importantStatistic" style="margin-top: 25px">
                Εγγεγραμμένα Γραφεία Πρακτικής :
                <asp:Label ID="Label3" runat="server" Font-Bold="true"><%# Eval("TotalInternshipOffices", "{0:n0}").ToString().Replace(",", ".")%></asp:Label>
            </p>
            <p class="importantStatistic">
                Πιστοποιημένα Γραφεία Πρακτικής :
                <asp:Label ID="Label4" runat="server" Font-Bold="true"><%# Eval("TotalInternshipOffices_Verified", "{0:n0}").ToString().Replace(",", ".")%></asp:Label>
            </p>
            <p class="importantStatistic" style="margin-top: 25px">
                Εγγεγραμμένοι Φοιτητές :
                <asp:Label ID="Label9" runat="server" Font-Bold="true"><%# Eval("TotalStudents", "{0:n0}").ToString().Replace(",", ".")%></asp:Label>
            </p>
            <p class="importantStatistic" style="margin-top: 25px">
                Δημιουργημένες Θέσεις Πρακτικής Άσκησης : <a id="A1" runat="server" class="hyperlink"
                    href="javascript:void(0);" onclick=<%# string.Format("window.open('InternshipPositionPopup.aspx','','width=1024, height=768, directories=no, location=no, menubar=no, resizable=yes, scrollbars=1, status=no, toolbar=no');")%>>
                    <%# Eval("TotalInternshipPositions", "{0:n0}").ToString().Replace(",", ".")%></a>&nbsp;
                (καταχωρισμένες από ΓΠΑ: <a id="A12" runat="server" class="hyperlink"
                    href="javascript:void(0);" onclick=<%# string.Format("window.open('InternshipPositionPopup.aspx?crType={0}','','width=1024, height=768, directories=no, location=no, menubar=no, resizable=yes, scrollbars=1, status=no, toolbar=no');", (int)enPositionCreationType.FromOffice)%>>
                    <%# Eval("TotalFromOfficeInternshipPositions", "{0:n0}").ToString().Replace(",", ".")%></a>)
                <p style="margin-left: 81px">
                    Μη δημοσιευμένες: <a id="A2" runat="server" class="hyperlink" href="javascript:void(0);"
                        onclick=<%# string.Format("window.open('InternshipPositionPopup.aspx?pStatus={0}','','width=1024, height=768, directories=no, location=no, menubar=no, resizable=yes, scrollbars=1, status=no, toolbar=no');", (int)enPositionStatus.UnPublished)%>>
                        <%# Eval("TotalUnPublishedInternshipPositions", "{0:n0}").ToString().Replace(",", ".")%></a>
                    <br />
                    Ελεύθερες: <a id="A3" runat="server" class="hyperlink" href="javascript:void(0);"
                        onclick=<%# string.Format("window.open('InternshipPositionPopup.aspx?pStatus={0}','','width=1024, height=768, directories=no, location=no, menubar=no, resizable=yes, scrollbars=1, status=no, toolbar=no');", (int)enPositionStatus.Available)%>>
                        <%# Eval("TotalAvailableInternshipPositions", "{0:n0}").ToString().Replace(",", ".")%></a>
                    <br />
                    Προδεσμευμένες: <a id="A4" runat="server" class="hyperlink" href="javascript:void(0);"
                        onclick=<%# string.Format("window.open('InternshipPositionPopup.aspx?pStatus={0}','','width=1024, height=768, directories=no, location=no, menubar=no, resizable=yes, scrollbars=1, status=no, toolbar=no');", (int)enPositionStatus.PreAssigned)%>>
                        <%# Eval("TotalPreAssignedInternshipPositions", "{0:n0}").ToString().Replace(",", ".")%></a>
                    <br />
                    Αντιστοιχισμένες: <a id="A5" runat="server" class="hyperlink" href="javascript:void(0);"
                        onclick=<%# string.Format("window.open('InternshipPositionPopup.aspx?pStatus={0}','','width=1024, height=768, directories=no, location=no, menubar=no, resizable=yes, scrollbars=1, status=no, toolbar=no');", (int)enPositionStatus.Assigned)%>>
                        <%# Eval("TotalAssignedInternshipPositions", "{0:n0}").ToString().Replace(",", ".")%></a>
                    <br />
                    Υπό διενέργεια: <a id="A6" runat="server" class="hyperlink" href="javascript:void(0);"
                        onclick=<%# string.Format("window.open('InternshipPositionPopup.aspx?pStatus={0}','','width=1024, height=768, directories=no, location=no, menubar=no, resizable=yes, scrollbars=1, status=no, toolbar=no');", (int)enPositionStatus.UnderImplementation)%>>
                        <%# Eval("TotalUnderImplementationInternshipPositions", "{0:n0}").ToString().Replace(",", ".")%></a>
                    <br />
                    Ολοκληρωμένες: <a id="A7" runat="server" class="hyperlink" href="javascript:void(0);"
                        onclick=<%# string.Format("window.open('InternshipPositionPopup.aspx?pStatus={0}','','width=1024, height=768, directories=no, location=no, menubar=no, resizable=yes, scrollbars=1, status=no, toolbar=no');", (int)enPositionStatus.Completed)%>>
                        <%# Eval("TotalCompletedInternshipPositions", "{0:n0}").ToString().Replace(",", ".")%></a>&nbsp;
                    (καταχωρισμένες από ΓΠΑ: <a id="A11" runat="server" class="hyperlink" href="javascript:void(0);"
                        onclick=<%# string.Format("window.open('InternshipPositionPopup.aspx?pStatus={0}&crType={1}','','width=1024, height=768, directories=no, location=no, menubar=no, resizable=yes, scrollbars=1, status=no, toolbar=no');", (int)enPositionStatus.Completed, (int)enPositionCreationType.FromOffice)%>>
                        <%# Eval("TotalCompletedFromOfficeInternshipPositions", "{0:n0}").ToString().Replace(",", ".")%></a>)
                    <br />
                    Ακυρωμένες: <a id="A8" runat="server" class="hyperlink" href="javascript:void(0);"
                        onclick=<%# string.Format("window.open('InternshipPositionPopup.aspx?pStatus={0}&cReason=1','','width=1024, height=768, directories=no, location=no, menubar=no, resizable=yes, scrollbars=1, status=no, toolbar=no');", (int)enPositionStatus.Canceled)%>>
                        <%# Eval("TotalCanceledInternshipPositionsFromOffice", "{0:n0}").ToString().Replace(",", ".")%></a>
                    <br />
                    Αποσυρμένες: <a id="A9" runat="server" class="hyperlink" href="javascript:void(0);"
                        onclick=<%# string.Format("window.open('InternshipPositionPopup.aspx?pStatus={0}&cReason=2','','width=1024, height=768, directories=no, location=no, menubar=no, resizable=yes, scrollbars=1, status=no, toolbar=no');", (int)enPositionStatus.Canceled)%>>
                        <%# Eval("TotalRevokedInternshipPositions", "{0:n0}").ToString().Replace(",", ".")%></a>
                    <br />
                    Διεγραμμένες: <a id="A10" runat="server" class="hyperlink" href="javascript:void(0);"
                        onclick=<%# string.Format("window.open('InternshipPositionPopup.aspx?gStatus={0}','','width=1024, height=768, directories=no, location=no, menubar=no, resizable=yes, scrollbars=1, status=no, toolbar=no');", (int)enPositionGroupStatus.Deleted)%>>
                        <%# Eval("TotalDeletedInternshipPositions", "{0:n0}").ToString().Replace(",", ".")%></a>
                    <br />
                </p>
            </p>
        </ItemTemplate>
    </asp:Repeater>
    <asp:SqlDataSource ID="sdsStatistics" runat="server" ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
        ProviderName="System.Data.SqlClient" SelectCommand="SELECT TOP 1 * FROM [vReportsDefault]"
        OnSelecting="sdsStatistics_Selecting" CacheDuration="300"></asp:SqlDataSource>
</asp:Content>
