using ReactiveUI;
using Wpf.Ui.Abstractions.Controls;

namespace Connect.UI.Views.Ticket;

public sealed partial class TicketControlView
    : IViewFor<TicketControlViewModel>, INavigableView<TicketControlViewModel>
{
#region Variables

    public TicketControlViewModel ViewModel { get; set; }
    object IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (TicketControlViewModel) value;
    }

#endregion

#region Lifecycle

    public TicketControlView(
        TicketControlViewModel model
    ) {
        ViewModel = model;
        DataContext = ViewModel;
        InitializeComponent();
    }

#endregion
}
