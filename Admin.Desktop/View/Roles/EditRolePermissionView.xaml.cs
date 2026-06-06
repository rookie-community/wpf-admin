using Admin.Desktop.ViewModel.Roles;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Admin.Desktop.View.Roles
{
    /// <summary>
    /// EditRolePermissionView.xaml 的交互逻辑
    /// </summary>
    public partial class EditRolePermissionView : Window
    {
        private readonly EditRolePermissionVM vm;
        private bool _isLoaded = false;
        public EditRolePermissionView(Guid roleId)
        {
            InitializeComponent();
            vm = App.Current.Services.GetService<EditRolePermissionVM>()!;
            Loaded += async (s, e) =>
            {
                if (_isLoaded)
                {
                    // 避免重复加载数据
                    return;
                }
                _isLoaded = true;
                await vm.InitialAsync(this);
            };
            DataContext = vm;
        }
    }
}
