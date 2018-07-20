using Base.Logger;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Base.DB.Model.Models.SqlServer.V2008
{
    public class M : M<SqlConnection, SqlParameter>
    {
        public M(string table, string keyField = "id") : base(table, keyField)
        {
        }
        
        /// <summary>
        /// Fetch SQL script for the current model.
        /// </summary>
        /// <returns>SQL script</returns>
        public override string FetchSql()
        {
            var result = new StringBuilder();

            // SELECT
            result.Append(SelectClause());
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
            // Put ORDER BY and UNION outside this wrap.
            if (LimitLength > 0)
            {
                // the prefix
                var prefix = string.Format("SELECT {0} FROM(\n", SelectFieldsInStyle(SelectFieldStyle.FieldNameOrAlias));

                // the postfix
                var postfix = string.Format("\n) AS T\nWHERE ROW_NUMBER BETWEEN {0} AND {1}",
                    LimitOffset + 1,
                    LimitOffset + LimitLength);

                // wrap
                result.Insert(0, prefix);
                result.Append(postfix);
            }

            // ORDER BY
            result.Append(OrderByFields.Count > 0 ? "\nORDER BY " + string.Join(", ", OrderByFields) : "");

            return result.ToString();
        }

        /// <summary>
        /// Accordingly add DISTINCT after SELECT.
        /// Fetch style-specified fields into to select clause.
        /// </summary>
        /// <returns></returns>
        protected override string SelectClause()
        {
            var result = new StringBuilder();

            result.Append("SELECT");

            // if distinct
            if (IsDistinct) result.Append(" DISTINCT");

            result.Append("\n");

            // Fields to be selected
            if (LimitLength > 0) // Implement LIMIT with ROW_NUMBER()
            {
                // if exist grouped fields, ROW_NUMBER sorts over them.
                var rowNumberOrderBy = (GroupByFields.Count > 0 ? string.Join(", ", GroupByFields) : KeyField);

                result.Append(string.Format("ROW_NUMBER() OVER(ORDER BY {0}) AS ROW_NUMBER, ", rowNumberOrderBy));
            }
            result.Append(SelectFieldsInStyle());

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

                fields += k.ToString();
                pNames += "@p_" + k;
            }

            return string.Format("INSERT INTO {0} ({1}) OUTPUT inserted.{2} VALUES({3})", TableName, fields, KeyField, pNames);
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

                expr += string.Format("{0} = {1}", k, "@p_" + k);
            }

            return string.Format("UPDATE {0} SET {1} OUTPUT inserted.{2}  WHERE {2} = {3}", TableName, expr, KeyField, key);
        }

        /// <summary>
        /// Static method, to generate a plain table.
        /// </summary>
        /// <param name="tableName">Table to be created.</param>
        /// <param name="fieldDescriptions">Descriptions to all fields.</param>
        /// <param name="checkExists">Drop table first if already exists.</param>
        /// <returns>If generation was successful.</returns>
        public static bool Create(string tableName, IEnumerable<FieldDescription> fieldDescriptions, bool checkExists = true)
        {
            // Drop if exists
            if (checkExists) Drop(tableName);

            // Create table
            var indexFields = new List<string>();
            var sqlCreateTable = string.Format("CREATE TABLE {0}(\n", tableName);
            foreach (var description in fieldDescriptions)
            {
                var sbField = new StringBuilder();

                // field name
                sbField.Append(description.FieldName);
                // field type
                if (!string.IsNullOrEmpty(description.FieldType)) sbField.Append(" " + description.FieldType);
                // constrains
                if (!string.IsNullOrEmpty(description.Constraints)) sbField.Append(" " + description.Constraints);
                // index fields
                if (description.IfIndex) indexFields.Add(description.FieldName);

                sqlCreateTable += sbField + ",\n"; // rest comma doesn't matter
            }
            sqlCreateTable += "\n);";
            Conn<SqlConnection, SqlParameter>.ExecuteNonQuery(sqlCreateTable);
            LogHelper.WriteLogInfo(string.Format("Create table {0}.", tableName));

            // Create indexes
            if (indexFields.Count > 0)
            {
                var fields = string.Join(", ", indexFields);
                var sqlCreateTableIndex = string.Format("CREATE INDEX {0}_INDEX ON {0}(\n{1}\n);", tableName, fields);
                Conn<SqlConnection, SqlParameter>.ExecuteNonQuery(sqlCreateTableIndex);
                LogHelper.WriteLogInfo(string.Format("Create index {0} on table {1}", fields, tableName));
            }

            return true;
        }

        /// <summary>
        /// Static method, to drop a plain table.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static bool Drop(string tableName)
        {
            var sqlDropIfExists = string.Format(
                    "IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID('{0}')) DROP TABLE {0};",
                    tableName);
            Conn<SqlConnection, SqlParameter>.ExecuteNonQuery(sqlDropIfExists);
            LogHelper.WriteLogInfo(string.Format("Drop table {0} if exists.", tableName));
            return true;
        }
    }
}
