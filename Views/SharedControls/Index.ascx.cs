#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="Index.ascx.cs" company="DNN Corp®">
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
using DotNetNuke.Wiki.BusinessObjects.Models;
using DotNetNuke.Wiki.Utilities;
using System.Linq;

namespace DotNetNuke.Wiki.Views.SharedControls
{
    /// <summary>
    /// Wiki Module Base Class
    /// </summary>
    public partial class Index : WikiModuleBase
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Index"/> class.
        /// </summary>
        public Index()
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
            this.DisplayIndex();
        }

        #endregion Events

        #region Methods

        /// <summary>
        /// Displays the index.
        /// </summary>
        private void DisplayIndex()
        {
            this.Session["wiki" + ModuleId.ToString() + "ShowIndex"] = true;
            var topics = GetIndex().ToArray();
            System.Text.StringBuilder tableText = new System.Text.StringBuilder();
            //// Dim TopicTable As String
            Topic t = default(Topic);
            int i = 0;
            tableText.Append("&nbsp;&nbsp;&nbsp<a class=\"CommandButton\" href=\"");
            tableText.Append(this.HomeURL + "\"><img src=\"");
            tableText.Append(this.DNNWikiModuleRootPath);
            tableText.Append("/Resources/images/Home.gif\" border=\"0\" align=\"middle\" alt=\"" + Localization.GetString("Home", this.LocalResourceFile) + "\" />&nbsp;");
            tableText.Append(Localization.GetString("Home", this.LocalResourceFile));
            tableText.Append("</a><br />");
            if (topics != null)
            {
                if (topics.Count() > 0)
                {
                    for (i = 0; i <= topics.Count() - 1; i++)
                    {
                        t = (Topic)topics[i];
                        if (t.Name != WikiModuleBase.WikiHomeName)
                        {
                            tableText.Append("&nbsp;&nbsp;&nbsp<a class=\"CommandButton\" href=\"");
                            tableText.Append(DotNetNuke.Common.Globals.NavigateURL(this.TabId, this.PortalSettings, string.Empty, "topic=" + WikiMarkup.EncodeTitle(t.Name)));
                            tableText.Append("\"><img src=\"");
                            tableText.Append(this.DNNWikiModuleRootPath);
                            tableText.Append("/Resources/images/Page.gif\" border=\"0\" align=\"middle\"  alt=\"" + WikiMarkup.EncodeTitle(t.Name) + "\" />&nbsp;");
                            tableText.Append(t.Name);
                            tableText.Append("</a><br />");
                        }
                    }
                }
            }

            tableText.Append(string.Empty);
            this.IndexList.Text = tableText.ToString();
            this.IndexList.Visible = true;
        }

        #endregion Methods
    }
}