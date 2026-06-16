using Admin.AuditLogs;
using Admin.Desktop.View.Identity.OrganizationUnits;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Windows;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Validation;
using MessageBox = HandyControl.Controls.MessageBox;

namespace Admin.Desktop.ViewModel.Identity.OrganizationUnits
{
    public partial class OrganizationUnitVM : ObservableObject, ITransientDependency
    {
        private readonly IAuditLogAppService _auditLogAppService;
        private readonly ILogger<OrganizationUnitVM> _logger;

        [ObservableProperty]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        public partial ObservableCollection<AuditLogDto> AuditLogs { get; set; } = new ObservableCollection<AuditLogDto>();

        [ObservableProperty]
        public partial int PageIndex { get; set; } = 1;

        [ObservableProperty]
        public partial long TotalCount { get; set; }

        [ObservableProperty]
        public partial int PageSize { get; set; } = 30;

        [ObservableProperty]
        public partial string DialogContainerToken { get; set; } = Guid.NewGuid().ToString();

        [ObservableProperty]
        public partial Dictionary<string, Visibility> BtnPerms { get; set; } = new Dictionary<string, Visibility>();

        public OrganizationUnitView Owner { get; private set; } = null!;

        public OrganizationUnitVM(IAuditLogAppService auditLogAppService, ILogger<OrganizationUnitVM> logger)
        {
            _auditLogAppService = auditLogAppService;
            _logger = logger;
        }

        internal async Task InitialAsync(OrganizationUnitView owner)
        {
            var loadDialog = Dialog.Show<LoadingCircle>(DialogContainerToken);
            try
            {
                Owner = owner;
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                MessageBox.Error(ex.Message);
            }
            finally
            {
                loadDialog.Close();
            }
        }

        [RelayCommand]
        private async Task SearchAsync()
        {
            var loadDialog = Dialog.Show<LoadingCircle>(DialogContainerToken);
            try
            {
                await LoadDataAsync();
            }
            catch (AbpValidationException abpEx)
            {
                _logger.LogException(abpEx);
                var errorMessages = abpEx.ValidationErrors.Select(x => x.ErrorMessage);
                MessageBox.Error(string.Join('.', errorMessages), abpEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
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
            Name = string.Empty;
        }

        [RelayCommand]
        private async Task PageChangedAsync(Tuple<int, int> pageArgs)
        {
            PageIndex = pageArgs.Item1;
            PageSize = pageArgs.Item2;
            await SearchCommand.ExecuteAsync(null);
        }

        private async Task LoadDataAsync()
        {
            var result = await _auditLogAppService.GetListAsync(new GetAuditLogListInput
            {
                UserName = Name,
                SkipCount = (PageIndex - 1) * PageSize,
                MaxResultCount = PageSize
            });
            TotalCount = result.TotalCount;
            AuditLogs = new ObservableCollection<AuditLogDto>(result.Items);
        }
    }
}
