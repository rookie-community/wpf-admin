using Admin.Desktop.ViewModel.Accounts;
using Microsoft.Extensions.DependencyInjection;
using Window = HandyControl.Controls.Window;

namespace Admin.Desktop.View.Accounts
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        private readonly LoginVM vm;
        public Login()
        {
            InitializeComponent();
            vm = App.Current.Services.GetService<LoginVM>()!;
            Loaded += (s, e) =>
            {
                vm.Initial(this);
            };
            DataContext = vm;
        }
    }
}
