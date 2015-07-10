#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="IEnumerableExtensions.cs" company="DNN Corp®">
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

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace DotNetNuke.Wiki.Extensions
{
    /// <summary>
    /// Extensions for the IEnumerable class
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Converts a IEnumerable collection of T items to a DataTable
        /// </summary>
        /// <typeparam name="T">the generic type</typeparam>
        /// <param name="collection">the collection to convert</param>
        /// <param name="tableName">the table name</param>
        /// <returns>returns a Data table</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> collection, string tableName)
        {
            DataTable tbl = ToDataTable(collection);
            tbl.TableName = tableName;
            return tbl;
        }

        /// <summary>
        /// Automatics the data table.
        /// </summary>
        /// <typeparam name="T">the type of the collection values</typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns>A DataTable</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> collection)
        {
            DataTable dt = new DataTable();
            Type t = typeof(T);
            PropertyInfo[] pia = t.GetProperties();

            // Create the columns in the DataTable.
            foreach (PropertyInfo pi in pia)
            {
                dt.Columns.Add(pi.Name, pi.PropertyType);
            }

            // Populate the table.
            foreach (T item in collection)
            {
                DataRow dr = dt.NewRow();
                dr.BeginEdit();
                foreach (PropertyInfo pi in pia)
                {
                    dr[pi.Name] = pi.GetValue(item, null);
                }

                dr.EndEdit();
                dt.Rows.Add(dr);
            }

            return dt;
        }
    }
}