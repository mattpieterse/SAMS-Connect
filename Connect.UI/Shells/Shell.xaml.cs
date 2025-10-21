using ReactiveUI;
using Serilog;

namespace Connect.UI.Shells;

public partial class Shell
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
