using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Base.Logger;

namespace Base.DB.Model.SqlServer.V2008
{
    public class M : M<SqlConnection, SqlParameter>
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
            if (LimitLength > 0) // Implement LIMIT with ROW_NUMBER()
            {
                // if exist grouped fields, ROW_NUMBER sorts over them.
                // otherwise, ROW_NUMBER sorts over KeyField.
                var rowNumberOrderBy = (GroupByFields.Count > 0 ? string.Join(", ", GroupByFields) : KeyField);
                result.Append($"ROW_NUMBER() OVER(ORDER BY {rowNumberOrderBy}) AS ROW_NUMBER, ");
            }
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

            // To impletment LIMIT, wrap select SQL with another SELECT having a range constrat on ROW_NUMBER.
            // Put ORDER BY outside this wrap.
            if (LimitLength > 0)
            {
                var prefix = $"SELECT {ConcatSelectFields(FieldNameStyle.FieldNameOrAlias)} FROM(\n";
                
                var postfix = $"\n) AS T\nWHERE ROW_NUMBER BETWEEN {LimitOffset + 1} AND {LimitOffset + LimitLength}";

                // wrap
                result.Insert(0, prefix);
                result.Append(postfix);
            }

            // ORDER BY
            result.Append(OrderByFields.Count > 0 ? "\nORDER BY " + string.Join(", ", OrderByFields) : "");

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

            return $"INSERT INTO {TableName} ({fields}) OUTPUT inserted.{KeyField} VALUES({pNames})";
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

            return $"UPDATE {TableName} SET {expr} OUTPUT inserted.{KeyField}  WHERE {KeyField} = {key}";
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
                sqlCreateTable += "\n);";

                Conn<SqlConnection, SqlParameter>.ExecuteNonQuery(sqlCreateTable);
                LogHelper.WriteLogInfo($"Created table {tableName}.");

                // Create indexes
                if (indexFields.Count > 0)
                {
                    var fields = string.Join(", ", indexFields);
                    var sqlCreateTableIndex = $"CREATE INDEX {tableName}_INDEX ON {tableName}(\n{fields}\n);";
                    Conn<SqlConnection, SqlParameter>.ExecuteNonQuery(sqlCreateTableIndex);
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
                var sqlDropIfExists = string.Format(
                        "IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID('{0}')) DROP TABLE {0};",
                        tableName);
                Conn<SqlConnection, SqlParameter>.ExecuteNonQuery(sqlDropIfExists);
                LogHelper.WriteLogInfo($"Dropped table {tableName}.");
                return true;
            }
            catch(Exception ex)
            {
                LogHelper.WriteLogError(
                    $"Error occured on dropping table {tableName}. Exception message: {ex.Message}");
                return false;
            }
        }
    }
}
