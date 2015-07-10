#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="Setting.cs" company="DNN Corp®">
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
using System.ComponentModel.DataAnnotations;
using System.Web.Caching;

namespace DotNetNuke.Wiki.BusinessObjects.Models
{
    /// <summary>
    /// The Settings Model base on the Setting Table
    /// </summary>
    [TableName("Wiki_Settings")]
    //// Setup the primary key for table
    [PrimaryKey("SettingId", AutoIncrement = true)]
    //// Configure caching using PetaPoco
    [Cacheable("Wiki_Settings", CacheItemPriority.Default, 20)]
    public class Setting
    {
        #region "Variables"

        private string mCommentNotifyRoles;
        private string mContentEditorRoles;

        #endregion "Variables"

        #region "Properties"

        /// <summary>
        /// Gets or sets the setting unique identifier.
        /// </summary>
        /// <value>The setting unique identifier.</value>
        public int SettingId { get; set; }

        /// <summary>
        /// Gets or sets the module unique identifier.
        /// </summary>
        /// <value>The module unique identifier.</value>
        public int ModuleId { get; set; }

        /// <summary>
        /// Gets or sets the content editor roles.
        /// </summary>
        /// <value>The content editor roles.</value>
        [Required]
        [StringLength(255)]
        public string ContentEditorRoles
        {
            get
            {
                return this.mContentEditorRoles;
            }

            set
            {
                this.mContentEditorRoles = value.TruncateString(255);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [allow discussions].
        /// </summary>
        /// <value><c>true</c> if [allow discussions]; otherwise, /c>.</value>
        public bool AllowDiscussions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [allow ratings].
        /// </summary>
        /// <value><c>true</c> if [allow ratings]; otherwise, /c>.</value>
        public bool AllowRatings { get; set; }

        /// <summary>
        /// Gets or sets the default discussion mode.
        /// </summary>
        /// <value>The default discussion mode.</value>
        public bool? DefaultDiscussionMode { get; set; }

        /// <summary>
        /// Gets or sets the default rating mode.
        /// </summary>
        /// <value>The default rating mode.</value>
        public bool? DefaultRatingMode { get; set; }

        /// <summary>
        /// Gets or sets the comment notify roles.
        /// </summary>
        /// <value>The comment notify roles.</value>
        [StringLength(255)]
        public string CommentNotifyRoles
        {
            get
            {
                return this.mCommentNotifyRoles;
            }

            set
            {
                this.mCommentNotifyRoles = value.TruncateString(255);
            }
        }

        /// <summary>
        /// Gets or sets the comment notify users.
        /// </summary>
        /// <value>The comment notify users.</value>
        public bool? CommentNotifyUsers { get; set; }

        #endregion "Properties"
    }
}