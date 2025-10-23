using System.Collections.ObjectModel;
using System.Globalization;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Connect.UI.Models;
using Connect.UI.Services.Appearance;
using JetBrains.Annotations;
using Lepo.i18n;
using Microsoft.Extensions.Localization;
using ReactiveUI;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace Connect.UI.Views.Home;

public sealed class AboutViewModel
    : ObservableObject, IActivatableViewModel
{
#region Variables

    public ViewModelActivator Activator { get; } = new();


    private ApplicationTheme _selectedAppTheme;
    public ApplicationTheme SelectedAppTheme
    {
        get => _selectedAppTheme;
        [UsedImplicitly] set => SetProperty(ref _selectedAppTheme, value);
    }


    private CultureInfo _selectedLanguage = null!;
    public CultureInfo SelectedLanguage
    {
        get => _selectedLanguage;
        [UsedImplicitly] set => SetProperty(ref _selectedLanguage, value);
    }


    private readonly ObservableCollection<TypedComboBoxOption<ApplicationTheme>> _appThemeOptions = [];
    public ObservableCollection<TypedComboBoxOption<ApplicationTheme>> AppThemeOptions
    {
        get => _appThemeOptions;
        private init => SetProperty(ref _appThemeOptions, value);
    }


    private readonly ObservableCollection<TypedComboBoxOption<CultureInfo>> _languageOptions = [];
    public ObservableCollection<TypedComboBoxOption<CultureInfo>> LanguageOptions
    {
        get => _languageOptions;
        private init => SetProperty(ref _languageOptions, value);
    }

#endregion

#region Lifecycle

    private readonly IStringLocalizer _localizer;
    private readonly IToastService _toastService;


    public AboutViewModel(
        IStringLocalizer localizer,
        IToastService toastService
    ) {
        _localizer = localizer;
        _toastService = toastService;

        AppThemeOptions = new ObservableCollection<TypedComboBoxOption<ApplicationTheme>>(ConstructAppThemeOptions());
        LanguageOptions = new ObservableCollection<TypedComboBoxOption<CultureInfo>>(ConstructLanguageOptions());
        SelectedAppTheme = ApplicationThemeManager.GetAppTheme();
        SelectedLanguage = CultureInfo.CurrentUICulture;

        this.WhenActivated(disposables => {
            this.WhenAnyValue(m => m.SelectedAppTheme)
                .Skip(1)
                .Subscribe(value => {
                    ApplicationThemeManager.Apply(value);
                    OnAppThemeChangedSuccess();
                })
                .DisposeWith(disposables);

            this.WhenAnyValue(m => m.SelectedLanguage)
                .Skip(1)
                .DistinctUntilChanged()
                .Subscribe(value => {
                    CultureInfo.CurrentUICulture = value;
                    new LocalizationCultureManager()
                        .SetCulture(value);

                    OnLanguageChangedSuccess();
                })
                .DisposeWith(disposables);
        });
    }

#endregion

#region Internals

    private void OnAppThemeChangedSuccess() {
        _toastService.Default(
            heading: (ApplicationThemeManager.GetAppTheme() == ApplicationTheme.Dark)
                ? _localizer["page.about.apptheme.dropdown.changed.to.night.on.success.toast.heading"]
                : _localizer["page.about.apptheme.dropdown.changed.to.light.on.success.toast.heading"]
            ,
            message: _localizer["page.about.apptheme.dropdown.changed.on.success.toast.message"],
            icon: new SymbolIcon {
                Symbol = SymbolRegular.ArrowSyncCheckmark24
            }
        );
    }


    private void OnLanguageChangedSuccess() {
        var cultureName = CultureInfo.CurrentUICulture.DisplayName;
        _toastService.Success(
            heading: _localizer["page.about.language.dropdown.changed.on.success.toast.heading", cultureName],
            message: _localizer["page.about.language.dropdown.changed.on.success.toast.message"],
            icon: new SymbolIcon {
                Symbol = SymbolRegular.ArrowSyncCheckmark24
            }
        );
    }


    private IEnumerable<TypedComboBoxOption<ApplicationTheme>> ConstructAppThemeOptions() {
        return [
            new TypedComboBoxOption<ApplicationTheme>(
                ApplicationTheme.Light,
                _localizer["page.about.apptheme.dropdown.option.light"]
            ),
            new TypedComboBoxOption<ApplicationTheme>(
                ApplicationTheme.Dark,
                _localizer["page.about.apptheme.dropdown.option.night"]
            )
        ];
    }


    private static IEnumerable<TypedComboBoxOption<CultureInfo>> ConstructLanguageOptions() {
        return [
            new TypedComboBoxOption<CultureInfo>(new CultureInfo("en-US"), "English (US)"),
            new TypedComboBoxOption<CultureInfo>(new CultureInfo("en-GB"), "English (UK)"),
            new TypedComboBoxOption<CultureInfo>(new CultureInfo("en-ZA"), "English (ZA)")
        ];
    }

#endregion
}
