using System.Globalization;
using System.Reflection;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using Connect.Data.Caches;
using Connect.UI.Models.Data;
using Connect.UI.Models.Data.Validations;
using Connect.UI.Services.Appearance;
using Connect.UI.Shells;
using Connect.UI.Views.Forum;
using Connect.UI.Views.Home;
using Connect.UI.Views.Ticket;
using FluentValidation;
using JetBrains.Annotations;
using Lepo.i18n.DependencyInjection;
using Lepo.i18n.Yaml;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.DependencyInjection;
using IThemeService = Connect.UI.Services.Appearance.IThemeService;
using ThemeService = Connect.UI.Services.Appearance.ThemeService;

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
            .AddStringLocalizer(builder => {
                    var app = Assembly.GetExecutingAssembly();
                    builder.FromYaml(app, "/Assets/Languages/Translations-en-US.yaml", new CultureInfo("en-US"));
                    builder.FromYaml(app, "/Assets/Languages/Translations-en-GB.yaml", new CultureInfo("en-GB"));
                    builder.FromYaml(app, "/Assets/Languages/Translations-en-ZA.yaml", new CultureInfo("en-ZA"));

                    var allowed = new List<string> {
                        new CultureInfo("en-GB").Name
                    };

                    var culture = allowed.Contains(CultureInfo.CurrentUICulture.Name)
                        ? CultureInfo.CurrentCulture
                        : new CultureInfo("en-GB");

                    CultureInfo.CurrentUICulture = culture;
                    CultureInfo.CurrentCulture = culture;
                }
            )
            .AddNavigationViewPageProvider()
            .AddSingleton<INavigationService, NavigationService>()
            .AddSingleton<IContentDialogService, ContentDialogService>()
            .AddSingleton<ISnackbarService, SnackbarService>()
            .AddSingleton<IToastService, ToastService>()
            .AddSingleton<IThemeService, ThemeService>()
            .AddSingleton<IValidator<TicketDto>, TicketDtoValidator>()
            .AddSingleton<TicketCache>();

        services
            .AddTransient<HomeView>()
            .AddTransient<AboutView>()
            .AddTransient<ForumView>()
            .AddTransient<TicketControlView>()
            .AddTransient<TicketUpsertView>();

        services
            .AddScoped<Shell>()
            .AddScoped<ShellViewModel>()
            .AddScoped<AboutViewModel>()
            .AddScoped<HomeViewModel>()
            .AddScoped<ForumViewModel>()
            .AddScoped<TicketControlViewModel>()
            .AddScoped<TicketUpsertViewModel>();

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
