<%@ Page Language="C#" MasterPageFile="~/Secure/BackOffice.master" AutoEventWireup="true"
    CodeBehind="StatisticsByDay.aspx.cs" Inherits="StudentPractice.Portal.Secure.Reports.StatisticsByDay"
    Title="Στοιχεία Θέσεων Ανά Ημέρα" %>

<%@ Import Namespace="StudentPractice.BusinessModel" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="reminder" style="width: 50%;">
        Τα στοιχεία ενημερώνονται κάθε πρωί στις 05:00
    </div>
    <div style="padding: 5px 0 10px;">
        <asp:LinkButton ID="btnExport" runat="server" Text="Εξαγωγή σε Excel" OnClick="btnExport_Click"
            CssClass="icon-btn bg-excel" />
    </div>
    <dx:ASPxGridView ID="gvStudents" runat="server" AutoGenerateColumns="False" DataSourceID="sdsPosition"
        EnableRowsCache="false" EnableCallBacks="true">
        <SettingsLoadingPanel Text="Παρακαλώ Περιμένετε..." />
        <Settings ShowFooter="true" ShowGroupFooter="VisibleAlways" />
        <Styles Footer-BackColor="DarkGray" Footer-Font-Bold="true" />
        <Templates>
            <EmptyDataRow>
                Δεν βρέθηκαν αποτελέσματα
            </EmptyDataRow>
        </Templates>
        <Columns>
            <dx:GridViewDataColumn FieldName="CreatedAt" Caption="Ημερομηνία" Width="70px" />
            <dx:GridViewDataTextColumn FieldName="CreatedPositions" Caption="Δημιουργημένες Θέσεις"
                Width="70px" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="True">
                <PropertiesTextEdit DisplayFormatString="{0:n0}" />
                <DataItemTemplate>
                    <a runat="server" class="hyperlink" href="javascript:void(0);" onclick=<%# string.Format("window.open('InternshipPositionPopup.aspx?createdAt={0:dd-MM-yyyy}','','width=1024, height=768, directories=no, location=no, menubar=no, resizable=yes, scrollbars=1, status=no, toolbar=no');", Eval("CreatedAt"))%>>
                        <%# string.Format("{0:n0}", Eval("CreatedPositions"))%>
                    </a>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="PublishedPositions" Caption="Δημοσιευμένες Θέσεις"
                Width="70px" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="True">
                <PropertiesTextEdit DisplayFormatString="{0:n0}" />
                <DataItemTemplate>
                    <a runat="server" class="hyperlink" href="javascript:void(0);" onclick=<%# string.Format("window.open('InternshipPositionPopup.aspx?availableAt={0:dd-MM-yyyy}','','width=1024, height=768, directories=no, location=no, menubar=no, resizable=yes, scrollbars=1, status=no, toolbar=no');", Eval("CreatedAt"))%>>
                        <%# string.Format("{0:n0}", Eval("PublishedPositions"))%></a>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="PreAssignedPositions" Caption="Προδεσμευμένες Θέσεις"
                Width="70px" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="True">
                <PropertiesTextEdit DisplayFormatString="{0:n0}" />
                <DataItemTemplate>
                    <a runat="server" class="hyperlink" href="javascript:void(0);" onclick=<%# string.Format("window.open('InternshipPositionPopup.aspx?preAssignedAt={0:dd-MM-yyyy}','','width=1024, height=768, directories=no, location=no, menubar=no, resizable=yes, scrollbars=1, status=no, toolbar=no');", Eval("CreatedAt"))%>>
                        <%# string.Format("{0:n0}", Eval("PreAssignedPositions"))%></a>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="AssignedPositions" Caption="Αντιστοιχισμένες Θέσεις"
                Width="70px" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="True">
                <PropertiesTextEdit DisplayFormatString="{0:n0}" />
                <DataItemTemplate>
                    <a runat="server" class="hyperlink" href="javascript:void(0);" onclick=<%# string.Format("window.open('InternshipPositionPopup.aspx?assignedAt={0:dd-MM-yyyy}','','width=1024, height=768, directories=no, location=no, menubar=no, resizable=yes, scrollbars=1, status=no, toolbar=no');", Eval("CreatedAt"))%>>
                        <%# string.Format("{0:n0}", Eval("AssignedPositions"))%></a>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="UnderImplementationPositions" Caption="Υπό διενέργεια Θέσεις"
                Width="100px" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="True">
                <PropertiesTextEdit DisplayFormatString="{0:n0}" />
                <DataItemTemplate>
                    <a runat="server" class="hyperlink" href="javascript:void(0);" onclick=<%# string.Format("window.open('InternshipPositionPopup.aspx?underImplementationAt={0:dd-MM-yyyy}','','width=1024, height=768, directories=no, location=no, menubar=no, resizable=yes, scrollbars=1, status=no, toolbar=no');", Eval("CreatedAt"))%>>
                        <%# string.Format("{0:n0}", Eval("UnderImplementationPositions"))%></a>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="CompletedPositions" Caption="Ολοκληρωμένες Θέσεις"
                Width="120px" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="True">
                <PropertiesTextEdit DisplayFormatString="{0:n0}" />
                <DataItemTemplate>
                    <a runat="server" class="hyperlink" href="javascript:void(0);" onclick=<%# string.Format("window.open('InternshipPositionPopup.aspx?completedAt={0:dd-MM-yyyy}','','width=1024, height=768, directories=no, location=no, menubar=no, resizable=yes, scrollbars=1, status=no, toolbar=no');", Eval("CreatedAt"))%>>
                        <%# string.Format("{0:n0}", Eval("CompletedPositions"))%></a>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="CompletedByOfficePositions" Caption="Ολοκληρωμένες Θέσεις (απο ΓΠΑ)"
                Width="120px" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="True">
                <PropertiesTextEdit DisplayFormatString="{0:n0}" />
                <DataItemTemplate>
                    <a id="A3" runat="server" class="hyperlink" href="javascript:void(0);" onclick=<%# string.Format("window.open('InternshipPositionPopup.aspx?completedAt={0:dd-MM-yyyy}&crType={1}','','width=1024, height=768, directories=no, location=no, menubar=no, resizable=yes, scrollbars=1, status=no, toolbar=no');", Eval("CreatedAt"), (int)enPositionCreationType.FromOffice)%>>
                        <%# string.Format("{0:n0}", Eval("CompletedByOfficePositions"))%></a>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="CanceledPositions" Caption="Ακυρωμένες Θέσεις"
                Width="100px" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="True">
                <PropertiesTextEdit DisplayFormatString="{0:n0}" />
                <DataItemTemplate>
                    <a runat="server" class="hyperlink" href="javascript:void(0);" onclick=<%# string.Format("window.open('InternshipPositionPopup.aspx?canceledAt={0:dd-MM-yyyy}','','width=1024, height=768, directories=no, location=no, menubar=no, resizable=yes, scrollbars=1, status=no, toolbar=no');", Eval("CreatedAt"))%>>
                        <%# string.Format("{0:n0}", Eval("CanceledPositions"))%>
                    </a>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="RevokedPositions" Caption="Αποσυρμένες Θέσεις"
                Width="100px" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="True">
                <PropertiesTextEdit DisplayFormatString="{0:n0}" />
                <DataItemTemplate>
                    <a id="A1" runat="server" class="hyperlink" href="javascript:void(0);" onclick=<%# string.Format("window.open('InternshipPositionPopup.aspx?revokedAt={0:dd-MM-yyyy}','','width=1024, height=768, directories=no, location=no, menubar=no, resizable=yes, scrollbars=1, status=no, toolbar=no');", Eval("CreatedAt"))%>>
                        <%# string.Format("{0:n0}", Eval("RevokedPositions"))%>
                    </a>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="DeletedPositions" Caption="Διεγραμμένες Θέσεις"
                Width="100px" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="True">
                <PropertiesTextEdit DisplayFormatString="{0:n0}" />
                <DataItemTemplate>
                    <a id="A2" runat="server" class="hyperlink" href="javascript:void(0);" onclick=<%# string.Format("window.open('InternshipPositionPopup.aspx?deletedAt={0:dd-MM-yyyy}','','width=1024, height=768, directories=no, location=no, menubar=no, resizable=yes, scrollbars=1, status=no, toolbar=no');", Eval("CreatedAt"))%>>
                        <%# string.Format("{0:n0}", Eval("DeletedPositions"))%>
                    </a>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
        </Columns>
        <TotalSummary>
            <dx:ASPxSummaryItem FieldName="CreatedPositions" SummaryType="Sum" DisplayFormat="{0:n0}" />
            <dx:ASPxSummaryItem FieldName="PublishedPositions" SummaryType="Sum" DisplayFormat="{0:n0}" />
            <dx:ASPxSummaryItem FieldName="PreAssignedPositions" SummaryType="Sum" DisplayFormat="{0:n0}" />
            <dx:ASPxSummaryItem FieldName="AssignedPositions" SummaryType="Sum" DisplayFormat="{0:n0}" />
            <dx:ASPxSummaryItem FieldName="UnderImplementationPositions" SummaryType="Sum" DisplayFormat="{0:n0}" />
            <dx:ASPxSummaryItem FieldName="CompletedPositions" SummaryType="Sum" DisplayFormat="{0:n0}" />
            <dx:ASPxSummaryItem FieldName="CompletedByOfficePositions" SummaryType="Sum" DisplayFormat="{0:n0}" />
            <dx:ASPxSummaryItem FieldName="CanceledPositions" SummaryType="Sum" DisplayFormat="{0:n0}" />
            <dx:ASPxSummaryItem FieldName="RevokedPositions" SummaryType="Sum" DisplayFormat="{0:n0}" />
            <dx:ASPxSummaryItem FieldName="DeletedPositions" SummaryType="Sum" DisplayFormat="{0:n0}" />
        </TotalSummary>
    </dx:ASPxGridView>
    <dx:ASPxGridViewExporter ID="gvePositions" runat="server" GridViewID="gvStudents">
    </dx:ASPxGridViewExporter>
    <asp:SqlDataSource ID="sdsPosition" runat="server" ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"
        ProviderName="System.Data.SqlClient" SelectCommand="SELECT * FROM StatisticsByDay">
    </asp:SqlDataSource>
</asp:Content>
