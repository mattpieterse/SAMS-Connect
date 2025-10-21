using ReactiveUI;

namespace Connect.UI.Shells;

public class ShellViewModel
    : IActivatableViewModel
{
#region Variables

    public ViewModelActivator Activator { get; } = new();

#endregion
}
