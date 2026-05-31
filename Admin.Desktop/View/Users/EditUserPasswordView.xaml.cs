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
        private EditUserPasswordVM vm;
        public EditUserPasswordView()
        {
            InitializeComponent();
            vm = App.Current.Services.GetService<EditUserPasswordVM>()!;
            vm.Initial(this);
            DataContext = vm;
        }
    }
}
