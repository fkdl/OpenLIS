using System.Data.Common;
using System.Linq;
using Base.Utilities;

namespace Base.DB.CondExpr
{
    public class CondCmpr<TDbParam> : Cond<TDbParam> where TDbParam : DbParameter, new()
    {
        public CondCmpr(string field, string opt, object value, string paramName = "")
        {
            opt = opt.ToUpper();

            // Empty field name or invalid opt makes an empty SQL.
            if (string.IsNullOrEmpty(field)) return;
            if (!(new[] {">", ">=", "<", "<=", "=", "<>", "LIKE", "NOT LIKE", "IS", "IS NOT"}.Contains(opt))) return;

            // "IS" or "IS NOT" for NULL value only.
            if (value == null)
            {
                if (opt == "IS" || opt == "IS NOT")
                    Sql = $"{field} {opt} NULL";
            }
            else
            {
                if (opt == "IS" || opt == "IS NOT") return;
                // get parameter name
                var pName = string.IsNullOrEmpty(paramName)
                    ? "@p" + StaticCounter.Next
                    : paramName;

                // construct SQL and push parameter
                Sql = $"{field} {opt} {pName}";
                DbParams.Add(new TDbParam {ParameterName = pName, Value = ConvertDbValue(value)});
            }
        }
    }
}
