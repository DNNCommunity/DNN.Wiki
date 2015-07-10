<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WikiButton.ascx.cs" Inherits="DotNetNuke.Wiki.Views.SharedControls.WikiButton" %>
<div class="WikiButtons CommandButton">

    <asp:HyperLink ID="cmdAdd" runat="server" CssClass="dnnPrimaryAction"></asp:HyperLink>

    <asp:HyperLink ID="lnkEdit" runat="server" CssClass="dnnSecondaryAction" />

    <asp:HyperLink ID="txtViewHistory" runat="server" rel="noindex,nofollow" CssClass="dnnSecondaryAction"></asp:HyperLink>
</div>