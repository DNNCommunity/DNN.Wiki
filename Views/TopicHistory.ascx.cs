#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="TopicHistory.ascx.cs" company="DNN Corp®">
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
////--------------------------------------------------------------------------------------------------------

#endregion Copyright

using DotNetNuke.Services.Localization;
using DotNetNuke.Wiki.Utilities;
using System;
using System.Globalization;

namespace DotNetNuke.Wiki.Views
{
    /// <summary>
    /// Topic History Control Class
    /// </summary>
    public partial class TopicHistory : WikiModuleBase
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TopicHistory"/> class.
        /// </summary>
        public TopicHistory()
        {
            this.Load += this.Page_Load;
        }

        #endregion Constructor

        #region Events

        /// <summary>
        /// Handles the Click event of the Restore control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        protected void CmdRestore_Click(object sender, System.EventArgs e)
        {
            if (this.Request.QueryString["ShowHistory"] != null)
            {
                string historyPK = this.Request.QueryString["ShowHistory"];
                var topicHistoryItem = TopicHistoryBo.GetItem(int.Parse(historyPK));
                topicHistoryItem.TabID = this.TabId;
                topicHistoryItem.PortalSettings = this.PortalSettings;
                var topicHistoryBO = new DotNetNuke.Wiki.BusinessObjects.Models.TopicHistory();
                topicHistoryBO.TabID = this.TabId;
                topicHistoryBO.PortalSettings = this.PortalSettings;
                topicHistoryBO.Content = CurrentTopic.Content;
                topicHistoryBO.TopicId = this.TopicId;
                topicHistoryBO.UpdatedBy = CurrentTopic.UpdatedBy;
                topicHistoryBO.UpdateDate = CurrentTopic.UpdateDate;
                topicHistoryBO.Name = this.PageTopic;
                topicHistoryBO.Title = CurrentTopic.Title;
                topicHistoryBO.UpdatedByUserID = CurrentTopic.UpdatedByUserID;

                CurrentTopic.Content = topicHistoryItem.Content;
                CurrentTopic.Name = topicHistoryItem.Name;
                CurrentTopic.Title = topicHistoryItem.Title;
                CurrentTopic.Keywords = topicHistoryItem.Keywords;
                CurrentTopic.Description = topicHistoryItem.Description;
                CurrentTopic.UpdatedBy = UserInfo.Username;
                CurrentTopic.UpdateDate = DateTime.Now;
                CurrentTopic.UpdatedByUserID = this.UserId;
                TopicBo.Update(this.CurrentTopic);
                TopicHistoryBo.Add(topicHistoryBO);

                Response.Redirect(this.HomeURL, true);
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

            if (!this.IsPostBack)
            {
                this.RestoreLbl.Visible = false;
                this.cmdRestore.Visible = false;
                if (this.Request.QueryString["ShowHistory"] != null)
                {
                    this.ShowOldVersion();
                }
                else
                {
                    this.ShowTopicHistoryList();
                }
            }
        }

        #endregion Events

        #region Methods

        /// <summary>
        /// Loads the localization.
        /// </summary>
        private void LoadLocalization()
        {
            this.Label1.Text = Localization.GetString("HistoryTitle", this.RouterResourceFile);
            this.BackBtn.Text = Localization.GetString("HistoryBack", this.RouterResourceFile);
            this.cmdRestore.Text = Localization.GetString("HistoryRestore", this.RouterResourceFile);
            this.RestoreLbl.Text = Localization.GetString("HistoryRestoreNotice", this.RouterResourceFile);
        }

        /// <summary>
        /// Shows the old version.
        /// </summary>
        private void ShowOldVersion()
        {
            if (this.CanEdit)
            {
                this.RestoreLbl.Visible = true;
                this.cmdRestore.Visible = true;
            }

            string historyPK = null;
            historyPK = this.Request.QueryString["ShowHistory"];
            var topicHistory = TopicHistoryBo.GetItem(int.Parse(historyPK));
            this.lblPageTopic.Text = PageTopic.Replace(WikiModuleBase.WikiHomeName, "Home");
            this.lblPageContent.Text = topicHistory.Cache;
            this.lblDateTime.Text = string.Format(Localization.GetString("HistoryAsOf", this.RouterResourceFile), topicHistory.UpdateDate.ToString(CultureInfo.CurrentCulture));
            this.BackBtn.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(
                this.TabId,
                this.PortalSettings,
                string.Empty,
                "loc=TopicHistory",
                "topic=" + WikiMarkup.EncodeTitle(this.PageTopic));
        }

        /// <summary>
        /// Shows the topic history list.
        /// </summary>
        private void ShowTopicHistoryList()
        {
            this.lblPageTopic.Text = PageTopic.Replace(WikiModuleBase.WikiHomeName, "Home");

            this.lblDateTime.Text = "...";
            this.lblPageContent.Text = Localization.GetString("HistoryListHeader", this.RouterResourceFile) + " <br /> " + this.CreateHistoryTable();
            this.BackBtn.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(
                this.TabId,
                this.PortalSettings,
                string.Empty,
                "topic=" + WikiMarkup.EncodeTitle(this.PageTopic));
        }

        #endregion Methods
    }
}