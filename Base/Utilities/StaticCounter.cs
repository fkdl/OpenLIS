namespace Base.Utilities
{
    public static class StaticCounter
    {
        private static int next = 0;

        public static int Next { get { return next++; } }
    }
}
