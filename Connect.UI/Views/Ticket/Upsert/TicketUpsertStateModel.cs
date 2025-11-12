using System.Reactive.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using Connect.UI.Models;
using JetBrains.Annotations;
using Microsoft.Extensions.Localization;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Connect.UI.Views.Ticket.Upsert;

public sealed partial class TicketUpsertStateModel
    : ReactiveObject
{
#region Variables

    [Reactive]
    private TicketUpsertFormIndex _currentIndex;


    [ObservableAsProperty]
    private string _breadcrumbText = string.Empty;


    [ObservableAsProperty]
    private string _negativeButtonText = string.Empty;


    [ObservableAsProperty]
    private string _positiveButtonText = string.Empty;


    [ObservableAsProperty]
    private Visibility _formPage1Visibility;


    [ObservableAsProperty]
    private Visibility _formPage2Visibility;

#endregion

#region Lifecycle

    /// <summary>
    /// See <see cref="IStringLocalizer"/>
    /// </summary>
    private readonly IStringLocalizer _localizer =
        Ioc.Default.GetRequiredService<IStringLocalizer>();


    /// <summary>
    /// Constructor for <see cref="TicketUpsertStateModel"/>
    /// </summary>
    public TicketUpsertStateModel() {
        var observeCurrentIndex = this.WhenAnyValue<TicketUpsertStateModel, TicketUpsertFormIndex>(model => model.CurrentIndex);

        _breadcrumbTextHelper = observeCurrentIndex
            .Select(index => _localizer[
                "page.ticket.upsert.form.pagination", (int) index, Enum.GetValues(typeof(TicketUpsertFormIndex)).Length
            ].ToString())
            .ToProperty(this, p => p.BreadcrumbText);

        _formPage1VisibilityHelper = observeCurrentIndex
            .Select(index => (index == TicketUpsertFormIndex.Input) ? Visibility.Visible : Visibility.Collapsed)
            .ToProperty(this, p => p.FormPage1Visibility);

        _formPage2VisibilityHelper = observeCurrentIndex
            .Select(index => (index == TicketUpsertFormIndex.Files) ? Visibility.Visible : Visibility.Collapsed)
            .ToProperty(this, p => p.FormPage2Visibility);

        _positiveButtonTextHelper = observeCurrentIndex
            .Select(index => (index == TicketUpsertFormIndex.Input)
                ? _localizer["page.ticket.upsert.form.inputs.on.positive"].ToString()
                : _localizer["page.ticket.upsert.form.files.on.positive"].ToString())
            .ToProperty(this, p => p.PositiveButtonText);

        _negativeButtonTextHelper = observeCurrentIndex
            .Select(index => (index == TicketUpsertFormIndex.Input)
                ? _localizer["page.ticket.upsert.form.inputs.on.negative"].ToString()
                : _localizer["page.ticket.upsert.form.files.on.negative"].ToString())
            .ToProperty(this, p => p.NegativeButtonText);

        CurrentIndex = TicketUpsertFormIndex.Input;
    }

#endregion

#region Extension

    /// <summary>
    /// Safely increments the current wizard page index.
    /// </summary>
    /// <seealso cref="Ticket.TicketUpsertStateModel.CurrentIndex"/>
    [UsedImplicitly]
    public void NavigateNext() {
        if (CurrentIndex == TicketUpsertFormIndex.Input)
            CurrentIndex = TicketUpsertFormIndex.Files;
    }


    /// <summary>
    /// Safely decrements the current wizard page index.
    /// </summary>
    /// <seealso cref="Ticket.TicketUpsertStateModel.CurrentIndex"/>
    [UsedImplicitly]
    public void NavigateBack() {
        if (CurrentIndex == TicketUpsertFormIndex.Files)
            CurrentIndex = TicketUpsertFormIndex.Input;
    }

#endregion
}
