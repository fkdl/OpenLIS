using Base.Utilities;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Base.DB.Model.Conditions
{
    public class CondIn : Cond
    {
        public CondIn(string field, object[] valueList)
        {
            // Any of the following situations makes an empty SQL:

            if (string.IsNullOrEmpty(field) ||  // 1) field not specified
                valueList == null ||            // 2) value list is null
                valueList.Length < 1 ||         // 3) no elements in value list
                valueList.All(m => m == null))  // 4) all elements in value list are null
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
                SqlParams.Add(new SqlParameter(pName, ConvertParam(v)));
            }
            
            Sql = string.Format("{0} IN ({1})", field, sbSql);
        }
    }
}
