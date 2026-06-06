using Admin.Desktop.ViewModel.Users;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace Admin.Desktop.View.Users
{
    /// <summary>
    /// UserInfoView.xaml 的交互逻辑
    /// </summary>
    public partial class UserInfoView : UserControl
    {
        private readonly UserInfoVM vm;
        private bool _isLoaded = false;
        public UserInfoView()
        {
            InitializeComponent();
            vm = App.Current.Services.GetService<UserInfoVM>()!;
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
