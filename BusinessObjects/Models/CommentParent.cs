#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="CommentParent.cs" company="DNN Corp®">
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
    /// The comment parent class indicates the parent of the current comment
    /// </summary>
    [TableName("Wiki_CommentParents")]
    //// setup the primary key for table
    [PrimaryKey("CommentParentId", AutoIncrement = true)]
    //// configure caching using PetaPoco
    [Cacheable("Wiki_CommentParents", CacheItemPriority.Default, 20)]
    public class CommentParent
    {
        #region "Variables"

        /// <summary>
        /// The _name
        /// </summary>
        private string mNameValue;

        #endregion "Variables"

        #region "Properties"

        /// <summary>
        /// Gets or sets the comment parent unique identifier.
        /// </summary>
        /// <value>The comment parent unique identifier.</value>
        public int CommentParentId { get; set; }

        /// <summary>
        /// Gets or sets the name of the parent.
        /// </summary>
        /// <value>The parent name.</value>
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
                this.mNameValue = value.TruncateString(50);
            }
        }

        /// <summary>
        /// Gets or sets the parent unique identifier.
        /// </summary>
        /// <value>The parent unique identifier.</value>
        public int ParentId { get; set; }

        #endregion "Properties"
    }
}