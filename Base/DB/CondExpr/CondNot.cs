using System.Data.Common;
using System.Diagnostics;

namespace Base.DB.CondExpr
{
    public class CondNot<TDbParam> : Cond<TDbParam> where TDbParam : DbParameter, new()
    {
        public CondNot(Cond<TDbParam> cond)
        {
            // Empty cond makes an empty SQL.
            var cSql = (cond == null ? string.Empty : cond.FetchSql());
            if (string.IsNullOrEmpty(cSql)) return;

            // set SQL
            Sql = $"NOT ({cSql})";

            // push parameters
            Debug.Assert(cond != null, ""); // Never asserts
            DbParams.AddRange(cond.FetchDbParams());
        }
    }
}
