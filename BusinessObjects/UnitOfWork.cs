#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="UnitOfWork.cs" company="DNN Corp®">
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

using DotNetNuke.Data;
using DotNetNuke.Entities.Users;
using System;
using System.Web;

namespace DotNetNuke.Wiki.BusinessObjects
{
    /// <summary>
    /// The Unit Of Work Business Object
    /// </summary>
    public class UnitOfWork : IDisposable
    {
        #region Variables

        private UserInfo mCurrentUser = null;
        private IDataContext mDataContext;
        private bool mDisposed = false;

        #endregion Variables

        #region Properties

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>The context.</value>
        internal IDataContext Context
        {
            get
            {
                return this.mDataContext;
            }
        }

        /// <summary>
        /// Gets the current user.
        /// </summary>
        /// <value>The current user.</value>
        internal UserInfo CurrentUser
        {
            get
            {
                return this.mCurrentUser;
            }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        public UnitOfWork()
        {
            this.mDataContext = DataContext.Instance();

            if (HttpContext.Current != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                this.mCurrentUser = UserController.GetCurrentUserInfo();
            }
        }

        #endregion Constructor

        #region transaction

        /// <summary>
        /// Starts a new transaction
        /// </summary>
        public void BeginTransaction()
        {
            this.mDataContext.BeginTransaction();
        }

        /// <summary>
        /// Commits a transaction
        /// </summary>
        public void CommitTransaction()
        {
            this.mDataContext.Commit();
        }

        /// <summary>
        /// Rolls back a transaction
        /// </summary>
        public void RollbackTransaction()
        {
            this.mDataContext.RollbackTransaction();
        }

        #endregion transaction

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            // This object will be cleaned up by the Dispose method. Therefore, you should call
            // GC.SupressFinalize to take this object off the finalization queue and prevent
            // finalization code for this object from executing a second time.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.mDisposed)
            {
                // If disposing equals true, dispose all managed and unmanaged resources.
                if (disposing)
                {
                    // Clean up all managed resources
                    if (this.mDataContext != null)
                    {
                        (this.mDataContext as IDisposable).Dispose();
                        this.mDataContext = null;
                    }

                    this.mCurrentUser = null;
                }

                // Clean up all native resources

                // Note disposing has been done.
                this.mDisposed = true;
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        ~UnitOfWork()
        {
            this.Dispose(false);
        }

        #endregion IDisposable Members
    }
}