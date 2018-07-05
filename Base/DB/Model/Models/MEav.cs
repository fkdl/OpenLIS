namespace Base.DB.Model.Models
{
    public class MEav
    {
        protected string TableName = "";
        protected string KeyField = "";

        public MEav(string table, string keyField = "ID")
        {
            TableName = table;
            KeyField = keyField;
        }


    }
}
