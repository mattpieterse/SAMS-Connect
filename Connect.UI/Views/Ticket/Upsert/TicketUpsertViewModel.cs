using System.Reactive;
using System.Reactive.Linq;
using Connect.Data.Caches;
using Connect.Data.Models;
using Connect.UI.Models;
using Connect.UI.Models.Annotations;
using Connect.UI.Services.Appearance;
using DynamicData.Binding;
using JetBrains.Annotations;
using Microsoft.Win32;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using Serilog;
using Wpf.Ui;

namespace Connect.UI.Views.Ticket.Upsert;

public sealed partial class TicketUpsertViewModel
    : ReactiveObject, IActivatableViewModel
{
#region Variables

    public ViewModelActivator Activator { get; } = new();
    public Upsert.TicketUpsertStateModel State { get; } = new();
    public Upsert.TicketUpsertFormModel Form { get; } = new();


    /* ATTRIBUTION:
     * Commands and command button bindings were adapted from YouTube.
     * - https://www.youtube.com/watch?v=HDSRG7GvPbo
     * - ToskersCorner (https://www.youtube.com/@ToskersCorner)
     */
    [Reactive]
    [Lateinit]
    private ReactiveCommand<Unit, Unit> _positiveCommand = null!;


    [Reactive]
    [Lateinit]
    private ReactiveCommand<Unit, Unit> _negativeCommand = null!;


    public ReactiveCommand<Unit, Unit> FileInsertCommand { get; }


    public ReactiveCommand<FileAttachment, Unit> FileDeleteCommand { get; }

#endregion

#region Lifecycle

    private readonly TicketCache _ticketCache;
    private readonly INavigationService _navigationService;


    /// <summary>
    /// Constructor for <see cref="TicketUpsertViewModel"/>
    /// </summary>
    public TicketUpsertViewModel(
        TicketCache ticketCache,
        INavigationService navigationService
    ) {
        _ticketCache = ticketCache;
        _navigationService = navigationService;

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

        FileInsertCommand = ReactiveCommand.Create(
            execute: AttachFile,
            canExecute: Form.Attachments
                .ToObservableChangeSet()
                .Select(_ => Form.Attachments.Count < 4)
                .StartWith(true)
        );

        FileDeleteCommand = ReactiveCommand.Create<FileAttachment>(RemoveFile);
    }

#endregion

#region Internals

    /// <summary>
    /// Safely attempts to submit the form.
    /// </summary>
    [UsedImplicitly]
    private void SubmitForm() {
        Log.Debug(nameof(SubmitForm));
        if (Form.IsValid) {
            _ticketCache.Insert(
                new Data.Models.Ticket {
                    Heading = Form.Heading,
                    Content = Form.Content,
                    Category = Form.Category,
                    FileAttachments = Form.Attachments
                        .Select(item => item.FileName)
                        .ToList()
                }
            );

            Form.Clear();
            State.CurrentIndex = TicketUpsertFormIndex.Input;
            _navigationService.GoBack();
        }
    }


    /// <summary>
    /// Safely attempts to cancel the form.
    /// </summary>
    [UsedImplicitly]
    private void CancelForm() {
        Form.Clear();
        State.CurrentIndex = TicketUpsertFormIndex.Input;
        _navigationService.GoBack();
    }


    [UsedImplicitly]
    private void AttachFile() {
        /* Attribution: OpenFileDialog
         * - https://learn.microsoft.com/en-us/dotnet/api/microsoft.win32.openfiledialog
         */
        var openFileDialog = new OpenFileDialog {
            Title = "Select your supporting images and documents",
            CheckFileExists = true,
            CheckPathExists = true,
            Multiselect = false,
        };

        if (openFileDialog.ShowDialog() == true) {
            Form.Attachments.Add(
                new FileAttachment(openFileDialog.FileName)
            );
        }
    }


    [UsedImplicitly]
    private void RemoveFile(FileAttachment attachment) {
        Form.Attachments.Remove(attachment);
    }

#endregion
}
