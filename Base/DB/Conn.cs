using System.Data;
using Base.Conf;
using Base.Logger;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Base.DB
{
    public static class Conn
    {
        private static readonly SqlConnection Connection;

        static Conn()
        {
            try
            {
                var connStr = string.Format("Data Source={0};User ID={1};Password={2};Initial Catalog={3};",
                    SysConf.DbAddress, SysConf.DbUserName, SysConf.DbPassword, SysConf.DbDefaultDb);
                Connection = new SqlConnection(connStr);

                LogHelper.WriteLogInfo(string.Format("DB connection established. Conn string: {0}", connStr));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLogError(
                    string.Format("Error occured on establishing DB connection. Exception message: {0}", ex.Message));
            }
        }

        public static SqlConnection GetConnection()
        {
            return Connection;
        }

        public static int ExecuteNonQuery(string sql, List<SqlParameter> sqlParams = null)
        {
            try
            {
                Connection.Open();
                var command = Connection.CreateCommand();
                command.CommandText = sql;

                // SQL parameters
                if (sqlParams != null && sqlParams.Count > 0)
                    command.Parameters.AddRange(sqlParams.ToArray());

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

        public static DataTable ExecuteDataTable(string sql, List<SqlParameter> sqlParams = null)
        {
            var result = new DataTable();

            try
            {
                Connection.Open();
                var command = Connection.CreateCommand();
                command.CommandText = sql;

                // SQL parameters
                if (sqlParams != null && sqlParams.Count > 0)
                    command.Parameters.AddRange(sqlParams.ToArray());

                var adapter = new SqlDataAdapter(command);
                adapter.Fill(result);

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
