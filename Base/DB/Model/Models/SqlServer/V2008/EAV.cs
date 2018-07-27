using Base.DB.Model.Models.SqlServer.CondExpr;
using System;
using System.Collections.Generic;
using System.Data;

namespace Base.DB.Model.Models.SqlServer.V2008
{
    public class EAV
    {
        protected string EntityModel = string.Empty;
        
        public EAV(string entityModel)
        {
            EntityModel = entityModel;
        }
        
        /// <summary>
        /// Gets entities of this model by specified code. If code is not assigned, returns all entities.
        /// </summary>
        /// <param name="entityCode">filtering by entity code</param>
        /// <returns></returns>
        public List<Entity> Entities(string entityCode = null)
        {
            var result = new List<Entity>();
            var m = new M("eav_entity")
                .Field("id")
                .Field("entity_model")
                .Field("entity_code")
                .Where(new ExprCompare("entity_model", "=", EntityModel));

            // filtered by entity code
            if (!string.IsNullOrEmpty(entityCode))
                m.Where(new ExprCompare("entity_code", "=", entityCode));
            
            // fetch data
            foreach (DataRow row in m.Select().Rows)
            {
                result.Add(new Entity
                {
                    Id = Convert.ToInt32(row["id"]),
                    Model = row["entity_model"].ToString(),
                    Code = row["entity_code"].ToString(),
                });
            }

            return result;
        }

        /// <summary>
        /// Gets attributes of this model by specified code. If code is not assigned, returns all attributes.
        /// </summary>
        /// <param name="attrCode">filtering by attribute code</param>
        /// <returns></returns>
        public List<Attr> Attributes(string attrCode = null)
        {
            var result = new List<Attr>();
            var m = new M("eav_attr")
                .Field("id")
                .Field("attr_code")
                .Field("attr_type")
                .Field("entity_model")
                .Where(new ExprCompare("entity_model", "=", EntityModel));

            if (!string.IsNullOrEmpty(attrCode))
                m.Where(new ExprCompare("attr_code", "=", attrCode));
            
            foreach (DataRow row in m.Select().Rows)
            {
                result.Add(new Attr
                {
                    Id = Convert.ToInt32(row["id"]),
                    Code = row["attr_code"].ToString(),
                    Type = row["attr_type"].ToString(),
                    Model = row["entity_model"].ToString(),
                });
            }

            return result;
        }

        public List<Value> Values(string entityCode, string attrCode)
        {
            var result = new List<Value>();

            var e = Entities(entityCode);
            var a = Attributes(attrCode);
            if (e.Count < 1 || a.Count < 1) return result;

            var vResults = new M("eav_values_" + a[0].Type)
                    .Field("id")
                    .Field("entity_id")
                    .Field("attr_id")
                    .Field("value")
                    .Where(new ExprCompare("entity_id", "=", e[0].Id))
                    .Where(new ExprCompare("attr_id", "=", a[0].Id))
                    .Select();

            foreach(DataRow row in vResults.Rows)
            {
                result.Add(new Value
                {
                    Id = Convert.ToInt32(row["id"]),
                    EntityId = Convert.ToInt32(row["entity_id"]),
                    AttrId = Convert.ToInt32(row["attr_id"]),
                    Val = row["value"]
                });
            }

            return result;
        }

        /// <summary>
        /// Get or set Value by specified entity code, attribute code and attribute type.
        /// </summary>
        /// <param name="entityCode"></param>
        /// <param name="attrCode"></param>
        /// <returns></returns>
        public object this[string entityCode, string attrCode]
        {
            get
            {
                var result = Values(entityCode, attrCode);
                return result.Count > 1 ? result[0].Val : null;
            }

            set
            {
                if (value == null) return; // null value need not be written

                string type;
                int entityId;
                int attrId;

                // get entity ID
                var entities = Entities(entityCode);
                if (entities.Count < 1) // code not exists
                {
                    var id = new M("eav_entity")
                        .Data("entity_model", EntityModel)
                        .Data("entity_code", entityCode)
                        .Save();
                    entityId = Convert.ToInt32(id);
                }
                else
                {
                    entityId = entities[0].Id;
                }

                // get attr type and attr ID
                var attrs = Attributes(attrCode);
                if (attrs.Count < 1) // code not exists
                {
                    if (value is int) type = "int";
                    else if (value is double || value is float) type = "decimal";
                    else if (value is DateTime) type = "datetime";
                    else if (value is string) type = "varchar";
                    else type = "varbinary";

                    var id = new M("eav_attr")
                        .Data("attr_code", attrCode)
                        .Data("attr_type", type)
                        .Data("entity_model", EntityModel)
                        .Save();
                    attrId = Convert.ToInt32(id);
                }
                else
                {
                    type = attrs[0].Type;
                    attrId = attrs[0].Id;
                }

                var v = Values(entityCode, attrCode);
                object vId;
                if (v.Count > 0) vId = v[0].Id;
                else vId = null;

                var m = new M("eav_values_" + type);

                if (vId == null) m.Data("entity_id", entityId).Data("attr_id", attrId);
                m.Data("value", value);
                m.Save(vId);
            }
        }
        
        /// <summary>
        /// Construct EAV tables.
        /// </summary>
        /// <param name="clearBefore">Clear EAV tables before contruction.</param>
        public static void Construct(bool clearBefore = true)
        {
            // Drop tables first
            if (clearBefore) Destruct();

            // Create Entity
            var eId = new FieldDesc { Name = "id", Type = "int", Constraints = "NOT NULL PRIMARY KEY IDENTITY(1, 1)", Index = true };
            var eModel = new FieldDesc { Name = "entity_model", Type = "nvarchar(128)", Constraints = "NOT NULL" };
            var eCode = new FieldDesc { Name = "entity_code", Type = "nvarchar(128)", Constraints = "NOT NULL" };
            M.Create("eav_entity", new List<FieldDesc> { eId, eModel, eCode }, false);

            // Create Attribute
            var aId = new FieldDesc { Name = "id", Type = "int", Constraints = "NOT NULL PRIMARY KEY IDENTITY(1, 1)", Index = true };
            var aCode = new FieldDesc { Name = "attr_code", Type = "nvarchar(128)", Constraints = "NOT NULL" };
            var aType = new FieldDesc { Name = "attr_type", Type = "nvarchar(64)", Constraints = "NOT NULL" };
            var aEModel = new FieldDesc { Name = "entity_model", Type = "nvarchar(128)", Constraints = "NOT NULL" };
            M.Create("eav_attr", new List<FieldDesc> { aId, aCode, aType, aEModel }, false);

            // Create Values
            var vId = new FieldDesc { Name = "id", Type = "int", Constraints = "NOT NULL PRIMARY KEY IDENTITY(1, 1)", Index = true };
            var vEId = new FieldDesc { Name = "entity_id", Type = "int", Constraints = "NOT NULL FOREIGN KEY REFERENCES eav_entity(id) ON DELETE CASCADE ON UPDATE CASCADE" };
            var vAId = new FieldDesc { Name = "attr_id", Type = "int", Constraints = "NOT NULL FOREIGN KEY REFERENCES eav_attr(id) ON DELETE CASCADE ON UPDATE CASCADE" };

            var vInt = new FieldDesc { Name = "value", Type = "int", };
            var vDecimal = new FieldDesc { Name = "value", Type = "DECIMAL(16, 16)" };
            var vVarChar = new FieldDesc { Name = "value", Type = "VARCHAR(max)" };
            var vDateTime = new FieldDesc { Name = "value", Type = "DATETIME" };
            var vBinary = new FieldDesc { Name = "value", Type = "VARBINARY(max)" };

            M.Create("eav_values_int", new List<FieldDesc> { vId, vEId, vAId, vInt }, false);
            M.Create("eav_values_decimal", new List<FieldDesc> { vId, vEId, vAId, vDecimal }, false);
            M.Create("eav_values_varchar", new List<FieldDesc> { vId, vEId, vAId, vVarChar }, false);
            M.Create("eav_values_datetime", new List<FieldDesc> { vId, vEId, vAId, vDateTime }, false);
            M.Create("eav_values_varbinary", new List<FieldDesc> { vId, vEId, vAId, vBinary }, false);
        }

        /// <summary>
        /// Destruct EAV tables.
        /// </summary>
        public static void Destruct()
        {
            M.Drop("eav_values_int");
            M.Drop("eav_values_decimal");
            M.Drop("eav_values_varchar");
            M.Drop("eav_values_datetime");
            M.Drop("eav_values_varbinary");
            M.Drop("eav_attr");
            M.Drop("eav_entity");
        }
    }
}
