using System;
using Base.Logger;
using System.Data.Common;

namespace Base.DB.Model.Models
{
    public abstract partial class M<TDbConn, TDbParam>
        where TDbConn : DbConnection, new()
        where TDbParam : DbParameter, new()
    {
        protected string TableName = string.Empty;
        protected string KeyField = string.Empty;

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

    public struct FieldDesc
    {
        public string Name;
        public string Type;
        public string Constraints;
        public bool Index;
    }
    
}

