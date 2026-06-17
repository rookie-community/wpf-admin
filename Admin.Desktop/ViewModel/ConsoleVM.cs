using Admin.Desktop.UserControls;
using Admin.Desktop.View;
using CommunityToolkit.Mvvm.ComponentModel;
using HandyControl.Controls;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using Volo.Abp.DependencyInjection;
using MessageBox = HandyControl.Controls.MessageBox;

namespace Admin.Desktop.ViewModel
{
    public partial class ConsoleVM : ObservableObject, ITransientDependency
    {
        private readonly ILogger<ConsoleVM> _logger;
        private readonly DispatcherTimer _timer;

        [ObservableProperty]
        public partial ObservableCollection<LogInfo> Logs { get; set; } = new ObservableCollection<LogInfo>();

        [ObservableProperty]
        public partial string DialogContainerToken { get; set; } = Guid.NewGuid().ToString();
        public double[] Values1 { get; set; } =
        [2, 1, 3, 5, 3, 4, 6];

        public int[] Values2 { get; set; } =
            [4, 2, 5, 2, 4, 5, 3];

        public ConsoleView Owner { get; private set; } = null!;

        public ConsoleVM(ILogger<ConsoleVM> logger)
        {
            _logger = logger;
            _timer = new DispatcherTimer
            {
                // 间隔：500毫秒执行一次
                Interval = TimeSpan.FromMilliseconds(500)
            };
            // 绑定定时触发事件
            _timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            Owner.logControl.Info($"定时输出：{DateTime.Now:HH:mm:ss}");
        }

        internal async Task InitialAsync(ConsoleView owner)
        {
            var loadDialog = Dialog.Show<LoadingCircle>(DialogContainerToken);
            try
            {
                Owner = owner;
                await Task.Run(() =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Logs.Add(new LogInfo { Messages = "Console initialized." });
                    });
                });
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                MessageBox.Error(ex.Message);
            }
            finally
            {
                // 启动
                _timer.Start();
                loadDialog.Close();
            }
        }
    }
}
