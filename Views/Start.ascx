<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Start.ascx.cs" Inherits="DotNetNuke.Wiki.Views.Start" %>

<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>

<%@ Register TagName="label" TagPrefix="dnn" Src="~/controls/labelcontrol.ascx" %>

<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/sectionheadcontrol.ascx" %>
<%@ Register TagPrefix="wiki" TagName="PageRating" Src="PageRatings.ascx" %>
<%@ Register TagPrefix="wiki" TagName="Ratings" Src="Ratings.ascx" %>
<%@ Register TagPrefix="comment" Namespace="DotNetNuke.Wiki.Utilities" Assembly="DotNetNuke.Wiki" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>

<div id="DnnWiki">
    <div class="WikiHeader clearfix">
        <div class="WikiTitle">
            <h1 class="Head">
                <asp:Label ID="lblPageTopic" runat="server"></asp:Label>
            </h1>
        </div>
        <div class="WikiRating">
            <wiki:pagerating id="m_pageRating" runat="server"></wiki:pagerating>
            <br />
            <comment:CommentCount id="CommentCount1" runat="server" cssClass="Normal"></comment:CommentCount>
        </div>
    </div>
    <div class="Normal">
        <asp:Literal ID="lblPageContent" runat="server" EnableViewState="False"></asp:Literal>
    </div>
    <br />
    <br />
    <dnn:sectionhead id="RatingSec" runat="server" includerule="false" isexpanded="true" section="ratingTbl"
        cssclass="NormalBold">
    </dnn:sectionhead>
    <table id="ratingTbl" style="width:100%; border:0;" runat="server">
        <tr>
            <td>
                <wiki:Ratings id="m_ratings" runat="server"></wiki:Ratings>
            </td>
        </tr>
    </table>
    <dnn:sectionhead id="CommentsSec" runat="server" includerule="false" isexpanded="true" section="CommentsTbl"
        cssclass="NormalBold">
    </dnn:sectionhead>
    <table id="CommentsTbl" style="width:100%; border:0;" runat="server">
        <tr>
            <td>
                <asp:LinkButton ID="AddCommentCommand" OnClick="AddCommentCommand_Click" runat="server" CssClass="CommandButton"></asp:LinkButton>
                <br />
                <comment:comments id="Comments2" runat="server" parentid="1" dateformat="dd-MM-yyyy HH:mm:ss" cacheitems="true"
                    hideemailaddress="False" cellspacing="5" hideemailurl="/getemail.aspx?commentid={0}" useoledb="True"
                    CssClass="Wiki_CommentsTable">
                </comment:comments>
                <br />
                <asp:Panel ID="AddCommentPane" runat="server" Visible="false">
                    <asp:Label ID="PostCommentLbl" CssClass="SubHead" runat="server"></asp:Label>
                    <br />
                    <comment:addcommentsform id="AddCommentsForm1" runat="server" checkemail="True" checkname="True" checkcomments="False"></comment:addcommentsform>
                </asp:Panel>
            </td>
        </tr>
    </table>
</div>