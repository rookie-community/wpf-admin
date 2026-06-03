using Volo.Abp.Application.Dtos;

namespace Admin.Users
{
    public class TokenResultDto : EntityDto<Guid>
    {
        public string UserName { get; set; } = null!;

        public string TokenType { get; set; } = null!;

        public string AccessToken { get; set; } = null!;

        public Guid RefreshToken { get; set; }

        /// <summary>
        /// 秒
        /// </summary>
        public double ExpiresIn { get; set; }
    }
}
