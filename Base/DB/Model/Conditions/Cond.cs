using Base.Conf;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Base.DB.Model.Conditions
{
    public abstract class Cond
    {
        protected string Sql { get; set; }
        protected List<SqlParameter> SqlParams { get; set; }

        public string FetchSql()
        {
            return Sql;
        }

        public List<SqlParameter> FetchParams()
        {
            return SqlParams;
        }

        protected Cond()
        {
            Sql = string.Empty;
            SqlParams = new List<SqlParameter>();
        }
        
        protected string ConvertParam(object x)
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
