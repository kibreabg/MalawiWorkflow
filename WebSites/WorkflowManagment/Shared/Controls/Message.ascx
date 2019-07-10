<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Message.ascx.cs"
    Inherits="Shared_Controls_Message" %>

<asp:Panel ID="pnlMessage" runat="server" Width="93.6%">
    <span>
        <asp:Literal ID="litMessageType" runat="server"></asp:Literal>
    </span>
    <asp:Literal ID="litMessage" runat="server"></asp:Literal>
</asp:Panel>
