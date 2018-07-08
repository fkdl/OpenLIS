using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Base.DB.Model.Conditions;
using System.Text;
using System.Data;
using Base.Logger;
using System.Data.SqlClient;

namespace Base.DB.Model.Models
{
    public class MPlain
    {
        protected string TableName = string.Empty;
        protected string KeyField = string.Empty;

        protected bool IsDistinct = false;
        protected string TableAlias = string.Empty;
        protected Hashtable SelectFields = new Hashtable();
        protected List<string> JoinConditions = new List<string>();
        protected List<string> WhereConditions = new List<string>();
        protected List<string> GroupFields = new List<string>();
        protected List<string> HavingConditions = new List<string>();
        protected List<string> OrderFields = new List<string>();
        protected List<string> UnionScripts = new List<string>();
        protected int LimitOffset = -1;
        protected int LimitLength = -1;

        protected List<SqlParameter> SqlParams = new List<SqlParameter>();

        public MPlain(string table, string keyField = "ID")
        {
            if (string.IsNullOrEmpty(table))
            {
                const string errMsg = "Error: Table name not specified.";

                LogHelper.WriteLogError(errMsg);
                throw new Exception(errMsg);
            }

            TableName = table;
            KeyField = keyField;
        }

        #region Select

        /// <summary>
        /// Set if distinct.
        /// </summary>
        /// <param name="distinct"></param>
        /// <returns></returns>
        public MPlain Distinct(bool distinct = true)
        {
            IsDistinct = distinct;
            return this;
        }

        /// <summary>
        /// Set alias name for model table.
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        public MPlain Alias(string alias)
        {
            if (!string.IsNullOrEmpty(alias))
            {
                TableAlias = alias;
            }
            return this;
        }

        /// <summary>
        /// Add field on SELECT clause.
        /// </summary>
        /// <param name="field">Field name</param>
        /// <param name="alias">Alias name of field</param>
        /// <returns></returns>
        public MPlain Field(string field, string alias = "")
        {
            if (string.IsNullOrEmpty(field)) return this;

            if (SelectFields.ContainsKey(field))
                SelectFields[field] = alias;
            else
                SelectFields.Add(field, alias);

            return this;
        }

        /// <summary>
        /// Add condition on WHERE clause.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public MPlain Where(Cond condition)
        {
            var condSql = (condition == null ? "" : condition.FetchSql());
            if (string.IsNullOrEmpty(condSql)) return this;

            WhereConditions.Add(condSql);
            if (condition != null) SqlParams.AddRange(condition.FetchParams());

            return this;
        }

        /// <summary>
        /// Add table for JOIN.
        /// </summary>
        /// <param name="table">Table name</param>
        /// <param name="alias">Alias name of table</param>
        /// <param name="condition">Condition on join</param>
        /// <param name="joinType">One of "INNER", "LEFT", "RIGHT" and "FULL"</param>
        /// <returns></returns>
        public MPlain Join(string table, string alias = "", Cond condition = null, string joinType = "INNER")
        {
            if (string.IsNullOrEmpty(table)) return this;

            joinType = joinType.ToUpper();
            if (joinType != "INNER" && joinType != "LEFT" && joinType != "RIGHT" && joinType != "FULL") return this;

            var condSql = (condition == null ? "" : condition.FetchSql());
            if (condition != null) SqlParams.AddRange(condition.FetchParams());

            JoinConditions.Add(
                joinType + " JOIN " +
                (alias == "" ? table : table + " AS " + alias) +
                (condSql == "" ? "" : " ON " + condSql));

            return this;
        }

        /// <summary>
        /// Add field on GROUP BY clause.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public MPlain GroupBy(string field)
        {
            if (string.IsNullOrEmpty(field)) return this;

            GroupFields.Add(field);
            return this;
        }

        /// <summary>
        /// Add condition on HAVING cluase.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public MPlain Having(Cond condition)
        {
            var condSql = condition == null ? "" : condition.FetchSql();
            if (string.IsNullOrEmpty(condSql)) return this;

            HavingConditions.Add(condSql);
            if (condition != null) SqlParams.AddRange(condition.FetchParams());

            return this;
        }

        /// <summary>
        /// Add field on ORDER BY clause.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="asc">If ASC</param>
        /// <returns></returns>
        public MPlain OrderBy(string field, bool asc = true)
        {
            if (string.IsNullOrEmpty(field)) return this;

            OrderFields.Add(field + (asc ? "" : " DESC"));
            return this;
        }

        /// <summary>
        /// Add model to union.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public MPlain Union(MPlain m)
        {
            var mScript = (m == null ? "" : m.FetchSql());
            if (string.IsNullOrEmpty(mScript)) return this;

            UnionScripts.Add(mScript);
            if (m != null) SqlParams.AddRange(m.SqlParams);

            return this;
        }

        /// <summary>
        /// Set offset and length for LIMIT
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public MPlain Limit(int offset, int length)
        {
            LimitOffset = offset;
            LimitLength = length;

            return this;
        }

        /// <summary>
        /// Fetch SQL script for the current model.
        /// </summary>
        /// <returns>SQL script</returns>
        public string FetchSql()
        {
            var result = new StringBuilder();

            // SELECT
            result.Append(SelectClause());
            // FROM
            result.Append("\nFROM " + TableName + (TableAlias == "" ? "" : " AS " + TableAlias));
            // JOIN
            result.Append(JoinConditions.Count > 0 ? "\n" + string.Join("\n", JoinConditions) : "");
            // WHERE
            result.Append(WhereConditions.Count > 0 ? "\nWHERE " + string.Join(" AND ", WhereConditions) : "");
            // GROUP BY
            result.Append(GroupFields.Count > 0 ? "\nGROUP BY " + string.Join(", ", GroupFields) : "");
            // HAVING
            result.Append(HavingConditions.Count > 0 ? "\nHAVING " + string.Join(" AND ", HavingConditions) : "");

            // To impletment LIMIT, wrap select SQL with another SELECT having a range constrat on ROW_NUMBER.
            // Put ORDER BY and UNION outside this wrap.
            if (LimitLength > 0)
            {
                // the prefix
                var prefix = string.Format("SELECT {0} FROM(\n",
                    SelectFieldsClause(SelectFieldStyle.FieldNameOrAlias));

                // the postfix
                var postfix = string.Format("\n) AS T\nWHERE ROW_NUMBER BETWEEN {0} AND {1}",
                    LimitOffset + 1,
                    LimitOffset + LimitLength);

                // wrap
                result.Insert(0, prefix);
                result.Append(postfix);
            }

            // ORDER BY
            result.Append(OrderFields.Count > 0 ? "\nORDER BY " + string.Join(", ", OrderFields) : "");
            // UNION
            result.Append(UnionScripts.Count > 0 ? "\nUNION\n" + string.Join("\nUNION\n", UnionScripts) : "");

            return result.ToString();
        }

        /// <summary>
        /// Accordingly add DISTINCT after SELECT.
        /// Fetch style-specified fields into to select clause. 
        /// </summary>
        /// <returns></returns>
        private string SelectClause()
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
                var rowNumberOrderBy = (GroupFields.Count > 0 ? string.Join(", ", GroupFields) : KeyField);

                result.Append(string.Format("ROW_NUMBER() OVER(ORDER BY {0}) AS ROW_NUMBER, ", rowNumberOrderBy));
            }
            result.Append(SelectFieldsClause());

            return result.ToString();
        }

        /// <summary>
        /// Join selected fields with specified style.
        /// </summary>
        /// <param name="style">
        ///     <para>SelectStyle.FieldNameOnly:      Use original field names only, without any alias names.</para>
        ///     <para>SelectStyle.FieldNameOrAlias:   Use alias names if specified, otherwise, use original.</para>
        ///     <para>SelectStyle.FieldNameAndAlias:  Use original field names, additionally, append " AS " + [alias] if specified.</para>
        /// </param>
        /// <returns></returns>
        private string SelectFieldsClause(SelectFieldStyle style = SelectFieldStyle.FieldNameAndAlias)
        {
            var result = new StringBuilder();

            if (SelectFields.Count < 1)
            {
                const string errMsg = "Error: No selection fields specified.";
                LogHelper.WriteLogError(errMsg);
                throw new Exception(errMsg);
            }

            foreach (var field in SelectFields.Keys)
            {
                var alias = SelectFields[field].ToString();

                if (result.Length > 0) result.Append(", "); // seperator

                switch (style)
                {
                    case SelectFieldStyle.FieldNameAndAlias:
                        result.Append(alias == ""
                            ? field
                            : string.Format("{0} AS {1}", field, alias));
                        break;
                    case SelectFieldStyle.FieldNameOrAlias:
                        result.Append(alias == "" ? field : alias);
                        break;
                    case SelectFieldStyle.FieldNameOnly:
                        result.Append(field);
                        break;
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Select data with the configured SQL, and reset all configurations.
        /// </summary>
        /// <param name="initAfter">Clear all configurations after excution.</param>
        /// <returns></returns>
        public DataTable Select(bool initAfter = false)
        {
            // Fetch select SQL.
            var sql = FetchSql();

            //  Clear all configurations.
            if (initAfter)
            {
                IsDistinct = false;
                TableAlias = "";

                WhereConditions.Clear();
                SelectFields.Clear();
                JoinConditions.Clear();
                OrderFields.Clear();
                LimitOffset = -1;
                LimitLength = -1;
                GroupFields.Clear();
                HavingConditions.Clear();
                UnionScripts.Clear();

                SqlParams.Clear();
            }

            // Fetch data.
            return Conn.ExecuteDataTable(sql, SqlParams);
        }

        /// <summary>
        /// Set value of specified parameter.
        /// </summary>
        /// <param name="paramName">Parameter name</param>
        /// <param name="newValue">New value</param>
        /// <returns></returns>
        public MPlain SetParamValue(string paramName, object newValue)
        {
            var paramIndex = SqlParams.FindIndex(m => m.ParameterName == paramName);
            if (paramIndex < 0) return this;

            SqlParams[paramIndex].Value = (newValue ?? DBNull.Value); // ?

            return this;
        }

        /// <summary>
        /// Get value of specified parameter.
        /// </summary>
        /// <param name="paramName">Parameter name</param>
        /// <returns></returns>
        public object GetParamValue(string paramName)
        {
            var paramIndex = SqlParams.FindIndex(m => m.ParameterName == paramName);
            if (paramIndex < 0) return null;

            return (SqlParams[paramIndex].Value == DBNull.Value ? null : SqlParams[paramIndex].Value); // ?
        }

        #endregion

        #region Insert/Update

        /// <summary>
        /// Variable "_cache" stores temp data to be inserted or updated,
        /// in the format of [k = field name, v = field value].
        /// </summary>
        protected readonly Hashtable Cache = new Hashtable();

        /// <summary>
        /// Set value in "TmpData".
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual MPlain Data(string field, object value)
        {
            // Empty field is not valid
            if (string.IsNullOrEmpty(field)) return this;

            // Add or Modify field value
            if (Cache.Contains(field))
                Cache[field] = value;
            else
                Cache.Add(field, value);

            return this;
        }

        /// <summary>
        /// Insert or update data stored in "TmpData".
        /// </summary>
        /// <param name="key">Do update if "key" is specified, otherwise do insert.</param>
        /// <returns></returns>
        public virtual int Save(object key = null)
        {
            var sql = (key == null ? CacheInsertSql() : CacheUpdateSql(key));
            var sqlParams = CacheParams();

            Cache.Clear(); // clear data after store.

            return Conn.ExecuteNonQuery(sql, sqlParams);
        }

        /// <summary>
        /// Fetch parameters by cached data
        /// </summary>
        /// <returns></returns>
        private List<SqlParameter> CacheParams()
        {
            return (
                from object k in Cache.Keys
                let v = (Cache[k] ?? DBNull.Value)
                select new SqlParameter("@p_" + k, v)).ToList();
        }

        /// <summary>
        /// Fetch INSER SQL by cached data.
        /// </summary>
        /// <returns></returns>
        private string CacheInsertSql()
        {
            var fields = string.Empty;
            var pNames = string.Empty;

            foreach (var k in Cache.Keys)
            {
                if (!string.IsNullOrEmpty(fields)) // seperator
                {
                    fields += ", ";
                    pNames += ", ";
                }

                fields += k.ToString();
                pNames += "@p_" + k;
            }

            return string.Format("INSERT INTO {0} ({1}) VALUES({2})", TableName, fields, pNames);
        }

        /// <summary>
        /// Ftech UPDATE SQL by cached data.
        /// </summary>
        /// <param name="key">To which key field equals.</param>
        /// <returns></returns>
        private string CacheUpdateSql(object key)
        {
            var expr = string.Empty;

            foreach (var k in Cache.Keys)
            {
                if (!string.IsNullOrEmpty(expr)) // seperator
                    expr += ", ";

                expr += string.Format("{0} = {1}", k, "@p_" + k);
            }

            return string.Format("UPDATE {0} SET {1} WHERE {2} = {3}", TableName, expr, KeyField, key);
        }

        #endregion

        #region Delete

        /// <summary>
        /// Delete records by condition.
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        public int Delete(Cond cond)
        {
            var sql = string.Format("DELETE FROM {0} WHERE {1}", TableName, cond.FetchSql());
            LogHelper.WriteLogInfo(sql);
            return Conn.ExecuteNonQuery(sql, cond.FetchParams());
        }

        /// <summary>
        /// Delete records by keys.
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public int Delete(object[] keys)
        {
            var cond = new CondIn(KeyField, keys);
            return Delete(cond);
        }

        /// <summary>
        /// Delete records by key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int Delete(object key)
        {
            var cond = new CondCmpr(KeyField, "=", key);
            return Delete(cond);
        }

        #endregion

        /// <summary>
        /// Static method, to generate a plain table.
        /// </summary>
        /// <param name="tableName">Table to be created.</param>
        /// <param name="fieldDescriptions">Descriptions to all fields.</param>
        /// <param name="checkExists">Drop table first if already exists.</param>
        /// <returns>If generation was successful.</returns>
        public static bool Create(string tableName, IEnumerable<FieldDescription> fieldDescriptions, bool checkExists = true)
        {
            var indexFields = new List<string>();

            // Drop if exists
            if (checkExists)
            {
                var sqlDropIfExists = string.Format(
                    "IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID('{0}')) DROP TABLE {0};",
                    tableName);
                Conn.ExecuteNonQuery(sqlDropIfExists);
                LogHelper.WriteLogInfo(string.Format("Drop table {0} if exists.", tableName));
            }

            // Create table
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
            Conn.ExecuteNonQuery(sqlCreateTable);
            LogHelper.WriteLogInfo(string.Format("Create table {0}.", tableName));

            // Create indexes
            if (indexFields.Count > 0)
            {
                var fields = string.Join(", ", indexFields);
                var sqlCreateTableIndex = string.Format("CREATE INDEX {0}_INDEX ON {0}(\n{1}\n);", tableName, fields);
                Conn.ExecuteNonQuery(sqlCreateTableIndex);
                LogHelper.WriteLogInfo(string.Format("Create index {0} on table {1}", fields, tableName));
            }

            return true;
        }
    }

    internal enum SelectFieldStyle
    {
        FieldNameOnly,
        FieldNameOrAlias,
        FieldNameAndAlias,
    }

    public struct FieldDescription
    {
        public string FieldName;
        public string FieldType;
        public string Constraints;
        public bool IfIndex;
    }
    
}
