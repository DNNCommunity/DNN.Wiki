#region Copyright

// --------------------------------------------------------------------------------------------------------
// <copyright file="_AbstractValidationException.cs" company="DNN Corp®"> DNN Corp® -
// http: //www.dnnsoftware.com Copyright (c) 2002-2013 by DNN Corp®
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
//// ------------------------------------------------------------------------------------------------------

#endregion Copyright

using DotNetNuke.Wiki.Utilities;
using System;

namespace DotNetNuke.Wiki.BusinessObjects
{
    /// <summary>
    /// Generic abstract class for handling Business objects error
    /// </summary>
    /// <typeparam name="E">will be replaced by a enumeration referring the type of errors a
    /// business object can throw on a crud operation</typeparam>
    public abstract class _AbstractValidationException<E> : Exception
    {
        #region "Variables"

        private SharedEnum.CrudOperation crudOperationEnum;
        private E crudException;

        #endregion "Variables"

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="_AbstractValidationException{E}"/> class.
        /// </summary>
        /// <param name="crudOperation">The crud operation.</param>
        /// <param name="crudError">The crud error.</param>
        public _AbstractValidationException(SharedEnum.CrudOperation crudOperation, E crudError)
            : base(string.Empty)
        {
            this.crudOperationEnum = crudOperation;
            this.crudException = crudError;
        }

        #endregion Constructor

        #region "Properties"

        /// <summary>
        /// Gets the crud operation error.
        /// </summary>
        /// <value>The crud operation.</value>
        public SharedEnum.CrudOperation CrudOperation
        {
            get
            {
                return this.crudOperationEnum;
            }
        }

        /// <summary>
        /// Gets the crud error that occurred.
        /// </summary>
        /// <value>The crud error.</value>
        public E CrudError
        {
            get
            {
                return this.crudException;
            }
        }

        #endregion "Properties"
    }
}