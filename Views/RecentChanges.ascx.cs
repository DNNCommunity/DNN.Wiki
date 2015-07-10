#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="RecentChanges.ascx.cs" company="DNN Corp®">
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

namespace DotNetNuke.Wiki.Views
{
    /// <summary>
    /// Recent changes class based on WikiModuleBase
    /// </summary>
    public partial class RecentChanges : WikiModuleBase
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RecentChanges"/> class.
        /// </summary>
        public RecentChanges()
        {
            this.Load += this.Page_Load;
        }

        #endregion Constructor

        #region Events

        /// <summary>
        /// Handles the Click event of the Last 7 Days control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        protected void CmdLast7Days_Click(object sender, System.EventArgs e)
        {
            this.HitTable.Text = this.CreateRecentChangeTable(7);
        }

        /// <summary>
        /// Handles the Click event of the Last 24 hours control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        protected void CmdLast24Hrs_Click(object sender, System.EventArgs e)
        {
            this.HitTable.Text = this.CreateRecentChangeTable(1);
        }

        /// <summary>
        /// Handles the Click event of the Last Month control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        protected void CmdLastMonth_Click(object sender, System.EventArgs e)
        {
            this.HitTable.Text = this.CreateRecentChangeTable(31);
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
                this.HitTable.Text = this.CreateRecentChangeTable(1);
            }
        }

        #endregion Events

        #region Methods

        /// <summary>
        /// Loads the localization.
        /// </summary>
        private void LoadLocalization()
        {
            this.TitleLbl.Text = Localization.GetString("RCTitle", this.RouterResourceFile);
            this.cmdLast24Hrs.Text = Localization.GetString("RCLast24h", this.RouterResourceFile);
            this.cmdLast7Days.Text = Localization.GetString("RCLast7d", this.RouterResourceFile);
            this.cmdLastMonth.Text = Localization.GetString("RCLastMonth", this.RouterResourceFile);
        }

        #endregion Methods
    }
}