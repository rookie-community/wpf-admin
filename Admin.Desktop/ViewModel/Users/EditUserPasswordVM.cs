using Admin.Desktop.View.Users;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace Admin.Desktop.ViewModel.Users
{
    public partial class EditUserPasswordVM : ObservableObject, ITransientDependency
    {
        private readonly ILogger<EditUserPasswordVM> _logger;

        public EditUserPasswordView Owner { get; private set; } = null!;

        public EditUserPasswordVM(ILogger<EditUserPasswordVM> logger)
        {
            _logger = logger;
        }

        internal void Initial(EditUserPasswordView owner)
        {
            Owner = owner;
        }
    }
}
