#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="WikiModuleBase.cs" company="DNN Corp®">
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
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Wiki.BusinessObjects;
using DotNetNuke.Wiki.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;

namespace DotNetNuke.Wiki.Utilities
{
    /// <summary>
    /// The Wiki Module Base Class
    /// </summary>
    public class WikiModuleBase : PortalModuleBase
    {
        #region Variables

        public const string SharedResources = "/DesktopModules/Wiki/Views/App_LocalResources/SharedResources.resx";

        public const string WikiHomeName = "WikiHomePage";

        private const string CSSWikiModuleCssId = "WikiModuleCss";
        private const string CSSWikiModuleCssPath = "/Resources/Css/module.css";

        private string mUserNameValue;
        private string mFirstNameValue;
        private string mLastNameValue;
        private bool mIsAdminValue = false;
        private string mPageTopicValue;
        private int mTopicIdValue;
        private Topic mTopicObject;
        private string mHomeUrlValue;
        private Setting mWikiSettingsObject;
        private bool mCanEditValue = false;

        private UnitOfWork mUnitOfWorkObject;
        private TopicBO mTopicBoObject;
        private TopicHistoryBO mTopicHistoryBoObject;

        #endregion Variables

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="WikiModuleBase"/> class.
        /// </summary>
        public WikiModuleBase()
        {
            this.Load += this.Page_Load;
            this.Unload += this.Page_Unload;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Gets the DNN wiki module root path.
        /// </summary>
        /// <value>The DNN wiki module root path.</value>
        public string DNNWikiModuleRootPath
        {
            get
            {
                if (this.TemplateSourceDirectory.EndsWith(@"/Views"))
                {
                    return this.TemplateSourceDirectory.Substring(0, this.TemplateSourceDirectory.IndexOf(@"/Views"));
                }
                else if (this.TemplateSourceDirectory.IndexOf(@"/Views/") > 0)
                {
                    return this.TemplateSourceDirectory.Substring(0, this.TemplateSourceDirectory.IndexOf(@"/Views/"));
                }

                return this.TemplateSourceDirectory;
            }
        }

        /// <summary>
        /// Gets or sets the wiki settings.
        /// </summary>
        /// <value>The wiki settings.</value>
        public Setting WikiSettings
        {
            get { return this.mWikiSettingsObject; }
            set { this.mWikiSettingsObject = value; }
        }

        /// <summary>
        /// Gets or sets the page topic.
        /// </summary>
        /// <value>The page topic.</value>
        public string PageTopic
        {
            get { return this.mPageTopicValue; }
            set { this.mPageTopicValue = value; }
        }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        public string LastName
        {
            get { return this.mLastNameValue; }
            set { this.mLastNameValue = value; }
        }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        public string FirstName
        {
            get { return this.mFirstNameValue; }
            set { this.mFirstNameValue = value; }
        }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName
        {
            get { return this.mUserNameValue; }
            set { this.mUserNameValue = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [is admin].
        /// </summary>
        /// <value><c>true</c> if [is admin]; otherwise, /c>.</value>
        public bool IsAdmin
        {
            get { return this.mIsAdminValue; }
            set { this.mIsAdminValue = value; }
        }

        /// <summary>
        /// Gets the home URL.
        /// </summary>
        /// <value>The home URL.</value>
        public string HomeURL
        {
            get { return this.mHomeUrlValue; }
        }

        /// <summary>
        /// Gets the topic unique identifier.
        /// </summary>
        /// <value>The topic unique identifier.</value>
        public int TopicId
        {
            get { return this.mTopicIdValue; }
        }

        /// <summary>
        /// Gets a value indicating whether [can edit].
        /// </summary>
        /// <value><c>true</c> if [can edit]; otherwise, /c>.</value>
        public bool CanEdit
        {
            get { return this.mCanEditValue; }
        }

        /// <summary>
        /// Gets the _ topic.
        /// </summary>
        /// <value>The _ topic.</value>
        public Topic CurrentTopic
        {
            get { return this.mTopicObject; }
        }

        /// <summary>
        /// Gets the router resource file.
        /// </summary>
        /// <value>The router resource file.</value>
        public string RouterResourceFile
        {
            get { return DotNetNuke.Services.Localization.Localization.GetResourceFile(this, "Router.ascx.resx"); }
        }

        /// <summary>
        /// Gets an instance of the topic history business object
        /// </summary>
        internal TopicHistoryBO TopicHistoryBo
        {
            get
            {
                if (this.mTopicHistoryBoObject == null)
                {
                    this.mTopicHistoryBoObject = new TopicHistoryBO(this.UoW);
                }

                return this.mTopicHistoryBoObject;
            }
        }

        /// <summary>
        /// Gets an instance of the topic business object
        /// </summary>
        internal TopicBO TopicBo
        {
            get
            {
                if (this.mTopicBoObject == null)
                {
                    this.mTopicBoObject = new TopicBO(this.UoW);
                }

                return this.mTopicBoObject;
            }
        }

        /// <summary>
        /// Gets an instance of the unit of work object
        /// </summary>
        internal UnitOfWork UoW
        {
            get
            {
                if (this.mUnitOfWorkObject == null)
                {
                    this.mUnitOfWorkObject = new UnitOfWork();
                }

                return this.mUnitOfWorkObject;
            }
        }

        #endregion Properties

        #region Events

        /// <summary>
        /// Handles the Unload event of the Page control and gets rid of initiated objects on the
        /// page startup.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Unload(object sender, EventArgs e)
        {
            if (this.mUnitOfWorkObject != null)
            {
                this.mUnitOfWorkObject.Dispose();
                this.mUnitOfWorkObject = null;
            }

            if (this.mTopicBoObject != null)
            {
                this.mTopicBoObject = null;
            }

            if (this.mTopicHistoryBoObject != null)
            {
                this.mTopicHistoryBoObject = null;
            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                // Include Css files
                this.AddStylePageHeader(CSSWikiModuleCssId, CSSWikiModuleCssPath);

                // congfigure the URL to the home page (the wiki without any parameters)
                this.mHomeUrlValue = DotNetNuke.Common.Globals.NavigateURL();

                // Get the pageTopic
                if (this.Request.QueryString["topic"] == null)
                {
                    if (this.Request.QueryString["add"] == null & this.Request.QueryString["loc"] == null)
                    {
                        this.mPageTopicValue = WikiHomeName;
                    }
                    else
                    {
                        this.mPageTopicValue = string.Empty;
                    }
                }
                else
                {
                    this.mPageTopicValue = WikiMarkup.DecodeTitle(this.Request.QueryString["topic"].ToString());
                }

                // Sets the wikiSettings
                this.SetWikiSettings();

                // Get the edit rights
                if (this.mWikiSettingsObject.ContentEditorRoles.Equals("UseDNNSettings"))
                {
                    this.mCanEditValue = this.IsEditable;
                }
                else
                {
                    // User is logged in
                    if (Request.IsAuthenticated)
                    {
                        if (this.UserInfo.IsSuperUser)
                        {
                            this.mCanEditValue = true;
                            this.mIsAdminValue = true;
                        }
                        else if (this.mWikiSettingsObject.ContentEditorRoles.IndexOf(";" + DotNetNuke.Common.Globals.glbRoleAllUsersName + ";") > -1)
                        {
                            this.mCanEditValue = true;
                        }
                        else
                        {
                            string[] editorRoles = this.mWikiSettingsObject.ContentEditorRoles.Split(
                                new char[] { '|' },
                                StringSplitOptions.RemoveEmptyEntries)[0].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string role in editorRoles)
                            {
                                if (UserInfo.IsInRole(role))
                                {
                                    this.mCanEditValue = true;
                                    break; // TODO: might not be correct. Was : Exit For
                                }
                            }
                        }
                    }
                    else
                    {
                        // User is NOT logged in
                        if ((this.mWikiSettingsObject.ContentEditorRoles.IndexOf(";" + DotNetNuke.Common.Globals.glbRoleAllUsersName + ";") > -1) | (this.mWikiSettingsObject.ContentEditorRoles.IndexOf(";" + DotNetNuke.Common.Globals.glbRoleUnauthUserName + ";") > -1))
                        {
                            this.mCanEditValue = true;
                        }
                    }
                }

                this.LoadTopic();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion Events

        #region Aux Functions

        /// <summary>
        /// Gets the index.
        /// </summary>
        /// <returns>Index of the Module</returns>
        public IEnumerable<Topic> GetIndex()
        {
            return this.TopicBo.GetAllByModuleID(this.ModuleId);
        }

        /// <summary>
        /// Loads the topic.
        /// </summary>
        protected void LoadTopic()
        {
            this.mTopicObject = this.TopicBo.GetByNameForModule(this.ModuleId, this.mPageTopicValue);
            if (this.mTopicObject == null)
            {
                this.mTopicObject = new Topic();
                this.mTopicObject.TopicID = 0;
            }

            this.mTopicObject.TabID = this.TabId;
            this.mTopicObject.PortalSettings = this.PortalSettings;
            this.mTopicIdValue = this.mTopicObject.TopicID;
        }

        /// <summary>
        /// Reads the topic.
        /// </summary>
        /// <returns>The Topic with WikiMarkup decoded to appear as standard HTML</returns>
        protected string ReadTopic()
        {
            return HttpUtility.HtmlEncode(this.mTopicObject.Cache) ?? string.Empty;
        }

        /// <summary>
        /// Reads the topic for edit.
        /// </summary>
        /// <returns>Topic Content with WikiMarkup included</returns>
        protected string ReadTopicForEdit()
        {
            return this.mTopicObject.Content;
        }

        /// <summary>
        /// Saves the topic.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="allowDiscuss">if set to <c>true</c> [allow discuss].</param>
        /// <param name="allowRating">if set to <c>true</c> [allow rating].</param>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <param name="keywords">The keywords.</param>
        /// <param name="out_crudOperation">The crud operation performed, only update or
        /// insert</param>
        protected void SaveTopic(
            string content,
            bool allowDiscuss,
            bool allowRating,
            string title,
            string description,
            string keywords,
            out SharedEnum.CrudOperation out_crudOperation)
        {
            TopicHistory topicHistory = new TopicHistory();
            topicHistory.TabID = this.TabId;
            topicHistory.PortalSettings = this.PortalSettings;
            if (this.mTopicObject.TopicID != 0)
            {
                if (!content.Equals(this.mTopicObject.Content) | !title.Equals(this.mTopicObject.Title) | !description.Equals(this.mTopicObject.Description) | !keywords.Equals(this.mTopicObject.Keywords))
                {
                    topicHistory.Name = this.mTopicObject.Name;
                    topicHistory.TopicId = this.mTopicObject.TopicID;
                    topicHistory.Content = this.mTopicObject.Content;
                    topicHistory.UpdatedBy = this.mTopicObject.UpdatedBy;
                    topicHistory.UpdateDate = DateTime.Now;
                    topicHistory.UpdatedByUserID = this.mTopicObject.UpdatedByUserID;
                    topicHistory.Title = this.mTopicObject.Title;
                    topicHistory.Description = this.mTopicObject.Description;
                    topicHistory.Keywords = this.mTopicObject.Keywords;

                    this.mTopicObject.UpdateDate = DateTime.Now;
                    if (UserInfo.UserID == -1)
                    {
                        this.mTopicObject.UpdatedBy = Localization.GetString("Anonymous", this.RouterResourceFile);
                    }
                    else
                    {
                        this.mTopicObject.UpdatedBy = UserInfo.Username;
                    }

                    this.mTopicObject.UpdatedByUserID = this.UserId;
                    this.mTopicObject.Content = content;
                    this.mTopicObject.Title = title;
                    this.mTopicObject.Description = description;
                    this.mTopicObject.Keywords = keywords;

                    this.TopicHistoryBo.Add(topicHistory);
                }

                this.mTopicObject.Name = this.mPageTopicValue;
                this.mTopicObject.Title = title;
                this.mTopicObject.Description = description;
                this.mTopicObject.Keywords = keywords;
                this.mTopicObject.AllowDiscussions = allowDiscuss;
                this.mTopicObject.AllowRatings = allowRating;
                this.mTopicObject.Content = content;

                this.TopicBo.Update(this.mTopicObject);
                out_crudOperation = SharedEnum.CrudOperation.Update;
            }
            else
            {
                this.mTopicObject = new Topic();
                this.mTopicObject.TabID = this.TabId;
                this.mTopicObject.PortalSettings = this.PortalSettings;
                this.mTopicObject.Content = content;
                this.mTopicObject.Name = this.mPageTopicValue;
                this.mTopicObject.ModuleId = this.ModuleId;
                if (UserInfo.UserID == -1)
                {
                    this.mTopicObject.UpdatedBy = Localization.GetString("Anonymous", this.RouterResourceFile);
                }
                else
                {
                    this.mTopicObject.UpdatedBy = UserInfo.Username;
                }

                this.mTopicObject.UpdatedByUserID = this.UserId;
                this.mTopicObject.UpdateDate = DateTime.Now;
                this.mTopicObject.AllowDiscussions = allowDiscuss;
                this.mTopicObject.AllowRatings = allowRating;
                this.mTopicObject.Title = title;
                this.mTopicObject.Description = description;
                this.mTopicObject.Keywords = keywords;

                this.mTopicObject = this.TopicBo.Add(this.mTopicObject);

                this.mTopicIdValue = this.mTopicObject.TopicID;
                out_crudOperation = SharedEnum.CrudOperation.Insert;
            }
        }

        /// <summary>
        /// Gets the recently changed.
        /// </summary>
        /// <param name="daysBack">The days back.</param>
        /// <returns>Recently Changed Topics based on days back parameter</returns>
        protected IEnumerable<Topic> GetRecentlyChanged(int daysBack)
        {
            return this.TopicBo.GetAllByModuleChangedWhen(this.ModuleId, daysBack);
        }

        /// <summary>
        /// Gets the history.
        /// </summary>
        /// <returns>History for the Topic</returns>
        protected IEnumerable<TopicHistory> GetHistory()
        {
            return this.TopicHistoryBo.GetHistoryForTopic(this.mTopicIdValue);
        }

        /// <summary>
        /// Searches the specified search string.
        /// </summary>
        /// <param name="searchString">The search string.</param>
        /// <returns>Search String for the Module</returns>
        protected IEnumerable<Topic> Search(string searchString)
        {
            return this.TopicBo.SearchWiki(searchString, this.ModuleId);
        }

        /// <summary>
        /// Creates the table.
        /// </summary>
        /// <param name="topicCollection">The topic collection.</param>
        /// <returns>html for the topics</returns>
        protected string CreateTable(List<Topic> topicCollection)
        {
            System.Text.StringBuilder tableHTML = new System.Text.StringBuilder();

            if (string.IsNullOrEmpty(WikiSettings.TableStyles))
            {
                tableHTML.Append("<table>");
            }
            else
            {
                tableHTML.Append($"<table class=\"{WikiSettings.TableStyles}\">");
            }

            tableHTML.Append("<tr><th>");
            tableHTML.Append(Localization.GetString("BaseCreateTableTopic", this.RouterResourceFile));
            tableHTML.Append("</th><th>");
            tableHTML.Append(Localization.GetString("BaseCreateTableModBy", this.RouterResourceFile));
            tableHTML.Append("</th><th>");
            tableHTML.Append(Localization.GetString("BaseCreateTableModDate", this.RouterResourceFile));
            tableHTML.Append("</th></tr>");
            //// Dim TopicTable As String
            Topic localTopic = new Topic();
            int i = 0;
            if (topicCollection != null && topicCollection.Any())
            {
                for (i = 0; i <= topicCollection.Count - 1; i++)
                {
                    localTopic = (Topic)topicCollection[i];
                    localTopic.TabID = this.TabId;
                    localTopic.PortalSettings = this.PortalSettings;

                    string nameToUse = string.Empty;
                    if (!string.IsNullOrWhiteSpace(localTopic.Title))
                    {
                        nameToUse = localTopic.Title.Replace(WikiModuleBase.WikiHomeName, "Home");
                    }
                    else
                    {
                        nameToUse = localTopic.Name.Replace(WikiHomeName, "Home");
                    }

                    tableHTML.Append("<tr>");
                    tableHTML.Append("<td><a class=\"CommandButton\" href=\"");
                    tableHTML.Append(DotNetNuke.Common.Globals.NavigateURL(this.TabId, this.PortalSettings, string.Empty, "topic=" + WikiMarkup.EncodeTitle(localTopic.Name)));
                    tableHTML.Append("\">");
                    tableHTML.Append(nameToUse);
                    tableHTML.Append("</a></td>");
                    tableHTML.Append("<td class=\"Normal\">");
                    tableHTML.Append(localTopic.UpdatedByUsername);
                    tableHTML.Append("</td>");
                    tableHTML.Append("<td class=\"Normal\">");
                    tableHTML.Append(localTopic.UpdateDate.ToString(CultureInfo.CurrentCulture));
                    tableHTML.Append("</td>");
                    tableHTML.Append("</tr>");
                }
            }
            else
            {
                tableHTML.Append("<tr><td colspan=3 class=\"Normal\">");
                tableHTML.Append(Localization.GetString("BaseCreateTableNoResults", this.RouterResourceFile));
                tableHTML.Append("</td></tr>");
            }

            tableHTML.Append("</table>");
            return tableHTML.ToString();
        }

        /// <summary>
        /// Creates the recent change table.
        /// </summary>
        /// <param name="daysBack">The days back.</param>
        /// <returns>html for the recent changes table</returns>
        protected string CreateRecentChangeTable(int daysBack)
        {
            return this.CreateTable(this.GetRecentlyChanged(daysBack).ToList());
        }

        /// <summary>
        /// Creates the search table.
        /// </summary>
        /// <param name="searchString">The search string.</param>
        /// <returns>html for the search results table</returns>
        protected string CreateSearchTable(string searchString)
        {
            return this.CreateTable(this.Search(searchString).ToList());
        }

        /// <summary>
        /// Creates the history table.
        /// </summary>
        /// <returns>html for the history table</returns>
        protected string CreateHistoryTable()
        {
            System.Text.StringBuilder tableText = new System.Text.StringBuilder(1000);

            if (string.IsNullOrEmpty(WikiSettings.TableStyles))
            {
                tableText.Append("<table>");
            }
            else
            {
                tableText.Append($"<table class=\"{WikiSettings.TableStyles}\">");
            }

            tableText.Append("<tr><th>");
            tableText.Append(Localization.GetString("BaseCreateTableTopic", this.RouterResourceFile));
            tableText.Append("</th><th>");
            tableText.Append(Localization.GetString("BaseCreateTableTitle", this.RouterResourceFile));
            tableText.Append("</th><th>");
            tableText.Append(Localization.GetString("BaseCreateTableModBy", this.RouterResourceFile));
            tableText.Append("</th><th>");
            tableText.Append(Localization.GetString("BaseCreateTableModDate", this.RouterResourceFile));
            tableText.Append("</th></tr>");

            var topicHistories = this.GetHistory();
            //// Dim TopicTable As StringBuilder = New StringBuilder(500)
            TopicHistory history = null;
            int i = 0;
            if (topicHistories != null && topicHistories.Any())
            {
                var topicHistoryCollection = topicHistories.ToArray();
                i = topicHistoryCollection.Count();
                while (i > 0)
                {
                    history = (TopicHistory)topicHistoryCollection[i - 1];
                    history.TabID = this.TabId;
                    history.PortalSettings = this.PortalSettings;
                    tableText.Append("<tr><td><a class=\"CommandButton\" rel=\"noindex,nofollow\" href=\"");
                    tableText.Append(DotNetNuke.Common.Globals.NavigateURL(this.TabId, this.PortalSettings, string.Empty, "topic=" + WikiMarkup.EncodeTitle(this.mPageTopicValue), "loc=TopicHistory", "ShowHistory=" + history.TopicHistoryId.ToString()));
                    tableText.Append("\">");
                    tableText.Append(history.Name.Replace(WikiHomeName, "Home"));

                    tableText.Append("</a></td>");
                    tableText.Append("<td class=\"Normal\">");
                    if (!string.IsNullOrWhiteSpace(history.Title))
                    {
                        tableText.Append(history.Title.Replace(WikiHomeName, "Home"));
                    }
                    else
                    {
                        tableText.Append(history.Name.Replace(WikiHomeName, "Home"));
                    }

                    tableText.Append("</td>");
                    tableText.Append("<td class=\"Normal\">");
                    tableText.Append(history.UpdatedByUsername);
                    tableText.Append("</td>");
                    tableText.Append("<td Class=\"Normal\">");
                    tableText.Append(history.UpdateDate.ToString(CultureInfo.CurrentCulture));
                    tableText.Append("</td>");
                    tableText.Append("</tr>");
                    i = i - 1;
                }
            }
            else
            {
                tableText.Append("<tr><td colspan=\"3\" class=\"Normal\">");
                tableText.Append(Localization.GetString("BaseCreateHistoryTableEmpty", this.RouterResourceFile));
                tableText.Append("</td></tr>");
            }

            tableText.Append("</table>");
            return tableText.ToString();
        }

        /// <summary>
        /// Sets the wiki settings entity
        /// </summary>
        private void SetWikiSettings()
        {
            // if (wikiSettings == null) {
            SettingBO wikiController = new SettingBO(this.UoW);
            this.mWikiSettingsObject = wikiController.GetByModuleID(this.ModuleId);
            if (this.mWikiSettingsObject == null)
            {
                this.mWikiSettingsObject = new Setting();
                this.mWikiSettingsObject.ContentEditorRoles = "UseDNNSettings";
            }
            //// }
        }

        /// <summary>
        /// Adds a page header of type CSS
        /// </summary>
        /// <param name="cssId">the id of the header tag</param>
        /// <param name="cssPath">the Style Sheet path to the source file</param>
        private void AddStylePageHeader(string cssId, string cssPath)
        {
            HtmlGenericControl scriptInclude = (HtmlGenericControl)Page.Header.FindControl(cssId);
            if (scriptInclude == null)
            {
                scriptInclude = new HtmlGenericControl("link");
                scriptInclude.Attributes["rel"] = "stylesheet";
                scriptInclude.Attributes["type"] = "text/css";
                scriptInclude.Attributes["href"] = this.DNNWikiModuleRootPath + cssPath;
                scriptInclude.ID = cssId;

                Page.Header.Controls.Add(scriptInclude);
            }
        }

        #endregion Aux Functions
    }
}