using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Base.DB.CondExpr;

namespace Base.DB.Model
{
    public class Eav<TDbConn, TDbParam, TModel>
        where TDbConn : DbConnection, new()
        where TDbParam : DbParameter, new()
        where TModel: M<TDbConn, TDbParam>
    {
        protected string EntityModel;

        public Eav(string entityModel)
        {
            EntityModel = entityModel;
        }

        /// <summary>
        /// Get instance of TModel.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="keyField"></param>
        /// <returns></returns>
        protected TModel M(string tableName, string keyField = "id")
        {
            return (TModel) Activator.CreateInstance(typeof (TModel), tableName, keyField);
        }

        /// <summary>
        /// Gets entity of this model by specified code. Returns null if not found.
        /// </summary>
        /// <param name="entityCode">filtering by entity code</param>
        /// <returns></returns>
        public EavEntity GetEntity(string entityCode)
        {
            if (string.IsNullOrEmpty(entityCode)) return null;

            var m = M("eav_entity")
                .Field("id")
                .Field("entity_model")
                .Field("entity_code")
                .Where(new CondCmpr<TDbParam>("entity_model", "=", EntityModel))
                .Where(new CondCmpr<TDbParam>("entity_code", "=", entityCode));

            // fetch data
            var rows = m.Select().Rows;
            if (rows.Count < 1) return null;

            return new EavEntity
            {
                Id = Convert.ToInt32(rows[0]["id"]),
                Model = rows[0]["entity_model"].ToString(),
                Code = rows[0]["entity_code"].ToString(),
            };
        }

        /// <summary>
        /// Sets entity by code. Only for non-existing case.
        /// </summary>
        /// <param name="entityCode"></param>
        /// <returns></returns>
        public Eav<TDbConn, TDbParam, TModel> SetEntity(string entityCode)
        {
            // check code not null
            if (string.IsNullOrEmpty(entityCode)) return this;

            // check existing
            var e = GetEntity(entityCode);
            if (e != null) return this;

            // insert
            M("eav_entity")
                .Data("entity_model", EntityModel)
                .Data("entity_code", entityCode)
                .Save();

            return this;
        }

        /// <summary>
        /// Removes entity by code. Only for existing case.
        /// </summary>
        /// <param name="entityCode"></param>
        /// <returns></returns>
        public Eav<TDbConn, TDbParam, TModel> RemoveEntity(string entityCode)
        {
            // check code not null
            if (string.IsNullOrEmpty(entityCode)) return this;

            // check existing
            var e = GetEntity(entityCode);
            if (e == null) return this;

            // delete
            M("eav_entity").Delete(e.Id);
            return this;
        }

        /// <summary>
        /// Gets attribute of this model by specified code. Return null if not found.
        /// </summary>
        /// <param name="attrCode">filtering by attribute code</param>
        /// <returns></returns>
        public EavAttribute GetAttribute(string attrCode)
        {
            if (string.IsNullOrEmpty(attrCode)) return null;

            var m = M("eav_attr")
                .Field("id")
                .Field("attr_code")
                .Field("attr_type")
                .Field("entity_model")
                .Where(new CondCmpr<TDbParam>("entity_model", "=", EntityModel))
                .Where(new CondCmpr<TDbParam>("attr_code", "=", attrCode));

            // fetch data
            var rows = m.Select().Rows;
            if (rows.Count < 1) return null;

            return new EavAttribute
            {
                Id = Convert.ToInt32(rows[0]["id"]),
                Code = rows[0]["attr_code"].ToString(),
                Type = rows[0]["attr_type"].ToString(),
                Model = rows[0]["entity_model"].ToString(),
            };
        }

        /// <summary>
        /// Sets attribute by code and type. Only for non-existing case.
        /// </summary>
        /// <param name="attrCode"></param>
        /// <param name="attrType"></param>
        /// <returns></returns>
        public Eav<TDbConn, TDbParam, TModel> SetAttribute(string attrCode, string attrType)
        {
            // check code not null
            if (string.IsNullOrEmpty(attrCode)) return this;

            // check existing
            var a = GetAttribute(attrCode);
            if (a != null) return this;

            // insert
            M("eav_attr")
                .Data("attr_code", attrCode)
                .Data("attr_type", attrType)
                .Data("entity_model", EntityModel)
                .Save();

            return this;
        }

        /// <summary>
        /// Removes attribute by code. Only for existing case.
        /// </summary>
        /// <param name="attrCode"></param>
        /// <returns></returns>
        public Eav<TDbConn, TDbParam, TModel> RemoveAttribute(string attrCode)
        {
            // check code not null
            if (string.IsNullOrEmpty(attrCode)) return this;

            // check existing
            var a = GetAttribute(attrCode);
            if (a == null) return this;

            // delete
            M("eav_attr").Delete(a.Id);

            return this;
        }

        /// <summary>
        /// Gets value of this model by specified entity and attribute codes. 
        /// </summary>
        /// <param name="entityCode"></param>
        /// <param name="attrCode"></param>
        /// <returns></returns>
        public EavValue GetValue(string entityCode, string attrCode)
        {
            var e = GetEntity(entityCode);
            var a = GetAttribute(attrCode);
            if (e == null || a == null) return null;

            var m = M("eav_values_" + a.Type)
                .Field("id")
                .Field("entity_id")
                .Field("attr_id")
                .Field("value")
                .Where(new CondCmpr<TDbParam>("entity_id", "=", e.Id))
                .Where(new CondCmpr<TDbParam>("attr_id", "=", a.Id));

            // fetch data
            var rows = m.Select().Rows;
            if (rows.Count < 1) return null;

            return new EavValue
            {
                Id = Convert.ToInt32(rows[0]["id"]),
                EntityId = Convert.ToInt32(rows[0]["entity_id"]),
                AttrId = Convert.ToInt32(rows[0]["attr_id"]),
                Val = rows[0]["value"]
            };
        }

        /// <summary>
        /// Sets value by entity and attribute code. Automatically insert if not existing.
        /// </summary>
        /// <param name="entityCode"></param>
        /// <param name="attrCode"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Eav<TDbConn, TDbParam, TModel> SetValue(string entityCode, string attrCode, object value)
        {
            if (value == null) return this;

            // get type by value
            string type;
            if (value is int)
                type = "int";
            else if (value is double || value is float)
                type = "decimal";
            else if (value is DateTime)
                type = "datetime";
            else if (value is string)
                type = "varchar";
            else
                type = "varbinary";

            SetEntity(entityCode);
            SetAttribute(attrCode, type);

            var e = GetEntity(entityCode);
            var a = GetAttribute(attrCode);
            if (e == null || a == null) return this; // unable to set entity or attribute

            if (type != a.Type) return this; // type not match

            var v = GetValue(entityCode, attrCode);
            if (v == null)
                M("eav_values_" + type).Data("entity_id", e.Id).Data("attr_id", a.Id).Data("value", value).Save();
            else
                M("eav_values_" + type).Data("value", value).Save(v.Id);

            return this;
        }

        /// <summary>
        /// Removes value by entity and attribute codes. Only for existing case.
        /// </summary>
        /// <param name="entityCode"></param>
        /// <param name="attrCode"></param>
        /// <returns></returns>
        public Eav<TDbConn, TDbParam, TModel> RemoveValue(string entityCode, string attrCode)
        {
            var e = GetEntity(entityCode);
            var a = GetAttribute(attrCode);
            if (e == null || a == null) return this;

            // delete
            var cond = new CondGroup<TDbParam>();
            cond.Add(new CondCmpr<TDbParam>("entity_id", "=", e.Id));
            cond.Add(new CondCmpr<TDbParam>("attr_id", "=", a.Id));
            M("eav_values_" + a.Type).Delete(cond);

            return this;
        }

        /// <summary>
        /// Gets or sets value by specified entity and attribute codes.
        /// </summary>
        /// <param name="entityCode"></param>
        /// <param name="attrCode"></param>
        /// <returns></returns>
        public object this[string entityCode, string attrCode]
        {
            get { return GetValue(entityCode, attrCode)?.Val; }
            set { SetValue(entityCode, attrCode, value); }
        }

        public List<EavEntity> AllEntities()
        {
            var m = M("eav_entity")
                .Field("id")
                .Field("entity_model")
                .Field("entity_code")
                .Where(new CondCmpr<TDbParam>("entity_model", "=", EntityModel));

            return (
                from DataRow row in m.Select().Rows
                select new EavEntity
                {
                    Id = Convert.ToInt32(row["id"]),
                    Model = row["entity_model"].ToString(),
                    Code = row["entity_code"].ToString(),
                }).ToList();
        }

        public List<EavAttribute> AllAttributes()
        {
            var m = M("eav_attr")
                .Field("id")
                .Field("attr_code")
                .Field("attr_type")
                .Field("entity_model")
                .Where(new CondCmpr<TDbParam>("entity_model", "=", EntityModel));

            return (
                from DataRow row in m.Select().Rows
                select new EavAttribute
                {
                    Id = Convert.ToInt32(row["id"]),
                    Code = row["attr_code"].ToString(),
                    Type = row["attr_type"].ToString(),
                    Model = row["entity_model"].ToString(),
                }).ToList();
        }
    }

    public class EavEntity
    {
        public int Id;
        public string Model;
        public string Code;
    }

    public class EavAttribute
    {
        public int Id;
        public string Code;
        public string Type;
        public string Model;
    }

    public class EavValue
    {
        public int Id;
        public int EntityId;
        public int AttrId;
        public object Val;
    }
}
