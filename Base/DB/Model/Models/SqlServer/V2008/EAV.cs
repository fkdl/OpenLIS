using Base.DB.Model.CondExpr;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Base.DB.Model.Models.SqlServer.V2008
{
    public class EAV
    {
        protected string EntityModel = string.Empty;

        protected M VInt = new M("eav_values_int");
        protected M VDecimal = new M("eav_values_decimal");
        protected M VVarchar = new M("eav_values_varchar");
        protected M VDatetime = new M("eav_values_datetime");
        protected M Entity = new M("eav_entity");
        protected M Attr = new M("eav_attr");

        public EAV(string entityModel)
        {
            EntityModel = entityModel;

            VInt.Field("value").
                Join("eav_entity", "e", new CondRaw<SqlParameter>("entity_id = e.id"), "LEFT").
                Join("eav_attr", "a", new CondRaw<SqlParameter>("attr_id = a.id"), "LEFT").
                Where(new CondCmpr<SqlParameter>("e.entity_model", "=", entityModel)).
                Where(new CondCmpr<SqlParameter>("e.entity_code", "=", "", "@p_entity_code_int")).
                Where(new CondCmpr<SqlParameter>("a.attr_type", "=", "int")).
                Where(new CondCmpr<SqlParameter>("a.attr_code", "=", "", "@p_arrt_code_int"));

        }

        public object this[string entityCode, string attrCode]
        {
            get
            {
                VInt.SetParamValue("@p_entity_code_int", entityCode);
                VInt.SetParamValue("@p_arrt_code_int", attrCode);
                var result = VInt.Select();

                return result.Rows.Count == 1 ? result.Rows[0]["value"] : 0;
            }
            set { if (value == null) throw new ArgumentNullException("value == null"); }
        }

        /// <summary>
        /// Create EAV tables.
        /// </summary>
        /// <returns></returns>
        public static bool Create()
        {
            // Drop tables first
            M.Drop("eav_values_int");
            M.Drop("eav_values_decimal");
            M.Drop("eav_values_varchar");
            M.Drop("eav_values_datetime");
            M.Drop("eav_attr");
            M.Drop("eav_entity");

            // Create Entity
            var eId = new FieldDescription
            {
                FieldName = "id",
                FieldType = "int",
                Constraints = "NOT NULL PRIMARY KEY IDENTITY(1, 1)",
                IfIndex = true,
            };
            var eModel = new FieldDescription
            {
                FieldName = "entity_model",
                FieldType = "nvarchar(128)",
                Constraints = "NOT NULL",
                IfIndex = true,
            };
            var eCode = new FieldDescription
            {
                FieldName = "entity_code",
                FieldType = "nvarchar(128)",
                Constraints = "NOT NULL",
                IfIndex = true,
            };
            M.Create("eav_entity", new List<FieldDescription> { eId, eModel, eCode }, false);

            // Create Attribute
            var aId = new FieldDescription
            {
                FieldName = "id",
                FieldType = "int",
                Constraints = "NOT NULL PRIMARY KEY IDENTITY(1, 1)",
                IfIndex = true,
            };
            var aCode = new FieldDescription
            {
                FieldName = "attr_code",
                FieldType = "nvarchar(128)",
                Constraints = "NOT NULL",
                IfIndex = true,
            };
            var aType = new FieldDescription
            {
                FieldName = "attr_type",
                FieldType = "nvarchar(64)",
                Constraints = "NOT NULL",
                IfIndex = true,
            };
            M.Create("eav_attr", new List<FieldDescription> { aId, aCode, aType }, false);

            // Create Values
            var vId = new FieldDescription
            {
                FieldName = "id",
                FieldType = "int",
                Constraints = "NOT NULL PRIMARY KEY IDENTITY(1, 1)",
                IfIndex = true,
            };
            var vEId = new FieldDescription
            {
                FieldName = "entity_id",
                FieldType = "int",
                Constraints = "NOT NULL FOREIGN KEY REFERENCES eav_entity(id) ON DELETE CASCADE ON UPDATE CASCADE",
                IfIndex = true,
            };
            var vAId = new FieldDescription
            {
                FieldName = "attr_id",
                FieldType = "int",
                Constraints = "NOT NULL FOREIGN KEY REFERENCES eav_attr(id) ON DELETE CASCADE ON UPDATE CASCADE",
                IfIndex = true,
            };
            var vInt = new FieldDescription { FieldName = "value", FieldType = "int", };
            var vDecimal = new FieldDescription { FieldName = "value", FieldType = "DECIMAL(16, 16)", };
            var vVarChar = new FieldDescription { FieldName = "value", FieldType = "VARCHAR(max)", };
            var vDateTime = new FieldDescription { FieldName = "value", FieldType = "DATETIME", };

            M.Create("eav_values_int", new List<FieldDescription> { vId, vEId, vAId, vInt }, false);
            M.Create("eav_values_decimal", new List<FieldDescription> { vId, vEId, vAId, vDecimal }, false);
            M.Create("eav_values_varchar", new List<FieldDescription> { vId, vEId, vAId, vVarChar }, false);
            M.Create("eav_values_datetime", new List<FieldDescription> { vId, vEId, vAId, vDateTime }, false);

            return true;
        }
    }
}
