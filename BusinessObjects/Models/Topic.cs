#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="Topic.cs" company="DNN Corp®">
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
    /// Wiki Topic Model
    /// </summary>
    [TableName("Wiki_Topic")]
    //// Setup the primary key for table
    [PrimaryKey("TopicID", AutoIncrement = true)]
    //// Configure caching using PetaPoco
    [Cacheable("Wiki_Topics", CacheItemPriority.Default, 20)]
    //// Scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    [Scope("ModuleId")]
    public class Topic : WikiMarkup
    {
        #region "Variables"

        private string mContentValue;
        private string mNameValue;
        private string mUpdateUserByValue;
        private string mDescriptionValue;
        private string mKeywordsValue;
        private string mTitleValue;

        #endregion "Variables"

        #region "Properties"

        /// <summary>
        /// Gets or sets the topic unique identifier.
        /// </summary>
        /// <value>The topic unique identifier.</value>
        public int TopicID { get; set; }

        /// <summary>
        /// Gets or sets the module unique identifier.
        /// </summary>
        /// <value>The module unique identifier.</value>
        public int ModuleId { get; set; }

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
        /// Gets or sets the Topic name.
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
                return this.mUpdateUserByValue;
            }

            set
            {
                this.mUpdateUserByValue = value.TruncateString(101);
            }
        }

        /// <summary>
        /// Gets or sets the updated by user unique identifier.
        /// </summary>
        /// <value>The updated by user unique identifier.</value>
        public int UpdatedByUserID { get; set; }

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
        /// Gets or sets the rating one count.
        /// </summary>
        /// <value>The rating one count.</value>
        public int RatingOneCount { get; set; }

        /// <summary>
        /// Gets or sets the rating two count.
        /// </summary>
        /// <value>The rating two count.</value>
        public int RatingTwoCount { get; set; }

        /// <summary>
        /// Gets or sets the rating three count.
        /// </summary>
        /// <value>The rating three count.</value>
        public int RatingThreeCount { get; set; }

        /// <summary>
        /// Gets or sets the rating four count.
        /// </summary>
        /// <value>The rating four count.</value>
        public int RatingFourCount { get; set; }

        /// <summary>
        /// Gets or sets the rating five count.
        /// </summary>
        /// <value>The rating five count.</value>
        public int RatingFiveCount { get; set; }

        /// <summary>
        /// Gets or sets the rating six count.
        /// </summary>
        /// <value>The rating six count.</value>
        public int RatingSixCount { get; set; }

        /// <summary>
        /// Gets or sets the rating seven count.
        /// </summary>
        /// <value>The rating seven count.</value>
        public int RatingSevenCount { get; set; }

        /// <summary>
        /// Gets or sets the rating eight count.
        /// </summary>
        /// <value>The rating eight count.</value>
        public int RatingEightCount { get; set; }

        /// <summary>
        /// Gets or sets the rating nine count.
        /// </summary>
        /// <value>The rating nine count.</value>
        public int RatingNineCount { get; set; }

        /// <summary>
        /// Gets or sets the rating ten count.
        /// </summary>
        /// <value>The rating ten count.</value>
        public int RatingTenCount { get; set; }

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

        /// <summary>
        /// Gets the ten point ratings recorded.
        /// </summary>
        /// <value>The ten point ratings recorded.</value>
        [IgnoreColumn]
        public int TenPointRatingsRecorded
        {
            get { return this.RatingOneCount + this.RatingTwoCount + this.RatingThreeCount + this.RatingFourCount + this.RatingFiveCount + this.RatingSixCount + this.RatingSevenCount + this.RatingEightCount + this.RatingNineCount + this.RatingTenCount; }
        }

        /// <summary>
        /// Gets the five point ratings recorded.
        /// </summary>
        /// <value>The five point ratings recorded.</value>
        [IgnoreColumn]
        public int FivePointRatingsRecorded
        {
            get { return this.RatingOneCount + this.RatingTwoCount + this.RatingThreeCount + this.RatingFourCount + this.RatingFiveCount; }
        }

        /// <summary>
        /// Gets the five point average.
        /// </summary>
        /// <value>The five point average.</value>
        [IgnoreColumn]
        public double FivePointAverage
        {
            get { return (Convert.ToDouble(this.RatingOneCount) + Convert.ToDouble(this.RatingTwoCount * 2) + Convert.ToDouble(this.RatingThreeCount * 3) + Convert.ToDouble(this.RatingFourCount * 4) + Convert.ToDouble(this.RatingFiveCount * 5)) / Convert.ToDouble(this.FivePointRatingsRecorded); }
        }

        /// <summary>
        /// Gets the ten point average.
        /// </summary>
        /// <value>The ten point average.</value>
        [IgnoreColumn]
        public double TenPointAverage
        {
            get { return (Convert.ToDouble(this.RatingOneCount) + Convert.ToDouble(this.RatingTwoCount * 2) + Convert.ToDouble(this.RatingThreeCount * 3) + Convert.ToDouble(this.RatingFourCount * 4) + Convert.ToDouble(this.RatingFiveCount * 5) + Convert.ToDouble(this.RatingSixCount * 6) + Convert.ToDouble(this.RatingSevenCount * 7) + Convert.ToDouble(this.RatingEightCount * 8) + Convert.ToDouble(this.RatingNineCount * 9) + Convert.ToDouble(this.RatingTenCount * 10)) / Convert.ToDouble(this.TenPointRatingsRecorded); }
        }

        #endregion "Properties"
    }
}