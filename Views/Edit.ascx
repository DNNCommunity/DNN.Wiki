<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="DotNetNuke.Wiki.Views.Edit" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelControl.ascx" %>
<%@ Register Src="SharedControls/Messages.ascx" TagName="Messages" TagPrefix="uc1" %>
<div id="DnnWiki">
    <asp:Label ID="lblMessage" runat="server" />
    <div class="WikiTable" id="divWikiEdit" runat="server">
        <table border="0">
            <tr>
                <td style="width: 100%">
                    <dnn:Label id="plPageName" CssClass="SubHead" runat="server">
                    </dnn:Label>
                    <asp:TextBox ID="txtPageName" runat="server" CssClass="NormalTextBox" Columns="75" TextMode="multiLine"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="PageNameValidator" runat="server" ControlToValidate="txtPageName"
                        ErrorMessage="You Must Enter a Page Name" SetFocusOnError="True">You Must Enter a Page Name</asp:RequiredFieldValidator>&nbsp;<br />
                    <dnn:SectionHead ID="shSeo" cssclass="NormalBold" runat="server" IsExpanded="false"
                        ResourceKey="shSeo" Section="wikiSeo" />
                    <div id="wikiSeo" runat="server">
                        <dnn:Label id="plTitle" CssClass="SubHead" runat="server">
                        </dnn:Label>
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="NormalTextBox" Columns="50"></asp:TextBox><br />
                        <dnn:Label id="plDescription" CssClass="SubHead" runat="server">
                        </dnn:Label>
                        <asp:TextBox ID="txtDescription" CssClass="NormalTextBox" TextMode="multiLine" runat="server"
                            Columns="50"></asp:TextBox><br />
                        <dnn:Label id="plKeywords" CssClass="SubHead" runat="server">
                        </dnn:Label>
                        <asp:TextBox ID="txtKeywords" CssClass="NormalTextBox" TextMode="multiLine" runat="server"
                            Columns="50"></asp:TextBox><br />
                    </div>
                    <dnn:TextEditor id="teContent" runat="server" Width="100%" Height="500">
                    </dnn:TextEditor><br />
                    <dnn:sectionhead id="WikiTextDirections" runat="server" includerule="false" isexpanded="false"
                        section="TextDirections" cssclass="NormalBold">
                    </dnn:sectionhead>
                    <table id="TextDirections" style="width: 100%;" summary="Text Directions Design Table"
                        border="0" runat="server">
                        <tr>
                            <td class="inlineStyle">
                                <span class="normal">
                                    <asp:Literal ID="WikiDirectionsDetails" runat="server"></asp:Literal>
                                </span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="AllowDiscuss" runat="server" CssClass="NormalBold"></asp:CheckBox>&nbsp;

                    <asp:CheckBox ID="AllowRating" runat="server" CssClass="NormalBold"></asp:CheckBox>
                </td>
            </tr>
        </table>
        <asp:Label ID="lblPageCreationError" CssClass="NormalRed" runat="server"></asp:Label>
        <br />
        <asp:LinkButton ID="cmdSave" OnClick="CmdSave_Click" runat="server" CssClass="dnnPrimaryAction"></asp:LinkButton>
        <asp:LinkButton ID="cmdSaveAndContinue" OnClick="CmdSaveAndContinue_Click" runat="server" CssClass="dnnSecondaryAction"></asp:LinkButton>
        <asp:LinkButton ID="cmdCancel" runat="server" OnClick="CmdCancel_Click" CssClass="dnnSecondaryAction" CausesValidation="false"></asp:LinkButton>
        <asp:LinkButton ID="DeleteBtn" runat="server" OnClick="DeleteBtn_Click" CssClass="dnnSecondaryAction" CausesValidation="false"></asp:LinkButton>
        <asp:Label ID="DeleteLbl" runat="server"></asp:Label>
    </div>
</div>
<uc1:Messages ID="Messages" runat="server" />