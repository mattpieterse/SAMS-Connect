using ReactiveUI;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace Connect.UI.Shells;

public sealed partial class Shell
    : IViewFor<ShellViewModel>
{
#region Variables

    public ShellViewModel? ViewModel { get; set; }
    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (ShellViewModel?) value;
    }

#endregion

#region Lifecycle

    public Shell(
        ShellViewModel model,
        INavigationService navigationService,
        IContentDialogService contentDialogService,
        ISnackbarService snackbarService
    ) {
        ViewModel = model;
        DataContext = model;
        InitializeComponent();

        navigationService.SetNavigationControl(NavigationView);
        contentDialogService.SetDialogHost(DialogHost);
        snackbarService.SetSnackbarPresenter(Toaster);
    }

#endregion
}
