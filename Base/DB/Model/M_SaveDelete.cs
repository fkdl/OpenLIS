using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Base.DB.CondExpr;
using Base.Logger;

namespace Base.DB.Model
{
    public partial class M<TDbConn, TDbParam>
        where TDbConn : DbConnection, new()
        where TDbParam : DbParameter, new()
    {
        #region Insert/Update

        /// <summary>
        /// Stores temp data to be inserted or updated.
        /// </summary>
        protected readonly Dictionary<string, object> DataCache = new Dictionary<string, object>();

        /// <summary>
        /// Set value in "DataCache".
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual M<TDbConn, TDbParam> Data(string field, object value)
        {
            // Empty field is not valid
            if (string.IsNullOrEmpty(field)) return this;

            // Add or Modify field value
            if (DataCache.ContainsKey(field))
                DataCache[field] = value;
            else
                DataCache.Add(field, value);

            return this;
        }

        /// <summary>
        /// Insert or update data stored in "DataCache".
        /// </summary>
        /// <param name="key">Do update if "key" is specified, otherwise do insert.</param>
        /// <returns>Key of latest updated/inserted record</returns>
        public virtual object Save(object key = null)
        {
            var sql = (key == null ? InsertSql() : UpdateSql(key));
            var sqlParams = DataCacheParams();

            DataCache.Clear(); // clear data after store.
            LogHelper.WriteLogInfo(sql);
            
            return Conn<TDbConn, TDbParam>.ExecuteScalar(sql, sqlParams);
        }

        /// <summary>
        /// Fetch parameters by cached data.
        /// </summary>
        /// <returns></returns>
        private List<TDbParam> DataCacheParams()
        {
            return (
                from string k in DataCache.Keys
                let v = (DataCache[k] ?? DBNull.Value)
                select new TDbParam { ParameterName = "@p_" + k, Value = v }).ToList();
        }

        /// <summary>
        /// Fetch INSER SQL by cached data.
        /// </summary>
        /// <returns></returns>
        protected virtual string InsertSql()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Ftech UPDATE SQL by cached data.
        /// </summary>
        /// <param name="key">To which key field equals.</param>
        /// <returns></returns>
        protected virtual string UpdateSql(object key)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Delete

        /// <summary>
        /// Delete records by condition.
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        public virtual int Delete(Cond<TDbParam> cond)
        {
            var sql = $"DELETE FROM {TableName} WHERE {cond.FetchSql()}";
            LogHelper.WriteLogInfo(sql);
            return Conn<TDbConn, TDbParam>.ExecuteNonQuery(sql, cond.FetchDbParams());
        }

        /// <summary>
        /// Delete records by keys.
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public virtual int Delete(object[] keys)
        {
            var cond = new CondIn<TDbParam>(KeyField, keys);
            return Delete(cond);
        }

        /// <summary>
        /// Delete records by key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual int Delete(object key)
        {
            var cond = new CondCmpr<TDbParam>(KeyField, "=", key);
            return Delete(cond);
        }

        #endregion

    }
}