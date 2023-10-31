namespace url_shortener.Helpers
{
    public static class Shortener
    {
        private static char[] Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMOPQRSTUVWXYZ12345789".ToCharArray();
        private static int Base = 62;
        public static string GetShortUrl()
        {
            Random random = new Random();
            string url = "";
            for (int iterate = 0; iterate < 8; iterate++)
                url += Alphabet[random.Next() % Base];
            return url;
        }
    }
}
