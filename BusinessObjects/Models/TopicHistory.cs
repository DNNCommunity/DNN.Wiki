#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="TopicHistory.cs" company="DNN Corp®">
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
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using DotNetNuke.Wiki.Extensions;
using DotNetNuke.Wiki.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Caching;

namespace DotNetNuke.Wiki.BusinessObjects.Models
{
    /// <summary>
    /// Topic History Model
    /// </summary>
    [TableName("Wiki_TopicHistory")]
    //// setup the primary key for table
    [PrimaryKey("TopicHistoryId", AutoIncrement = true)]
    //// configure caching using PetaPoco
    [Cacheable("Wiki_TopicHistory", CacheItemPriority.Default, 20)]
    public class TopicHistory : WikiMarkup
    {
        #region "Variables"

        private string mContentValue;
        private string mNameValue;
        private string mUpdatedByValue;
        private string mTitleValue;
        private string mDescriptionValue;
        private string mKeywordsValue;

        #endregion "Variables"

        #region "Properties"

        /// <summary>
        /// Gets or sets the topic history unique identifier.
        /// </summary>
        /// <value>The topic history unique identifier.</value>
        public int TopicHistoryId { get; set; }

        /// <summary>
        /// Gets or sets the topic unique identifier.
        /// </summary>
        /// <value>The topic unique identifier.</value>
        public int TopicId { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public override string Content
        {
            get
            {
                return this.mContentValue;
            }

            set
            {
                this.mContentValue = value;
                if (this.CanUseWikiText)
                {
                    this.Cache = this.RenderedContent;
                }
            }
        }

        /// <summary>
        /// Gets or sets the cache.
        /// </summary>
        /// <value>The cache.</value>
        public string Cache { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [StringLength(255)]
        public string Name
        {
            get
            {
                return this.mNameValue;
            }

            set
            {
                this.mNameValue = value.TruncateString(255);
            }
        }

        /// <summary>
        /// Gets or sets the update date.
        /// </summary>
        /// <value>The update date.</value>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the updated by.
        /// </summary>
        /// <value>The updated by.</value>
        [Required]
        [StringLength(101)]
        public string UpdatedBy
        {
            get
            {
                return this.mUpdatedByValue;
            }

            set
            {
                this.mUpdatedByValue = value.TruncateString(101);
            }
        }

        /// <summary>
        /// Gets or sets the updated by user unique identifier.
        /// </summary>
        /// <value>The updated by user unique identifier.</value>
        public int UpdatedByUserID { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        [StringLength(256)]
        public string Title
        {
            get
            {
                return this.mTitleValue;
            }

            set
            {
                this.mTitleValue = value.TruncateString(256);
            }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [StringLength(500)]
        public string Description
        {
            get
            {
                return this.mDescriptionValue;
            }

            set
            {
                this.mDescriptionValue = value.TruncateString(500);
            }
        }

        /// <summary>
        /// Gets or sets the keywords.
        /// </summary>
        /// <value>The keywords.</value>
        [StringLength(500)]
        public string Keywords
        {
            get
            {
                return this.mKeywordsValue;
            }

            set
            {
                this.mKeywordsValue = value.TruncateString(500);
            }
        }

        /// <summary>
        /// Gets the updated by user name.
        /// </summary>
        /// <value>The updated by user name.</value>
        [IgnoreColumn]
        public string UpdatedByUsername
        {
            get
            {
                UserInfo user = UserController.GetUserById(
                    PortalController.GetCurrentPortalSettings().PortalId,
                    this.UpdatedByUserID);
                if (user != null)
                {
                    return user.DisplayName;
                }

                return string.Empty;
            }
        }

        #endregion "Properties"
    }
}