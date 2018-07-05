using System.Diagnostics;

namespace Base.DB.Model.Conditions
{
    public class CondGroup : Cond
    {
        protected string CondJoin = "AND";

        public CondGroup Add(Cond cond)
        {
            // Empty cond makes an empty SQL.
            var cSql = (cond == null ? string.Empty : cond.FetchSql());
            if (string.IsNullOrEmpty(cSql)) return this;

            // append SQL
            if (string.IsNullOrEmpty(Sql))
                Sql = string.Format("({0})", cSql);
            else
                Sql += string.Format(" {0} ({1})", CondJoin, cSql);

            // push parameters
            Debug.Assert(cond != null, ""); // Never asserts
            SqlParams.AddRange(cond.FetchParams());

            return this;
        }

        public CondGroup JoinWith(string join)
        {
            CondJoin = "AND";
            if (join.ToUpper() == "OR") CondJoin = "OR";

            return this;
        }
    }
}
