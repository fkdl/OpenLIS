using System.Data;
using Base.Conf;
using Base.Logger;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Base.DB
{
    public static class Conn<TDbConn, TDbParam>
        where TDbConn : DbConnection, new()
        where TDbParam : DbParameter, new()
    {
        private static readonly TDbConn Connection;

        static Conn()
        {
            try
            {
                var connStr = string.Format("Data Source={0};User ID={1};Password={2};Database={3};",
                    SysConf.DbAddress, SysConf.DbUserName, SysConf.DbPassword, SysConf.DbDefaultDb);
                Connection = new TDbConn { ConnectionString = connStr };

                LogHelper.WriteLogInfo(string.Format("DB connection established. Conn string: {0}", connStr));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLogError(
                    string.Format("Error occured on establishing DB connection. Exception message: {0}", ex.Message));
            }
        }
        
        public static TDbConn DbConnection
        {
            get
            {
                return Connection;
            }
        }

        public static int ExecuteNonQuery(string sql, List<TDbParam> dbParams = null)
        {
            try
            {
                Connection.Open();
                var command = Connection.CreateCommand();
                command.CommandText = sql;

                // DB parameters
                if (dbParams != null && dbParams.Count > 0)
                    command.Parameters.AddRange(dbParams.ToArray());

                var result = command.ExecuteNonQuery();

                command.Parameters.Clear();
                Connection.Close();

                LogHelper.WriteLogInfo(string.Format("Non-query excuted. Affected lines = {0}", result));

                return result;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLogError(
                    string.Format("Error occured on executing non-query: {0} Error Message: {1}",
                        sql,
                        ex.Message));

                return -1;
            }
        }

        public static object ExecuteScalar(string sql, List<TDbParam> dbParams = null)
        {
            object result = null;

            try
            {
                Connection.Open();
                var command = Connection.CreateCommand();
                command.CommandText = sql;

                // Db parameters
                if (dbParams != null && dbParams.Count > 0)
                    command.Parameters.AddRange(dbParams.ToArray());

                result = command.ExecuteScalar();

                command.Parameters.Clear();
                Connection.Close();

                LogHelper.WriteLogInfo(string.Format("Scalar excuted. Returned = {0}", result));

                return result;

            }
            catch (Exception ex)
            {
                LogHelper.WriteLogError(
                    string.Format("Error occured on executing scalar: {0} Error Message: {1}",
                        sql,
                        ex.Message));
            }

            return result;
        }

        public static DataTable ExecuteDataTable(string sql, List<TDbParam> dbParams = null)
        {
            var result = new DataTable();

            try
            {
                Connection.Open();
                var command = Connection.CreateCommand();
                command.CommandText = sql;

                // Db parameters
                if (dbParams != null && dbParams.Count > 0)
                    command.Parameters.AddRange(dbParams.ToArray());

                // Fill data
                using (var reader = command.ExecuteReader())
                {
                    for (var i = 0; i < reader.FieldCount; i++)
                        result.Columns.Add(reader.GetName(i), reader.GetFieldType(i));

                    while (reader.Read())
                    {
                        var newRow = result.NewRow();
                        for (var i = 0; i < reader.FieldCount; i++)
                            newRow[i] = reader[i];

                        result.Rows.Add(newRow);
                    }
                }

                // Finalization
                command.Parameters.Clear();
                Connection.Close();

                LogHelper.WriteLogInfo(string.Format("Data table fetched. Total lines = {0}", result.Rows.Count));

            }
            catch (Exception ex)
            {
                LogHelper.WriteLogError(
                    string.Format("Error occured on executing data table: {0} Error Message: {1}",
                        sql,
                        ex.Message));
            }

            return result;
        }
    }
}
