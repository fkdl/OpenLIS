using Base.Conf;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Base.DB.Model.CondExpr
{
    public abstract class Cond<TDbParam> where TDbParam : DbParameter, new()
    {
        protected string Sql { get; set; }
        protected List<TDbParam> DbParams { get; set; }

        public string FetchSql()
        {
            return Sql;
        }

        public List<TDbParam> FetchDbParams()
        {
            return DbParams;
        }

        protected Cond()
        {
            Sql = string.Empty;
            DbParams = new List<TDbParam>();
        }

        protected string ConvertDbValue(object x)
        {
            // numbers
            if (x is int || x is float || x is double || x is decimal)
                return Convert.ToString(x);

            // date & time
            if (x is DateTime)
                return ((DateTime)x).ToString(SysConf.DateTimeFormat);

            // string
            return x.ToString();
        }
    }
}
