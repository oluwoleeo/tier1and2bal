using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;
using System.Configuration;

namespace Tier1And2BalanceEnforcement
{
    public partial class SqlMapper
    {

        /// <summary>
        /// This will create insert statement using the property names of the passed
        /// Entity. 
        /// </summary>
        /// <param name="connection">IDbConnection instance</param>
        /// <param name="entityToInsert">Entity to insert</param>
        /// <param name="excludeFieldList">list of field names in the entity to be ignored, null otherwise</param>
        /// <param name="databaseTableName">the database table name, if not supplied, database table name is infered from the entity type name</param>
        /// <param name="fieldNameMapping">a dictionary that allows field name in the entity to different to the database table field name. 
        /// The entity field name is the key while the corresponding database table field name is the value, null otherwise</param>
        /// <param name="transaction">IDbTransaction object, null otherwise.</param>
        /// <returns></returns>
        public static int Insert<T>(this IDbConnection connection, T entityToInsert, string[] excludeFieldList = null, string databaseTableName = null, IDictionary<string, string> fieldNameMapping = null, IDbTransaction transaction = null)
        {
            PropertyInfo idField = null;
            var type = typeof(T);  // entityToInsert.GetType();
            var query = BuildInsert(type, out idField, excludeFieldList, databaseTableName, fieldNameMapping);
            int id = transaction == null ?
                       connection.Execute(query, entityToInsert) :
                       connection.Execute(query, entityToInsert, transaction: transaction);

            //if (idField != null)
            //{
            //    id = (int)(transaction == null ?
            //        connection.Query<Int64>("SELECT SCOPE_IDENTITY() AS Id").FirstOrDefault() :
            //        connection.Query<Int64>("SELECT SCOPE_IDENTITY() AS Id", transaction: transaction).FirstOrDefault());
            //    idField.SetValue(entityToInsert, id, null);
            //}
            return id;

        }

        /// <summary>
        /// This will create insert statement using the property names of the passed
        /// Entity. 
        /// </summary>
        /// <param name="connection">IDbConnection instance</param>
        /// <param name="entitiesToInsert">IENumerable of entities to insert</param>
        /// <param name="excludeFieldList">list of field names in the entity to be ignored, null otherwise</param>
        /// <param name="databaseTableName">the database table name, if not supplied, database table name is infered from the entity type name</param>
        /// <param name="fieldNameMapping">a dictionary that allows field name in the entity to different to the database table field name. 
        /// The entity field name is the key while the corresponding database table field name is the value, null otherwise</param>
        /// <param name="transaction">IDbTransaction object, null otherwise.</param>
        /// <returns></returns>
        public static int InsertMany<T>(this IDbConnection connection, IEnumerable<T> entitiesToInsert, string[] excludeFieldList = null, string databaseTableName = null, IDictionary<string, string> fieldNameMapping = null, IDbTransaction transaction = null)
        {
            PropertyInfo idField = null;
            var type = typeof(T);
            var query = BuildInsert(type, out idField, excludeFieldList, databaseTableName, fieldNameMapping);
            return transaction == null ?
                   connection.Execute(query, entitiesToInsert) :
                   connection.Execute(query, entitiesToInsert, transaction: transaction);
        }

        public static int Update<T>(this IDbConnection connection, T entityToUpdate, string[] excludeFieldList = null, string databaseTableName = null, IDictionary<string, string> fieldNameMapping = null, string[] primaryKeyList = null, IDbTransaction transaction = null)
        {
            if (primaryKeyList == null)
                throw new InvalidOperationException("Update cannot be performed without primary key(s)");

            var type = typeof(T);
            var query = BuildUpdate(type, excludeFieldList, databaseTableName, fieldNameMapping, primaryKeyList);
            return transaction == null ?
                   connection.Execute(query, entityToUpdate) :
                   connection.Execute(query, entityToUpdate, transaction: transaction);
        }

        public static int UpdateMany<T>(this IDbConnection connection, IEnumerable<T> entitiesToUpdate, string[] excludeFieldList = null, string databaseTableName = null, IDictionary<string, string> fieldNameMapping = null, string[] primaryKeyList = null, IDbTransaction transaction = null)
        {
            if (primaryKeyList == null)
                throw new InvalidOperationException("Update cannot be performed without primary key(s)");

            var type = typeof(T);
            var query = BuildUpdate(type, excludeFieldList, databaseTableName, fieldNameMapping, primaryKeyList);
            return transaction == null ?
                   connection.Execute(query, entitiesToUpdate) :
                   connection.Execute(query, entitiesToUpdate, transaction: transaction);
        }

        private static string BuildUpdate(Type type, string[] excludeFieldList = null, string databaseTableName = null, IDictionary<string, string> fieldNameMapping = null, string[] primaryKeyList = null)
        {
            const string updateTemplate = @"UPDATE {0} SET {1} WHERE {2}";
            var updateSet = new StringBuilder();
            var keySet = new StringBuilder();

            var properties = excludeFieldList == null ?
                             type.GetProperties().Select(p => p.Name).ToArray() :
                             type.GetProperties().Select(p => p.Name).ToArray().Except(excludeFieldList).ToArray();

            properties = properties.Except(primaryKeyList).ToArray();

            var status = false;
            if (fieldNameMapping != null)
            {
                for (var i = 0; i < properties.Length; i++)
                {
                    var temp = properties[i];
                    var temp2 = string.Empty;
                    status = fieldNameMapping.TryGetValue(temp, out temp2);
                    if (status)
                    {
                        properties[i] = temp2;
                    }
                    updateSet.AppendFormat("{0} = @{1}, ", status ? temp2 : properties[i], properties[i]);
                }
            }
            else
            {
                for (var i = 0; i < properties.Length; i++)
                {
                    updateSet.AppendFormat("{0} = @{0}, ", properties[i]);
                }
            }

            foreach (var d in primaryKeyList)
            {
                keySet.AppendFormat("{0} = @{0} AND ", d);
            }

            var query = string.Format(updateTemplate, string.IsNullOrEmpty(databaseTableName) ? type.Name : databaseTableName, updateSet.Remove(updateSet.Length - 2, 2).ToString(), keySet.Remove(keySet.Length - 4, 4).ToString());
            return query;
        }

        private static string BuildInsert(Type type, out PropertyInfo idField, string[] excludeFieldList = null, string databaseTableName = null, IDictionary<string, string> fieldNameMapping = null)
        {
            idField = type.GetProperties().Where(g => (g.GetCustomAttributes(true).Any(b => b.GetType() == typeof(IdentityPrimaryKeyAttribute)))).FirstOrDefault();
            var identityFieldName = idField == null ? string.Empty : idField.Name;
            const string insertTemplate = @"INSERT INTO {0} ({1}) VALUES ({2})";
            var properties = excludeFieldList == null ?
                             type.GetProperties().Where(f => f.Name != identityFieldName).Select(p => p.Name).ToArray() :
                             type.GetProperties().Where(f => f.Name != identityFieldName).Select(p => p.Name).ToArray().Except(excludeFieldList).ToArray();

            var values = string.Join(",", properties.Select(n => "@" + n).ToArray());
            var status = false;
            if (fieldNameMapping != null)
            {
                for (var i = 0; i < properties.Length; i++)
                {
                    var temp = properties[i];
                    var temp2 = string.Empty;
                    status = fieldNameMapping.TryGetValue(temp, out temp2);
                    if (status)
                    {
                        properties[i] = temp2;
                    }
                }
            }
            var names = string.Join(",", properties);

            var query = string.Format(insertTemplate, string.IsNullOrEmpty(databaseTableName) ? type.Name : databaseTableName, names, values);
            return query;
        }

    }
}
