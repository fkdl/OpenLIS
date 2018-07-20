using Base.Utilities;
using System.Data.Common;

namespace Base.DB.Model.CondExpr
{
    public class CondBtwn<TDbParam> : Cond<TDbParam> where TDbParam : DbParameter, new()
    {
        public CondBtwn(string field, object from, object to, string paramNameFrom = "", string paramNameTo = "")
        {
            // Empty field makes an empty SQL.
            if (string.IsNullOrEmpty(field)) return;

            // Null "from" or "to" makes an empty SQL.
            var fromStr = (from == null ? string.Empty : ConvertDbValue(from));
            var toStr = (to == null ? string.Empty : ConvertDbValue(to));
            if (string.IsNullOrEmpty(fromStr) || string.IsNullOrEmpty(toStr)) return;

            // get parameter names
            var pNameFrom = string.IsNullOrEmpty(paramNameFrom)
                ? "@p" + StaticCounter.Next
                : paramNameFrom;

            var pNameTo = string.IsNullOrEmpty(paramNameTo)
                ? "@p" + StaticCounter.Next
                : paramNameTo;

            // construct SQL and push parameters
            Sql = string.Format("{0} BETWEEN {1} AND {2}", field, pNameFrom, pNameTo);
            DbParams.Add(new TDbParam { ParameterName = pNameFrom, Value = fromStr });
            DbParams.Add(new TDbParam { ParameterName = pNameTo, Value = toStr });
        }
    }
}
