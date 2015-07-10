#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="Comments.cs" company="DNN Corp®">
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
using DotNetNuke.Wiki.BusinessObjects.Models;
using DotNetNuke.Wiki.Extensions;
using System;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DotNetNuke.Wiki.Utilities
{
    /// <summary>
    /// The Comments Class
    /// </summary>
    [DefaultProperty("ID"), ToolboxData("<{0}:Comments runat=server></{0}:Comments>")]
    public class Comments : System.Web.UI.WebControls.Table
    {
        #region Variables

        //// Private _dateFormat As String = "dd/MM/yyyy HH:mm"
        //// TODO: create a module setting for the date format
        private string mDateFormatValue = "dd/MM/yyyy HH:mm";

        private int mBreakCountValue = 1;
        private bool mCacheItemsValue;
        private CommentBO mCommentBOObject;

        private bool mHideEmailAddressValue;
        private string mHideEmailUrlValue = "http://localhost/getemail.aspx?commentid={0}";

        private int mParentIdValue;
        private UnitOfWork mUnitOfWork;

        private bool mIsAdminValue = false;

        #endregion Variables

        #region Properties

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
        /// Gets an instance of the comment business object
        /// </summary>
        public CommentBO CommentBo
        {
            get
            {
                if (this.mCommentBOObject == null)
                {
                    this.mCommentBOObject = new CommentBO(this.UoW);
                }

                return this.mCommentBOObject;
            }
        }

        /// <summary>
        /// Gets the collection of rows in the <see cref="T:System.Web.UI.WebControls.Table" />
        /// control.
        /// </summary>
        /// <returns>A <see cref="T:System.Web.UI.WebControls.TableRowCollection" /> that contains
        /// the <see cref="T:System.Web.UI.WebControls.TableRow" /> objects in the
        /// <see cref="T:System.Web.UI.WebControls.Table" /> control.</returns>
        [Browsable(false)]
        public override TableRowCollection Rows
        {
            get { return base.Rows; }
        }

        /// <summary>
        /// Gets or sets the URL of the background image to display behind the
        /// <see cref="T:System.Web.UI.WebControls.Table" /> control.
        /// </summary>
        /// <returns>The URL of the background image for the
        /// <see cref="T:System.Web.UI.WebControls.Table" /> control. The default value is see
        /// cref="F:System.String.Empty" />.</returns>
        [Browsable(false)]
        public override string BackImageUrl
        {
            get { return base.BackImageUrl; }

            set { }
        }

        /// <summary>
        /// Gets the <see cref="T:System.Web.UI.HtmlTextWriterTag" /> value that corresponds to this
        /// Web server control. This property is used primarily by control developers.
        /// </summary>
        /// <returns>One of the <see cref="T:System.Web.UI.HtmlTextWriterTag" /> enumeration
        /// values.</returns>
        protected override HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Table; }
        }

        /// <summary>
        /// Gets or sets the parent unique identifier.
        /// </summary>
        /// <value>The parent unique identifier.</value>
        [Description("The id of the parent (page) the comments are for."), Category("Data")]
        public int ParentId
        {
            get { return this.mParentIdValue; }
            set { this.mParentIdValue = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [hide email address].
        /// </summary>
        /// <value><c>true</c> if [hide email address]; otherwise, /c>.</value>
        [Description("Whether to suppress displaying the email address. This option should be used in conjunction with the HideEmailUrl property."), Category("Behaviour")]
        public bool HideEmailAddress
        {
            get { return this.mHideEmailAddressValue; }
            set { this.mHideEmailAddressValue = value; }
        }

        /// <summary>
        /// Gets or sets the hide email URL.
        /// </summary>
        /// <value>The hide email URL.</value>
        [Description("The url that the email address will point to. This enables you to create a page that will show the email address (after a turing test is performed), to stop the email address being 'spam harvested'."), Category("Behaviour")]
        public string HideEmailUrl
        {
            get { return this.mHideEmailUrlValue; }
            set { this.mHideEmailUrlValue = value; }
        }

        /// <summary>
        /// Gets or sets the break count.
        /// </summary>
        /// <value>The break count.</value>
        [Description("The number of breaks (<br />) tags between each table."), Category("Appearance")]
        public int BreakCount
        {
            get { return this.mBreakCountValue; }
            set { this.mBreakCountValue = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [cache items].
        /// </summary>
        /// <value><c>true</c> if [cache items]; otherwise, /c>.</value>
        [Description("Caches the comments indefinitely using ASP.NET's caching mechanism. The cache is cleared when a new item is added, but NOT when a comment is deleted in the Manager application."), Category("Behaviour")]
        public bool CacheItems
        {
            get { return this.mCacheItemsValue; }
            set { this.mCacheItemsValue = value; }
        }

        /// <summary>
        /// Gets or sets the date format.
        /// </summary>
        /// <value>The date format.</value>
        [Description("The format that the date the comment was posted displays in. See the DateTimeFormatInfo for details of the tokens available."), Category("Behaviour")]
        public string DateFormat
        {
            get { return this.mDateFormatValue; }
            set { this.mDateFormatValue = value; }
        }

        /// <summary>
        /// Gets an instance of the unit of work object
        /// </summary>
        internal UnitOfWork UoW
        {
            get
            {
                if (this.mUnitOfWork == null)
                {
                    this.mUnitOfWork = new UnitOfWork();
                }

                return this.mUnitOfWork;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Unload" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains event
        /// data.</param>
        protected override void OnUnload(EventArgs e)
        {
            if (this.mUnitOfWork != null)
            {
                this.mUnitOfWork.Dispose();
                this.mUnitOfWork = null;
            }

            if (this.mCommentBOObject != null)
            {
                this.mCommentBOObject = null;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event
        /// data.</param>
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            ////'Check for delete flag on query string via CommentId (cid)

            if (Context.Request.QueryString["cid"] != null & this.mIsAdminValue)
            {
                int commentId = Convert.ToInt32(Context.Request.QueryString["cid"]);
                this.CommentBo.Delete(new Comment { CommentId = Convert.ToInt32(commentId) });
                this.Context.Cache.Remove("WikiComments" + this.mParentIdValue.ToString());

                this.Context.Response.Redirect(this.ReconstructQueryStringWithoutId());
            }
        }

        /// <summary>
        /// Reconstructs the query string without unique identifier.
        /// </summary>
        /// <returns>The URL Requested</returns>
        private string ReconstructQueryStringWithoutId()
        {
            ////TODO: make this use NavigateURL
            StringBuilder url = new StringBuilder(128);

            url.Append(Context.Request.Url.Scheme);
            url.Append("://");
            url.Append(Context.Request.Url.Authority);
            url.Append(Context.Request.ApplicationPath);
            url.Append("Default.aspx?");
            int i = 0;
            for (i = 0; i <= Context.Request.QueryString.Keys.Count - 1; i++)
            {
                string key = Context.Request.QueryString.Keys[i].ToString();
                if (!key.Equals("cid"))
                {
                    url.Append(key);
                    url.Append("=");
                    url.Append(Context.Request.QueryString[i]);
                    url.Append("&");
                }
            }
            ////strip last &
            url.Remove(url.Length - 1, 1);

            return url.ToString();
        }

        /// <summary>
        /// Renders the rows in the table control to the specified writer.
        /// </summary>
        /// <param name="writer">An <see cref="T:System.Web.UI.HtmlTextWriter" /> that represents
        /// the output stream to render HTML content on the client.</param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (this.Context != null)
            {
                DataTable dataTable = null;
                if (this.mCacheItemsValue)
                {
                    if (this.Context.Cache["WikiComments" + this.mParentIdValue.ToString()] != null)
                    {
                        dataTable = (DataTable)this.Context.Cache["WikiComments" + this.mParentIdValue.ToString()];
                    }
                    else
                    {
                        dataTable = this.CommentBo.GetCommentsByParent(this.mParentIdValue).ToDataTable<Comment>();
                        this.Context.Cache.Insert("WikiComments" + this.mParentIdValue.ToString(), dataTable);
                    }
                }
                else
                {
                    dataTable = this.CommentBo.GetCommentsByParent(this.mParentIdValue).ToDataTable<Comment>();
                }

                if (dataTable != null)
                {
                    if (dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow dataRow in dataTable.Rows)
                        {
                            this.RenderRow(
                                writer,
                                Convert.ToInt32(dataRow["CommentId"]),
                                Convert.ToString(dataRow["Name"]),
                                Convert.ToString(dataRow["Email"]),
                                Convert.ToString(dataRow["CommentText"]),
                                (DateTime)dataRow["Datetime"]);
                        }
                    }
                    else
                    {
                        this.CssClass = "WikiTable";
                        writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "NormalBold");
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);

                        writer.Write(Localization.GetString("NoComments.Text", WikiModuleBase.SharedResources));
                        dynamic breakcount2 = this.mBreakCountValue;
                        this.mBreakCountValue = 0;
                        writer.RenderEndTag();

                        this.RenderEndTag(writer);
                        this.mBreakCountValue = breakcount2;
                    }
                }
                else
                {
                    this.RenderRow(writer, 1, Localization.GetString("ExampleName.Text", WikiModuleBase.SharedResources), Localization.GetString("ExampleEmail.Text", WikiModuleBase.SharedResources), Localization.GetString("ExampleComments.Text", WikiModuleBase.SharedResources), DateTime.Now);
                }
            }
        }

        /// <summary>
        /// Renders the row.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="commentId">The comment unique identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="email">The email.</param>
        /// <param name="comments">The comments.</param>
        /// <param name="postDate">The post date.</param>
        private void RenderRow(HtmlTextWriter writer, int commentId, string name, string email, string comments, DateTime postDate)
        {
            ////Delete the comments
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "NormalBold");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            if (this.mHideEmailAddressValue)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "NormalBold");
                writer.AddAttribute(HtmlTextWriterAttribute.Href, string.Format(this.mHideEmailUrlValue, commentId));
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.Write(name);
                writer.RenderEndTag();
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "NormalBold");
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "mailto:" + email);
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.Write(name);
                writer.RenderEndTag();
            }

            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            comments = System.Web.HttpUtility.HtmlDecode(comments);
            comments = comments.Replace(string.Empty + "\t" + string.Empty, "<br />");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write(comments);

            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            if (this.IsValidDateFormat(this.mDateFormatValue))
            {
                writer.Write(Localization.GetString("PostedAt", WikiModuleBase.SharedResources) + " " + postDate.ToString(this.mDateFormatValue));
            }
            else
            {
                writer.Write(Localization.GetString("PostedAt", WikiModuleBase.SharedResources) + " " + postDate.ToString(CultureInfo.CurrentCulture));
            }

            writer.RenderEndTag();
            writer.RenderEndTag();

            if (this.mIsAdminValue)
            {
                ////add delete link here.
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.AddAttribute(HtmlTextWriterAttribute.Href, this.BuildDeleteQueryString(commentId));
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, this.BuildDeleteConfirm(Localization.GetString("ConfirmDelete", WikiModuleBase.SharedResources)));
                writer.RenderBeginTag(HtmlTextWriterTag.A);

                ////writer.Write("Delete Comment")
                writer.Write(Localization.GetString("DeleteComment", WikiModuleBase.SharedResources));
                writer.RenderEndTag();
                writer.RenderEndTag();
                writer.RenderEndTag();
            }

            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write("<hr align=\"left\" width=\"90%\" size=\"1\" noshade=\"noshade\" />");
            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        /// <summary>
        /// Builds the delete confirm JavaScript function
        /// </summary>
        /// <param name="statement">The statement to show</param>
        /// <returns>returns the delete confirmation action</returns>
        private string BuildDeleteConfirm(string statement)
        {
            return string.Concat("return confirm('", statement, "');");
        }

        /// <summary>
        /// Builds the delete query string.
        /// </summary>
        /// <param name="commentId">The comment unique identifier.</param>
        /// <returns>The URL requested</returns>
        private string BuildDeleteQueryString(int commentId)
        {
            ////Build correct url
            string href = Context.Request.Url.ToString();
            href = href + "&cid=" + commentId.ToString();

            return href;
        }

        /// <summary>
        /// Determines whether [is valid date format] [the specified format].
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>True of False based on the correct date format.</returns>
        private bool IsValidDateFormat(string format)
        {
            try
            {
                string f = DateTime.Now.ToString(format);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Renders the HTML opening tag of the <see cref="T:System.Web.UI.WebControls.Table" />
        /// control to the specified writer.
        /// </summary>
        /// <param name="writer">An <see cref="T:System.Web.UI.HtmlTextWriter" /> that represents
        /// the output stream to render HTML content on the client.</param>
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            ////writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, Me.CellSpacing.ToString)
            ////writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, Me.CellPadding.ToString)
            ////writer.AddAttribute(HtmlTextWriterAttribute.Width, Me.Width.ToString)
            ////writer.AddAttribute(HtmlTextWriterAttribute.Height, Me.Height.ToString)
            writer.AddAttribute(HtmlTextWriterAttribute.Class, this.CssClass.ToString());
            ////writer.AddAttribute(HtmlTextWriterAttribute.Bgcolor, ColorTranslator.ToHtml(Me.BackColor))
            ////writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, Me.BorderWidth.ToString)
            ////writer.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, Me.BorderStyle.ToString)
            ////writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, Me.BorderWidth.ToString)
            ////writer.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, ColorTranslator.ToHtml(Me.BorderColor))
            writer.RenderBeginTag(this.TagName);
        }

        /// <summary>
        /// Renders the HTML closing tag of the control into the specified writer. This method is
        /// used primarily by control developers.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter" /> that represents the
        /// output stream to render HTML content on the client.</param>
        public override void RenderEndTag(HtmlTextWriter writer)
        {
            base.RenderEndTag(writer);
            if (this.Context != null)
            {
                int i = 0;
                while (i < this.mBreakCountValue)
                {
                    writer.Write("<br />");
                    writer.WriteLine(string.Empty);
                    System.Math.Min(System.Threading.Interlocked.Increment(ref i), i - 1);
                }
            }
        }

        /// <summary>
        /// Writes the errors.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="message">The message.</param>
        public static void WriteErrors(HtmlTextWriter writer, string message)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "NormalRed");
            ////writer.AddStyleAttribute(HtmlTextWriterStyle.FontFamily, "Arial,helvetica")
            ////writer.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "10pt")
            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.Write(message);
            writer.RenderEndTag();
            writer.RenderBeginTag(HtmlTextWriterTag.Br);
            writer.RenderEndTag();
        }

        #endregion Methods
    }
}