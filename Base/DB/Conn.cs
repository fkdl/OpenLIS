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
        static Conn()
        {
            try
            {
                var connStr =
                    $"Data Source={SysConf.DbAddress};User ID={SysConf.DbUserName};Password={SysConf.DbPassword};Database={SysConf.DbDefaultDb};SslMode={SysConf.DbSslMode};";
                DbConnection = new TDbConn {ConnectionString = connStr};

                LogHelper.WriteLogInfo($"DB connection established. Conn string: {connStr}");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLogError(
                    $"Error occured on establishing DB connection. Exception message: {ex.Message}");
            }
        }

        public static TDbConn DbConnection { get; }

        public static int ExecuteNonQuery(string sql, List<TDbParam> dbParams = null)
        {
            try
            {
                DbConnection.Open();
                var command = DbConnection.CreateCommand();
                command.CommandText = sql;

                // DB parameters
                if (dbParams != null && dbParams.Count > 0)
                    command.Parameters.AddRange(dbParams.ToArray());

                var result = command.ExecuteNonQuery();

                command.Parameters.Clear();
                DbConnection.Close();

                LogHelper.WriteLogInfo($"Non-query excuted. Affected lines = {result}");

                return result;
            }
            catch (Exception ex)
            {
                if ((DbConnection.State & ConnectionState.Open) != 0) DbConnection.Close();

                LogHelper.WriteLogError(
                    $"Error occured on executing non-query: {sql} Error Message: {ex.Message}");

                return -1;
            }
        }

        public static object ExecuteScalar(string sql, List<TDbParam> dbParams = null)
        {
            object result = null;

            try
            {
                DbConnection.Open();
                var command = DbConnection.CreateCommand();
                command.CommandText = sql;

                // Db parameters
                if (dbParams != null && dbParams.Count > 0)
                    command.Parameters.AddRange(dbParams.ToArray());

                result = command.ExecuteScalar();

                command.Parameters.Clear();
                DbConnection.Close();

                LogHelper.WriteLogInfo($"Scalar excuted. Returned = {result}");

                return result;

            }
            catch (Exception ex)
            {
                if ((DbConnection.State & ConnectionState.Open) != 0) DbConnection.Close();

                LogHelper.WriteLogError(
                    $"Error occured on executing scalar: {sql} Error Message: {ex.Message}");
            }

            return result;
        }

        public static DataTable ExecuteDataTable(string sql, List<TDbParam> dbParams = null)
        {
            var result = new DataTable();

            try
            {
                DbConnection.Open();
                var command = DbConnection.CreateCommand();
                command.CommandText = sql;

                // Db parameters
                if (dbParams != null && dbParams.Count > 0)
                    command.Parameters.AddRange(dbParams.ToArray());

                // Fill data
                using (var reader = command.ExecuteReader())
                {
                    // fields
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        var fieldName = reader.GetName(i);
                        var fieldType = reader.GetFieldType(i);
                        if (fieldType != null) result.Columns.Add(fieldName, fieldType);
                    }

                    // rows
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
                DbConnection.Close();

                LogHelper.WriteLogInfo($"Data table fetched. Total lines = {result.Rows.Count}");

            }
            catch (Exception ex)
            {
                if ((DbConnection.State & ConnectionState.Open) != 0) DbConnection.Close();

                LogHelper.WriteLogError(
                    $"Error occured on executing data table: {sql} Error Message: {ex.Message}");
            }

            return result;
        }
    }
}
