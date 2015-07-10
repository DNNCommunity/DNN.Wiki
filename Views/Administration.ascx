<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Administration.ascx.cs" Inherits="DotNetNuke.Wiki.Views.Administration" %>

<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="DualListControl" Src="~/controls/DualListControl.ascx" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>
<%@ Register TagPrefix="dnnweb" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<div class="dnnForm dnnWikiSettings dnnClear" id="dnnWikiSettings">
    <div class="dnnFormExpandContent">
        <a href="#">
            <%=LocalizeString("ExpandAll")%></a>
    </div>
    <h2 id="dnnSitePanel-SecuritySettings" class="dnnFormSectionHead">
        <a href="#" class="dnnSectionExpanded">
            <%=LocalizeString("SecuritySettings")%></a></h2>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label ID="SecuritySettings" runat="server" resourcekey="DNNSecurityChk" />
            <asp:CheckBox ID="DNNSecurityChk" runat="server" Checked="True" OnCheckedChanged="DNNSecurityChk_CheckedChanged" AutoPostBack="True"></asp:CheckBox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="WikiSecurity" runat="server" resourcekey="WikiSecurity" />
            <uc1:DualListControl ID="ContentEditors" runat="server" ListBoxWidth="170" ListBoxHeight="130"></uc1:DualListControl>
        </div>
    </fieldset>
    <h2 id="dnnSitePanel-NotificationSettings" class="dnnFormSectionHead">
        <a href="#">
            <%=Localization.GetString("NotificationSettings", LocalResourceFile)%></a></h2>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label ID="lblNotifyMethodEditRoles" runat="server" />
            <asp:CheckBox ID="NotifyMethodEditRoles" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblNotifyMethodViewRoles" runat="server" />
            <asp:CheckBox ID="NotifyMethodViewRoles" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblNotifyMethodCustomRoles" runat="server" />
            <asp:CheckBox ID="NotifyMethodCustomRoles" runat="server" OnCheckedChanged="NotifyMethodCustomRoles_CheckedChanged" AutoPostBack="True" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblNotifyRoles" runat="server" />
            <uc1:DualListControl ID="NotifyRoles" runat="server" ListBoxWidth="170" ListBoxHeight="130">
            </uc1:DualListControl>
        </div>
    </fieldset>
    <h2 id="dnnSitePanel-CommentSettings" class="dnnFormSectionHead">
        <a href="">
            <%=LocalizeString("CommentSettings")%></a></h2>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label ID="lblAllowPageComments" runat="server" />
            <asp:CheckBox ID="AllowPageComments" runat="server" OnCheckedChanged="AllowPageComments_CheckedChanged" AutoPostBack="True" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblActivateComments" runat="server" />
            <asp:CheckBox ID="ActivateComments" runat="server" Enabled="False" AutoPostBack="True" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblDefaultCommentsMode" runat="server" />
            <asp:CheckBox ID="DefaultCommentsMode" runat="server" Enabled="false" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblNotifyMethodUserComments" runat="server" />
            <asp:CheckBox ID="NotifyMethodUserComments" runat="server" />
        </div>
    </fieldset>
    <h2 id="dnnSitePanel-RatingSettings" class="dnnFormSectionHead">
        <a href="">
            <%=LocalizeString("RatingSettings")%></a></h2>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label ID="lblAllowPageRatings" runat="server" />
            <asp:CheckBox ID="AllowPageRatings" OnCheckedChanged="AllowPageRatings_CheckedChanged" AutoPostBack="True" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblActivateRatings" runat="server" />
            <asp:CheckBox ID="ActivateRatings" runat="server" Enabled="False" AutoPostBack="True" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblDefaultRatingMode" runat="server" />
            <asp:CheckBox ID="DefaultRatingMode" runat="server" Enabled="false" />
        </div>
    </fieldset>
</div>
<br />
<asp:LinkButton ID="SaveButton" resourcekey="SaveButton" OnClick="SaveButton_Click" runat="server" CssClass="dnnPrimaryAction" />
<asp:LinkButton ID="CancelButton" resourcekey="CancelButton" OnClick="CancelButton_Click" runat="server" CssClass="dnnSecondaryAction" />
<script language="javascript" type="text/javascript">
    /*globals jQuery, window, Sys */
    (function ($, Sys) {
        function setupWikiSettings() {
            $('#dnnWikiSettings').dnnPanels();
            $('#dnnWikiSettings .dnnFormExpandContent a').dnnExpandAll({ expandText: '<%=Localization.GetString("ExpandAll", LocalResourceFile)%>', collapseText: '<%=Localization.GetString("CollapseAll", LocalResourceFile)%>', targetArea: '#dnnWikiSettings' });
        }

        $(document).ready(function () {
            setupWikiSettings();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setupWikiSettings();
            });
        });

    }(jQuery, window.Sys));
</script>