using Admin.Desktop.View;
using CommunityToolkit.Mvvm.ComponentModel;
using Volo.Abp.DependencyInjection;

namespace Admin.Desktop.ViewModel
{
    public partial class MainVM : ObservableValidator, ITransientDependency
    {
        public MainWindow Owner { get; private set; } = null!;

        public void Initial(MainWindow owner) 
        {
            Owner = owner;
        }
    }
}
