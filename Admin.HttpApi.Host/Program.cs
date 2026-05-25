// 配置Serilog日志记录器
using Admin.HttpApi.Host;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
#if DEBUG
    .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Async(c => c.File("Logs/logs.txt"))
    .WriteTo.Async(c => c.Console())
    .CreateLogger();

try
{
    Log.Information("Starting web host.");

    // 创建Web应用程序构建器
    var builder = WebApplication.CreateBuilder(args);

    // 配置主机，添加设置文件、Autofac容器和Serilog日志
    builder.Host.AddAppSettingsSecretsJson()
        .UseAutofac()
        .UseSerilog();

    // 添加ABP应用程序模块
    await builder.AddApplicationAsync<AdminHttpApiModuleHostModule>();

    // 构建Web应用程序
    var app = builder.Build();
    // 初始化应用程序，包括数据库迁移等
    await app.InitializeApplicationAsync();

    // 启动Web应用程序
    await app.RunAsync();
}
catch (Exception ex)
{
    // 处理启动过程中的异常
    if (ex is HostAbortedException)
    {
        throw;
    }

    Log.Fatal(ex, "Host terminated unexpectedly during startup");
}
finally
{
    // 确保日志记录器正确关闭
    Log.CloseAndFlush();
}