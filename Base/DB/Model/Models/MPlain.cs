using System;
using System.Collections;
using System.Collections.Generic;
using Base.DB.Model.Conditions;
using System.Text;
using System.Data;
using Base.Logger;
using System.Data.SqlClient;

namespace Base.DB.Model.Models
{
    public class MPlain
    {
        protected string TableName = "";
        protected string KeyField = "";

        protected bool IsDistinct = false;
        protected string TableAlias = "";
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
            if (field == null) return this;

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
            SqlParams.AddRange(condition.FetchParams());

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
            if (condSql != "") SqlParams.AddRange(condition.FetchParams());

            JoinConditions.Add(
                joinType + " JOIN " +
                (alias == "" ? table : table + " AS " + alias) +
                (condSql == "" ? "" : " ON " + condSql));

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
            SqlParams.AddRange(condition.FetchParams());

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
            SqlParams.AddRange(m.SqlParams);

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
                    SelectFieldsClause(SelectStyle.FieldNameOrAlias));

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
                string rowNumberOrderBy;

                // if exist grouped fields, ROW_NUMBER sorts over them.
                if (GroupFields.Count > 0)
                    rowNumberOrderBy = string.Join(", ", GroupFields);
                else
                    rowNumberOrderBy = KeyField;
                
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
        private string SelectFieldsClause(SelectStyle style = SelectStyle.FieldNameAndAlias)
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
                    case SelectStyle.FieldNameAndAlias:
                        result.Append(alias == ""
                            ? field
                            : string.Format("{0} AS {1}", field, alias));
                        break;
                    case SelectStyle.FieldNameOrAlias:
                        result.Append(alias == "" ? field : alias);
                        break;
                    case SelectStyle.FieldNameOnly:
                        result.Append(field);
                        break;
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Select data with the configured SQL, and reset all configurations.
        /// </summary>
        /// <param name="clearConfigs">Clear all configurations after excution.</param>
        /// <returns></returns>
        public virtual DataTable Select(bool clearConfigs = false)
        {
            // Fetch select SQL.
            var sql = FetchSql();

            //  Clear all configurations.
            if (clearConfigs)
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

        #endregion

        #region Insert/Update
        
        private readonly Hashtable _cache = new Hashtable();

        /// <summary>
        /// Set value in "TmpData".
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual MPlain Data(string field, object value)
        {
            if (_cache.Contains(field))
                _cache[field] = value;
            else
                _cache.Add(field, value);

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

            _cache.Clear();

            return Conn.ExecuteNonQuery(sql);
        }

        private string CacheInsertSql()
        {
            var fields = "";
            var values = "";
            foreach (var k in _cache.Keys)
            {
                if (fields == "")
                {
                    fields = k.ToString();
                    values = _cache[k].ToString();
                }
                else
                {
                    fields += ", " + k;
                    values += ", " + _cache[k];
                }
            }

            return string.Format("INSERT INTO {0} ({1}) VALUES({2})", TableName, fields, values);
        }

        private string CacheUpdateSql(object key)
        {
            var expr = "";
            foreach (var k in _cache.Keys)
            {
                if (expr == "")
                    expr = string.Format("{0} = {1}", k, _cache[k]);
                else
                    expr += string.Format(", {0} = {1}", k, _cache[k]);
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
            return Conn.ExecuteNonQuery(sql);
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

    }

    internal enum SelectStyle
    {
        FieldNameOnly,
        FieldNameOrAlias,
        FieldNameAndAlias,
    }
}
