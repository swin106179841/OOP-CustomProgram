namespace CustomProgram
{
    static class ObjectIDs
    {
        private static int _ids;
        public static int NewID()
        {
            return ++_ids;
        }
    }
}