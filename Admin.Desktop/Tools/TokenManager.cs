using Duende.IdentityModel.Client;

namespace Admin.Desktop.Tools
{
    public static class TokenManager
    {
        public static string AccessToken { get; private set; } = null!;
        public static string RefreshToken { get; private set; } = null!;

        public static void SetTokens(TokenResponse tokenResponse)
        {
            AccessToken = tokenResponse.AccessToken!;
            RefreshToken = tokenResponse.RefreshToken!;
        }
    }
}
