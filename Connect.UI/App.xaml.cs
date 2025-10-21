using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using Connect.UI.Shells;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Wpf.Ui.Appearance;
using ThemeService = Connect.UI.Services.ThemeService;

namespace Connect.UI;

public sealed partial class App
{
#region Lifecycle

    /// <summary>
    /// The main entry-point for the application.
    /// </summary>
    [UsedImplicitly]
    private void OnStartup(object sender, StartupEventArgs e) {
        Log.Logger = GetLogsConfiguration().CreateLogger();
        Log.Information(
            "Application started successfully."
        );

        Ioc.Default.ConfigureServices(
            serviceProvider: GetServiceCollection().BuildServiceProvider()
        );

        UseApplicationThemes();
        var window = Ioc.Default.GetRequiredService<Shell>();
        window.Show();
    }


    protected override void OnExit(ExitEventArgs e) {
        Log.CloseAndFlush();
        base.OnExit(e);
    }

#endregion

#region Internals

    private static ServiceCollection GetServiceCollection() {
        var services = new ServiceCollection();

        services
            .AddScoped<Shell>()
            .AddScoped<ShellViewModel>()
            .AddSingleton<ThemeService>();

        return services;
    }


    private static LoggerConfiguration GetLogsConfiguration() {
        var configuration = new LoggerConfiguration();

        configuration
            .MinimumLevel.Debug()
            .WriteTo.File("logs.txt", rollingInterval: RollingInterval.Day)
            .WriteTo.Console();

        return configuration;
    }


    private static void UseApplicationThemes() {
        var globalThemeService = Ioc.Default.GetService<ThemeService>();
        globalThemeService?.Listen();

        ApplicationThemeManager.Apply(
            applicationTheme: ApplicationThemeManager.GetAppTheme()
        );
    }

#endregion
}
