using System.Data;
using System.Data.OleDb;
using Comm.UI.MDI;
using Comm.UI.MDI.CachedData;

namespace Comm.Class.Comm.CachedData
{
    public abstract class AOleDbComm<TMdiOleDb> : AComm
        where TMdiOleDb : MdiOleDb, new()
    {
        protected AOleDbComm(string moduleName, string moduleVersion)
        {
            ModuleName = moduleName;
            ModuleVersion = moduleVersion;
            MdiChild = new TMdiOleDb();
        }
        
        public override string ModuleName { get; }

        public override string ModuleVersion { get; }
        
        public override MdiCommon MdiChild { get; }
        
    }

    public class OleDbAccesser
    {
        public string ConnectionString { get; set; }
        
        /// <summary>
        /// Execute SQL for data table.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string sql)
        {
            var result = new DataTable();
            var conn = new OleDbConnection();

            // connect
            conn.ConnectionString = ConnectionString;
            conn.Open();

            // fetch data
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            var rs = cmd.ExecuteReader();
            if (rs == null) return result;

            // data fields
            for (var i = 0; i < rs.FieldCount; i++)
            {
                var fieldName = rs.GetName(i);
                var fieldType = rs.GetFieldType(i);
                if (fieldType != null) result.Columns.Add(fieldName, fieldType);
            }

            // data rows
            while (rs.Read())
            {
                var newRow = result.NewRow();
                for (var i = 0; i < rs.FieldCount; i++)
                    newRow[i] = rs[i];

                result.Rows.Add(newRow);
            }

            cmd.Dispose();
            conn.Close();
            conn.Dispose();

            return result;
        }

        /// <summary>
        /// Execute SQL for non-query.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql)
        {
            var conn = new OleDbConnection();

            // connect
            conn.ConnectionString = ConnectionString;
            conn.Open();
            
            // execute
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            var rs = cmd.ExecuteNonQuery();

            cmd.Dispose();
            conn.Close();
            conn.Dispose();

            return rs;
        }
    }
}
