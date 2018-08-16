using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Base.Utilities;

namespace Base.DB.CondExpr
{
    public class CondIn<TDbParam> : Cond<TDbParam> where TDbParam : DbParameter, new()
    {
        public CondIn(string field, ICollection<object> valueList)
        {
            // Any of the following situations makes an empty SQL:

            if (string.IsNullOrEmpty(field) || // 1) field not specified
                valueList == null || // 2) value list is null
                valueList.Count < 1 || // 3) no elements in value list
                valueList.All(m => m == null)) // 4) all elements in value list are null
            {
                return;
            }

            // Combine all elements into IN clause
            var sbSql = new StringBuilder();
            foreach (var v in valueList.Where(m => m != null))
            {
                // append SQL
                if (sbSql.Length > 0) sbSql.Append(", "); // seperator
                var pName = "@p" + StaticCounter.Next;
                sbSql.Append(pName);

                // push parameters
                DbParams.Add(new TDbParam {ParameterName = pName, Value = ConvertDbValue(v)});
            }

            Sql = $"{field} IN ({sbSql})";
        }
    }
}
