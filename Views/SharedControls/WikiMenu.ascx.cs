#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="WikiMenu.ascx.cs" company="DNN Corp®">
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

namespace DotNetNuke.Wiki.Views.SharedControls
{
    /// <summary>
    /// Wiki Menu Control Code
    /// </summary>
    public partial class WikiMenu : WikiModuleBase
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="WikiMenu"/> class.
        /// </summary>
        public WikiMenu()
        {
            this.Load += this.Menu_Page_Load;
        }

        #endregion Constructor

        #region Variables

        private bool mShowNav;
        private bool mShowIndex;

        #endregion Variables

        #region Events

        /// <summary>
        /// Handles the Load event of the Menu Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        private void Menu_Page_Load(object sender, System.EventArgs e)
        {
            // TODO: we shouldn't use session
            if (this.Session["wiki" + ModuleId.ToString() + "ShowIndex"] == null)
            {
                this.Session.Add("wiki" + ModuleId.ToString() + "ShowIndex", false);
                this.mShowIndex = false;
            }
            else
            {
                this.mShowIndex = Convert.ToBoolean(this.Session["wiki" + ModuleId.ToString() + "ShowIndex"]);
            }

            if (this.Session["wiki" + ModuleId.ToString() + "ShowNav"] == null)
            {
                this.Session.Add("wiki" + ModuleId.ToString() + "ShowNav", true);
                this.mShowNav = true;
            }
            else
            {
                this.mShowNav = Convert.ToBoolean(this.Session["wiki" + ModuleId.ToString() + "ShowNav"]);
            }

            if (this.mShowNav)
            {
                //// Me.ImageButton1.AlternateText = Localization.GetString("HideNavigation",
                //// LocalResourceFile) ' "Show Navigation" Me.ImageButton1.ImageUrl =
                //// TemplateSourceDirectory + "/images/HideNav.gif"
                this.LinksPanel.Visible = true;
            }
            else
            {
                //// Me.ImageButton1.AlternateText = Localization.GetString("ShowNavigation",
                //// LocalResourceFile) ' "Show Navigation" Me.ImageButton1.ImageUrl =
                //// TemplateSourceDirectory + "/images/ShowNav.gif"
                this.LinksPanel.Visible = false;
            }

            this.SetURLs();
        }

        #endregion Events

        #region Methods

        /// <summary>
        /// Sets the current URL.
        /// </summary>
        private void SetURLs()
        {
            this.HomeBtn.NavigateUrl = Common.Globals.NavigateURL();
            this.HomeBtn.Text = "<img src=\"" + this.DNNWikiModuleRootPath + "/Resources/images/Home.gif\" border=\"0\" align=\"middle\" alt=\"" + Localization.GetString("Home", this.LocalResourceFile) + "\" />&nbsp;" + Localization.GetString("Home", this.LocalResourceFile);
            this.SearchBtn.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(this.TabId, this.PortalSettings, string.Empty, "loc=search");
            this.SearchBtn.Text = "<img src=\"" + this.DNNWikiModuleRootPath + "/Resources/images/Search.gif\" border=\"0\" align=\"middle\" alt=\"" + Localization.GetString("Search", this.LocalResourceFile) + "\" />&nbsp;" + Localization.GetString("Search", this.LocalResourceFile);
            this.RecChangeBtn.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(this.TabId, this.PortalSettings, string.Empty, "loc=recentchanges");
            this.RecChangeBtn.Text = "<img src=\"" + this.DNNWikiModuleRootPath + "/Resources/images/RecentChanges.gif\" border=\"0\" align=\"middle\" alt=\"" + Localization.GetString("RecentChanges", this.LocalResourceFile) + "\" />&nbsp;" + Localization.GetString("RecentChanges", this.LocalResourceFile);

            this.IndexBtn.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(this.TabId, this.PortalSettings, string.Empty, "loc=index");

            this.IndexBtn.Text = "<img src=\"" + this.DNNWikiModuleRootPath + "/Resources/images/Index.gif\" border=\"0\" align=\"middle\" alt=\"" + Localization.GetString("Index", this.LocalResourceFile) + "\" />&nbsp;" + Localization.GetString("Index", this.LocalResourceFile);
        }

        #endregion Methods
    }
}