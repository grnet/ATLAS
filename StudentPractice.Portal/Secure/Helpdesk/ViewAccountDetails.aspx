<%@ Page Title="" Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" CodeBehind="ViewAccountDetails.aspx.cs" Inherits="StudentPractice.Portal.Secure.Helpdesk.ViewAccountDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <style type="text/css">
        .dvTable {
            width: 440px;
        }

            .dvTable tr {
                height: 35px;
            }

        .icon-btn {
            margin-left: 10px;
        }

        .img-btn {
            border: dashed 1px gray;
            padding: 2px;
            background-repeat: no-repeat;
            background-position: 2px 3px;
            margin-right: 15px;
            outline: none;
        }

        #email,
        #mobilePhone {
            width: 300px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <script src="/_js/UserContactDetails.js" type="text/javascript"></script>
    <table class="dv dvTable" style="width: 100%">
        <tr>
            <th style="width: 85px">Username:
            </th>
            <td>
                <asp:Label runat="server" ID="ltrUsername" ForeColor="Blue" />
            </td>
        </tr>
        <tr>
            <th>Email:
            </th>
            <td><a href="#" title="Αλλαγή Email" class="img-btn bg-emailEdit" id="btnEmail">
                <img alt="" src="/_img/s.gif" style="width: 16px; height: 16px" /></a>
                <asp:Label runat="server" ID="ltrEmail" ForeColor="Blue" ClientIDMode="Static" />
                <span id="emailError" />
            </td>
        </tr>
        <tr>
            <th>Κλειδωμένος:
            </th>
            <td>
                <asp:PlaceHolder runat="server" ID="phIsLocked"><a href="#" title="Ξεκλείδωμα Χρήστη" class="img-btn  bg-unlock" id="btnUnlock">
                    <img alt="" src="/_img/s.gif" style="width: 16px; height: 16px" /></a> </asp:PlaceHolder>
                <asp:Label runat="server" ID="ltrIsLockedOut" ForeColor="Blue" ClientIDMode="Static" />
                <span id="unlockError" />
            </td>
        </tr>
        <tr id="rowAPI" runat="server" visible="false">
            <th>Χρήση του API:
            </th>
            <td>
                <asp:Label runat="server" ID="ltrApiApproved" ForeColor="Blue" ClientIDMode="Static" />
                <asp:LinkButton ID="btnCancel" runat="server" Text="Απενεργοποιήση" Visible="false" OnClick="btnCancel_Click" CssClass="icon-btn bg-reject" />
                <asp:LinkButton ID="btnSubmit" runat="server" Text="Ενεργοποιήση" Visible="false" OnClick="btnSubmit_Click" CssClass="icon-btn bg-accept" />
            </td>
        </tr>
    </table>
</asp:Content>
