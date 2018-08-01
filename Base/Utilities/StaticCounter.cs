namespace Base.Utilities
{
    public static class StaticCounter
    {
        private static int _next;

        public static int Next => _next++;
    }
}
