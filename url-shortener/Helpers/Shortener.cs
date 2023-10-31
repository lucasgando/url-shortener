namespace url_shortener.Helpers
{
    public static class Shortener
    {
        private static char[] Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ123456789".ToCharArray();
        private static int Base = Alphabet.Length;
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
