using Admin.Identity;

namespace Admin.Users
{
    public class UserApplicationService : AdminAppService, IUserApplicationService
    {
        private readonly IUserRepository _userRepository;

        public UserApplicationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<LoginResultDto> Login(LoginDto input)
        {
            var user = await _userRepository.FindAsync(x => x.Account == input.Account && x.Password == input.Password);
            if (user == null)
            {
                return new LoginResultDto();
            }
            var result = new LoginResultDto
            {
                Id = user.Id,
                Account = user.Account,
                UserName = user.UserName,
                RefreshToken = Guid.NewGuid()
            };
            return result;
        }
    }
}
