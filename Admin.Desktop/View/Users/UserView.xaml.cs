using Admin.Desktop.ViewModel.Users;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace Admin.Desktop.View.Users
{
    /// <summary>
    /// UserView.xaml 的交互逻辑
    /// </summary>
    public partial class UserView : UserControl
    {
        private UserVM vm;
        public UserView()
        {
            InitializeComponent();
            vm = App.Current.Services.GetService<UserVM>()!;
            vm.Initial(this);
            DataContext = vm;
        }
    }
}
