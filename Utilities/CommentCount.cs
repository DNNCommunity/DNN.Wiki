#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="CommentCount.cs" company="DNN Corp®">
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

using DotNetNuke.Services.Localization;
using DotNetNuke.Wiki.BusinessObjects;
using System.ComponentModel;
using System.Web.UI;

namespace DotNetNuke.Wiki.Utilities
{
    /// <summary>
    /// Comment Count Class
    /// </summary>
    [DefaultProperty("ID"), ToolboxData("<{0}:CommentCount runat=server></{0}:CommentCount>")]
    public class CommentCount : System.Web.UI.WebControls.Label
    {
        #region Variables

        private string mTextValue;
        private int mParentIdValue;

        #endregion Variables

        #region Properties

        /// <summary>
        /// Gets or sets the parent unique identifier.
        /// </summary>
        /// <value>The parent unique identifier.</value>
        [Description("The id of the parent (page) the comment count is for."), Category("Data")]
        public int ParentId
        {
            get { return this.mParentIdValue; }
            set { this.mParentIdValue = value; }
        }

        /// <summary>
        /// Gets or sets the text content of the <see cref="T:System.Web.UI.WebControls.Label" />
        /// control.
        /// </summary>
        /// <returns>The text content of the control. The default value is see
        /// cref="F:System.String.Empty" />.</returns>
        [Description("The text for the link. {0} will be replaced with the number of comments."), Category("Appearance")]
        public new string Text
        {
            get { return this.mTextValue; }
            set { this.mTextValue = value; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Renders the contents of the <see cref="T:System.Web.UI.WebControls.Label" /> into the
        /// specified writer.
        /// </summary>
        /// <param name="writer">The output stream that renders HTML content to the client.</param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            using (UnitOfWork uof = new UnitOfWork())
            {
                var commentBo = new CommentBO(uof);

                int commentCount = commentBo.GetCommentCount(this.mParentIdValue);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, this.CssClass);
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                if (commentCount > 0)
                {
                    //// writer.Write(String.Format(Me._text, commentCount))

                    if (this.mTextValue == null)
                    {
                        writer.Write(string.Format(Localization.GetString("FeedBack.Text", WikiModuleBase.SharedResources), commentCount));
                    }
                    else
                    {
                        writer.Write(string.Format(this.mTextValue, commentCount));
                    }
                }
                else
                {
                    writer.Write(Localization.GetString("NoComments.Text", WikiModuleBase.SharedResources));
                }

                writer.RenderEndTag();
            }
        }

        #endregion Methods
    }
}