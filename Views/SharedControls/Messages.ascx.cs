#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="Messages.ascx.cs" company="DNN Corp®">
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

namespace DotNetNuke.Wiki.Views.SharedControls
{
    /// <summary>
    /// Presents user friendly messages to the user
    /// </summary>
    public partial class Messages : System.Web.UI.UserControl
    {
        #region Variables

        internal const string ERRORCLASS = "dnnFormMessage dnnFormError";
        internal const string WARNINGCLASS = "dnnFormMessage dnnFormWarning";
        internal const string SUCCESSCLASS = "dnnFormMessage dnnFormSuccess";
        internal const string INFOCLASS = "dnnFormMessage dnnFormInfo";

        #endregion Variables

        #region Aux Functions

        /// <summary>
        /// Clears message labels, for warning and errors
        /// </summary>
        public void ClearMessages()
        {
            this.pnl_message.CssClass = string.Empty;
            this.pnl_message.Visible = false;
            this.lt_message.Text = string.Empty;
        }

        /// <summary>
        /// Shows an error message
        /// </summary>
        /// <param name="message">if empty hides the message control, else, shows the
        /// message</param>
        public void ShowError(string message)
        {
            this.pnl_message.CssClass = ERRORCLASS;
            this.pnl_message.Visible = !string.IsNullOrWhiteSpace(message);
            this.lt_message.Text = message;
        }

        /// <summary>
        /// Shows an warning message
        /// </summary>
        /// <param name="message">if empty hides the message control, else, shows the
        /// message</param>
        public void ShowWarning(string message)
        {
            this.pnl_message.CssClass = WARNINGCLASS;
            this.pnl_message.Visible = !string.IsNullOrWhiteSpace(message);
            this.lt_message.Text = message;
        }

        /// <summary>
        /// Shows an success message
        /// </summary>
        /// <param name="message">if empty hides the message control, else, shows the
        /// message</param>
        public void ShowSuccess(string message)
        {
            this.pnl_message.CssClass = SUCCESSCLASS;
            this.pnl_message.Visible = !string.IsNullOrWhiteSpace(message);
            this.lt_message.Text = message;
        }

        /// <summary>
        /// Shows an information message
        /// </summary>
        /// <param name="message">if empty hides the message control, else, shows the
        /// message</param>
        public void ShowInfo(string message)
        {
            this.pnl_message.CssClass = INFOCLASS;
            this.pnl_message.Visible = !string.IsNullOrWhiteSpace(message);
            this.lt_message.Text = message;
        }

        #endregion Aux Functions
    }
}