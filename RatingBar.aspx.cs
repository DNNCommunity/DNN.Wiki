#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="RatingBar.aspx.cs" company="DNN Corp®">
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

using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace DotNetNuke.Wiki
{
    /// <summary>
    /// This class represents the rating bar controller
    /// </summary>
    public partial class RatingBar : System.Web.UI.Page
    {
        #region Variables

        private Color backColor = Color.Silver;
        private Color foreColor = Color.Blue;
        private Color ratingBackColor = Color.White;

        #endregion Variables

        #region Events

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!((Request.QueryString["rating"] == null) | Request.QueryString["rating"].Equals("NaN")))
            {
                double ratingpoints = Convert.ToDouble(Request.QueryString["rating"]);
                int maxImageLength = 0;
                int maxImageHeight = 0;
                maxImageHeight = 10;
                //// pixels
                maxImageLength = 112;
                //// pixels
                int ratingValue = 0;
                ratingValue = Convert.ToInt32((ratingpoints / 5) * maxImageLength);
                Bitmap objBitmap = new Bitmap(maxImageLength, maxImageHeight);
                Graphics objGraphics = Graphics.FromImage(objBitmap);
                SolidBrush objBrushRating = new SolidBrush(this.foreColor);
                SolidBrush objBrushBorder = new SolidBrush(this.backColor);
                SolidBrush objBrushNoRate = new SolidBrush(this.ratingBackColor);
                SolidBrush objBrushRatingHighBorder = new SolidBrush(Color.FromArgb(this.MaxInt(Convert.ToInt32(this.foreColor.R) + 150), this.MaxInt(Convert.ToInt32(this.foreColor.G) + 150), this.MaxInt(Convert.ToInt32(this.foreColor.B) + 150)));
                SolidBrush objBrushRatingLowBorder = new SolidBrush(Color.FromArgb(this.MaxInt(Convert.ToInt32(this.foreColor.R) - 60), this.MaxInt(Convert.ToInt32(this.foreColor.G) - 60), this.MaxInt(Convert.ToInt32(this.foreColor.B) - 60)));
                SolidBrush objBrushBorderDark = new SolidBrush(Color.FromArgb(this.MaxInt(Convert.ToInt32(this.backColor.R) - 60), this.MaxInt(Convert.ToInt32(this.backColor.G) - 60), this.MaxInt(Convert.ToInt32(this.backColor.B) - 60)));
                objGraphics.FillRectangle(objBrushBorderDark, 0, 0, maxImageLength, maxImageHeight);
                objGraphics.FillRectangle(objBrushBorder, 1, 1, maxImageLength - 2, maxImageHeight - 2);
                objGraphics.FillRectangle(objBrushNoRate, 2, 2, maxImageLength - 3, maxImageHeight - 4);
                objGraphics.FillRectangle(objBrushRating, 2, 2, ratingValue - 3, maxImageHeight - 4);
                objGraphics.FillRectangle(objBrushRatingHighBorder, 2, 2, ratingValue - 3, 1);
                objGraphics.FillRectangle(objBrushRatingHighBorder, 2, 2, 1, maxImageHeight - 5);
                objGraphics.FillRectangle(objBrushRatingLowBorder, 2, maxImageHeight - 3, ratingValue - 3, 1);
                objGraphics.FillRectangle(objBrushRatingLowBorder, ratingValue - 2, 3, 1, maxImageHeight - 5);
                ////objGraphics.FillRectangle(objBrushBorder, 0, 0, 2 , MaxImageHeight)
                objGraphics.FillRectangle(objBrushBorder, 22, 1, 2, maxImageHeight - 2);
                objGraphics.FillRectangle(objBrushBorder, 44, 1, 2, maxImageHeight - 2);
                objGraphics.FillRectangle(objBrushBorder, 66, 1, 2, maxImageHeight - 2);
                objGraphics.FillRectangle(objBrushBorder, 88, 1, 2, maxImageHeight - 2);
                objGraphics.FillRectangle(objBrushBorder, 110, 1, 1, maxImageHeight - 2);
                Response.ContentType = "image/png";
                System.IO.MemoryStream imageStream = new System.IO.MemoryStream();
                objBitmap.Save(imageStream, ImageFormat.Png);
                imageStream.WriteTo(Response.OutputStream);
                objBitmap.Dispose();
                objGraphics.Dispose();
            }
            else
            {
                Bitmap objBitmap = new Bitmap(1, 1);
                Graphics objGraphics = Graphics.FromImage(objBitmap);
                SolidBrush objBrushRating = new SolidBrush(Color.Transparent);
                objGraphics.FillRectangle(objBrushRating, 0, 0, 1, 1);
                Response.ContentType = "image/png";
                System.IO.MemoryStream imageStream = new System.IO.MemoryStream();
                objBitmap.Save(imageStream, ImageFormat.Png);
                imageStream.WriteTo(Response.OutputStream);
                objBitmap.Dispose();
                objGraphics.Dispose();
            }
        }

        #endregion Events

        #region Aux Functions

        /// <summary>
        /// Sets the maximum integer value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The max integer value.</returns>
        public int MaxInt(int value)
        {
            if (value > 255)
            {
                return 255;
            }
            else
            {
                if (value < 0)
                {
                    return 0;
                }
                else
                {
                    return value;
                }
            }
        }

        #endregion Aux Functions
    }
}