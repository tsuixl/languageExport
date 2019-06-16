namespace Language.Util
{
    public static class SystemUtil
    {
        public static void Kill (object o)
        {
            throw new System.Exception (o.ToString());
        }
    }
}