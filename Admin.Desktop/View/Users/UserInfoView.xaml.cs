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
        public UserInfoView()
        {
            InitializeComponent();
            vm = App.Current.Services.GetService<UserInfoVM>()!;
            vm.Initial(this);
            DataContext = vm;
        }
    }
}
