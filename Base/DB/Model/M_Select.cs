using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Base.DB.CondExpr;
using Base.Logger;

namespace Base.DB.Model
{
    public partial class M<TDbConn, TDbParam>
        where TDbConn : DbConnection, new()
        where TDbParam : DbParameter, new()
    {
        protected List<TDbParam> DbParams = new List<TDbParam>();
        protected string CachedSelectSql = string.Empty;

        protected bool IsDistinct;
        protected string TableAlias = string.Empty;
        protected Dictionary<string, string> SelectFields = new Dictionary<string, string>();
        protected List<string> JoinTables = new List<string>();
        protected List<string> WhereConditions = new List<string>();
        protected List<string> GroupByFields = new List<string>();
        protected List<string> HavingConditions = new List<string>();
        protected List<string> OrderByFields = new List<string>();
        protected int LimitOffset = -1;
        protected int LimitLength = -1;

        #region SQL Construction

        /// <summary>
        /// Set if distinct.
        /// </summary>
        /// <param name="distinct"></param>
        /// <returns></returns>
        public virtual M<TDbConn, TDbParam> Distinct(bool distinct = true)
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
        public virtual M<TDbConn, TDbParam> Alias(string alias)
        {
            if (string.IsNullOrEmpty(alias)) return this;
            TableAlias = alias;
            CachedSelectSql = string.Empty;

            return this;
        }

        /// <summary>
        /// Add field on SELECT clause.
        /// </summary>
        /// <param name="field">Field name</param>
        /// <param name="alias">Alias name of field</param>
        /// <returns></returns>
        public virtual M<TDbConn, TDbParam> Field(string field, string alias = "")
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
        public virtual M<TDbConn, TDbParam> Where(Cond<TDbParam> condition)
        {
            var condSql = (condition == null ? string.Empty : condition.FetchSql());
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
        public virtual M<TDbConn, TDbParam> Join(string table, string alias = "", Cond<TDbParam> condition = null, string joinType = "INNER")
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
        public virtual M<TDbConn, TDbParam> GroupBy(string field)
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
        public virtual M<TDbConn, TDbParam> Having(Cond<TDbParam> condition)
        {
            var condSql = condition == null ? string.Empty : condition.FetchSql();
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
        public virtual M<TDbConn, TDbParam> OrderBy(string field, bool asc = true)
        {
            if (string.IsNullOrEmpty(field)) return this;

            OrderByFields.Add(field + (asc ? string.Empty : " DESC"));

            CachedSelectSql = string.Empty;
            return this;
        }

        /// <summary>
        /// Set offset and length for LIMIT
        /// </summary>
        /// <param name="offset">Starts from 0.</param>
        /// <param name="length"></param>
        /// <returns></returns>
        public virtual M<TDbConn, TDbParam> Limit(int offset, int length)
        {
            LimitOffset = offset;
            LimitLength = length;

            CachedSelectSql = string.Empty;
            return this;
        }

        #endregion

        /// <summary>
        /// Fetch SELECT SQL by current configuration.
        /// </summary>
        /// <returns>SQL script</returns>
        protected virtual string SelectSql()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Concat SELECT fields with specified style.
        /// </summary>
        /// <param name="style">SelectStyle</param>
        /// <returns></returns>
        protected string ConcatSelectFields(FieldNameStyle style = FieldNameStyle.FieldNameAndAlias)
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
                var alias = SelectFields[field];

                if (result.Length > 0) result.Append(", "); // seperator

                switch (style)
                {
                    case FieldNameStyle.FieldNameAndAlias:
                        result.Append(string.IsNullOrEmpty(alias)
                            ? field
                            : $"{field} AS {alias}");
                        break;
                    case FieldNameStyle.FieldNameOrAlias:
                        result.Append(alias == "" ? field : alias);
                        break;
                    case FieldNameStyle.FieldNameOnly:
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
        public virtual DataTable Select(bool initAfter = false)
        {
            if (string.IsNullOrEmpty(CachedSelectSql))
                CachedSelectSql = SelectSql();

            var result = Conn<TDbConn, TDbParam>.ExecuteDataTable(CachedSelectSql, DbParams);

            if (initAfter) ResetSelectConfig();

            return result;
        }
        
        /// <summary>
        /// Reset SELECT SQL configurations.
        /// </summary>
        /// <returns></returns>
        public M<TDbConn, TDbParam> ResetSelectConfig()
        {
            DbParams.Clear();
            CachedSelectSql = string.Empty;

            IsDistinct = false;
            TableAlias = string.Empty;

            WhereConditions.Clear();
            SelectFields.Clear();
            JoinTables.Clear();
            OrderByFields.Clear();
            LimitOffset = -1;
            LimitLength = -1;
            GroupByFields.Clear();
            HavingConditions.Clear();

            return this;
        }

        #region Parameter Values

        /// <summary>
        /// Get value of specified parameter.
        /// </summary>
        /// <param name="paramName">Parameter name</param>
        /// <returns></returns>
        public object GetParam(string paramName)
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
        public M<TDbConn, TDbParam> SetParam(string paramName, object newValue)
        {
            var paramIndex = DbParams.FindIndex(m => m.ParameterName == paramName);
            if (paramIndex < 0) return this;

            DbParams[paramIndex].Value = (newValue ?? DBNull.Value);

            return this;
        }

        #endregion
    }

    public enum FieldNameStyle
    {
        FieldNameOnly,      // Use original field names only, without any alias names.
        FieldNameOrAlias,   // Use alias names if specified, otherwise, use original.
        FieldNameAndAlias,  // Use original field names, additionally, append " AS " + [alias] if specified.
    }
}