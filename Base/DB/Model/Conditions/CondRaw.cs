using System.Collections.Generic;
using System.Data.SqlClient;

namespace Base.DB.Model.Conditions
{
    public class CondRaw : Cond
    {
        public CondRaw(string raw, IEnumerable<SqlParameter> paramList = null)
        {
            // Null or empty raw string makes an empty SQL:
            if (string.IsNullOrEmpty(raw)) return;

            Sql = raw;
            if (paramList != null) SqlParams.AddRange(paramList);
        }
    }
}
