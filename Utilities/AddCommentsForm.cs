#region Copyright

// <copyright file="AddCommentsForm.cs" company="DNN Corp®"> DNN Corp® - http://www.dnnsoftware.com
// Copyright (c) 2002-2013 by DNN Corp®
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute,
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
////

#endregion Copyright

using DotNetNuke.Security;
using DotNetNuke.Services.Localization;
using DotNetNuke.Wiki.BusinessObjects;
using DotNetNuke.Wiki.BusinessObjects.Models;
using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DotNetNuke.Wiki.Utilities
{
    /// <summary>
    /// Cancel Handler
    /// </summary>
    /// <param name="s">The arguments.</param>
    public delegate void CancelHandler(object s);

    /// <summary>
    /// Submit Handler
    /// </summary>
    /// <param name="s">The arguments.</param>
    public delegate void SubmitHandler(object s);

    /// <summary>
    /// Web Controls for the Comments form
    /// </summary>
    [ToolboxData("<{0}:AddCommentsForm runat=server></{0}:AddCommentsForm>")]
    public class AddCommentsForm : WebControl
    {
        #region Variables

        private bool mCheckCommentsValue = true;
        private bool mCheckEmailValue = true;
        private bool mCheckNameValue = true;
        private int mCommentsMaxLengthValue = 500;
        private bool mSuccessValue = true;

        #endregion Variables

        #region Events

        /// <summary>
        /// Occurs when [post canceled].
        /// </summary>
        public event CancelHandler PostCanceled;

        /// <summary>
        /// Occurs when [post submitted].
        /// </summary>
        public event SubmitHandler PostSubmitted;

        #endregion Events

        #region Controls

        /// <summary>
        /// The comments control
        /// </summary>
        private System.Web.UI.WebControls.TextBox txtComment;

        /// <summary>
        /// The email control
        /// </summary>
        private System.Web.UI.WebControls.TextBox txtEmail;

        /// <summary>
        /// The label parent unique identifier
        /// </summary>
        private System.Web.UI.WebControls.Label lblParentID;

        /// <summary>
        /// The name control
        /// </summary>
        private System.Web.UI.WebControls.TextBox txtName;

        /// <summary>
        /// The subscribe automatic notifications
        /// </summary>
        private System.Web.UI.WebControls.CheckBox chkSubscribeToNotifications;

        /// <summary>
        /// The with events field_ cancel button
        /// </summary>
        private System.Web.UI.WebControls.LinkButton lnkCancelButton;

        /// <summary>
        /// The with events field_ submit button
        /// </summary>
        private System.Web.UI.WebControls.LinkButton lnkSubmitButton;

        #endregion Controls

        //// private Entities.CommentsController commentBo = new Entities.CommentsController();

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether [check comments].
        /// </summary>
        /// <value><c>true</c> if [check comments]; otherwise, /c>.</value>
        [Description("Whether to check if the comments field is empty before submitting."), Category("Behaviour")]
        public bool CheckComments
        {
            get { return this.mCheckCommentsValue; }
            set { this.mCheckCommentsValue = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [check email].
        /// </summary>
        /// <value><c>true</c> if [check email]; otherwise, /c>.</value>
        [Description("Whether to check (clientside) the email address is valid before the user can submit the form."), Category("Behaviour")]
        public bool CheckEmail
        {
            get { return this.mCheckEmailValue; }
            set { this.mCheckEmailValue = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [check name].
        /// </summary>
        /// <value><c>true</c> if [check name]; otherwise, /c>.</value>
        [Description("Whether to check if the name field is empty before submitting."), Category("Behaviour")]
        public bool CheckName
        {
            get { return this.mCheckNameValue; }
            set { this.mCheckNameValue = value; }
        }

        /// <summary>
        /// Gets or sets the maximum length of the comments.
        /// </summary>
        /// <value>The maximum length of the comments.</value>
        [Description("The maximum length (in characters) the comment can be. Enter 0 for unlimited length."), Category("Behaviour")]
        public int CommentsMaxLength
        {
            get { return this.mCommentsMaxLengthValue; }
            set { this.mCommentsMaxLengthValue = value; }
        }

        /// <summary>
        /// Gets or sets the comment text.
        /// </summary>
        /// <value>The comment text.</value>
        public string CommentText
        {
            get { return this.txtComment.Text; }
            set { this.txtComment.Text = value; }
        }

        /// <summary>
        /// Gets or sets the email text.
        /// </summary>
        /// <value>The email text.</value>
        public string EmailText
        {
            get { return this.txtEmail.Text; }
            set { this.txtEmail.Text = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable comment].
        /// </summary>
        /// <value><c>true</c> if [enable comment]; otherwise, /c>.</value>
        public bool EnableComment
        {
            get { return this.txtComment.Enabled; }
            set { this.txtComment.Enabled = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable email].
        /// </summary>
        /// <value><c>true</c> if [enable email]; otherwise, /c>.</value>
        public bool EnableEmail
        {
            get { return this.txtEmail.Enabled; }
            set { this.txtEmail.Enabled = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable name].
        /// </summary>
        /// <value><c>true</c> if [enable name]; otherwise, /c>.</value>
        public bool EnableName
        {
            get { return this.txtName.Enabled; }
            set { this.txtName.Enabled = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable notify].
        /// </summary>
        /// <value><c>true</c> if [enable notify]; otherwise, /c>.</value>
        public bool EnableNotify
        {
            get { return this.chkSubscribeToNotifications.Visible; }
            set { this.chkSubscribeToNotifications.Visible = value; }
        }

        /// <summary>
        /// Gets or sets the name text.
        /// </summary>
        /// <value>The name text.</value>
        public string NameText
        {
            get { return this.txtName.Text; }
            set { this.txtName.Text = value; }
        }

        /// <summary>
        /// Gets or sets the parent unique identifier.
        /// </summary>
        /// <value>The parent unique identifier.</value>
        [Description("The id of the parent (page) the comment is being added to."), Category("Data")]
        public int ParentId
        {
            get { return Convert.ToInt32(this.lblParentID.Text); }
            set { this.lblParentID.Text = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets the cancel button.
        /// </summary>
        /// <value>The cancel button.</value>
        protected System.Web.UI.WebControls.LinkButton CancelButton
        {
            get
            {
                return this.lnkCancelButton;
            }

            set
            {
                if (this.lnkCancelButton != null)
                {
                    this.lnkCancelButton.Click -= this.CancelButton_Click;
                }

                this.lnkCancelButton = value;

                if (this.lnkCancelButton != null)
                {
                    this.lnkCancelButton.Click += this.CancelButton_Click;
                }
            }
        }

        /// <summary>
        /// Gets or sets the submit button.
        /// </summary>
        /// <value>The submit button.</value>
        protected System.Web.UI.WebControls.LinkButton SubmitButton
        {
            get
            {
                return this.lnkSubmitButton;
            }

            set
            {
                if (this.lnkSubmitButton != null)
                {
                    this.lnkSubmitButton.Click -= this.SubmitButton_Click;
                }

                this.lnkSubmitButton = value;

                if (this.lnkSubmitButton != null)
                {
                    this.lnkSubmitButton.Click += this.SubmitButton_Click;
                }
            }
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

        #endregion Properties

        #region Methods

        /// <summary>
        /// Renders the HTML opening tag of the control to the specified writer. This method is used
        /// primarily by control developers.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter" /> that represents the
        /// output stream to render HTML content on the client.</param>
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (this.mCheckCommentsValue || this.mCheckEmailValue || this.mCheckNameValue || this.mCommentsMaxLengthValue > 0)
            {
                writer.WriteLine("<script language=\"JavaScript\">");
                writer.WriteLine("function wikiFormCheck(form)");
                writer.WriteLine("{");
                string clause = string.Empty;
                if (this.mCheckNameValue)
                {
                    writer.WriteLine("\tif ( form." + this.txtName.ClientID + ".value == \"\" )");
                    writer.WriteLine("\t{");

                    writer.WriteLine("\t\talert(\"" + Localization.GetString("EnterAName.Text", WikiModuleBase.SharedResources) + "\");");
                    writer.WriteLine("\t\treturn false;");
                    writer.WriteLine("\t}");
                    clause = "else ";
                }

                if (this.mCheckEmailValue)
                {
                    writer.WriteLine("\t" + clause + "if ( form." + this.txtEmail.ClientID + ".value == \"\" )");
                    writer.WriteLine("\t{");
                    writer.WriteLine("\t\talert(\"" + Localization.GetString("EnterAnEmailAddress.Text", WikiModuleBase.SharedResources) + "\");");
                    writer.WriteLine("\t\treturn false;");
                    writer.WriteLine("\t}");
                    clause = "else ";
                }

                if (this.mCheckCommentsValue)
                {
                    writer.WriteLine("\t" + clause + "if ( form." + this.txtComment.ClientID + ".value == \"\" )");
                    writer.WriteLine("\t{");
                    writer.WriteLine("\t\talert(\"" + Localization.GetString("EnterSomeComments.Text", WikiModuleBase.SharedResources) + "\");");
                    writer.WriteLine("\t\treturn false;");
                    writer.WriteLine("\t}");
                    clause = "else ";
                }

                if (this.mCommentsMaxLengthValue > 0)
                {
                    writer.WriteLine("\t" + clause + "if ( form." + this.txtComment.ClientID + ".value.length > " + this.mCommentsMaxLengthValue + " )");
                    writer.WriteLine("\t{");
                    writer.WriteLine("\t\talert(\"" + string.Format(Localization.GetString("EnterAName.Text", WikiModuleBase.SharedResources), this.mCommentsMaxLengthValue) + "\");");
                    writer.WriteLine("\t\treturn false;");
                    writer.WriteLine("\t}");
                }

                writer.WriteLine("\t");
                writer.WriteLine("\treturn true;");
                writer.WriteLine("}");
                writer.WriteLine("</script>");
            }
            ////MyBase.RenderBeginTag(writer)
        }

        /// <summary>
        /// Renders the contents of the control to the specified writer. This method is used
        /// primarily by control developers.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter" /> that represents the
        /// output stream to render HTML content on the client.</param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            base.RenderBeginTag(writer);
            if (this.mCheckCommentsValue || this.mCheckEmailValue || this.mCheckNameValue)
            {
                this.SubmitButton.Attributes.Add(HtmlTextWriterAttribute.Onclick.ToString(), "return wikiFormCheck(this.form)");
            }

            if (!this.mSuccessValue)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "NormalRed");
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                writer.Write(Localization.GetString("Failed.Text", WikiModuleBase.SharedResources));
                writer.RenderEndTag();
                writer.RenderEndTag();
                writer.RenderEndTag();
            }

            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "NormalBold");
            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.Write(Localization.GetString("Name", WikiModuleBase.SharedResources));
            writer.RenderBeginTag(HtmlTextWriterTag.Br);
            this.txtName.RenderControl(writer);
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            ////writer.RenderEndTag()

            ////writer.RenderBeginTag(HtmlTextWriterTag.Tr)
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "NormalBold");
            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.Write(Localization.GetString("Email", WikiModuleBase.SharedResources));
            writer.RenderBeginTag(HtmlTextWriterTag.Br);
            this.txtEmail.RenderControl(writer);
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "2");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "NormalBold");

            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.Write(Localization.GetString("Comments", WikiModuleBase.SharedResources));
            writer.RenderBeginTag(HtmlTextWriterTag.Br);
            this.txtComment.RenderControl(writer);
            writer.RenderBeginTag(HtmlTextWriterTag.Br);
            this.chkSubscribeToNotifications.RenderControl(writer);

            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write(" | ");
            this.SubmitButton.RenderControl(writer);
            writer.Write(" | ");
            this.CancelButton.RenderControl(writer);
            writer.Write(" | ");
            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        /// <summary>
        /// Handles the Initialize event of the AddCommentsForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        private void AddCommentsForm_Init(object sender, System.EventArgs e)
        {
            this.txtName = new System.Web.UI.WebControls.TextBox();
            this.txtName.ID = "Name";
            this.txtName.EnableViewState = true;
            this.txtName.CssClass = "NormalTextBox";
            this.txtEmail = new System.Web.UI.WebControls.TextBox();
            this.txtEmail.ID = "Email";
            this.txtEmail.EnableViewState = true;
            this.txtEmail.CssClass = "NormalTextBox";
            this.txtComment = new System.Web.UI.WebControls.TextBox();
            this.txtComment.ID = "Comment";
            this.txtComment.EnableViewState = true;
            this.txtComment.CssClass = "NormalTextBox";
            this.txtComment.TextMode = TextBoxMode.MultiLine;
            this.txtComment.Width = new System.Web.UI.WebControls.Unit(350);
            this.txtComment.Height = new System.Web.UI.WebControls.Unit(50);
            this.txtComment.MaxLength = this.CommentsMaxLength;
            this.SubmitButton = new System.Web.UI.WebControls.LinkButton();
            this.SubmitButton.CssClass = "CommandButton";
            this.SubmitButton.Text = Localization.GetString("PostComment", WikiModuleBase.SharedResources);
            ////"Post Comment"
            this.CancelButton = new System.Web.UI.WebControls.LinkButton();
            this.CancelButton.CssClass = "CommandButton";
            this.CancelButton.Text = Localization.GetString("Cancel", WikiModuleBase.SharedResources);
            ////"Cancel"
            this.lblParentID = new System.Web.UI.WebControls.Label();
            this.lblParentID.ID = "CurrParent";
            this.lblParentID.Visible = false;
            this.lblParentID.EnableViewState = true;

            this.chkSubscribeToNotifications = new System.Web.UI.WebControls.CheckBox();
            this.chkSubscribeToNotifications.ID = "EmailNotify";
            this.chkSubscribeToNotifications.Text = Localization.GetString("CommentsNotification", WikiModuleBase.SharedResources);

            ////If EnableNotify = False Then SubscribeToNotifications.Visible = False Else SubscribeToNotifications.Visible = True

            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.txtComment);
            this.Controls.Add(this.SubmitButton);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.lblParentID);
            this.Controls.Add(this.chkSubscribeToNotifications);
        }

        /// <summary>
        /// Handles the Click event of the CancelButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        private void CancelButton_Click(object sender, System.EventArgs e)
        {
            this.txtName.Text = string.Empty;
            this.txtEmail.Text = string.Empty;
            this.txtComment.Text = string.Empty;
            if (this.PostCanceled != null)
            {
                this.PostCanceled(this);
            }
        }

        /// <summary>
        /// Handles the Click event of the SubmitButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        private void SubmitButton_Click(object sender, System.EventArgs e)
        {
            using (UnitOfWork uOw = new UnitOfWork())
            {
                var commentBo = new CommentBO(uOw);

                string commentText = this.txtComment.Text;
                DotNetNuke.Security.PortalSecurity objSec = new DotNetNuke.Security.PortalSecurity();

                if (commentText.Length > this.CommentsMaxLength)
                {
                    commentText = commentText.Substring(0, this.CommentsMaxLength);
                }
                ////4.8.3 has better control for NoMarkup
                var comment = new Comment
                {
                    ParentId = this.ParentId,
                    Name = objSec.InputFilter(this.txtName.Text, DotNetNuke.Security.PortalSecurity.FilterFlag.NoMarkup),
                    Email = objSec.InputFilter(this.txtEmail.Text, DotNetNuke.Security.PortalSecurity.FilterFlag.NoMarkup),
                    CommentText = objSec.InputFilter(commentText, PortalSecurity.FilterFlag.NoMarkup),
                    Ip = objSec.InputFilter(this.Context.Request.ServerVariables["REMOTE_ADDR"], DotNetNuke.Security.PortalSecurity.FilterFlag.NoMarkup),
                    EmailNotify = this.chkSubscribeToNotifications.Checked,
                    Datetime = DateTime.Now
                };
                comment = commentBo.Add(comment);

                ////send the notification
                var topic = new TopicBO(uOw).Get(this.ParentId);
                DNNUtils.SendNotifications(uOw, topic, comment.Name, comment.Email, comment.CommentText, comment.Ip);
                this.mSuccessValue = comment.CommentId > 0;

                if (this.mSuccessValue)
                {
                    this.txtName.Text = string.Empty;
                    this.txtEmail.Text = string.Empty;
                    this.txtComment.Text = string.Empty;
                    this.Context.Cache.Remove("WikiComments" + this.ParentId.ToString());
                    if (this.PostSubmitted != null)
                    {
                        this.PostSubmitted(this);
                    }
                }
            }
        }

        #endregion Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="AddCommentsForm"/> class.
        /// </summary>
        public AddCommentsForm()
        {
            this.Init += this.AddCommentsForm_Init;
        }
    }
}