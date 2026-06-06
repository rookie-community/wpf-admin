using Admin.Desktop.View.Users;
using Admin.Users;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
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

        [ObservableProperty]
        private Dictionary<string, Visibility> buttonVis = new Dictionary<string, Visibility>();

        public UserView Owner { get; private set; } = null!;

        private readonly IUserApplicationService _userApplicationService;
        private readonly ILogger<UserVM> _logger;
        public UserVM(IUserApplicationService userApplicationService, ILogger<UserVM> logger)
        {
            _userApplicationService = userApplicationService;
            _logger = logger;
        }

        internal async Task InitialAsync(UserView owner)
        {
            Owner = owner;
            await SearchCommand.ExecuteAsync(null);
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
                //TotalCount = result.TotalCount;
                TotalCount = 1000;
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

        [RelayCommand]
        private async Task AddAsync()
        {
            try
            {
                await SearchCommand.ExecuteAsync(null);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                MessageBox.Error(ex.Message);
            }
        }

        [RelayCommand]
        private async Task EditAsync(UserDto user)
        {
            try
            {
                await SearchCommand.ExecuteAsync(null);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                MessageBox.Error(ex.Message);
            }
        }

        [RelayCommand]
        private async Task Delete(UserDto user)
        {
            Dialog? loadDialog = null;
            try
            {
                var result = MessageBox.Ask($"确认删除数据？");
                if (result != MessageBoxResult.OK)
                {
                    return;
                }
                loadDialog = Dialog.Show<LoadingCircle>();
                await _userApplicationService.BatchDelete(new List<Guid> { user.Id });
                await SearchCommand.ExecuteAsync(null);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                MessageBox.Error(ex.Message);
            }
            finally
            {
                loadDialog?.Close();
            }
        }

        [RelayCommand]
        private async Task BatchDelete(IList sender)
        {
            Dialog? loadDialog = null;
            try
            {
                var userIds = sender.Cast<UserDto>().Select(x => x.Id).ToList();
                if (userIds.Count == 0)
                {
                    MessageBox.Warning("未选中任何数据！");
                    return;
                }
                var result = MessageBox.Ask($"确认删除选中数据？");
                if (result != MessageBoxResult.OK)
                {
                    return;
                }
                loadDialog = Dialog.Show<LoadingCircle>();
                await _userApplicationService.BatchDelete(userIds);
                await SearchCommand.ExecuteAsync(null);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                MessageBox.Error(ex.Message);
            }
            finally
            {
                loadDialog?.Close();
            }
        }

        [RelayCommand]
        private async Task PageChangedAsync(Tuple<int, int> pageArgs)
        {
            MessageBox.Show($"{pageArgs.Item1}_{pageArgs.Item2}");
            PageIndex = pageArgs.Item1;
            PageSize = pageArgs.Item2;
            await SearchCommand.ExecuteAsync(null);
        }
    }
}
