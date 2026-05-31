using Admin.Desktop.View.Users;
using CommunityToolkit.Mvvm.ComponentModel;
using Volo.Abp.DependencyInjection;

namespace Admin.Desktop.ViewModel.Users
{
    public partial class UserVM : ObservableObject, ITransientDependency
    {
        public UserView Owner { get; private set; } = null!;

        internal void Initial(UserView owner)
        {
            Owner = owner;
        }
    }
}
