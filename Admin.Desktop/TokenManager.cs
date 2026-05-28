using System.IdentityModel.Tokens.Jwt;

namespace Admin.Desktop
{
    public static class TokenManager
    {
        public static string AccessToken { get; private set; } = null!;
        public static Guid RefreshToken { get; private set; }

        public static void SetTokens(string accessToken, Guid refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;

            // 直接将 JWT 字符串 转换成 JwtSecurityToken 对象
            // 这里只会解析 Header 和 Payload，不会验证签名
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(accessToken) ?? throw new InvalidOperationException("无法解析 JWT 字符串");
        }
    }
}
