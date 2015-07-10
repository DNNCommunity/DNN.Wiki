#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="Ratings.ascx.cs" company="DNN Corp®">
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
using DotNetNuke.Wiki.BusinessObjects.Models;
using DotNetNuke.Wiki.Utilities;
using System;
using System.Web;
using System.Web.UI.WebControls;

namespace DotNetNuke.Wiki.Views
{
    /// <summary>
    /// Ratings class based on the Wiki Module Base class
    /// </summary>
    public partial class Ratings : WikiModuleBase
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Ratings"/> class.
        /// </summary>
        public Ratings()
        {
            this.PreRender += this.Page_PreRender;
            this.Load += this.Page_Load;
        }

        #endregion Constructor

        #region Variables

        private WikiModuleBase mModule;
        private Topic mTopic;

        #endregion Variables

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether [has voted].
        /// </summary>
        /// <value><c>true</c> if [has voted]; otherwise, /c>.</value>
        public bool HasVoted
        {
            get
            {
                if (Request.Cookies["WikiRating"] == null)
                {
                    return false;
                }
                else
                {
                    if (Request.Cookies["WikiRating"]["ContentID-" + this.InnerTopic.TopicID.ToString()] == null)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            set
            {
                if (Request.Cookies["WikiRating"] == null)
                {
                    Response.Cookies.Add(new HttpCookie("WikiRating"));
                }

                Response.Cookies["WikiRating"]["ContentID-" + this.InnerTopic.TopicID.ToString()] = "true";
                Response.Cookies["WikiRating"].Expires = DateTime.Now.AddYears(1);
            }
        }

        /// <summary>
        /// Gets the inner topic.
        /// </summary>
        /// <value>The inner topic.</value>
        public Topic InnerTopic
        {
            get
            {
                if (this.mTopic == null)
                {
                    System.Web.UI.Control uplevel = default(System.Web.UI.Control);
                    uplevel = this.Parent;
                    while (!(uplevel is WikiModuleBase))
                    {
                        uplevel = uplevel.Parent;
                    }

                    this.mTopic = ((WikiModuleBase)uplevel).CurrentTopic;
                }

                return this.mTopic;
            }
        }

        /// <summary>
        /// Gets my module.
        /// </summary>
        /// <value>My module.</value>
        public WikiModuleBase MyModule
        {
            get
            {
                if (this.mModule == null)
                {
                    System.Web.UI.Control uplevel = default(System.Web.UI.Control);
                    uplevel = this.Parent;
                    while (!(uplevel is WikiModuleBase))
                    {
                        uplevel = uplevel.Parent;
                    }

                    this.mModule = (WikiModuleBase)uplevel;
                    this.mModule.ModuleConfiguration = this.ModuleConfiguration;
                }

                return this.mModule;
            }
        }

        #endregion Properties

        #region Events

        /// <summary>
        /// Handles the Click event of the Submit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        protected void BtnSubmit_Click(object sender, System.EventArgs e)
        {
            bool save = false;
            save = false;
            if (this.rating1.Checked)
            {
                this.InnerTopic.RatingOneCount = this.InnerTopic.RatingOneCount + 1;
                save = true;
            }
            else if (this.rating2.Checked)
            {
                this.InnerTopic.RatingTwoCount = this.InnerTopic.RatingTwoCount + 1;
                save = true;
            }
            else if (this.rating3.Checked)
            {
                this.InnerTopic.RatingThreeCount = this.InnerTopic.RatingThreeCount + 1;
                save = true;
            }
            else if (this.rating4.Checked)
            {
                this.InnerTopic.RatingFourCount = this.InnerTopic.RatingFourCount + 1;
                save = true;
            }
            else if (this.rating5.Checked)
            {
                this.InnerTopic.RatingFiveCount = this.InnerTopic.RatingFiveCount + 1;
                save = true;
            }

            if (save)
            {
                TopicBo.Update(this.InnerTopic);
            }

            this.HasVoted = true;
            this.DisplayHasVoted();
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event
        /// data.</param>
        protected new void Page_Load(object sender, System.EventArgs e)
        {
            this.LoadLocalization();

            if (this.Visible)
            {
                if (this.HasVoted)
                {
                    this.DisplayHasVoted();
                }
                else
                {
                    this.DisplayCanVote();
                }
            }
        }

        /// <summary>
        /// Handles the PreRender event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        private void Page_PreRender(object sender, System.EventArgs e)
        {
            if (this.Visible)
            {
                if (this.InnerTopic.FivePointRatingsRecorded > 0)
                {
                    this.lblAverageRating.Text = this.InnerTopic.FivePointAverage.ToString("#.#");
                    this.lblRatingCount.Text = string.Format(
                        Localization.GetString(
                        "RatingsNumberOf", this.RouterResourceFile),
                        this.InnerTopic.FivePointRatingsRecorded.ToString());

                    int i = 0;
                    i = 0;
                    for (i = 0; i <= 4; i++)
                    {
                        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                        img.ImageUrl = this.DNNWikiModuleRootPath + "/Resources/images/bcImage.gif";
                        img.Width = Unit.Pixel(10);

                        int currentCount = 0;
                        switch (i)
                        {
                            case 0:
                                currentCount = this.InnerTopic.RatingOneCount;
                                break;

                            case 1:
                                currentCount = this.InnerTopic.RatingTwoCount;
                                break;

                            case 2:
                                currentCount = this.InnerTopic.RatingThreeCount;
                                break;

                            case 3:
                                currentCount = this.InnerTopic.RatingFourCount;
                                break;

                            case 4:
                                currentCount = this.InnerTopic.RatingFiveCount;
                                break;
                        }

                        img.Height = Unit.Pixel(Convert.ToInt32(25f * (Convert.ToDouble(currentCount) / Convert.ToDouble(this.InnerTopic.FivePointRatingsRecorded))));
                        img.AlternateText = currentCount.ToString();
                        this.RatingsGraphTable.Rows[0].Cells[i].Controls.Add(img);
                    }
                }
                else
                {
                    this.lblAverageRating.Text = Localization.GetString("RatingsNotRatedYet", this.RouterResourceFile);
                    this.lblRatingCount.Text = string.Format(Localization.GetString("RatingsNumberOf", this.RouterResourceFile), "0");

                    this.RatingsGraphTable.Visible = false;
                }
            }
        }

        #endregion Events

        #region Methods

        /// <summary>
        /// Displays the can vote.
        /// </summary>
        private void DisplayCanVote()
        {
            this.pnlCastVote.Visible = true;
            this.pnlVoteCast.Visible = false;
        }

        /// <summary>
        /// Displays the has voted.
        /// </summary>
        private void DisplayHasVoted()
        {
            this.pnlCastVote.Visible = false;
            this.pnlVoteCast.Visible = true;
        }

        /// <summary>
        /// Loads the localization.
        /// </summary>
        private void LoadLocalization()
        {
            this.RatePagelbl.Text = Localization.GetString("RatingsRateThisPage", this.RouterResourceFile);
            this.LowRating.Text = Localization.GetString("RatingsLowRating", this.RouterResourceFile);
            this.HighRating.Text = Localization.GetString("RatingsHighRating", this.RouterResourceFile);
            this.lblAverageRatingMessage.Text = Localization.GetString("RatingsAverageRatingTitle", this.RouterResourceFile);
            this.lblVoteCastMessage.Text = Localization.GetString("RatingsPageRated", this.RouterResourceFile);
            this.btnSubmit.Text = Localization.GetString("RatingsSubmitRating", this.RouterResourceFile);
        }

        #endregion Methods
    }
}