using System;
using System.Collections.Generic;
using Base.DB.Model.Conditions;

namespace Base.DB.Model.Models
{
    public class MEav
    {
        protected string EntityModel = string.Empty;

        protected MPlain VInt = new MPlain("eav_values_int");
        protected MPlain VDecimal = new MPlain("eav_values_decimal");
        protected MPlain VVarchar = new MPlain("eav_values_varchar");
        protected MPlain VDatetime = new MPlain("eav_values_datetime");
        protected MPlain Entity = new MPlain("eav_entity");
        protected MPlain Attr = new MPlain("eav_attr");

        public MEav(string entityModel)
        {
            EntityModel = entityModel;

            VInt.Field("value").
                Join("eav_entity", "e", new CondRaw("entity_id = e.id"), "LEFT").
                Join("eav_attr", "a", new CondRaw("attr_id = a.id"), "LEFT").
                Where(new CondCmpr("e.entity_model", "=", entityModel)).
                Where(new CondCmpr("e.entity_code", "=", "", "@p_entity_code_int")).
                Where(new CondCmpr("a.attr_type", "=", "int")).
                Where(new CondCmpr("a.attr_code", "=", "", "@p_arrt_code_int"));

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
            set { if (value == null) throw new ArgumentNullException("value"); }
        }

        /// <summary>
        /// Create EAV tables.
        /// </summary>
        /// <returns></returns>
        public static bool Create()
        {
            // Drop tables first
            Conn.ExecuteNonQuery("IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID('eav_values_int')) DROP TABLE eav_values_int");
            Conn.ExecuteNonQuery("IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID('eav_values_decimal')) DROP TABLE eav_values_decimal");
            Conn.ExecuteNonQuery("IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID('eav_values_varchar')) DROP TABLE eav_values_varchar");
            Conn.ExecuteNonQuery("IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID('eav_values_datetime')) DROP TABLE eav_values_datetime");
            Conn.ExecuteNonQuery("IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID('eav_attr')) DROP TABLE eav_attr");
            Conn.ExecuteNonQuery("IF EXISTS (SELECT * FROM sysobjects WHERE id = OBJECT_ID('eav_entity')) DROP TABLE eav_entity");

            // Entity
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
            MPlain.Create("eav_entity", new List<FieldDescription> {eId, eModel, eCode}, false);

            // Attribute
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
            MPlain.Create("eav_attr", new List<FieldDescription> {aId, aCode, aType}, false);

            // Values
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
            var vInt = new FieldDescription {FieldName = "value", FieldType = "int",};
            var vDecimal = new FieldDescription {FieldName = "value", FieldType = "DECIMAL(16, 16)",};
            var vVarChar = new FieldDescription {FieldName = "value", FieldType = "VARCHAR(max)",};
            var vDateTime = new FieldDescription {FieldName = "value", FieldType = "DATETIME",};

            MPlain.Create("eav_values_int", new List<FieldDescription> { vId, vEId, vAId, vInt }, false);
            MPlain.Create("eav_values_decimal", new List<FieldDescription> { vId, vEId, vAId, vDecimal }, false);
            MPlain.Create("eav_values_varchar", new List<FieldDescription> { vId, vEId, vAId, vVarChar }, false);
            MPlain.Create("eav_values_datetime", new List<FieldDescription> { vId, vEId, vAId, vDateTime }, false);

            return true;
        }
    }
}
