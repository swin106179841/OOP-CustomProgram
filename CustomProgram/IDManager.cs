namespace CustomProgram
{
    static class ObjectIDs
    {
        private static int _ids = 0;
        public static int NewID()
        {
            return ++_ids;
        }
    }
}