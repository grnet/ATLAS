<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FinishedPositionGroupInput.ascx.cs" Inherits="StudentPractice.Portal.UserControls.FinishedInternshipPositionControls.InputControls.FinishedPositionGroupInput" %>

<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">&raquo; Γενικά Στοιχεία Θέσης</th>
    </tr>
    <tr>
        <th>Τίτλος:
        </th>
        <td>
            <asp:TextBox ID="txtTitle" runat="server" MaxLength="500" ClientIDMode="Static" Width="60%" title="<%$ Resources:PositionInput, Title %>" />

            <asp:RequiredFieldValidator ID="rfvTitle" Display="Dynamic" runat="server" ControlToValidate="txtTitle"
                ErrorMessage="Το πεδίο 'Τίτλος' είναι υποχρεωτικό">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Τίτλος' είναι υποχρεωτικό" />
            </asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <th>Περιγραφή:
        </th>
        <td>
            <asp:TextBox ID="txtDescription" runat="server" MaxLength="500" ClientIDMode="Static"
                TextMode="MultiLine" Rows="6" Width="60%" title="<%$ Resources:PositionInput, Description %>" />

            <asp:RequiredFieldValidator ID="rfvDescription" Display="Dynamic" runat="server" ControlToValidate="txtDescription"
                ErrorMessage="Το πεδίο 'Περιγραφή' είναι υποχρεωτικό">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Τίτλος' είναι υποχρεωτικό" />
            </asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr id="trCountry" runat="server">
        <th>Χώρα:
        </th>
        <td>
            <asp:DropDownList ID="ddlCountry" AutoPostBack="true" runat="server" ClientIDMode="Static" Width="61%"
                OnInit="ddlCountry_Init" DataTextField="Name" DataValueField="ID" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" />

            <asp:RequiredFieldValidator ID="rfvCountry" Display="Dynamic" runat="server" ControlToValidate="ddlCountry"
                ErrorMessage="Το πεδίο 'Χώρα' είναι υποχρεωτικό">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Χώρα' είναι υποχρεωτικό" />
            </asp:RequiredFieldValidator>

        </td>
    </tr>
    <asp:MultiView ID="mvCountry" runat="server" ActiveViewIndex="0" ClientIDMode="Static">
        <asp:View runat="server" ID="vGreekCity" ClientIDMode="Static">
            <tr>
                <th>
                    <asp:Literal ID="litPrefecture" runat="server" />
                </th>
                <td>
                    <asp:DropDownList ID="ddlPrefecture" runat="server" ClientIDMode="Static" Width="61%" OnInit="ddlPrefecture_Init" DataTextField="Name" DataValueField="ID" />

                    <asp:RequiredFieldValidator ID="rfvPrefecture" Display="Dynamic" runat="server" ControlToValidate="ddlPrefecture">
                        <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Νομός' είναι υποχρεωτικό" />
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <th id="cityLit" runat="server">
                    <asp:Literal ID="litCity" runat="server" />
                </th>
                <td>
                    <asp:DropDownList ID="ddlCity" runat="server" ClientIDMode="Static" Width="61%" DataTextField="Name" DataValueField="ID" />

                    <asp:RequiredFieldValidator ID="rfvCity" Display="Dynamic" runat="server" ControlToValidate="ddlCity">
                        <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Πόλη' είναι υποχρεωτικό" />
                    </asp:RequiredFieldValidator>

                    <act:CascadingDropDown ID="cddCity" runat="server" TargetControlID="ddlCity"
                        ParentControlID="ddlPrefecture" Category="Cities" ServicePath="~/PortalServices/Services.asmx" ServiceMethod="GetCities" LoadingText="Παρακαλω περιμένετε">
                    </act:CascadingDropDown>
                </td>
            </tr>
        </asp:View>
        <asp:View runat="server" ID="vForeignCity" ClientIDMode="Static">
            <tr>
                <th>Πόλη:
                </th>
                <td>
                    <asp:TextBox ID="txtCityText" runat="server" ClientIDMode="Static" Width="60%" title="<%$ Resources:PositionInput, ForeignCity %>" />

                    <asp:RequiredFieldValidator ID="rfvCityText" Display="Dynamic" runat="server" ControlToValidate="txtCityText"
                        ErrorMessage="Το πεδίο 'Πόλη' είναι υποχρεωτικό">
                        <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Πόλη' είναι υποχρεωτικό" />
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
        </asp:View>
    </asp:MultiView>
    <tr>
        <th>Είδος θέσης:
        </th>
        <td>
            <asp:DropDownList ID="ddlPositionType" runat="server" ClientIDMode="Static" OnInit="ddlPositionType_Init" Width="61%" />

            <asp:RequiredFieldValidator ID="rfvPositionType" Display="Dynamic" runat="server" ControlToValidate="ddlPositionType"
                ErrorMessage="Το πεδίο 'Είδος θέσης' είναι υποχρεωτικό">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Είδος θέσης' είναι υποχρεωτικό" />
            </asp:RequiredFieldValidator>

        </td>
    </tr>
    <tr>
        <th>Ον/μο Επόπτη:
        </th>
        <td>
            <asp:TextBox ID="txtSupervisor" runat="server" MaxLength="500" ClientIDMode="Static" Width="60%" title="<%$ Resources:PositionInput, Supervisor %>" />
        </td>
    </tr>
    <tr>
        <th>E-mail Επόπτη:
        </th>
        <td>
            <asp:TextBox ID="txtSupervisorEmail" runat="server" MaxLength="256" ClientIDMode="Static" Width="60%" title="<%$ Resources:PositionInput, SupervisorEmail %>" />

            <asp:RegularExpressionValidator ID="revSupervisorEmail" runat="server" Display="Dynamic" ControlToValidate="txtSupervisorEmail"
                ValidationExpression="^([a-zA-Z0-9_\-])+(\.([a-zA-Z0-9_\-])+)*@((\[(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5]))\]))|((([a-zA-Z0-9])+(([\-])+([a-zA-Z0-9])+)*\.)+([a-zA-Z])+(([\-])+([a-zA-Z0-9])+)*))$"
                ErrorMessage="Το πεδίο 'E-mail Επόπτη' δεν είναι έγκυρο">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'E-mail Επόπτη' δεν είναι έγκυρο" />
            </asp:RegularExpressionValidator>

        </td>
    </tr>
    <tr>
        <th>Τηλέφωνο επικοινωνίας:
        </th>
        <td>
            <asp:TextBox ID="txtContactPhone" runat="server" MaxLength="10" ClientIDMode="Static" Width="20%" title="<%$ Resources:PositionInput, ContactPhone %>" />

            <asp:RequiredFieldValidator ID="rfvContactPhone" Display="Dynamic" runat="server" ControlToValidate="txtContactPhone"
                ErrorMessage="Το πεδίο 'Τηλέφωνο επικοινωνίας' είναι υποχρεωτικό">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Τηλέφωνο επικοινωνίας' είναι υποχρεωτικό" />
            </asp:RequiredFieldValidator>

            <asp:RegularExpressionValidator ID="revContactPhone" runat="server" ControlToValidate="txtContactPhone"
                Display="Dynamic" ValidationExpression="^(2[0-9]{9})|(69[0-9]{8})$" ErrorMessage="Το πεδίο 'Τηλέφωνο επικοινωνίας' πρέπει να αποτελείται από ακριβώς 10 ψηφία και να ξεκινάει από 2 αν πρόκειται για σταθερό ή από 69 αν πρόκειται για κινητό.">
                <img src="/_img/error.gif" class="errortip" id="revContactPhoneTip" runat="server" title="Το πεδίο 'Τηλέφωνο επικοινωνίας' πρέπει να αποτελείται από ακριβώς 10 ψηφία και να ξεκινάει από 2 αν πρόκειται για σταθερό ή από 69 αν πρόκειται για κινητό." />
            </asp:RegularExpressionValidator>
        </td>
    </tr>
</table>
