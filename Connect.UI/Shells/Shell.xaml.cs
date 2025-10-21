using ReactiveUI;

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
        ShellViewModel model
    ) {
        ViewModel = model;
        DataContext = model;
        InitializeComponent();
    }

#endregion
}
