﻿using Base.Utilities;
using System.Data.SqlClient;

namespace Base.DB.Model.Conditions
{
    public class CondBtwn : Cond
    {
        public CondBtwn(string field, object from, object to, string paramNameFrom = "", string paramNameTo = "")
        {
            // Empty field makes an empty SQL.
            if (string.IsNullOrEmpty(field)) return;

            // Null "from" or "to" makes an empty SQL.
            var fromStr = (from == null ? string.Empty : ConvertParam(from));
            var toStr = (to == null ? string.Empty : ConvertParam(to));
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
            SqlParams.Add(new SqlParameter(pNameFrom, fromStr));
            SqlParams.Add(new SqlParameter(pNameTo, toStr));
        }
    }
}