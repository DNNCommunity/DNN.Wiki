<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Ratings.ascx.cs" Inherits="DotNetNuke.Wiki.Views.Ratings" %>
<table width="100%" border="0">
    <tr>
        <td width="60%" align="left" valign="top">
            <asp:Panel runat="server" ID="pnlCastVote">
                <table id="RatingsTable" runat="server">
                    <tr>
                        <td nowrap="nowrap">
                            <asp:Label ID="RatePagelbl" runat="server" CssClass="NormalBold"></asp:Label></td>
                        <td>
                            <asp:Label ID="LowRating" runat="server" CssClass="Normal"></asp:Label></td>
                        <td>
                            <asp:RadioButton ID="rating1" runat="server" GroupName="rating"></asp:RadioButton></td>
                        <td>
                            <asp:RadioButton ID="rating2" runat="server" GroupName="rating"></asp:RadioButton></td>
                        <td>
                            <asp:RadioButton ID="rating3" runat="server" GroupName="rating"></asp:RadioButton></td>
                        <td>
                            <asp:RadioButton ID="rating4" runat="server" GroupName="rating"></asp:RadioButton></td>
                        <td>
                            <asp:RadioButton ID="rating5" runat="server" GroupName="rating"></asp:RadioButton></td>
                        <td class="Normal">
                            <asp:Label ID="HighRating" runat="server" CssClass="Normal"></asp:Label></td>
                        <td class="Normal" nowrap="nowrap">&nbsp;&nbsp;|&nbsp;

                            <asp:LinkButton ID="btnSubmit" OnClick="BtnSubmit_Click" runat="server" CssClass="CommandButton"></asp:LinkButton>&nbsp;|</td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlVoteCast" Visible="false">
                <asp:Label ID="lblVoteCastMessage" runat="server" CssClass="Normal"></asp:Label>
            </asp:Panel>
        </td>
        <td align="right">
            <table border="0">
                <tr>
                    <td align="left" valign="top">
                        <asp:Label runat="server" ID="lblAverageRatingMessage" CssClass="Normal"></asp:Label>&nbsp;

                        <asp:Label runat="server" CssClass="NormalBold" ID="lblAverageRating"></asp:Label>&nbsp;&nbsp;&nbsp;<br />
                        <asp:Table ID="RatingsGraphTable" runat="server" Height="50px">
                            <asp:TableRow>
                                <asp:TableCell VerticalAlign="Bottom"></asp:TableCell>
                                <asp:TableCell VerticalAlign="Bottom"></asp:TableCell>
                                <asp:TableCell VerticalAlign="Bottom"></asp:TableCell>
                                <asp:TableCell VerticalAlign="Bottom"></asp:TableCell>
                                <asp:TableCell VerticalAlign="Bottom"></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell CssClass="normal">1</asp:TableCell>
                                <asp:TableCell CssClass="normal">2</asp:TableCell>
                                <asp:TableCell CssClass="normal">3</asp:TableCell>
                                <asp:TableCell CssClass="normal">4</asp:TableCell>
                                <asp:TableCell CssClass="normal">5</asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                        <asp:Label runat="server" ID="lblRatingCount" CssClass="Normal"></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>