using Admin.Desktop.View.Tenants;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using Volo.Abp.DependencyInjection;
using Volo.Abp.TenantManagement;
using Volo.Abp.Validation;

namespace Admin.Desktop.ViewModel.Tenants
{
    public partial class TenantVM : ObservableObject, ITransientDependency
    {
        private readonly ITenantAppService _tenantAppService;
        private readonly ILogger<TenantVM> _logger;

        [ObservableProperty]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        public partial ObservableCollection<TenantDto> Tenants { get; set; } = new ObservableCollection<TenantDto>();

        [ObservableProperty]
        public partial int PageIndex { get; set; } = 1;

        [ObservableProperty]
        public partial long TotalCount { get; set; }

        [ObservableProperty]
        public partial int PageSize { get; set; } = 30;

        [ObservableProperty]
        public partial string DialogContainerToken { get; set; } = Guid.NewGuid().ToString();

        public TenantView Owner { get; private set; } = null!;

        public TenantVM(ITenantAppService tenantAppService, ILogger<TenantVM> logger)
        {
            _tenantAppService = tenantAppService;
            _logger = logger;
        }

        internal async Task InitialAsync(TenantView owner)
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
                var result = await _tenantAppService.GetListAsync(new GetTenantsInput
                {
                    Filter = Name,
                    SkipCount = (PageIndex - 1) * PageSize,
                    MaxResultCount = PageSize
                });
                TotalCount = result.TotalCount;
                Tenants = new ObservableCollection<TenantDto>(result.Items);
            }
            catch (AbpValidationException abpEx)
            {
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
            MessageBox.Show($"{pageArgs.Item1}_{pageArgs.Item2}");
            PageIndex = pageArgs.Item1;
            PageSize = pageArgs.Item2;
            await SearchCommand.ExecuteAsync(null);
        }
    }
}
