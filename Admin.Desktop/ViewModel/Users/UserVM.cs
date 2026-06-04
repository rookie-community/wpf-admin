using Admin.Desktop.View.Users;
using Admin.Users;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Validation;
using MessageBox = HandyControl.Controls.MessageBox;

namespace Admin.Desktop.ViewModel.Users
{
    public partial class UserVM : ObservableObject, ITransientDependency
    {
        [ObservableProperty]
        private string userName = string.Empty;

        [ObservableProperty]
        private string phoneNumber = string.Empty;

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private ObservableCollection<UserDto> users = new ObservableCollection<UserDto>();

        [ObservableProperty]
        private int pageIndex = 1;

        [ObservableProperty]
        private long totalCount;

        [ObservableProperty]
        private int pageSize = 30;

        [ObservableProperty]
        private string dialogContainerToken = Guid.NewGuid().ToString();

        public UserView Owner { get; private set; } = null!;

        private readonly IUserApplicationService _userApplicationService;
        private readonly ILogger<UserVM> _logger;
        public UserVM(IUserApplicationService userApplicationService, ILogger<UserVM> logger)
        {
            _userApplicationService = userApplicationService;
            _logger = logger;
        }

        internal void Initial(UserView owner)
        {
            Owner = owner;
        }

        [RelayCommand]
        private async Task SearchAsync()
        {
            var loadDialog = Dialog.Show(new LoadingCircle(), DialogContainerToken);
            try
            {
                var result = await _userApplicationService.GetListAsync(new GetUserListDto
                {
                    UserName = UserName,
                    PhoneNumber = PhoneNumber,
                    Email = Email,
                    SkipCount = (PageIndex - 1) * PageSize,
                    MaxResultCount = PageSize
                });
                TotalCount = result.TotalCount;
                Users = new ObservableCollection<UserDto>(result.Items);
            }
            catch (AbpValidationException abpEx)
            {
                var errorMessages = abpEx.ValidationErrors.Select(x => x.ErrorMessage);
                MessageBox.Error(string.Join('.', errorMessages), abpEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Error(ex.Message);
            }
            finally
            {
                loadDialog.Close();
            }
        }

        [RelayCommand]
        private void Reset()
        {
            UserName = string.Empty;
            PhoneNumber = string.Empty;
            Email = string.Empty;
        }
    }
}
