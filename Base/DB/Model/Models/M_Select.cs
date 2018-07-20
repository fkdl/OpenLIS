using Base.DB.Model.CondExpr;
using Base.Logger;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Base.DB.Model.Models
{
    public abstract partial class M<TDbConn, TDbParam>
        where TDbConn : DbConnection, new()
        where TDbParam : DbParameter, new()
    {
        protected List<TDbParam> DbParams = new List<TDbParam>();
        protected string CachedSelectSql = string.Empty;

        protected bool IsDistinct = false;
        protected string TableAlias = string.Empty;
        protected Hashtable SelectFields = new Hashtable();
        protected List<string> JoinTables = new List<string>();
        protected List<string> WhereConditions = new List<string>();
        protected List<string> GroupByFields = new List<string>();
        protected List<string> HavingConditions = new List<string>();
        protected List<string> OrderByFields = new List<string>();
        protected int LimitOffset = -1;
        protected int LimitLength = -1;

        /// <summary>
        /// Set if distinct.
        /// </summary>
        /// <param name="distinct"></param>
        /// <returns></returns>
        public M<TDbConn, TDbParam> Distinct(bool distinct = true)
        {
            IsDistinct = distinct;
            CachedSelectSql = string.Empty;
            return this;
        }

        /// <summary>
        /// Set alias name for model table.
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        public M<TDbConn, TDbParam> Alias(string alias)
        {
            if (!string.IsNullOrEmpty(alias))
            {
                TableAlias = alias;
                CachedSelectSql = string.Empty;
            }
            return this;
        }

        /// <summary>
        /// Add field on SELECT clause.
        /// </summary>
        /// <param name="field">Field name</param>
        /// <param name="alias">Alias name of field</param>
        /// <returns></returns>
        public M<TDbConn, TDbParam> Field(string field, string alias = "")
        {
            if (string.IsNullOrEmpty(field)) return this;

            if (SelectFields.ContainsKey(field))
                SelectFields[field] = alias;
            else
                SelectFields.Add(field, alias);
            CachedSelectSql = string.Empty;

            return this;
        }

        /// <summary>
        /// Add condition on WHERE clause.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public M<TDbConn, TDbParam> Where(Cond<TDbParam> condition)
        {
            var condSql = (condition == null ? "" : condition.FetchSql());
            if (string.IsNullOrEmpty(condSql)) return this;

            WhereConditions.Add(condSql);
            if (condition != null) DbParams.AddRange(condition.FetchDbParams());
            CachedSelectSql = string.Empty;

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
        public M<TDbConn, TDbParam> Join(string table, string alias = "", Cond<TDbParam> condition = null, string joinType = "INNER")
        {
            if (string.IsNullOrEmpty(table)) return this;

            joinType = joinType.ToUpper();
            if (joinType != "INNER" && joinType != "LEFT" && joinType != "RIGHT" && joinType != "FULL") return this;

            var condSql = (condition == null ? "" : condition.FetchSql());
            if (condition != null) DbParams.AddRange(condition.FetchDbParams());

            JoinTables.Add(
                joinType + " JOIN " +
                (alias == "" ? table : table + " AS " + alias) +
                (condSql == "" ? "" : " ON " + condSql));
            CachedSelectSql = string.Empty;

            return this;
        }

        /// <summary>
        /// Add field on GROUP BY clause.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public M<TDbConn, TDbParam> GroupBy(string field)
        {
            if (string.IsNullOrEmpty(field)) return this;

            GroupByFields.Add(field);
            CachedSelectSql = string.Empty;

            return this;
        }

        /// <summary>
        /// Add condition on HAVING cluase.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public M<TDbConn, TDbParam> Having(Cond<TDbParam> condition)
        {
            var condSql = condition == null ? "" : condition.FetchSql();
            if (string.IsNullOrEmpty(condSql)) return this;

            HavingConditions.Add(condSql);
            if (condition != null) DbParams.AddRange(condition.FetchDbParams());
            CachedSelectSql = string.Empty;

            return this;
        }

        /// <summary>
        /// Add field on ORDER BY clause.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="asc">If ASC</param>
        /// <returns></returns>
        public M<TDbConn, TDbParam> OrderBy(string field, bool asc = true)
        {
            if (string.IsNullOrEmpty(field)) return this;

            OrderByFields.Add(field + (asc ? "" : " DESC"));
            CachedSelectSql = string.Empty;

            return this;
        }

        /// <summary>
        /// Set offset and length for LIMIT
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public M<TDbConn, TDbParam> Limit(int offset, int length)
        {
            LimitOffset = offset;
            LimitLength = length;
            CachedSelectSql = string.Empty;

            return this;
        }

        /// <summary>
        /// Fetch SQL script for the current model.
        /// </summary>
        /// <returns>SQL script</returns>
        public abstract string FetchSql();

        public string SelectSql
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Accordingly add DISTINCT after SELECT.
        /// Fetch style-specified fields into to select clause. 
        /// </summary>
        /// <returns></returns>
        protected abstract string SelectClause();

        /// <summary>
        /// Concat SELECT fields with specified style.
        /// </summary>
        /// <param name="style">SelectStyle</param>
        /// <returns></returns>
        protected string SelectFieldsInStyle(SelectFieldStyle style = SelectFieldStyle.FieldNameAndAlias)
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
                JoinTables.Clear();
                OrderByFields.Clear();
                LimitOffset = -1;
                LimitLength = -1;
                GroupByFields.Clear();
                HavingConditions.Clear();

                DbParams.Clear();
            }

            // Fetch data.
            return Conn<TDbConn, TDbParam>.ExecuteDataTable(sql, DbParams);
        }

        /// <summary>
        /// Get value of specified parameter.
        /// </summary>
        /// <param name="paramName">Parameter name</param>
        /// <returns></returns>
        public object GetParamValue(string paramName)
        {
            var paramIndex = DbParams.FindIndex(m => m.ParameterName == paramName);
            if (paramIndex < 0) return null;

            return (DbParams[paramIndex].Value == DBNull.Value ? null : DbParams[paramIndex].Value); // ?
        }

        /// <summary>
        /// Set value of specified parameter.
        /// </summary>
        /// <param name="paramName">Parameter name</param>
        /// <param name="newValue">New value</param>
        /// <returns></returns>
        public M<TDbConn, TDbParam> SetParamValue(string paramName, object newValue)
        {
            var paramIndex = DbParams.FindIndex(m => m.ParameterName == paramName);
            if (paramIndex < 0) return this;

            DbParams[paramIndex].Value = (newValue ?? DBNull.Value); // ?

            return this;
        }
    }
}