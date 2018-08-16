using System.Collections.Generic;
using System.Data.Common;

namespace Base.DB.CondExpr
{
    public class CondRaw<TDbParam> : Cond<TDbParam> where TDbParam : DbParameter, new()
    {
        public CondRaw(string raw, IEnumerable<TDbParam> paramList = null)
        {
            // Null or empty raw string makes an empty SQL:
            if (string.IsNullOrEmpty(raw)) return;

            Sql = raw;
            if (paramList != null) DbParams.AddRange(paramList);
        }
    }
}
