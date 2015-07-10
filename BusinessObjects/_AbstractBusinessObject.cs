#region Copyright

// <copyright file="_AbstractBusinessObject.cs" company="DNN Corp®"> DNN Corp® -
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
////

#endregion Copyright

using DotNetNuke.Data;
using DotNetNuke.Wiki.Interfaces;
using DotNetNuke.Wiki.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DotNetNuke.Wiki.BusinessObjects
{
    /// <summary>
    /// Abstract Business Object Class
    /// </summary>
    /// <typeparam name="T">Generic type for the Model entity.</typeparam> <typeparam name="I">Type
    /// of the primary key for the T model.</typeparam>
    public abstract class _AbstractBusinessObject<T, I> : IBusinessObject<T, I> where T : class
    {
        #region "Variables"

        private readonly IRepository<T> mRepositoryInterface;

        private IDataContext mDatabaseContextObject;

        #endregion "Variables"

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="_AbstractBusinessObject{T, I}"/> class.
        /// </summary>
        /// <param name="databaseContext">The _context.</param>
        /// <exception cref="System.ArgumentNullException">Context error</exception>
        public _AbstractBusinessObject(IDataContext databaseContext)
        {
            if (databaseContext == null)
            {
                throw new ArgumentNullException("Context");
            }

            this.mDatabaseContextObject = databaseContext;
            this.mRepositoryInterface = this.mDatabaseContextObject.GetRepository<T>();
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Gets the data base owner.
        /// </summary>
        /// <value>The data base owner.</value>
        internal string DataBaseOwner
        {
            get
            {
                return DataProvider.Instance().DatabaseOwner;
            }
        }

        /// <summary>
        /// Gets the object qualifier.
        /// </summary>
        /// <value>The object qualifier.</value>
        internal string ObjectQualifier
        {
            get
            {
                return DataProvider.Instance().ObjectQualifier;
            }
        }

        /// <summary>
        /// Gets or sets the database context.
        /// </summary>
        /// <value>The database context.</value>
        public IDataContext DatabaseContext
        {
            get { return this.mDatabaseContextObject; }
            set { this.mDatabaseContextObject = value; }
        }

        #endregion Properties

        #region IbusinessObject<T> Members

        /// <summary>
        /// Based on the user permissions, filters the collection of T elements to return
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>returns collection of T elements, filtered by the user access</returns>
        public virtual T FilterByAccess(T entity)
        {
            return entity;
        }

        /// <summary>
        /// Based on the user permissions, filters the collection of T elements to return
        /// </summary>
        /// <param name="collection">collection of T elements</param>
        /// <returns>returns collection of T elements, filtered by the user access</returns>
        public virtual IEnumerable<T> FilterByAccess(IEnumerable<T> collection)
        {
            return collection;
        }

        /// <summary>
        /// Creates a new entity, but before creating it, parses it by calling the
        /// ParseUserAbleToInsert method
        /// </summary>
        /// <param name="entity">entity to create</param>
        /// <returns>returns the entity that was created</returns>
        public virtual T Add(T entity)
        {
            try
            {
                this.ParseUserAbleToInsert(entity);

                this.OnBeforeInsertOperation(entity);

                this.RepositoryAdd(ref entity);
            }
            catch (SqlException exc)
            {
                this.Entity_EvaluateSqlException(exc, SharedEnum.CrudOperation.Insert);
            }

            return entity;
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Entity that was passed in.</returns>
        public virtual T Delete(T entity)
        {
            try
            {
                this.ParseUserAbleToDelete(entity);

                this.OnBeforeDeleteOperation(entity);

                this.RepositoryDelete(ref entity);
            }
            catch (SqlException exc)
            {
                this.Entity_EvaluateSqlException(exc, SharedEnum.CrudOperation.Insert);
            }

            return entity;
        }

        /// <summary>
        /// Updates an entity, but before updating it, parses it by calling the
        /// ParseUserAbleToUpdate method
        /// </summary>
        /// <param name="entity">Entity being passed in</param>
        /// <returns>returns the entity that was updated</returns>
        public virtual T Update(T entity)
        {
            try
            {
                this.ParseUserAbleToUpdate(entity);

                this.OnBeforeUpdateOperation(entity);

                this.RepositoryUpdate(ref entity);
            }
            catch (SqlException exc)
            {
                this.Entity_EvaluateSqlException(exc, SharedEnum.CrudOperation.Update);
            }

            return entity;
        }

        /// <summary>
        /// Collects all entities
        /// </summary>
        /// <returns>returns collection of entities</returns>
        public virtual IEnumerable<T> GetAll()
        {
            return this.FilterByAccess(this.mRepositoryInterface.Get());
        }

        /// <summary>
        /// Collects a specific entity based on a condition
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <returns>returns a specific entity</returns>
        public virtual T Get(I id)
        {
            return this.FilterByAccess(this.mRepositoryInterface.GetById<I>(id));
        }

        /// <summary>
        /// Filters for specific data in the database
        /// </summary>
        /// <param name="sql">SQL query</param>
        /// <param name="args">data for the SQL query</param>
        /// <returns>returns i enumerable of T items</returns>
        public IEnumerable<T> Find(string sql, params object[] args)
        {
            return this.mDatabaseContextObject.ExecuteQuery<T>(CommandType.Text, sql, args);
        }

        /// <summary>
        /// Called when a insert operation is going to be done against the database, so the entity
        /// to be created, can be changed before being submitted
        /// </summary>
        /// <param name="entity">The entity being passed in</param>
        internal virtual void OnBeforeInsertOperation(T entity)
        {
        }

        /// <summary>
        /// Called when a insert operation is going to be done against the database, so the entity
        /// to be created, can be changed before being submitted
        /// </summary>
        /// <param name="entity">The entity being passed in</param>
        internal virtual void OnBeforeUpdateOperation(T entity)
        {
        }

        /// <summary>
        /// Called when a insert operation is going to be done against the database, so the entity
        /// to be created, can be changed before being submitted
        /// </summary>
        /// <param name="entity">The entity being passed in</param>
        internal virtual void OnBeforeDeleteOperation(T entity)
        {
        }

        /// <summary>
        /// Creates a new entity using the repository interface, this method should only be
        /// overridden if the insertion mechanism in the database has to be changed
        /// </summary>
        /// <param name="entity">entity to create</param>
        internal virtual void RepositoryAdd(ref T entity)
        {
            this.mRepositoryInterface.Insert(entity);
        }

        /// <summary>
        /// Deletes an entity using the repository interface, this method should only be overridden
        /// if the deletion mechanism in the database has to be changed
        /// </summary>
        /// <param name="entity">entity to delete</param>
        internal virtual void RepositoryDelete(ref T entity)
        {
            this.mRepositoryInterface.Delete(entity);
        }

        /// <summary>
        /// Updates an entity using the repository interface, this method should only be overridden
        /// if the update mechanism in the database has to be changed
        /// </summary>
        /// <param name="entity">entity to delete</param>
        internal virtual void RepositoryUpdate(ref T entity)
        {
            this.mRepositoryInterface.Update(entity);
        }

        /// <summary>
        /// Method called when a SQL operation happens after a crud operation
        /// </summary>
        /// <param name="exc">The SQL Exception</param>
        /// <param name="crudOperation">The crud operation.</param>
        internal abstract void Entity_EvaluateSqlException(SqlException exc, SharedEnum.CrudOperation crudOperation);

        /// <summary>
        /// Parses the user able automatic insert.
        /// </summary>
        /// <param name="entity">The entity.</param>
        internal virtual void ParseUserAbleToInsert(T entity)
        {
        }

        /// <summary>
        /// Parses the user able automatic update.
        /// </summary>
        /// <param name="entity">The entity.</param>
        internal virtual void ParseUserAbleToUpdate(T entity)
        {
        }

        /// <summary>
        /// Parses the user able automatic delete.
        /// </summary>
        /// <param name="entity">The entity.</param>
        internal virtual void ParseUserAbleToDelete(T entity)
        {
        }

        #endregion IbusinessObject<T> Members
    }
}