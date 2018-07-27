namespace Base.DB.Model.Models
{
    public class Entity
    {
        public int Id;
        public string Model;
        public string Code;
    }

    public class Attr
    {
        public int Id;
        public string Code;
        public string Type;
        public string Model;
    }

    public class Value
    {
        public int Id;
        public int EntityId;
        public int AttrId;
        public object Val;
    }
}
