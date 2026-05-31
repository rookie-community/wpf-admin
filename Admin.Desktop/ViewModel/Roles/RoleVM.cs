using Admin.Desktop.View.Roles;
using CommunityToolkit.Mvvm.ComponentModel;
using Volo.Abp.DependencyInjection;

namespace Admin.Desktop.ViewModel.Roles
{
    public partial class RoleVM : ObservableObject, ITransientDependency
    {
        public RoleView Owner { get; private set; } = null!;

        internal void Initial(RoleView owner)
        {
            Owner = owner;
        }
    }
}
