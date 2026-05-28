using Admin.Desktop.View;
using Admin.Permissions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Volo.Abp.DependencyInjection;

namespace Admin.Desktop.ViewModel
{
    public partial class MainVM : ObservableValidator, ITransientDependency
    {
        [ObservableProperty]
        private ObservableCollection<MenuItem> menuItems = new ObservableCollection<MenuItem>();

        public MainWindow Owner { get; private set; } = null!;

        private readonly IPermissionApplicationService permissionApplicationService;
        private ILogger<MainVM> _logger;

        public MainVM(IPermissionApplicationService permissionApplicationService, ILogger<MainVM> logger)
        {
            this.permissionApplicationService = permissionApplicationService;
            _logger = logger;
        }

        public void Initial(MainWindow owner)
        {
            Owner = owner;
            var permissions = permissionApplicationService.Test().Result;
        }

        [RelayCommand]
        private void CurrentViewShow()
        {
            Owner.Show();
        }
    }
}
