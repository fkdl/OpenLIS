using System.Collections.Generic;
using System.Data.SqlClient;
using Sql08M = Base.DB.Model.Models.SqlServer.V2008.M;

namespace Base.DB.Model.Models.SqlServer.V2008
{
    public class Eav : Eav<SqlConnection, SqlParameter, Sql08M>
    {
        public Eav(string entityModel) : base(entityModel)
        {

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
            var eId = new FieldDesc
            {
                Name = "id",
                Type = "int",
                Constraints = "NOT NULL PRIMARY KEY IDENTITY(1, 1)",
                Index = true
            };
            var eModel = new FieldDesc {Name = "entity_model", Type = "nvarchar(128)", Constraints = "NOT NULL"};
            var eCode = new FieldDesc {Name = "entity_code", Type = "nvarchar(128)", Constraints = "NOT NULL"};
            Sql08M.Create("eav_entity", new List<FieldDesc> {eId, eModel, eCode}, null, false);

            // Create Attribute
            var aId = new FieldDesc
            {
                Name = "id",
                Type = "int",
                Constraints = "NOT NULL PRIMARY KEY IDENTITY(1, 1)",
                Index = true
            };
            var aCode = new FieldDesc {Name = "attr_code", Type = "nvarchar(128)", Constraints = "NOT NULL"};
            var aType = new FieldDesc {Name = "attr_type", Type = "nvarchar(64)", Constraints = "NOT NULL"};
            var aEModel = new FieldDesc {Name = "entity_model", Type = "nvarchar(128)", Constraints = "NOT NULL"};
            Sql08M.Create("eav_attr", new List<FieldDesc> {aId, aCode, aType, aEModel}, null, false);

            // Create Values
            var vId = new FieldDesc
            {
                Name = "id",
                Type = "int",
                Constraints = "NOT NULL PRIMARY KEY IDENTITY(1, 1)",
                Index = true
            };
            var vEId = new FieldDesc
            {
                Name = "entity_id",
                Type = "int",
                Constraints = "NOT NULL FOREIGN KEY REFERENCES eav_entity(id) ON DELETE CASCADE ON UPDATE CASCADE"
            };
            var vAId = new FieldDesc
            {
                Name = "attr_id",
                Type = "int",
                Constraints = "NOT NULL FOREIGN KEY REFERENCES eav_attr(id) ON DELETE CASCADE ON UPDATE CASCADE"
            };

            var vInt = new FieldDesc {Name = "value", Type = "int",};
            var vDecimal = new FieldDesc {Name = "value", Type = "DECIMAL(38, 18)"};
            var vVarChar = new FieldDesc {Name = "value", Type = "VARCHAR(max)"};
            var vDateTime = new FieldDesc {Name = "value", Type = "DATETIME"};
            var vBinary = new FieldDesc {Name = "value", Type = "VARBINARY(max)"};

            Sql08M.Create("eav_values_int", new List<FieldDesc> {vId, vEId, vAId, vInt}, null, false);
            Sql08M.Create("eav_values_decimal", new List<FieldDesc> {vId, vEId, vAId, vDecimal}, null, false);
            Sql08M.Create("eav_values_varchar", new List<FieldDesc> {vId, vEId, vAId, vVarChar}, null, false);
            Sql08M.Create("eav_values_datetime", new List<FieldDesc> {vId, vEId, vAId, vDateTime}, null, false);
            Sql08M.Create("eav_values_varbinary", new List<FieldDesc> {vId, vEId, vAId, vBinary}, null, false);
        }

        /// <summary>
        /// Destruct EAV tables.
        /// </summary>
        public static void Destruct()
        {
            Sql08M.Drop("eav_values_int");
            Sql08M.Drop("eav_values_decimal");
            Sql08M.Drop("eav_values_varchar");
            Sql08M.Drop("eav_values_datetime");
            Sql08M.Drop("eav_values_varbinary");
            Sql08M.Drop("eav_attr");
            Sql08M.Drop("eav_entity");
        }
    }
}
