using Admin.Desktop.ViewModel.Users;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace Admin.Desktop.View.Users
{
    /// <summary>
    /// UserView.xaml 的交互逻辑
    /// </summary>
    public partial class UserView : UserControl
    {
        private readonly UserVM vm;
        public UserView()
        {
            InitializeComponent();
            vm = App.Current.Services.GetService<UserVM>()!;
            vm.Initial(this);
            DataContext = vm;
        }

        private void PaginationPlus_PageChanged(int arg1, int arg2)
        {
            MessageBox.Show($"{arg1}_{arg2}");
        }
    }
}
