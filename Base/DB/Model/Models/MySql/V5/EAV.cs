using MySql.Data.MySqlClient;
using System.Collections.Generic;
using MySql5M = Base.DB.Model.Models.MySql.V5.M;

namespace Base.DB.Model.Models.MySql.V5
{
    public class Eav : Eav<MySqlConnection, MySqlParameter, MySql5M>
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
                Constraints = "NOT NULL PRIMARY KEY AUTO_INCREMENT",
                Index = true
            };
            var eModel = new FieldDesc { Name = "entity_model", Type = "nvarchar(128)", Constraints = "NOT NULL" };
            var eCode = new FieldDesc { Name = "entity_code", Type = "nvarchar(128)", Constraints = "NOT NULL" };
            MySql5M.Create("eav_entity", new List<FieldDesc> {eId, eModel, eCode}, null, false);

            // Create Attribute
            var aId = new FieldDesc
            {
                Name = "id",
                Type = "int",
                Constraints = "NOT NULL PRIMARY KEY AUTO_INCREMENT",
                Index = true
            };
            var aCode = new FieldDesc { Name = "attr_code", Type = "nvarchar(128)", Constraints = "NOT NULL" };
            var aType = new FieldDesc { Name = "attr_type", Type = "nvarchar(64)", Constraints = "NOT NULL" };
            var aEModel = new FieldDesc { Name = "entity_model", Type = "nvarchar(128)", Constraints = "NOT NULL" };
            MySql5M.Create("eav_attr", new List<FieldDesc> { aId, aCode, aType, aEModel }, null, false);

            // Create Values
            var vId = new FieldDesc
            {
                Name = "id",
                Type = "int",
                Constraints = "NOT NULL PRIMARY KEY AUTO_INCREMENT",
                Index = true
            };
            var vEId = new FieldDesc {Name = "entity_id", Type = "int"};
            var vAId = new FieldDesc {Name = "attr_id", Type = "int"};

            var vInt = new FieldDesc { Name = "value", Type = "int", };
            var vDecimal = new FieldDesc { Name = "value", Type = "DECIMAL(38, 18)" };
            var vVarChar = new FieldDesc { Name = "value", Type = "LONGTEXT" };
            var vDateTime = new FieldDesc { Name = "value", Type = "DATETIME" };
            var vBinary = new FieldDesc { Name = "value", Type = "LONGBLOB" };

            var tableConstraints = new List<string>
            {
                "FOREIGN KEY (entity_id) REFERENCES eav_entity(id) ON DELETE CASCADE ON UPDATE CASCADE",
                "FOREIGN KEY (attr_id) REFERENCES eav_attr(id) ON DELETE CASCADE ON UPDATE CASCADE",
            };

            MySql5M.Create("eav_values_int", new List<FieldDesc> { vId, vEId, vAId, vInt }, tableConstraints, false);
            MySql5M.Create("eav_values_decimal", new List<FieldDesc> { vId, vEId, vAId, vDecimal }, tableConstraints, false);
            MySql5M.Create("eav_values_varchar", new List<FieldDesc> { vId, vEId, vAId, vVarChar }, tableConstraints, false);
            MySql5M.Create("eav_values_datetime", new List<FieldDesc> { vId, vEId, vAId, vDateTime }, tableConstraints, false);
            MySql5M.Create("eav_values_varbinary", new List<FieldDesc> { vId, vEId, vAId, vBinary }, tableConstraints, false);
        }


        /// <summary>
        /// Destruct EAV tables.
        /// </summary>
        public static void Destruct()
        {
            MySql5M.Drop("eav_values_int");
            MySql5M.Drop("eav_values_decimal");
            MySql5M.Drop("eav_values_varchar");
            MySql5M.Drop("eav_values_datetime");
            MySql5M.Drop("eav_values_varbinary");
            MySql5M.Drop("eav_attr");
            MySql5M.Drop("eav_entity");
        }
    }
}
