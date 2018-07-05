using System.Diagnostics;

namespace Base.DB.Model.Conditions
{
    public class CondNot: Cond
    {
        public CondNot(Cond cond)
        {
            // Empty cond makes an empty SQL.
            var cSql = (cond == null ? string.Empty : cond.FetchSql());
            if (string.IsNullOrEmpty(cSql)) return;

            // set SQL
            Sql = string.Format("NOT ({0})", cSql);

            // push parameters
            Debug.Assert(cond != null, "CondNot: cond == null");
            SqlParams.AddRange(cond.FetchParams());
        }
    }
}
