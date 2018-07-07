namespace Base.DB.Model.Models
{
    public class MEav
    {
        protected string TableName = "";
        protected string KeyField = "";

        protected MPlain E;
        protected MPlain A;
        protected MPlain V;

        public MEav(string table, string keyField = "ID")
        {
            TableName = table;
            KeyField = keyField;

            E = new MPlain(table + "_eav_entity");
            A = new MPlain(table + "_eav_attribbute");
            V = new MPlain(table + "_eav_value");
        }

        public static bool Create(string tableName)
        {
            // TODO
            return true;
        }
    }
}
