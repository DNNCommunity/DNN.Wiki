<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Router.ascx.cs" Inherits="DotNetNuke.Wiki.Views.Router" %>
<div class="WikiWrapper clearfix">
    <div class="WikiMenu">
        <asp:PlaceHolder ID="phWikiMenu" runat="server" />
    </div>
    <div class="WikiContent">
        <asp:PlaceHolder ID="phWikiContent" runat="server" />
    </div>
</div>