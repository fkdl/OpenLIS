using Base.DB.Model.CondExpr;
using Base.Logger;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Base.DB.Model.Models
{
    public abstract partial class M<TDbConn, TDbParam>
        where TDbConn : DbConnection, new()
        where TDbParam : DbParameter, new()
    {
        #region Insert/Update

        /// <summary>
        /// Variable "_cache" stores temp data to be inserted or updated,
        /// in the format of [k = field name, v = field value].
        /// </summary>
        protected readonly Hashtable DataCache = new Hashtable();

        /// <summary>
        /// Set value in "TmpData".
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual M<TDbConn, TDbParam> Data(string field, object value)
        {
            // Empty field is not valid
            if (string.IsNullOrEmpty(field)) return this;

            // Add or Modify field value
            if (DataCache.Contains(field))
                DataCache[field] = value;
            else
                DataCache.Add(field, value);

            return this;
        }

        /// <summary>
        /// Insert or update data stored in "TmpData".
        /// </summary>
        /// <param name="key">Do update if "key" is specified, otherwise do insert.</param>
        /// <returns>Key of latest updated/inserted record</returns>
        public virtual object Save(object key = null)
        {
            var sql = (key == null ? InsertSql() : UpdateSql(key));
            var sqlParams = ParamsForDataCache();

            DataCache.Clear(); // clear data after store.
            LogHelper.WriteLogInfo(sql);
            return Conn<TDbConn, TDbParam>.ExecuteScalar(sql, sqlParams);
        }

        /// <summary>
        /// Fetch parameters by cached data
        /// </summary>
        /// <returns></returns>
        private List<TDbParam> ParamsForDataCache()
        {
            return (
                from object k in DataCache.Keys
                let v = (DataCache[k] ?? DBNull.Value)
                select new TDbParam { ParameterName = "@p_" + k, Value = v }).ToList();
        }

        /// <summary>
        /// Fetch INSER SQL by cached data.
        /// </summary>
        /// <returns></returns>
        protected abstract string InsertSql();

        /// <summary>
        /// Ftech UPDATE SQL by cached data.
        /// </summary>
        /// <param name="key">To which key field equals.</param>
        /// <returns></returns>
        protected abstract string UpdateSql(object key);

        #endregion

        #region Delete

        /// <summary>
        /// Delete records by condition.
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        public int Delete(Cond<TDbParam> cond)
        {
            var sql = string.Format("DELETE FROM {0} WHERE {1}", TableName, cond.FetchSql());
            LogHelper.WriteLogInfo(sql);
            return Conn<TDbConn, TDbParam>.ExecuteNonQuery(sql, cond.FetchDbParams());
        }

        /// <summary>
        /// Delete records by keys.
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public int Delete(object[] keys)
        {
            var cond = new CondIn<TDbParam>(KeyField, keys);
            return Delete(cond);
        }

        /// <summary>
        /// Delete records by key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int Delete(object key)
        {
            var cond = new CondCmpr<TDbParam>(KeyField, "=", key);
            return Delete(cond);
        }

        #endregion

    }
}