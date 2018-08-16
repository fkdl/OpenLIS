using System;
using System.Data.Common;
using Base.Logger;

namespace Base.DB.Model
{
    public partial class M<TDbConn, TDbParam>
        where TDbConn : DbConnection, new()
        where TDbParam : DbParameter, new()
    {
        protected string TableName;
        protected string KeyField;

        public M(string table, string keyField = "id")
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
    }

    /// <summary>
    /// Description of table fields.
    /// </summary>
    public struct FieldDesc
    {
        public string Name;
        public string Type;
        public string Constraints;
        public bool Index;
    }
    
}

