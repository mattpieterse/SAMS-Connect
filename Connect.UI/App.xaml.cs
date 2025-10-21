using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
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
        Ioc.Default.ConfigureServices(
            serviceProvider: GetServiceCollection().BuildServiceProvider()
        );

        UseApplicationThemes();
        var window = Ioc.Default.GetRequiredService<Shell>();
            window.Show();
    }

#endregion

#region Internals

    private static ServiceCollection GetServiceCollection() {
        var services = new ServiceCollection();

        services
            .AddScoped<Shell>()
            .AddSingleton<ThemeService>();

        return services;
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
