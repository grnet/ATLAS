<%@ Page Title="Υποβολή Ερωτήματος" MasterPageFile="~/Browse/Browse.Master" Inherits="StudentPractice.Portal.Browse.ContactForm" CodeBehind="ContactForm.aspx.cs" Language="C#" AutoEventWireup="true" %>

<asp:Content runat="server" ContentPlaceHolderID="cphMain">

    <asp:MultiView ID="mvContact" runat="server" ActiveViewIndex="0">

        <asp:View ID="vSend" runat="server">

            <table class="dv" style="width: 100%">
                <colgroup>
                    <col style="width: 130px" />
                </colgroup>
                <tr>
                    <th>
                        <asp:Literal runat="server" Text="<%$ Resources:HelpdeskContact, Form_Name %>" />
                    </th>
                    <td>
                        <dx:ASPxTextBox ID="txtContactName" runat="server" Width="300px">
                            <ClientSideEvents KeyUp="Imis.Lib.ToUpper" />
                            <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="<%$ Resources:HelpdeskContact, Form_NameRequired %>" ErrorDisplayMode="ImageWithTooltip" />
                        </dx:ASPxTextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Literal runat="server" Text="<%$ Resources:HelpdeskContact, Form_Phone %>" />
                    </th>
                    <td>
                        <dx:ASPxTextBox ID="txtContactPhone" runat="server" MaxLength="<%$ Resources:HelpdeskContact, Form_PhoneMaxLength %>" Width="300px">
                            <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="<%$ Resources:HelpdeskContact, Form_PhoneRequired %>"
                                RegularExpression-ValidationExpression="<%$ Resources:HelpdeskContact, Form_PhoneRegex %>" RegularExpression-ErrorText="<%$ Resources:HelpdeskContact, Form_PhoneRegexErrorMessage %>" ErrorDisplayMode="ImageWithTooltip" />
                        </dx:ASPxTextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Literal runat="server" Text="<%$ Resources:HelpdeskContact, Form_Email %>" />
                    </th>
                    <td>
                        <dx:ASPxTextBox ID="txtContactEmail" runat="server" Width="300px">
                            <ClientSideEvents Validation="Imis.Lib.CheckEmail" />
                            <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="<%$ Resources:HelpdeskContact, Form_EmailRequired %>" ErrorDisplayMode="ImageWithTooltip" />
                        </dx:ASPxTextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Literal runat="server" Text="<%$ Resources:HelpdeskContact, Form_Reporter %>" />
                    </th>
                    <td>
                        <dx:ASPxComboBox ID="ddlReporterType" runat="server" ClientInstanceName="ddlReporterType" ValueType="System.Int32" OnInit="ddlReporterType_Init" Width="300px">
                            <ClientSideEvents SelectedIndexChanged="function(s, e) { ddlIncidentType.PerformCallback(ddlReporterType.GetValue().toString()); cbpAcademicDetails.PerformCallback(ddlReporterType.GetValue().toString()); }" />
                            <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="<%$ Resources:HelpdeskContact, Form_ReporterRequired %>" ErrorDisplayMode="ImageWithTooltip" />
                        </dx:ASPxComboBox>
                    </td>
                </tr>
            </table>

            <dx:ASPxCallbackPanel ID="cbpAcademicDetails" runat="server" ClientInstanceName="cbpAcademicDetails" OnCallback="cbpAcademicDetails_Callback">
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <asp:PlaceHolder ID="phAcademicDetails" runat="server" Visible="false">

                            <table class="dv" style="margin-top: 0px; border-top: 0; width: 100%">
                                <colgroup>
                                    <col style="width: 130px" />
                                </colgroup>
                                <tr>
                                    <th style="border-top: 0;">
                                        <asp:Literal runat="server" Text="<%$ Resources:HelpdeskContact, Form_Institution %>" />
                                    </th>
                                    <td style="border-top: 0;">
                                        <dx:ASPxComboBox ID="ddlInstitution" runat="server" ClientInstanceName="ddlInstitution" ValueType="System.Int32" OnInit="ddlInstitution_Init" Width="300px">
                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { ddlDepartment.PerformCallback(ddlInstitution.GetValue()); }" />
                                        </dx:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr id="trDepartment" runat="server">
                                    <th>
                                        <asp:Literal runat="server" Text="<%$ Resources:HelpdeskContact, Form_Department %>" />
                                    </th>
                                    <td>
                                        <dx:ASPxComboBox ID="ddlDepartment" runat="server" ClientInstanceName="ddlDepartment" ValueType="System.Int32" OnCallback="ddlDepartment_Callback" Width="300px" />
                                    </td>
                                </tr>
                            </table>
                        </asp:PlaceHolder>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>

            <table class="dv" style="margin-top: 0px; border-top: 0; width: 100%">
                <colgroup>
                    <col style="width: 130px" />
                </colgroup>
                <tr>
                    <th style="border-top: 0;">
                        <asp:Literal runat="server" Text="<%$ Resources:HelpdeskContact, Form_ReportType %>" />
                    </th>
                    <td style="border-top: 0;">
                        <dx:ASPxComboBox ID="ddlIncidentType" runat="server" ClientInstanceName="ddlIncidentType" ValueType="System.Int32" OnCallback="ddlIncidentType_Callback" Width="300px">
                            <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="<%$ Resources:HelpdeskContact, Form_ReportTypeRequired %>" ErrorDisplayMode="ImageWithTooltip" />
                        </dx:ASPxComboBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Literal runat="server" Text="<%$ Resources:HelpdeskContact, Form_Text %>" />
                    </th>
                    <td>
                        <dx:ASPxMemo ID="txtReportText" runat="server" Rows="10" Width="300px">
                            <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="<%$ Resources:HelpdeskContact, Form_TextRequired %>" ErrorDisplayMode="ImageWithTooltip" />
                        </dx:ASPxMemo>
                    </td>
                </tr>
            </table>

            <div style="text-align: center; margin-top: 15px">
                <dx:ASPxButton ID="btnSubmit" runat="server" Text="<%$ Resources:HelpdeskContact, SubmitButtonLabel %>" Image-Url="~/_img/iconEmail.png" OnClick="btnSubmit_Click" />
            </div>

            <br />

            <div class="summaryContainer">
                <dx:ASPxValidationSummary runat="server" ClientInstanceName="validationSummary" RenderMode="BulletedList" ShowErrorsInEditors="true" />
            </div>

            <div class="message">

                <asp:Literal runat="server" Text="<%$ Resources:HelpdeskContact, HelpdeskContactInfo %>" />

            </div>

        </asp:View>
        <asp:View ID="vComplete" runat="server">

            <asp:Literal ID="ltSentEmailConfirmation" runat="server" />

        </asp:View>

        <asp:View ID="vOnlineReportsNotAllowed" runat="server">
            <div class="message">
                <p>
                    Για πληροφορίες μπορείτε να απευθύνεστε στο τηλέφωνο <em>215 215 7860</em>
                </p>
            </div>
        </asp:View>

    </asp:MultiView>

</asp:Content>
