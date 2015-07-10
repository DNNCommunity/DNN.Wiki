<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WikiMenu.ascx.cs" Inherits="DotNetNuke.Wiki.Views.SharedControls.WikiMenu" %>
<asp:Panel id="LinksPanel" runat="server" CssClass="WikiLinksPanel">
    <ul >
        <li><asp:HyperLink id="HomeBtn" runat="server" CssClass="CommandButton wikiIndex"></asp:HyperLink></li>

	    <li><asp:HyperLink id="SearchBtn" runat="server" CssClass="CommandButton wikiIndex"></asp:HyperLink></li>

	    <li><asp:HyperLink id="RecChangeBtn" runat="server" CssClass="CommandButton wikiIndex"></asp:HyperLink></li>

        <li><asp:HyperLink id="IndexBtn" runat="server" CssClass="CommandButton wikiIndex"></asp:HyperLink></li>
	
	</ul>
</asp:Panel>