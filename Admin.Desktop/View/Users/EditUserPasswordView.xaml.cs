using Admin.Desktop.ViewModel.Users;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace Admin.Desktop.View.Users
{
    /// <summary>
    /// EditUserPassword.xaml 的交互逻辑
    /// </summary>
    public partial class EditUserPasswordView : UserControl
    {
        private readonly EditUserPasswordVM vm;
        private bool _isLoaded;
        public EditUserPasswordView()
        {
            InitializeComponent();
            vm = App.Current.Services.GetService<EditUserPasswordVM>()!;
            Loaded += (s, e) =>
            {
                if (_isLoaded)
                {
                    // 避免重复加载数据
                    return;
                }
                _isLoaded = true;
                vm.Initial(this);
            };
            DataContext = vm;
        }
    }
}
