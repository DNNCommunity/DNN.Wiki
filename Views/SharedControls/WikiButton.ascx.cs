#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="WikiButton.ascx.cs" company="DNN Corp®">
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
    /// Wiki Button Code
    /// </summary>
    public partial class WikiButton : WikiModuleBase
    {
        #region "Properties"

        /// <summary>
        /// Gets or sets a value indicating whether to show only add control.
        /// </summary>
        /// <value>true if to show only add control; otherwise, false.</value>
        public bool ShowOnlyAddTopicControl
        {
            get
            {
                if (this.ViewState["WikiButton_ShowOnlyAddTopicControl"] != null)
                {
                    return Convert.ToBoolean(this.ViewState["WikiButton_ShowOnlyAddTopicControl"]);
                }

                return false;
            }

            set
            {
                this.ViewState["WikiButton_ShowOnlyAddTopicControl"] = value;
            }
        }

        #endregion "Properties"

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="WikiButton"/> class.
        /// </summary>
        public WikiButton()
        {
            this.Load += this.Page_Load;
        }

        #endregion Constructor

        #region Events

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event
        /// data.</param>
        protected new void Page_Load(object sender, System.EventArgs e)
        {
            this.LocalResourceFile = DotNetNuke.Services.Localization.Localization.GetResourceFile(this, "WikiButton.ascx.resx");

            this.lnkEdit.Text = Localization.GetString("StartEdit.Text", this.LocalResourceFile);
            this.cmdAdd.Text = Localization.GetString("StartAdd", this.LocalResourceFile);
            this.txtViewHistory.Text = Localization.GetString("StartViewHistory", this.LocalResourceFile);

            this.SetDisplay();
        }

        #endregion Events

        #region Methods

        /// <summary>
        /// Sets the display.
        /// </summary>
        private void SetDisplay()
        {
            this.cmdAdd.Visible = this.CanEdit;
            this.lnkEdit.Visible = false;

            this.txtViewHistory.Visible = false;

            if (!this.ShowOnlyAddTopicControl &&
                (CurrentTopic.TopicID >= 0 || (Request.QueryString["topic"] != null)))
            {
                this.cmdAdd.Visible = this.CanEdit;
                this.lnkEdit.Visible = this.CanEdit;

                this.txtViewHistory.Visible = true;
                this.txtViewHistory.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(this.TabId, this.PortalSettings, string.Empty, "loc=TopicHistory", "topic=" + WikiMarkup.EncodeTitle(this.PageTopic));
                this.lnkEdit.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(this.TabId, string.Empty, "topic=" + WikiMarkup.EncodeTitle(this.PageTopic) + "&loc=edit");
            }

            this.cmdAdd.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(this.TabId, string.Empty, "&loc=edit&add=true");
        }

        #endregion Methods
    }
}