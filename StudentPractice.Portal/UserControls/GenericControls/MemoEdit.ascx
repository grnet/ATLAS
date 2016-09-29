<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MemoEdit.ascx.cs" Inherits="StudentPractice.Portal.UserControls.GenericControls.MemoEdit" %>
<%@ Register Assembly="DevExpress.Web.v12.2, Version=12.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>

<div id="<%= ClientID %>">
    <table id="tbMemo" runat="server" style="table-layout: fixed; overflow: hidden;">
        <tr>
            <td style="width: 100%;">
                <dx:ASPxMemo runat="server" ID="memoArea" Width="100%" Height="100%" AutoResizeWithContainer="false" >
                    <ValidationSettings RequiredField-IsRequired="false" RequiredField-ErrorText="Το πεδίο είναι υποχρεωτικό" ErrorDisplayMode="ImageWithTooltip" Display="Dynamic" />
                </dx:ASPxMemo>
            </td>
        </tr>
        <tr runat="server" id="counterRow" style="height: 20px;">
            <td style="padding-top: 6px">
                <span id="spCharCounter" runat="server"></span>
            </td>
        </tr>
    </table>
</div>
<asp:HiddenField ID="hfClientState" runat="server" />
