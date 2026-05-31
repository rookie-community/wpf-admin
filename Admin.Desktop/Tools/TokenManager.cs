namespace Admin.Desktop.Tools
{
    public static class TokenManager
    {
        public static string AccessToken { get; private set; } = null!;
        public static Guid RefreshToken { get; private set; }

        public static void SetTokens(string accessToken, Guid refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
