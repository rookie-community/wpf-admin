namespace Admin.Users
{
    public class LoginResultDto : UserDto
    {
        public string AccessToken { get; set; } = null!;

        public Guid RefreshToken { get; set; }

        /// <summary>
        /// 秒
        /// </summary>
        public int ExpiresIn { get; set; }
    }
}
