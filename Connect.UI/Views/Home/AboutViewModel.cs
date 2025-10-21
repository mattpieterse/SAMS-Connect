using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;

namespace Connect.UI.Views.Home;

public sealed partial class AboutViewModel
    : ObservableObject, IActivatableViewModel
{
#region Variables

    public ViewModelActivator Activator { get; } = new();

#endregion
}
