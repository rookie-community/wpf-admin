using Admin.Desktop.ViewModel.Roles;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace Admin.Desktop.View.Roles
{
    /// <summary>
    /// RoleView.xaml 的交互逻辑
    /// </summary>
    public partial class RoleView : UserControl
    {
        private RoleVM vm;
        public RoleView()
        {
            InitializeComponent();
            vm = App.Current.Services.GetService<RoleVM>()!;
            vm.Initial(this);
            DataContext = vm;
        }
    }
}
