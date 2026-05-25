namespace Admin.Users
{
    public class LoginResultDto : UserDto
    {
        public string Token { get; set; } = null!;

        public Guid RefreshToken { get; set; }
    }
}
