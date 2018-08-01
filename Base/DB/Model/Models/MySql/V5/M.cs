using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base.Logger;
using MySql.Data.MySqlClient;

namespace Base.DB.Model.Models.MySql.V5
{
    public class M: M<MySqlConnection, MySqlParameter>
    {
        public M(string table, string keyField = "id") : base(table, keyField)
        {

        }
        
        /// <summary>
        /// Fetch SELECT SQL by current configuration.
        /// </summary>
        /// <returns>SQL script</returns>
        protected override string SelectSql()
        {
            var result = new StringBuilder();

            // SELECT
            result.Append("SELECT");
            if (IsDistinct) result.Append(" DISTINCT");// if distinct
            result.Append("\n");

            result.Append(ConcatSelectFields());// Fields to be selected

            // FROM
            result.Append("\nFROM " + TableName + (TableAlias == "" ? "" : " AS " + TableAlias));
            // JOIN
            result.Append(JoinTables.Count > 0 ? "\n" + string.Join("\n", JoinTables) : "");
            // WHERE
            result.Append(WhereConditions.Count > 0 ? "\nWHERE " + string.Join(" AND ", WhereConditions) : "");
            // GROUP BY
            result.Append(GroupByFields.Count > 0 ? "\nGROUP BY " + string.Join(", ", GroupByFields) : "");
            // HAVING
            result.Append(HavingConditions.Count > 0 ? "\nHAVING " + string.Join(" AND ", HavingConditions) : "");
            // ORDER BY
            result.Append(OrderByFields.Count > 0 ? "\nORDER BY " + string.Join(", ", OrderByFields) : "");
            // LIMIT
            result.Append(LimitLength > 0 ? $"\nLIMIT {LimitOffset}, {LimitLength}" : "");

            return result.ToString();
        }

        /// <summary>
        /// Fetch INSER SQL by cached data.
        /// </summary>
        /// <returns></returns>
        protected override string InsertSql()
        {
            var fields = string.Empty;
            var pNames = string.Empty;

            foreach (var k in DataCache.Keys)
            {
                if (!string.IsNullOrEmpty(fields)) // seperator
                {
                    fields += ", ";
                    pNames += ", ";
                }

                fields += k;
                pNames += "@p_" + k;
            }

            return $"INSERT INTO {TableName} ({fields}) VALUES({pNames}); SELECT LAST_INSERT_ID();";
        }

        /// <summary>
        /// Ftech UPDATE SQL by cached data.
        /// </summary>
        /// <param name="key">To which key field equals.</param>
        /// <returns></returns>
        protected override string UpdateSql(object key)
        {
            var expr = string.Empty;

            foreach (var k in DataCache.Keys)
            {
                if (!string.IsNullOrEmpty(expr)) // seperator
                    expr += ", ";

                expr += $"{k} = {"@p_" + k}";
            }

            return $"UPDATE {TableName} SET {expr} WHERE {KeyField} = {key}; SELECT {key}";
        }

        /// <summary>
        /// To generate a plain table.
        /// </summary>
        /// <param name="tableName">Table to be created.</param>
        /// <param name="fieldDescriptions">Descriptions to all fields.</param>
        /// <param name="tableConstraints">Constraints for table.</param>
        /// <param name="clearBefore">Drop table first if already exists.</param>
        /// <returns>If generation was successful.</returns>
        public static bool Create(string tableName, IEnumerable<FieldDesc> fieldDescriptions,
            IEnumerable<string> tableConstraints = null, bool clearBefore = true)
        {
            try
            {
                // Drop if exists
                if (clearBefore) Drop(tableName);

                // Create table
                var indexFields = new List<string>(); // store index fields
                var sqlCreateTable = $"CREATE TABLE {tableName}(\n";

                // fields
                foreach (var description in fieldDescriptions)
                {
                    var sbField = new StringBuilder();

                    // field name
                    sbField.Append(description.Name);
                    // field type
                    if (!string.IsNullOrEmpty(description.Type)) sbField.Append(" " + description.Type);
                    // constrain
                    if (!string.IsNullOrEmpty(description.Constraints)) sbField.Append(" " + description.Constraints);
                    // if index field
                    if (description.Index) indexFields.Add(description.Name);

                    sqlCreateTable += sbField + ",\n";
                }

                // table constrains
                if (tableConstraints != null)
                {
                    sqlCreateTable = tableConstraints.Aggregate(
                        sqlCreateTable,
                        (current, constraint) => current + constraint + ",\n");
                }

                sqlCreateTable = sqlCreateTable.Remove(sqlCreateTable.Length - 2, 2); // remove rest ",\n"
                sqlCreateTable += "\n) ENGINE=InnoDB DEFAULT CHARSET=utf8;";

                Conn<MySqlConnection, MySqlParameter>.ExecuteNonQuery(sqlCreateTable);
                LogHelper.WriteLogInfo($"Created table {tableName}.");

                // Create indexes
                if (indexFields.Count > 0)
                {
                    var fields = string.Join(", ", indexFields);
                    var sqlCreateTableIndex = $"CREATE INDEX {tableName}_INDEX ON {tableName}(\n{fields}\n);";
                    Conn<MySqlConnection, MySqlParameter>.ExecuteNonQuery(sqlCreateTableIndex);
                    LogHelper.WriteLogInfo($"Created index {fields} on table {tableName}");
                }

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLogError(
                    $"Error occured on creating table {tableName}. Exception message: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Static method, to drop a plain table.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static bool Drop(string tableName)
        {
            try
            {
                var sqlDropIfExists = $"DROP TABLE IF EXISTS {tableName};";
                Conn<MySqlConnection, MySqlParameter>.ExecuteNonQuery(sqlDropIfExists);
                LogHelper.WriteLogInfo($"Dropped table {tableName}.");
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLogError(
                    $"Error occured on dropping table {tableName}. Exception message: {ex.Message}");
                return false;
            }
        }
    }
}
