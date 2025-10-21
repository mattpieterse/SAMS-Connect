using ReactiveUI;
using Wpf.Ui.Abstractions.Controls;

namespace Connect.UI.Views.Ticket;

public sealed partial class TicketUpsertView
    : IViewFor<TicketUpsertViewModel>, INavigableView<TicketUpsertViewModel>
{
#region Variables

    public TicketUpsertViewModel ViewModel { get; set; }
    object IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (TicketUpsertViewModel) value;
    }

#endregion

#region Lifecycle

    public TicketUpsertView(
        TicketUpsertViewModel model
    ) {
        ViewModel = model;
        DataContext = ViewModel;
        InitializeComponent();
    }

#endregion
}
