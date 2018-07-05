using Base.Utilities;
using System.Data.SqlClient;
using System.Linq;

namespace Base.DB.Model.Conditions
{
    public class CondCmpr : Cond
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
                    Sql = string.Format("{0} {1} NULL", field, opt);
            }
            else
            {
                if (opt != "IS" && opt != "IS NOT")
                {
                    // get parameter name
                    var pName = string.IsNullOrEmpty(paramName)
                        ? "@p" + StaticCounter.Next
                        : paramName;

                    // construct SQL and push parameter
                    Sql = string.Format("{0} {1} {2}", field, opt, pName);
                    SqlParams.Add(new SqlParameter(pName, ConvertParam(value)));
                }
            }
        }
    }
}
