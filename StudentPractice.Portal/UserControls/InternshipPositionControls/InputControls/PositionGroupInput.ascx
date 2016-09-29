<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PositionGroupInput.ascx.cs" Inherits="StudentPractice.Portal.UserControls.InternshipPositionControls.InputControls.PositionGroupInput" %>

<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">&raquo;  
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, Header %>" />
            <a id="example" runat="server" style="font-size: 11px; text-decoration: underline; color: Blue" href="javascript:void(0)"
                onclick="window.open('/Secure/InternshipProviders/InternshipPositionExample.aspx','internshipPositionExample','toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=700, height=450'); return false;">
                (<asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, Example %>" />)
            </a>
        </th>
    </tr>

    <tr>
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, Title %>" />
        </th>
        <td>
            <asp:TextBox ID="txtTitle" runat="server" MaxLength="500" ClientIDMode="Static" Width="60%" title="<%$ Resources:PositionInput, Title %>" />

            <asp:RequiredFieldValidator ID="rfvTitle" Display="Dynamic" runat="server" ControlToValidate="txtTitle" ErrorMessage="<%$ Resources:PositionGroupInput, TitleRequired %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:PositionGroupInput, TitleRequired %>" />
            </asp:RequiredFieldValidator>
        </td>
    </tr>

    <tr id="rowPositionCount" runat="server">
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, PositionCount %>" />
        </th>
        <td>
            <asp:TextBox ID="txtPositionCount" runat="server" ClientIDMode="Static" Columns="20" MaxLength="2" Width="20%" title="<%$ Resources:PositionInput, PositionCount %>" />

            <asp:RequiredFieldValidator ID="rfvPositionCount" runat="server" ControlToValidate="txtPositionCount"
                Display="Dynamic" ErrorMessage="<%$ Resources:PositionGroupInput, PositionCountRequired %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:PositionGroupInput, PositionCountRequired %>" />
            </asp:RequiredFieldValidator>

            <asp:RangeValidator ID="rvPositionCount" runat="server" MinimumValue="1" MaximumValue="99"
                ControlToValidate="txtPositionCount" ErrorMessage="<%$ Resources:PositionGroupInput, PositionCountRange %>"
                ValidateEmptyText="false" Display="Dynamic" OnServerValidate="rvPositionCount_ServerValidate">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:PositionGroupInput, PositionCountRange %>" />
            </asp:RangeValidator>

        </td>
    </tr>

    <tr>
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, Description %>" />
        </th>
        <td>
            <asp:TextBox ID="txtDescription" runat="server" MaxLength="500" ClientIDMode="Static" TextMode="MultiLine" Rows="6" Width="60%" title="<%$ Resources:PositionInput, Description %>" />

            <asp:RequiredFieldValidator ID="rfvDescription" Display="Dynamic" runat="server" ControlToValidate="txtDescription" ErrorMessage="<%$ Resources:PositionGroupInput, DescriptionRequired %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:PositionGroupInput, DescriptionRequired %>" />
            </asp:RequiredFieldValidator>

        </td>
    </tr>

    <tr>
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, Duration %>" />
        </th>
        <td>
            <asp:TextBox ID="txtDuration" runat="server" ClientIDMode="Static" Columns="20" MaxLength="2" Width="20%" title="<%$ Resources:PositionInput, Duration %>" />

            <asp:RequiredFieldValidator ID="rfvDuration" runat="server" ControlToValidate="txtDuration"
                Display="Dynamic" ErrorMessage="<%$ Resources:PositionGroupInput, DurationRequired %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:PositionGroupInput, DurationRequired %>" />
            </asp:RequiredFieldValidator>

            <asp:RangeValidator ID="rvDuration" runat="server" MinimumValue="1" MaximumValue="99"
                ControlToValidate="txtDuration" ErrorMessage="<%$ Resources:PositionGroupInput, DurationRange %>"
                ValidateEmptyText="false" Display="Dynamic" OnServerValidate="rvDuration_ServerValidate">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:PositionGroupInput, DurationRange %>" />
            </asp:RangeValidator>

        </td>
    </tr>
    <tr id="trCountry" runat="server">
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, Country %>" />
        </th>
        <td>
            <asp:DropDownList ID="ddlCountry" AutoPostBack="true" runat="server" ClientIDMode="Static" Width="61%"
                OnInit="ddlCountry_Init" DataTextField="Name" DataValueField="ID" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" />

            <asp:RequiredFieldValidator ID="rfvCountry" Display="Dynamic" runat="server" ControlToValidate="ddlCountry" ErrorMessage="<%$ Resources:PositionGroupInput, CountryRequired %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:PositionGroupInput, CountryRequired %>" />
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

                    <asp:RequiredFieldValidator ID="rfvPrefecture" Display="Dynamic" runat="server" ControlToValidate="ddlPrefecture" ErrorMessage="<%$ Resources:PositionGroupInput, PrefectureGrRequired %>">
                        <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:PositionGroupInput, PrefectureGrRequired %>" />
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <th id="cityLit" runat="server">
                    <asp:Literal ID="litCity" runat="server" />
                </th>
                <td>
                    <asp:DropDownList ID="ddlCity" runat="server" ClientIDMode="Static" Width="61%" DataTextField="Name" DataValueField="ID" />

                    <asp:RequiredFieldValidator ID="rfvCity" Display="Dynamic" runat="server" ControlToValidate="ddlCity" ErrorMessage="<%$ Resources:PositionGroupInput, Kali_CityGrRequired %>">
                        <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:PositionGroupInput, Kali_CityGrRequired %>" />
                    </asp:RequiredFieldValidator>

                    <act:CascadingDropDown ID="cddCity" runat="server" TargetControlID="ddlCity"
                        ParentControlID="ddlPrefecture" Category="Cities" ServicePath="~/PortalServices/Services.asmx" ServiceMethod="GetCities" LoadingText="Παρακαλω περιμένετε">
                    </act:CascadingDropDown>
                </td>
            </tr>
        </asp:View>

        <asp:View runat="server" ID="vForeignCity" ClientIDMode="Static">
            <tr>
                <th>
                    <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, City %>" />
                </th>
                <td>
                    <asp:TextBox ID="txtCityText" runat="server" ClientIDMode="Static" Width="60%" title="<%$ Resources:PositionInput, ForeignCity %>" />

                    <asp:RequiredFieldValidator ID="rfvCityText" Display="Dynamic" runat="server" ControlToValidate="txtCityText" ErrorMessage="<%$ Resources:PositionGroupInput, CityRequired %>">
                        <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:PositionGroupInput, CityRequired %>" />
                    </asp:RequiredFieldValidator>

                </td>
            </tr>
        </asp:View>
    </asp:MultiView>

    <tr id="rowNoTimeLimit" runat="server">
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, TimeLimit %>" />
        </th>
        <td>
            <asp:DropDownList ID="ddlNoTimeLimit" runat="server" ClientIDMode="Static" Width="61%">
                <asp:ListItem Text="<%$ Resources:PositionGroupInput, TimeLimitWithout %>" Value="1" Selected="True" />
                <asp:ListItem Text="<%$ Resources:PositionGroupInput, TimeLimitWith %>" Value="0" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr id="trStartDate" runat="server" class="hidable" style="display: none">
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, StartDate %>" />
        </th>
        <td>
            <asp:TextBox ID="txtStartDate" runat="server" MaxLength="10" ClientIDMode="Static" Width="20%" title="<%$ Resources:PositionInput, StartDate %>" />
            <asp:HyperLink ID="lnkSelectStartDate" runat="server" NavigateUrl="#">
                    <img runat="server" style="border: none; vertical-align: middle" src="~/_img/iconCalendar.png" />
            </asp:HyperLink>

            <act:CalendarExtender ID="ceSelectStartDate" runat="server" PopupButtonID="lnkSelectStartDate" TargetControlID="txtStartDate" Format="<%$ Resources:GlobalProvider, DateFormat %>" />

            <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ControlToValidate="txtStartDate" ClientIDMode="Static"
                Display="Dynamic" ErrorMessage="<%$ Resources:PositionGroupInput, StartDateRequired %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:PositionGroupInput, StartDateRequired %>" />
            </asp:RequiredFieldValidator>

            <asp:CompareValidator ID="cvStartDate" runat="server" Display="Dynamic" Type="Date" Operator="DataTypeCheck" ClientIDMode="Static"
                ControlToValidate="txtStartDate" ErrorMessage="<%$ Resources:PositionGroupInput, StartDateCompare %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:PositionGroupInput, StartDateCompare %>" />
            </asp:CompareValidator>

            <asp:CustomValidator ID="cvMinDuration" runat="server" ControlToValidate="txtStartDate" ClientIDMode="Static"
                Display="Dynamic" OnServerValidate="cvMinDuration_ServerValidate" ValidateEmptyText="true"
                ErrorMessage="<%$ Resources:PositionGroupInput, StartDateCustom %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:PositionGroupInput, StartDateCustom %>" />
            </asp:CustomValidator>

        </td>
    </tr>
    <tr id="trEndDate" runat="server" class="hidable" style="display: none">
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, EndDate %>" />
        </th>
        <td>
            <asp:TextBox ID="txtEndDate" runat="server" MaxLength="10" ClientIDMode="Static" Width="20%" title="<%$ Resources:PositionInput, EndDate %>" />
            <asp:HyperLink ID="lnkSelectEndDate" runat="server" NavigateUrl="#">
                    <img runat="server" style="border: none; vertical-align: middle" src="~/_img/iconCalendar.png" />
            </asp:HyperLink>

            <act:CalendarExtender ID="ceSelectEndDate" runat="server" PopupButtonID="lnkSelectEndDate" TargetControlID="txtEndDate" Format="<%$ Resources:GlobalProvider, DateFormat %>" />

            <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ControlToValidate="txtEndDate" ClientIDMode="Static"
                Display="Dynamic" ErrorMessage="<%$ Resources:PositionGroupInput, EndDateRequired %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:PositionGroupInput, EndDateRequired %>" />
            </asp:RequiredFieldValidator>

            <asp:CompareValidator ID="cvEndDate" runat="server" Display="Dynamic" Type="Date" Operator="DataTypeCheck" ClientIDMode="Static"
                ControlToValidate="txtEndDate" ErrorMessage="<%$ Resources:PositionGroupInput, EndDateCompare %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:PositionGroupInput, EndDateCompare %>" />
            </asp:CompareValidator>

            <asp:CustomValidator ID="cvMinEndDate" runat="server" ControlToValidate="txtEndDate" ClientIDMode="Static"
                Display="Dynamic" OnServerValidate="cvMinEndDate_ServerValidate" ValidateEmptyText="true"
                ErrorMessage="<%$ Resources:PositionGroupInput, EndDateCustom %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:PositionGroupInput, EndDateCustom %>" />
            </asp:CustomValidator>
        </td>
    </tr>

    <tr>
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, PositionType %>" />
        </th>
        <td>
            <asp:DropDownList ID="ddlPositionType" runat="server" ClientIDMode="Static" OnInit="ddlPositionType_Init" Width="61%" />
            <asp:RequiredFieldValidator ID="rfvPositionType" Display="Dynamic" runat="server" ControlToValidate="ddlPositionType"
                ErrorMessage="<%$ Resources:PositionGroupInput, PositionTypeRequired %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:PositionGroupInput, PositionTypeRequired %>" />
            </asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, Supervisor %>" />
        </th>
        <td>
            <asp:TextBox ID="txtSupervisor" runat="server" MaxLength="500" ClientIDMode="Static" Width="60%" title="<%$ Resources:PositionInput, Supervisor %>" />
        </td>
    </tr>
    <tr>
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, SupervisorEmail %>" />
        </th>
        <td>
            <asp:TextBox ID="txtSupervisorEmail" runat="server" MaxLength="256" ClientIDMode="Static" Width="60%" title="<%$ Resources:PositionInput, SupervisorEmail %>" />
            <asp:RegularExpressionValidator ID="revSupervisorEmail" runat="server" Display="Dynamic" ControlToValidate="txtSupervisorEmail"
                ValidationExpression="^([a-zA-Z0-9_\-])+(\.([a-zA-Z0-9_\-])+)*@((\[(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5]))\]))|((([a-zA-Z0-9])+(([\-])+([a-zA-Z0-9])+)*\.)+([a-zA-Z])+(([\-])+([a-zA-Z0-9])+)*))$"
                ErrorMessage="<%$ Resources:PositionGroupInput, SupervisorEmailRegex %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:PositionGroupInput, SupervisorEmailRegex %>" />
            </asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, ContactPhone %>" />
        </th>
        <td>
            <asp:TextBox ID="txtContactPhone" runat="server" MaxLength="10" ClientIDMode="Static" Width="20%" title="<%$ Resources:PositionInput, ContactPhone %>" />

            <asp:RequiredFieldValidator ID="rfvContactPhone" Display="Dynamic" runat="server" ControlToValidate="txtContactPhone"
                ErrorMessage="<%$ Resources:PositionGroupInput, ContactPhoneRequired %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:PositionGroupInput, ContactPhoneRequired %>" />
            </asp:RequiredFieldValidator>

            <asp:RegularExpressionValidator ID="revContactPhone" runat="server" ControlToValidate="txtContactPhone"
                Display="Dynamic" ValidationExpression="^(2[0-9]{9})|(69[0-9]{8})$" ErrorMessage="<%$ Resources:PositionGroupInput, ContactPhoneGrRegex %>">
                <img src="/_img/error.gif" id="revContactPhoneTip" class="errortip" runat="server" title="<%$ Resources:PositionGroupInput, ContactPhoneGrRegex %>" />
            </asp:RegularExpressionValidator>
        </td>
    </tr>
</table>
<script type="text/javascript">
    function changeDateTimeBoxes(visible) {
        if (visible == true) {
            $('.hidable').show();
        }
        else {
            $('.hidable').hide();
        }
        ValidatorEnable(document.getElementById('rfvStartDate'), visible);
        ValidatorEnable(document.getElementById('rfvEndDate'), visible);
        ValidatorEnable(document.getElementById('cvStartDate'), visible);
        ValidatorEnable(document.getElementById('cvEndDate'), visible);
        ValidatorEnable(document.getElementById('cvMinDuration'), visible);
        ValidatorEnable(document.getElementById('cvMinEndDate'), visible);
    }

    $(document).ready(function () {
        $('#ddlNoTimeLimit').change(function () {
            changeDateTimeBoxes($(this).val() == 0)
        });
        changeDateTimeBoxes($('#ddlNoTimeLimit').val() == 0)
    });
</script>
