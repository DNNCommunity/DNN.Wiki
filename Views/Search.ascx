<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Search.ascx.cs" Inherits="DotNetNuke.Wiki.Views.Search" %>
<asp:Panel runat="server" ID="pnlSearch" DefaultButton="cmdSearch">
    <p>
        <asp:Label ID="lblHead" runat="server" CssClass="Head"></asp:Label><br />
        <asp:Label ID="lblSubHead" runat="server" CssClass="SubHead"></asp:Label>&nbsp;
    <asp:TextBox ID="txtTextToSearch" runat="server" Width="104px"></asp:TextBox>
        <asp:LinkButton ID="cmdSearch" OnClick="CmdSearch_Click" runat="server" CssClass="dnnPrimaryAction"></asp:LinkButton>
    </p>
</asp:Panel>
<p>
    <asp:Literal ID="HitTable" runat="server"></asp:Literal>
</p>