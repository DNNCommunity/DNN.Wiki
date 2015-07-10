#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="Edit.ascx.cs" company="DNN Corp®">
//      DNN Corp® - http://www.dnnsoftware.com Copyright (c) 2002-2013 by DNN Corp®
//
//      Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
//      associated documentation files (the "Software"), to deal in the Software without restriction,
//      including without limitation the rights to use, copy, modify, merge, publish, distribute,
//      sub license, and/or sell copies of the Software, and to permit persons to whom the Software is
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
////--------------------------------------------------------------------------------------------------------

#endregion Copyright

using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Utilities;
using DotNetNuke.Wiki.BusinessObjects.Exceptions;
using DotNetNuke.Wiki.BusinessObjects.Models;
using DotNetNuke.Wiki.Utilities;
using System.Web;

namespace DotNetNuke.Wiki.Views
{
    /// <summary>
    /// The Edit Class based on the Wiki Module Base
    /// </summary>
    public partial class Edit : WikiModuleBase
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Edit"/> class.
        /// </summary>
        public Edit()
        {
            this.PreRender += this.Page_PreRender;
            this.Load += this.Page_Load;
        }

        #endregion Constructor

        #region Events

        /// <summary>
        /// Handles the Click event of the Cancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        protected void CmdCancel_Click(object sender, System.EventArgs e)
        {
            this.CancelChanges();
        }

        /// <summary>
        /// Handles the Click event of the save control. Creates/updates a topic
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        protected void CmdSave_Click(object sender, System.EventArgs e)
        {
            // If we've change the Topic Name we need to create a new topic.
            Topic ti = null;
            ////if (string.IsNullOrWhiteSpace(PageTopic) | PageTopic != WikiMarkup.DecodeTitle(txtPageName.Text.Trim()))
            ////{
            ////    PageTopic = WikiMarkup.DecodeTitle(txtPageName.Text.Trim());
            ////    _Topic.TopicID = 0;
            ////    ti = TopicBo.GetByNameForModule(ModuleId, PageTopic);
            ////}

            this.PageTopic = WikiMarkup.DecodeTitle(this.txtPageName.Text.Trim());

            if (ti == null)
            {
                this.SaveChanges();
                if (this.PageTopic == WikiModuleBase.WikiHomeName)
                {
                    Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(this.TabId));
                }

                Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(this.TabId, this.PortalSettings, string.Empty, string.Empty, "topic=" + WikiMarkup.EncodeTitle(this.PageTopic)), false);
            }
            else
            {
                this.lblPageCreationError.Text = Localization.GetString("lblPageCreationError", this.LocalResourceFile);
            }
        }

        //// PageTopic = string.Empty LoadTopic() Me.EditPage()
        ////End Sub

        /// <summary>
        /// Handles the Click event of the Save And Continue control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        protected void CmdSaveAndContinue_Click(object sender, System.EventArgs e)
        {
            this.PageTopic = this.txtPageName.Text.Trim();
            this.SaveAndContinue();
            ////Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(Me.tabID, Me.portalSettings, String.Empty, "", "topic=" & WikiMarkup.EncodeTitle(Me.PageTopic)), False)
        }

        /// <summary>
        /// Handles the Click event of the Delete Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        protected void DeleteBtn_Click(object sender, System.EventArgs e)
        {
            try
            {
                UoW.BeginTransaction();

                TopicBo.Delete(this.CurrentTopic);

                UoW.CommitTransaction();
                Response.Redirect(this.HomeURL, false);
            }
            catch (System.Exception exc)
            {
                UoW.RollbackTransaction();

                Exceptions.LogException(exc);
                this.Messages.ShowError(Localization.GetString("ErrorDeletingTopic", this.LocalResourceFile));
            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event
        /// data.</param>
        public new void Page_Load(object sender, System.EventArgs e)
        {
            this.LoadLocalization();

            if (this.CanEdit)
            {
                if (this.teContent != null)
                {
                    this.teContent.HtmlEncode = false;
                }

                this.LoadTopic();
                if (string.IsNullOrWhiteSpace(CurrentTopic.Name))
                {
                    if (this.Request.QueryString["topic"] == null)
                    {
                        this.PageTopic = WikiHomeName.Replace("[L]", string.Empty);
                        CurrentTopic.Name = this.PageTopic;
                    }
                    else
                    {
                        this.PageTopic = WikiMarkup.DecodeTitle(this.Request.QueryString["topic"].ToString()).Replace("[L]", string.Empty);
                        CurrentTopic.Name = this.PageTopic;
                    }
                }

                this.EditPage();

                // CommentsSec.IsExpanded = FalseB
                if (this.Request.QueryString["add"] != null)
                {
                    this.PageTopic = string.Empty;
                    this.LoadTopic();
                    this.EditPage();
                }
                else
                {
                }

                // Add confirmation to the delete button.
                ClientAPI.AddButtonConfirm(this.DeleteBtn, Localization.GetString("ConfirmDelete", WikiModuleBase.SharedResources));
            }
            else
            {
                // User doesn't have edit rights to this module, load up a message stating so.
                this.lblMessage.Text = Localization.GetString("NoEditAccess", this.LocalResourceFile);
                this.divWikiEdit.Visible = false;
            }
        }

        /// <summary>
        /// Handles the PreRender event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        private void Page_PreRender(object sender, System.EventArgs e)
        {
        }

        ////Protected Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        ////    'TODO: none of this is currently working..... not sure why

        #endregion Events

        #region Methods

        /// <summary>
        /// Cancels the changes.
        /// </summary>
        private void CancelChanges()
        {
            // Send back to the Page View.
            if (string.IsNullOrEmpty(this.PageTopic))
            {
                Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(this.TabId), false);
            }
            else
            {
                Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(this.TabId, this.PortalSettings, string.Empty, string.Empty, "topic=" + WikiMarkup.EncodeTitle(this.PageTopic)), false);
            }
        }

        /// <summary>
        /// Displays the topic.
        /// </summary>
        private void DisplayTopic()
        {
            this.cmdSave.Visible = true;
            this.cmdSaveAndContinue.Visible = true;
            this.cmdCancel.Visible = true;
            this.teContent.Text = this.ReadTopicForEdit();

            if (this.WikiSettings.AllowDiscussions)
            {
                this.AllowDiscuss.Enabled = true;
                this.AllowDiscuss.Checked = this.CurrentTopic.AllowDiscussions || this.WikiSettings.DefaultDiscussionMode == true;
            }
            else
            {
                this.AllowDiscuss.Enabled = false;
                this.AllowDiscuss.Checked = false;
            }

            if (this.WikiSettings.AllowRatings)
            {
                this.AllowRating.Enabled = true;
                this.AllowRating.Checked = this.CurrentTopic.AllowRatings || this.WikiSettings.DefaultRatingMode == true;
            }
            else
            {
                this.AllowRating.Enabled = false;
                this.AllowRating.Checked = false;
            }

            this.DeleteBtn.Visible =
            this.DeleteLbl.Visible = this.CurrentTopic.Name != WikiModuleBase.WikiHomeName;

            if (string.IsNullOrWhiteSpace(this.CurrentTopic.Name))
            {
                this.txtPageName.Text = string.Empty;
                this.txtPageName.ReadOnly = false;
            }
            else
            {
                this.txtPageName.Text = HttpUtility.HtmlDecode(CurrentTopic.Name.Trim().Replace("[L]", string.Empty));
                this.txtPageName.ReadOnly = PageTopic.Equals(WikiModuleBase.WikiHomeName);
            }

            if (!string.IsNullOrWhiteSpace(this.CurrentTopic.Title))
            {
                this.txtTitle.Text = HttpUtility.HtmlDecode(CurrentTopic.Title.Replace("[L]", string.Empty));
            }

            this.txtDescription.Text = CurrentTopic.Description;

            this.txtKeywords.Text = CurrentTopic.Keywords;
        }

        /// <summary>
        /// Edits the page.
        /// </summary>
        private void EditPage()
        {
            // Redirect back to the topic URL.
            this.DisplayTopic();
        }

        /// <summary>
        /// Loads the localization.
        /// </summary>
        private void LoadLocalization()
        {
            this.AllowDiscuss.Text = Localization.GetString("StartAllowDiscuss", this.LocalResourceFile);
            this.AllowRating.Text = Localization.GetString("StartAllowRatings", this.LocalResourceFile);
            this.cmdCancel.Text = Localization.GetString("StartCancel", this.LocalResourceFile);
            ////CommentsSec.Text = Localization.GetString("StartCommentsSection", LocalResourceFile)
            this.DeleteBtn.Text = Localization.GetString("StartDelete", this.LocalResourceFile);
            this.cmdSave.Text = Localization.GetString("StartSave", this.LocalResourceFile);
            this.cmdSaveAndContinue.Text = Localization.GetString("StartSaveAndContinue", this.LocalResourceFile);
            this.WikiTextDirections.Text = Localization.GetString("StartWikiDirections", this.LocalResourceFile);
            this.WikiDirectionsDetails.Text = Localization.GetString("StartWikiDirectionDetails", this.LocalResourceFile);
            ////RatingSec.Text = Localization.GetString("StartRatingSec.Text", LocalResourceFile)
        }

        /// <summary>
        /// Saves the and continue.
        /// </summary>
        private void SaveAndContinue()
        {
            SharedEnum.CrudOperation crudOperation = SharedEnum.CrudOperation.Insert;
            try
            {
                DotNetNuke.Security.PortalSecurity objSec = new DotNetNuke.Security.PortalSecurity();
                this.SaveTopic(
                    HttpUtility.HtmlDecode(
                    objSec.InputFilter(objSec.InputFilter(this.teContent.Text, PortalSecurity.FilterFlag.NoMarkup), PortalSecurity.FilterFlag.NoScripting)),
                    this.AllowDiscuss.Checked,
                    this.AllowRating.Checked,
                    objSec.InputFilter(WikiMarkup.DecodeTitle(this.txtTitle.Text.Trim()), PortalSecurity.FilterFlag.NoMarkup),
                    objSec.InputFilter(this.txtDescription.Text.Trim(), PortalSecurity.FilterFlag.NoMarkup),
                    objSec.InputFilter(this.txtKeywords.Text.Trim(), PortalSecurity.FilterFlag.NoMarkup),
                    out crudOperation);
            }
            catch (TopicValidationException exc)
            {
                switch (exc.CrudError)
                {
                    case DotNetNuke.Wiki.BusinessObjects.TopicBO.TopicError.DUPLICATENAME:
                        this.Messages.ShowWarning(Localization.GetString("WarningDUPLICATENAME", this.LocalResourceFile));
                        break;

                    default:
                        throw exc;
                }
            }

            this.PostTopicToDNNJournal(crudOperation);
        }

        /// <summary>
        /// Posts the topic to DNN journal.
        /// </summary>
        /// <param name="crudOperation">The crud operation.</param>
        private void PostTopicToDNNJournal(SharedEnum.CrudOperation crudOperation)
        {
            string summary = string.Empty;
            SharedEnum.DNNJournalType journalType;

            // depending on the Crud operation, sets the summary and JournalType
            if (crudOperation == SharedEnum.CrudOperation.Insert)
            {
                summary =
                    Localization.GetString("JournalInsertTopicSummary", this.LocalResourceFile);
                journalType = SharedEnum.DNNJournalType.Wiki_Add;
            }
            else
            {
                summary =
                    Localization.GetString("JournalUpdateTopicSummary", this.LocalResourceFile);
                journalType = SharedEnum.DNNJournalType.Wiki_Update;
            }

            // post the topic
            DNNUtils.PostTopicCommentToJournal(
                summary.Replace("[TopicName]", this.PageTopic),
                this.PageTopic,
                string.Empty,
                DotNetNuke.Common.Globals.NavigateURL(this.TabId, this.PortalSettings, string.Empty, "topic=" + WikiMarkup.EncodeTitle(this.PageTopic)),
                this.TabId,
                this.PageTopic,
                journalType);
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        private void SaveChanges()
        {
            this.SaveAndContinue();
        }

        #endregion Methods
    }
}