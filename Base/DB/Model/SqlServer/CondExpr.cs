﻿using System.Collections.Generic;
using System.Data.SqlClient;
using Base.DB.CondExpr;

namespace Base.DB.Model.Models.SqlServer
{
    public class ExprBetween : CondBtwn<SqlParameter>
    {
        public ExprBetween(string field, object from, object to, string paramNameFrom = "", string paramNameTo = "")
            : base(field, from, to, paramNameFrom, paramNameTo)
        {
        }
    }

    public class ExprCompare : CondCmpr<SqlParameter>
    {
        public ExprCompare(string field, string opt, object value, string paramName = "")
            : base(field, opt, value, paramName)
        {
        }
    }

    public class ExprIn : CondIn<SqlParameter>
    {
        public ExprIn(string field, object[] valueList) : base(field, valueList)
        {
        }
    }

    public class ExprGroup : CondGroup<SqlParameter>
    {

    }

    public class ExprNot : CondNot<SqlParameter>
    {
        public ExprNot(Cond<SqlParameter> cond) : base(cond)
        {
        }
    }

    public class ExprRaw : CondRaw<SqlParameter>
    {
        public ExprRaw(string raw, IEnumerable<SqlParameter> paramList = null) : base(raw, paramList)
        {
        }
    }
}
