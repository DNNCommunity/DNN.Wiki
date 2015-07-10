<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopicHistory.ascx.cs" Inherits="DotNetNuke.Wiki.Views.TopicHistory" %>
<asp:Label ID="Label1" runat="server" CssClass="Head"></asp:Label>
<asp:Label ID="lblPageTopic" runat="server" CssClass="Head">DotWiki</asp:Label>
<asp:Label ID="lblDateTime" runat="server" CssClass="SubSubHead"></asp:Label>
<p>
    <asp:Label ID="lblPageContent" runat="server" CssClass="Normal"></asp:Label>&nbsp;&nbsp;
</p>
<p>
    |<asp:HyperLink ID="BackBtn" runat="server" CssClass="CommandButton"></asp:HyperLink>&nbsp;|&nbsp;&nbsp;

    <asp:LinkButton ID="cmdRestore" OnClick="CmdRestore_Click" runat="server" CssClass="CommandButton"></asp:LinkButton>&nbsp;

    <asp:Label ID="RestoreLbl" runat="server" CssClass="Normal">|<br /></asp:Label>
</p>