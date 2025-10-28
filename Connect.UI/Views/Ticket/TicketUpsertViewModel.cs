using System.Reactive;
using System.Reactive.Linq;
using Connect.UI.Models;
using Connect.UI.Models.Annotations;
using JetBrains.Annotations;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using Serilog;

namespace Connect.UI.Views.Ticket;

public sealed partial class TicketUpsertViewModel
    : ReactiveObject, IActivatableViewModel
{
#region Variables

    public ViewModelActivator Activator { get; } = new();
    public TicketUpsertStateModel State { get; } = new();
    public TicketUpsertFormModel Form { get; } = new();


    [Reactive]
    [Lateinit]
    private ReactiveCommand<Unit, Unit> _positiveCommand = null!;


    [Reactive]
    [Lateinit]
    private ReactiveCommand<Unit, Unit> _negativeCommand = null!;

#endregion

#region Lifecycle

    /// <summary>
    /// Constructor for <see cref="TicketUpsertViewModel"/>
    /// </summary>
    public TicketUpsertViewModel() {
        var incrementFormCommand = ReactiveCommand.Create(
            State.NavigateNext,
            State.WhenAnyValue(
                state => state.CurrentIndex,
                index => (index == TicketUpsertFormIndex.Input)
            )
        );

        var decrementFormCommand = ReactiveCommand.Create(
            State.NavigateBack,
            State.WhenAnyValue(
                state => state.CurrentIndex,
                index => (index == TicketUpsertFormIndex.Files)
            )
        );

        var submitFormCommand = ReactiveCommand.Create(SubmitForm);
        var cancelFormCommand = ReactiveCommand.Create(CancelForm);

        State.WhenAnyValue(state => state.CurrentIndex)
            .Select(index => (
                onPositive: (index == TicketUpsertFormIndex.Input) ? incrementFormCommand : submitFormCommand,
                onNegative: (index == TicketUpsertFormIndex.Input) ? cancelFormCommand : decrementFormCommand
            ))
            .Subscribe(commands => {
                PositiveCommand = commands.onPositive;
                NegativeCommand = commands.onNegative;
            });
    }

#endregion

#region Internals

    /// <summary>
    /// Safely attempts to submit the form.
    /// </summary>
    [UsedImplicitly]
    private void SubmitForm() {
        Log.Debug(nameof(SubmitForm));
    }


    /// <summary>
    /// Safely attempts to cancel the form.
    /// </summary>
    [UsedImplicitly]
    private void CancelForm() {
        Log.Debug(nameof(CancelForm));
    }


    [UsedImplicitly]
    private void AttachFile() {
        Log.Debug(nameof(AttachFile));
    }


    [UsedImplicitly]
    private void RemoveFile() {
        Log.Debug(nameof(RemoveFile));
    }

#endregion
}
