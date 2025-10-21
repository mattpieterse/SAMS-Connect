using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;

namespace Connect.UI.Views.Ticket;

public sealed partial class TicketUpsertViewModel
    : ObservableObject, IActivatableViewModel
{
#region Variables

    public ViewModelActivator Activator { get; } = new();

#endregion
}
