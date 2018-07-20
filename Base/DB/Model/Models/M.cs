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

    public enum SelectFieldStyle
    {
        FieldNameOnly,      // Use original field names only, without any alias names.
        FieldNameOrAlias,   // Use alias names if specified, otherwise, use original.
        FieldNameAndAlias,  // Use original field names, additionally, append " AS " + [alias] if specified.
    }

    public struct FieldDescription
    {
        public string FieldName;
        public string FieldType;
        public string Constraints;
        public bool IfIndex;
    }
}
