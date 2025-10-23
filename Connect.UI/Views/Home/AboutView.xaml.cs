using System.Reactive.Disposables.Fluent;
using ReactiveUI;
using Wpf.Ui.Abstractions.Controls;

namespace Connect.UI.Views.Home;

public sealed partial class AboutView
    : IViewFor<AboutViewModel>, INavigableView<AboutViewModel>
{
#region Variables

    public AboutViewModel ViewModel { get; set; }
    object IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (AboutViewModel) value;
    }

#endregion

#region Lifecycle

    public AboutView(
        AboutViewModel model
    ) {
        ViewModel = model;
        DataContext = ViewModel;

        InitializeComponent();
        this.WhenActivated(disposables => {
            this.Bind(ViewModel, m => m.SelectedAppTheme, v => v.AppThemeComboBox.SelectedValue)
                .DisposeWith(disposables);

            this.OneWayBind(ViewModel, m => m.AppThemeOptions, v => v.AppThemeComboBox.ItemsSource)
                .DisposeWith(disposables);

            this.Bind(ViewModel, m => m.SelectedLanguage, v => v.LanguageComboBox.SelectedValue)
                .DisposeWith(disposables);

            this.OneWayBind(ViewModel, m => m.LanguageOptions, v => v.LanguageComboBox.ItemsSource)
                .DisposeWith(disposables);
        });
    }

#endregion
}
