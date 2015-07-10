#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="Administration.ascx.cs" company="DNN Corp®">
//      DNN Corp® - http://www.dnnsoftware.com Copyright (c) 2002-2013 by DNN Corp®
//
//      Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
//      associated documentation files (the "Software"), to deal in the Software without restriction,
//      including without limitation the rights to use, copy, modify, merge, publish, distribute,
//      sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
//      furnished to do so, subject to the following conditions:
//
//      The above copyright notice and this permission notice shall be included in all copies or
//      substantial portions of the Software.
//
//      THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
//      NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
//      NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
//      DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//      OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
////------------------------------------------------------------------------------------------------------

#endregion Copyright

using DotNetNuke.Entities.Modules;
using DotNetNuke.Security.Roles;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Wiki.BusinessObjects;
using DotNetNuke.Wiki.BusinessObjects.Models;
using System;
using System.Collections;
using System.Linq;
using System.Web.UI.WebControls;

namespace DotNetNuke.Wiki.Views
{
    /// <summary>
    /// Administration Partial Class
    /// </summary>
    public partial class Administration : PortalModuleBase
    {
        #region Variables

        private const string StrUseDNNSettings = "UseDNNSettings";

        private Setting mSettingsModel; ////Data from the WikiSettings Business Object

        #endregion Variables

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Administration"/> class.
        /// </summary>
        public Administration()
        {
            this.Load += this.CtrlPage_Load;
            this.Init += this.Page_Init;
        }

        #endregion Constructor

        #region Events

        /// <summary>
        /// Handles the CheckedChanged event of the AllowPageRatings control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        protected void AllowPageRatings_CheckedChanged(object sender, EventArgs e)
        {
            if (this.AllowPageComments.Checked)
            {
                this.ActivateRatings.Enabled = true;
                this.ActivateRatings.Checked = true;

                this.DefaultRatingMode.Enabled = true;
            }
            else
            {
                this.ActivateRatings.Enabled = false;
                this.ActivateRatings.Checked = false;

                this.DefaultRatingMode.Enabled = false;
                this.DefaultRatingMode.Checked = false;
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the AllowPageComments control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        protected void AllowPageComments_CheckedChanged(object sender, System.EventArgs e)
        {
            if (this.AllowPageComments.Checked)
            {
                this.ActivateComments.Enabled = true;
                this.ActivateComments.Checked = true;

                this.DefaultCommentsMode.Enabled = true;
            }
            else
            {
                this.ActivateComments.Enabled = false;
                this.ActivateComments.Checked = false;

                this.DefaultCommentsMode.Enabled = false;
                this.DefaultCommentsMode.Checked = false;
            }
        }

        /// <summary>
        /// Handles the Click event of the CancelButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(), true);
        }

        /// <summary>
        /// Handles the Load event of the CtrlPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event
        /// data.</param>
        private void CtrlPage_Load(object sender, System.EventArgs e)
        {
            try
            {
                using (UnitOfWork currentUnitOfWork = new UnitOfWork())
                {
                    var settingsBo = new SettingBO(currentUnitOfWork);

                    ////Put user code to initialize the page here

                    this.ContentEditors.DataTextField = "Text";
                    this.ContentEditors.DataValueField = "Text";
                    this.NotifyRoles.DataTextField = "Text";
                    this.NotifyRoles.DataValueField = "Text";

                    if (this.mSettingsModel == null)
                    {
                        this.mSettingsModel = settingsBo.GetByModuleID(this.ModuleId);
                        if (this.mSettingsModel == null)
                        {
                            this.mSettingsModel = new Setting();
                            this.mSettingsModel.ModuleId = -1;
                            this.mSettingsModel.ContentEditorRoles = StrUseDNNSettings;
                        }
                    }

                    if (!this.IsPostBack)
                    {
                        this.DNNSecurityChk.Checked = this.mSettingsModel.ContentEditorRoles.Equals(StrUseDNNSettings);
                        this.AllowPageComments.Checked = this.mSettingsModel.AllowDiscussions;
                        this.AllowPageRatings.Checked = this.mSettingsModel.AllowRatings;
                        this.DefaultCommentsMode.Checked = this.mSettingsModel.DefaultDiscussionMode == true;
                        this.DefaultRatingMode.Checked = this.mSettingsModel.DefaultRatingMode == true;
                        this.NotifyMethodUserComments.Checked = this.mSettingsModel.CommentNotifyUsers == true;

                        this.NotifyMethodCustomRoles.Checked =
                            !(!string.IsNullOrWhiteSpace(this.mSettingsModel.CommentNotifyRoles) &&
                            this.mSettingsModel.CommentNotifyRoles.StartsWith("UseDNNSettings;") &&
                            !string.IsNullOrWhiteSpace(this.mSettingsModel.CommentNotifyRoles));

                        if (this.NotifyMethodCustomRoles.Checked && !string.IsNullOrWhiteSpace(this.mSettingsModel.CommentNotifyRoles))
                        {
                            this.NotifyMethodEditRoles.Checked = this.mSettingsModel.CommentNotifyRoles.Contains(";Edit");
                            this.NotifyMethodViewRoles.Checked = this.mSettingsModel.CommentNotifyRoles.Contains(";View");
                        }

                        // Call the BindRights method
                        this.BindRights();

                        if (this.DNNSecurityChk.Checked == true)
                        {
                            this.ContentEditors.Visible = false;
                            this.WikiSecurity.Visible = false;
                        }
                        else
                        {
                            this.ContentEditors.Visible = true;
                            this.WikiSecurity.Visible = true;
                        }

                        if (this.AllowPageComments.Checked)
                        {
                            this.ActivateComments.Enabled = true;
                            this.DefaultCommentsMode.Enabled = true;
                        }
                        else
                        {
                            this.ActivateComments.Enabled = false;
                            this.ActivateComments.Checked = false;
                            this.DefaultCommentsMode.Enabled = false;
                            this.DefaultCommentsMode.Checked = false;
                        }

                        if (this.AllowPageRatings.Checked)
                        {
                            this.ActivateRatings.Enabled = true;
                            this.DefaultRatingMode.Enabled = true;
                        }
                        else
                        {
                            this.ActivateRatings.Enabled = false;
                            this.ActivateRatings.Checked = false;
                            this.DefaultRatingMode.Enabled = false;
                            this.DefaultRatingMode.Checked = false;
                        }

                        if (this.NotifyMethodCustomRoles.Checked)
                        {
                            this.NotifyRoles.Visible = true;
                            this.lblNotifyRoles.Visible = true;
                            this.NotifyMethodEditRoles.Enabled = false;
                            this.NotifyMethodViewRoles.Enabled = false;
                            this.NotifyMethodViewRoles.Checked = false;
                            this.NotifyMethodEditRoles.Checked = false;
                        }
                        else
                        {
                            this.NotifyMethodEditRoles.Enabled = true;
                            this.NotifyMethodViewRoles.Enabled = true;

                            this.NotifyRoles.Visible = false;
                            this.lblNotifyRoles.Visible = false;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the DNN Security Check control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        protected void DNNSecurityChk_CheckedChanged(object sender, System.EventArgs e)
        {
            if (this.DNNSecurityChk.Checked == true)
            {
                this.ContentEditors.Visible = false;
                this.WikiSecurity.Visible = false;
            }
            else
            {
                this.ContentEditors.Visible = true;
                this.WikiSecurity.Visible = true;
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the NotifyMethodCustomRoles control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        protected void NotifyMethodCustomRoles_CheckedChanged(object sender, System.EventArgs e)
        {
            if (this.NotifyMethodCustomRoles.Checked)
            {
                this.NotifyRoles.Visible = true;
                this.lblNotifyRoles.Visible = true;

                this.NotifyMethodEditRoles.Enabled = false;
                this.NotifyMethodViewRoles.Enabled = false;
                this.NotifyMethodViewRoles.Checked = false;
                this.NotifyMethodEditRoles.Checked = false;
            }
            else
            {
                this.NotifyMethodEditRoles.Enabled = true;
                this.NotifyMethodViewRoles.Enabled = true;
                this.lblNotifyRoles.Visible = false;
                this.NotifyRoles.Visible = false;
            }
        }

        /// <summary>
        /// Handles the Initialize event of the Page control.
        /// NOTE: The following placeholder declaration is required by the Web Form Designer.
        /// Do not delete or move it.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        private void Page_Init(object sender, System.EventArgs e)
        {
            Framework.jQuery.RequestUIRegistration();
            Framework.jQuery.RequestDnnPluginsRegistration();
        }

        /// <summary>
        /// Handles the Click event of the SaveButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            this.SaveSettings();
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(), true);
        }

        #endregion Events

        #region Methods

        /// <summary>
        /// Activates the items.
        /// </summary>
        /// <param name="currentUnitOfWork">The UnitOfWork.</param>
        private void ActivateItems(UnitOfWork currentUnitOfWork)
        {
            if (this.ActivateComments.Checked | this.ActivateRatings.Checked)
            {
                TopicBO topicBo = new TopicBO(currentUnitOfWork);

                var alltopics = topicBo.GetAllByModuleID(this.ModuleId);

                foreach (var topic in alltopics)
                {
                    if (topic.AllowDiscussions == false & this.ActivateComments.Checked)
                    {
                        topic.AllowDiscussions = true;
                    }

                    if (topic.AllowRatings == false & this.ActivateRatings.Checked)
                    {
                        topic.AllowRatings = true;
                    }

                    topicBo.Update(topic);
                }
            }
        }

        /// <summary>
        /// Gets the edit rights for user roles and binds them to the respective list control
        /// </summary>
        private void BindRights()
        {
            // declare variables
            ArrayList arrAvailableAuthViewRoles = new ArrayList();
            ArrayList arrAvailableNotifyRoles = new ArrayList();
            ArrayList arrAssignedAuthViewRoles = new ArrayList();
            ArrayList arrAssignedNotifyRoles = new ArrayList();
            Array arrAuthViewRoles = null;
            Array arrAuthNotifyRoles = null;

            // add an entry of All Users for the View roles
            arrAvailableAuthViewRoles.Add(new ListItem("All Users", DotNetNuke.Common.Globals.glbRoleAllUsersName));

            // add an entry of Unauthenticated Users for the View roles
            arrAvailableAuthViewRoles.Add(new ListItem("Unauthenticated Users", DotNetNuke.Common.Globals.glbRoleUnauthUserName));

            // process portal roles
            DotNetNuke.Security.Roles.RoleController objRoles = new DotNetNuke.Security.Roles.RoleController();

            var arrRoles = objRoles.GetPortalRoles(PortalId).OfType<RoleInfo>();
            foreach (var objRole in arrRoles)
            {
                arrAvailableAuthViewRoles.Add(new ListItem(objRole.RoleName, objRole.RoleName));
                arrAvailableNotifyRoles.Add(new ListItem(objRole.RoleName, objRole.RoleName));
            }

            // populate view roles
            if (this.mSettingsModel.ContentEditorRoles.Equals(StrUseDNNSettings))
            {
                arrAuthViewRoles = this.mSettingsModel.ContentEditorRoles.Split(new string[] { StrUseDNNSettings }, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                arrAuthViewRoles = this.mSettingsModel.ContentEditorRoles.Split(
                    new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[0]
                    .Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            }

            // populate the notify roles
            if (!string.IsNullOrWhiteSpace(this.mSettingsModel.CommentNotifyRoles))
            {
                if (this.mSettingsModel.CommentNotifyRoles.StartsWith("UseDNNSettings;"))
                {
                    this.mSettingsModel.CommentNotifyRoles = this.mSettingsModel.CommentNotifyRoles.Replace("UseDNNSettings;", string.Empty);
                    arrAuthNotifyRoles = this.mSettingsModel.CommentNotifyRoles.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string curRole in arrAuthNotifyRoles)
                    {
                        if (curRole.Equals("View"))
                        {
                            this.NotifyMethodViewRoles.Checked = true;
                        }
                        else if (curRole.Equals("Edit"))
                        {
                            this.NotifyMethodEditRoles.Checked = true;
                        }
                    }
                }
                else
                {
                    arrAuthNotifyRoles = this.mSettingsModel.CommentNotifyRoles.Split(new char[] { '|' })[0].Split(new char[] { ';' });
                }
            }

            if (arrAuthViewRoles != null)
            {
                foreach (string strRole in arrAuthViewRoles)
                {
                    if (!string.IsNullOrEmpty(strRole))
                    {
                        foreach (ListItem objListItem in arrAvailableAuthViewRoles)
                        {
                            if (objListItem.Value == strRole)
                            {
                                arrAssignedAuthViewRoles.Add(objListItem);
                                arrAvailableAuthViewRoles.Remove(objListItem);
                                break;
                            }
                        }
                    }
                }
            }

            if (arrAuthNotifyRoles != null)
            {
                foreach (string strRole in arrAuthNotifyRoles)
                {
                    if (!string.IsNullOrEmpty(strRole))
                    {
                        foreach (ListItem objListItem in arrAvailableNotifyRoles)
                        {
                            if (objListItem.Value == strRole)
                            {
                                arrAssignedNotifyRoles.Add(objListItem);
                                arrAvailableNotifyRoles.Remove(objListItem);
                                break;
                            }
                        }
                    }
                }
            }

            int x = arrAvailableAuthViewRoles.Count; // TODO Do we need this?
            int y = arrAssignedAuthViewRoles.Count; // TODO Do we need this?
            this.ContentEditors.Available = arrAvailableAuthViewRoles;
            this.ContentEditors.Assigned = arrAssignedAuthViewRoles;

            this.NotifyRoles.Assigned = arrAssignedNotifyRoles;
            this.NotifyRoles.Available = arrAvailableNotifyRoles;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        private void SaveSettings()
        {
            using (UnitOfWork currentUnitOfWork = new UnitOfWork())
            {
                var settingsBo = new SettingBO(currentUnitOfWork);
                if (this.DNNSecurityChk.Checked == true)
                {
                    this.mSettingsModel.ContentEditorRoles = StrUseDNNSettings;
                }
                else
                {
                    string list = ";";
                    foreach (ListItem li in this.ContentEditors.Assigned)
                    {
                        list = list + li.Value + ";";
                    }

                    this.mSettingsModel.ContentEditorRoles = list;
                }

                if (this.NotifyMethodCustomRoles.Checked == false)
                {
                    this.mSettingsModel.CommentNotifyRoles = StrUseDNNSettings;
                    if (this.NotifyMethodEditRoles.Checked == true)
                    {
                        this.mSettingsModel.CommentNotifyRoles = this.mSettingsModel.CommentNotifyRoles + ";Edit";
                    }

                    if (this.NotifyMethodViewRoles.Checked == true)
                    {
                        this.mSettingsModel.CommentNotifyRoles = this.mSettingsModel.CommentNotifyRoles + ";View";
                    }
                }
                else
                {
                    string list = ";";
                    foreach (ListItem li in this.NotifyRoles.Assigned)
                    {
                        list = list + li.Value + ";";
                    }

                    this.mSettingsModel.CommentNotifyRoles = list;
                }

                this.mSettingsModel.AllowDiscussions = this.AllowPageComments.Checked;
                this.mSettingsModel.AllowRatings = this.AllowPageRatings.Checked;
                this.mSettingsModel.DefaultDiscussionMode = this.DefaultCommentsMode.Checked;
                this.mSettingsModel.DefaultRatingMode = this.DefaultRatingMode.Checked;
                this.mSettingsModel.CommentNotifyUsers = this.NotifyMethodUserComments.Checked;

                if (this.mSettingsModel.ModuleId == -1)
                {
                    this.mSettingsModel.ModuleId = this.ModuleId;
                    settingsBo.Add(this.mSettingsModel);
                }
                else
                {
                    settingsBo.Update(this.mSettingsModel);
                }

                this.ActivateItems(currentUnitOfWork);
            }
        }

        ///// <summary> Binds role controls. </summary> <param name="roleType">Type of the
        ///// role.</param>
        // private void zzBindRoleControls(string roleType)
        // {
        //    // declare variables
        //    string roles = string.Empty;
        //    ListItem[] arrAuthRoles = null;
        //    ArrayList arrAssignedRoles = new ArrayList();
        //    ArrayList arrAvailableRoles = new ArrayList();

        // if (roleType == "ContentEditors") { roles = this.mSettingsModel.ContentEditorRoles; }
        // else if (roleType == "CommentNotifyRoles") { roles =
        // this.mSettingsModel.CommentNotifyRoles; } else { // TODO We've got problems if its not
        // "ContentEditors" or "CommentNotifyRoles" TODO // Need to raise an error }

        // if (roles != StrUseDNNSettings) { arrAuthRoles = roles.Split( new char[] { '|' },
        // StringSplitOptions.RemoveEmptyEntries)[0] .Split(new char[] { ';' },
        // StringSplitOptions.RemoveEmptyEntries).Select(p => new ListItem(p, p)).ToArray();

        // // Convert the arrAuthRoles array to an array list
        // arrAssignedRoles.AddRange(arrAuthRoles); }

        // if (roleType == "CommentNotifyRoles") { foreach (ListItem curRole in arrAssignedRoles) {
        // if (curRole.Value == string.Empty) { arrAvailableRoles.Remove(curRole); } } }

        // // Call the BuildAllRolesArray method to Build an Array of All Roles ---------------- //
        // Available Roles = All Roles - Assigned Roles AvailableRoles(arrAssignedRoles);

        // // Build Available Roles add an entry of All Users for the View roles
        // arrAvailableRoles.Add(new ListItem("All Users",
        // DotNetNuke.Common.Globals.glbRoleAllUsersName));

        // // add an entry of Unauthenticated Users for the View roles arrAvailableRoles.Add(new
        // ListItem("Unauthenticated Users", DotNetNuke.Common.Globals.glbRoleUnauthUserName));

        // // process portal roles DotNetNuke.Security.Roles.RoleController objRoles = new
        // DotNetNuke.Security.Roles.RoleController();

        // var arrRoles = objRoles.GetPortalRoles(PortalId).OfType<RoleInfo>(); foreach (var objRole
        // in arrRoles) { arrAvailableRoles.Add(new ListItem(objRole.RoleName, objRole.RoleName)); }

        // // Remove the Assigned Roles from the Available Roles if (arrAvailableRoles.Count > 0) {
        // foreach (ListItem curRole in arrAssignedRoles) { arrAvailableRoles.Remove(curRole); } }

        // if (roleType == "ContentEditors") { ContentEditors.Assigned = arrAssignedRoles;
        // ContentEditors.Available = arrAvailableRoles; } else if (roleType ==
        // "CommentNotifyRoles") { NotifyRoles.Assigned = arrAssignedRoles; NotifyRoles.Available =
        // arrAvailableRoles; } else { // TODO We've got problems if its not "ContentEditors" or
        // "CommentNotifyRoles" TODO // Need to raise an error } }

        ///// <summary> Get the available roles. </summary> <param name="arrAssignedRoles">An array
        ///// of assigned roles.</param> <returns>ArrayList of Available Roles</returns>
        // private ArrayList zzAvailableRoles(ArrayList arrAssignedRoles)
        // {
        //    // Declare Variables
        //    ArrayList arrAvailableRoles = new ArrayList();

        // // add an entry of All Users for the View roles arrAvailableRoles.Add(new ListItem("All
        // Users", DotNetNuke.Common.Globals.glbRoleAllUsersName));

        // // add an entry of Unauthenticated Users for the View roles arrAvailableRoles.Add(new
        // ListItem("Unauthenticated Users", DotNetNuke.Common.Globals.glbRoleUnauthUserName));

        // // process portal roles DotNetNuke.Security.Roles.RoleController objRoles = new
        // DotNetNuke.Security.Roles.RoleController();

        // var arrRoles = objRoles.GetPortalRoles(PortalId).OfType<RoleInfo>(); foreach (var objRole
        // in arrRoles) { arrAvailableRoles.Add(new ListItem(objRole.RoleName, objRole.RoleName)); }

        // // Remove the Assigned Roles from the Available Roles if (arrAvailableRoles.Count > 0) {
        // foreach (ListItem curRole in arrAssignedRoles) { arrAvailableRoles.Remove(curRole); } }

        //// return arrAvailableRoles; }

        #endregion Methods
    }
}