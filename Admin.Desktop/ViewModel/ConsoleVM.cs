using Admin.Desktop.UserControls;
using Admin.Desktop.View;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using Volo.Abp.DependencyInjection;

namespace Admin.Desktop.ViewModel
{
    public partial class ConsoleVM : ObservableObject, ITransientDependency
    {
        [ObservableProperty]
        private ObservableCollection<LogInfo> logs = new ObservableCollection<LogInfo>();

        public double[] Values1 { get; set; } =
        [2, 1, 3, 5, 3, 4, 6];

        public int[] Values2 { get; set; } =
            [4, 2, 5, 2, 4, 5, 3];

        private readonly ILogger<ConsoleVM> _logger;
        public ConsoleView Owner { get; private set; }

        public ConsoleVM(ILogger<ConsoleVM> logger)
        {
            _logger = logger;
        }

        internal void Initial(ConsoleView owner)
        {
            Owner = owner;
            Logs.Add(new LogInfo { Messages = "Console initialized." });
        }
    }
}
