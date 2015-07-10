#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="Comment.cs" company="DNN Corp®">
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

using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Wiki.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Caching;

namespace DotNetNuke.Wiki.BusinessObjects.Models
{
    /// <summary>
    /// Model for the Wiki_Comments table
    /// </summary>
    [TableName("Wiki_Comments")]
    ////setup the primary key for table
    [PrimaryKey("CommentId", AutoIncrement = true)]
    ////configure caching using PetaPoco
    [Cacheable("Wiki_Comments", CacheItemPriority.Default, 20)]
    public class Comment
    {
        #region Variables

        private string mNameValue;
        private string mEmailAddressValue;
        private string mCommentTextValue;
        private string mIpValue;

        #endregion Variables

        #region Properties

        /// <summary>
        /// Gets or sets the comment unique identifier.
        /// </summary>
        /// <value>The comment unique identifier.</value>
        public int CommentId { get; set; }

        /// <summary>
        /// Gets or sets the name of the comment.
        /// </summary>
        /// <value>The name.</value>
        [Required]
        [StringLength(64)]
        public string Name
        {
            get
            {
                return this.mNameValue;
            }

            set
            {
                this.mNameValue = value.TruncateString(64);
            }
        }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        [Required]
        [StringLength(64)]
        [DataType(DataType.EmailAddress)]
        public string Email
        {
            get
            {
                return this.mEmailAddressValue;
            }

            set
            {
                this.mEmailAddressValue = value.TruncateString(64);
            }
        }

        /// <summary>
        /// Gets or sets the comment text.
        /// </summary>
        /// <value>The comment text.</value>
        [Required]
        [StringLength(1024)]
        [ColumnName("Comment")]
        public string CommentText
        {
            get
            {
                return this.mCommentTextValue;
            }

            set
            {
                this.mCommentTextValue = value.TruncateString(1024);
            }
        }

        /// <summary>
        /// Gets or sets the date and time the comment was created.
        /// </summary>
        /// <value>The date and time of the comment.</value>
        public DateTime Datetime { get; set; }

        /// <summary>
        /// Gets or sets the IP address of the comment.
        /// </summary>
        /// <value>The IP.</value>
        [Required]
        [StringLength(50)]
        public string Ip
        {
            get
            {
                return this.mIpValue;
            }

            set
            {
                this.mIpValue = value.TruncateString(50);
            }
        }

        /// <summary>
        /// Gets or sets the parent unique identifier.
        /// </summary>
        /// <value>The parent unique identifier.</value>
        public int ParentId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [email notify].
        /// </summary>
        /// <value><c>true</c> if [email notify]; otherwise, /c>.</value>
        public bool EmailNotify { get; set; }

        #endregion Properties
    }
}