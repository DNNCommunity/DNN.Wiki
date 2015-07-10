#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="Start.ascx.cs" company="DNN Corp®">
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
using System.Web;

namespace DotNetNuke.Wiki.Views
{
    /// <summary>
    /// Start Class based on WikiModuleBase
    /// </summary>
    public partial class Start : WikiModuleBase
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Start"/> class.
        /// </summary>
        public Start()
        {
            this.PreRender += this.Page_PreRender;
            this.Load += this.Page_Load;
            this.Init += this.Page_Init;
        }

        #endregion Constructor

        #region Properties

        ////protected System.Web.UI.WebControls.Button m_cmdHistory;
        ////protected PageRatings m_pageRating;
        ////protected Ratings m_ratings;

        #endregion Properties

        #region Events

        /// <summary>
        /// Handles the Click event of the AddCommentCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        protected void AddCommentCommand_Click(object sender, System.EventArgs e)
        {
            this.AddCommentPane.Visible = true;
            this.Comments2.Visible = false;
            this.AddCommentCommand.Visible = false;
            this.CommentsSec.IsExpanded = true;
        }

        /// <summary>
        /// Adds the comments form1_ post canceled.
        /// </summary>
        /// <param name="s">The arguments.</param>
        protected void AddCommentsForm1_PostCanceled(object s)
        {
            this.AddCommentPane.Visible = false;
            this.Comments2.Visible = true;
            this.AddCommentCommand.Visible = true;
        }

        /// <summary>
        /// Adds the comments form1_ post submitted.
        /// </summary>
        /// <param name="s">The arguments.</param>
        protected void AddCommentsForm1_PostSubmitted(object s)
        {
            this.AddCommentPane.Visible = false;
            this.Comments2.Visible = true;
            this.AddCommentCommand.Visible = true;
        }

        /// <summary>
        /// Handles the Initialize event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        private void Page_Init(object sender, System.EventArgs e)
        {
            this.AddCommentsForm1.PostCanceled += this.AddCommentsForm1_PostCanceled;
            this.AddCommentsForm1.PostSubmitted += this.AddCommentsForm1_PostSubmitted;
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event
        /// data.</param>
        public new void Page_Load(object sender, System.EventArgs e)
        {
            if (this.UserId == -1)
            {
                this.UserName = "Anonymous";
            }
            else
            {
                this.UserName = this.UserInfo.Username;
            }

            this.AddCommentsForm1.EnableNotify = WikiSettings.CommentNotifyUsers == true;

            if (Request.IsAuthenticated)
            {
                if (this.UserInfo.IsSuperUser | this.UserInfo.IsInRole(PortalSettings.AdministratorRoleName))
                {
                    this.IsAdmin = true;
                }
            }

            this.LoadLocalization();
            this.AddCommentPane.Visible = false;
            this.Comments2.Visible = true;
            this.AddCommentCommand.Visible = true;
            //// Me.DeleteCommentCommand.Visible = True

            this.lblPageTopic.Text = this.PageTopic.Replace(WikiModuleBase.WikiHomeName, Localization.GetString("Home", this.RouterResourceFile));

            if (!string.IsNullOrWhiteSpace(CurrentTopic.Title))
            {
                this.lblPageTopic.Text = CurrentTopic.Title;
            }

            this.AddCommentsForm1.ParentId = CurrentTopic.TopicID;
            this.CommentCount1.ParentId = CurrentTopic.TopicID;

            this.Comments2.IsAdmin = this.IsAdmin;
            this.Comments2.ParentId = CurrentTopic.TopicID;

            //// CommentsSec.IsExpanded = False
            this.DisplayTopic();
        }

        /// <summary>
        /// Handles the PreRender event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        private void Page_PreRender(object sender, System.EventArgs e)
        {
            if (this.m_ratings.HasVoted)
            {
                this.RatingSec.IsExpanded = false;
            }

            this.CommentCount1.Visible = false;
            //// CommentsSec.IsExpanded = False

            if (Request.IsAuthenticated)
            {
                this.AddCommentsForm1.NameText = UserInfo.DisplayName;
                this.AddCommentsForm1.EnableName = false;
                this.AddCommentsForm1.EmailText = UserInfo.Email;
                this.AddCommentsForm1.EnableEmail = false;

                ////Dim lstEmails As List(Of String) = New Entities.CommentsController().GetNotificationEmails(Me.Topic)
                ////Me.AddCommentsForm1.CommentText = "Total: " & lstEmails.Count & vbCrLf
                ////Me.AddCommentsForm1.CommentText = Me.AddCommentsForm1.CommentText & "-----------" & vbCrLf
                ////For Each s As String In lstEmails
                ////Me.AddCommentsForm1.CommentText = Me.AddCommentsForm1.CommentText & s & vbCrLf
                ////Next
            }
        }

        #endregion Events

        #region Methods

        /// <summary>
        /// Edits the page.
        /// </summary>
        private void EditPage()
        {
            ////Me.chkPageInEditMode.Checked = True
            this.DisplayTopic();
        }

        /// <summary>
        /// Displays the topic.
        /// </summary>
        private void DisplayTopic()
        {
            this.lblPageContent.Visible = true;
            string topicContent = ReadTopic();
            this.lblPageContent.Text = HttpUtility.HtmlDecode(topicContent).ToString();
            this.ratingTbl.Visible = CurrentTopic.AllowRatings && WikiSettings.AllowRatings;
            this.RatingSec.Visible = CurrentTopic.AllowRatings && WikiSettings.AllowRatings;
            this.m_pageRating.Visible = CurrentTopic.AllowRatings && WikiSettings.AllowRatings;
            this.m_ratings.Visible = CurrentTopic.AllowRatings;
            this.AddCommentCommand.Visible = CurrentTopic.AllowDiscussions && WikiSettings.AllowDiscussions;
            this.CommentCount1.Visible = false;
            this.Comments2.Visible = CurrentTopic.AllowDiscussions && WikiSettings.AllowDiscussions;
            this.CommentsSec.Visible = CurrentTopic.AllowDiscussions && WikiSettings.AllowDiscussions;
            this.CommentsTbl.Visible = CurrentTopic.AllowDiscussions && WikiSettings.AllowDiscussions;

            DotNetNuke.Framework.CDefault p = default(DotNetNuke.Framework.CDefault);
            p = (DotNetNuke.Framework.CDefault)this.Page;

            // Set the page title, check for the Topic.Title, Topic.Name, then use PageTopic
            // parameter if all else fails.
            if (!string.IsNullOrWhiteSpace(CurrentTopic.Title))
            {
                p.Title = p.Title + " > " + CurrentTopic.Title;
            }
            else if (!string.IsNullOrWhiteSpace(CurrentTopic.Name))
            {
                p.Title = p.Title + " > " + CurrentTopic.Name;
            }
            else
            {
                p.Title = p.Title + " > " + this.PageTopic;
            }

            if ((CurrentTopic.Description != null) & !string.IsNullOrWhiteSpace(CurrentTopic.Description))
            {
                p.Description = CurrentTopic.Description + " " + p.Description;
            }

            if ((CurrentTopic.Keywords != null) & !string.IsNullOrWhiteSpace(CurrentTopic.Keywords))
            {
                p.KeyWords = CurrentTopic.Keywords + " " + p.KeyWords;
            }
        }

        /// <summary>
        /// Loads the localization.
        /// </summary>
        private void LoadLocalization()
        {
            this.AddCommentCommand.Text = Localization.GetString("StartAddComment", this.RouterResourceFile);

            this.CommentCount1.Text = Localization.GetString("StartCommentCount", this.RouterResourceFile);
            this.CommentsSec.Text = Localization.GetString("StartCommentsSection", this.RouterResourceFile);

            this.PostCommentLbl.Text = Localization.GetString("StartPostComment", this.RouterResourceFile);

            this.RatingSec.Text = Localization.GetString("StartRatingSec.Text", this.RouterResourceFile);
        }

        #endregion Methods
    }
}