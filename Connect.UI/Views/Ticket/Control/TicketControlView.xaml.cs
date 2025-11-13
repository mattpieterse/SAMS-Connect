using System.Diagnostics.CodeAnalysis;
using Connect.UI.Services.Appearance;
using ReactiveUI;
using Wpf.Ui.Abstractions.Controls;

namespace Connect.UI.Views.Ticket.Control;

public sealed partial class TicketControlView
    : IViewFor<TicketControlViewModel>, INavigableView<TicketControlViewModel>
{
#region Variables

    [AllowNull]
    public TicketControlViewModel ViewModel { get; set; }


    [NotNullIfNotNull(nameof(ViewModel))]
    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (TicketControlViewModel?) value;
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
