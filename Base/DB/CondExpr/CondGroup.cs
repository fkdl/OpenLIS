using System.Data.Common;
using System.Diagnostics;

namespace Base.DB.CondExpr
{
    public class CondGroup<TDbParam> : Cond<TDbParam> where TDbParam : DbParameter, new()
    {
        protected string CondJoin = "AND";

        public CondGroup<TDbParam> Add(Cond<TDbParam> cond)
        {
            // Empty cond makes an empty SQL.
            var cSql = (cond == null ? string.Empty : cond.FetchSql());
            if (string.IsNullOrEmpty(cSql)) return this;

            // append SQL
            if (string.IsNullOrEmpty(Sql))
                Sql = $"({cSql})";
            else
                Sql += $" {CondJoin} ({cSql})";

            // push parameters
            Debug.Assert(cond != null, ""); // Never asserts
            DbParams.AddRange(cond.FetchDbParams());

            return this;
        }

        public CondGroup<TDbParam> JoinWith(string join)
        {
            CondJoin = "AND";
            if (join.ToUpper() == "OR") CondJoin = "OR";

            return this;
        }
    }
}
