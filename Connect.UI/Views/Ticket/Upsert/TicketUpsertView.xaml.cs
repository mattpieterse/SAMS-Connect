using System.Diagnostics.CodeAnalysis;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using Microsoft.Extensions.Localization;
using ReactiveUI;
using Wpf.Ui.Abstractions.Controls;

namespace Connect.UI.Views.Ticket.Upsert;

public sealed partial class TicketUpsertView
    : IViewFor<Upsert.TicketUpsertViewModel>, INavigableView<Upsert.TicketUpsertViewModel>
{
#region Variables

    [AllowNull]
    public Upsert.TicketUpsertViewModel ViewModel { get; set; }


    [NotNullIfNotNull(nameof(ViewModel))]
    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (Upsert.TicketUpsertViewModel?) value;
    }

#endregion

#region Lifecycle

    public TicketUpsertView(
        Upsert.TicketUpsertViewModel viewModel,
        IStringLocalizer localizer
    ) {
        ViewModel = viewModel;
        DataContext = ViewModel;

        InitializeComponent();
        this.WhenActivated(disposables => {
            SetupStateBindings(disposables);
            SetupFormInputBindings(disposables);
            SetupFormErrorBindings(disposables);

            this.OneWayBind(ViewModel, bind => bind.Form.CategoryOptions, view => view.DepartmentInput.ItemsSource)
                .DisposeWith(disposables);

            this.BindCommand(ViewModel, bind => bind.FileInsertCommand, view => view.FileInsertButton)
                .DisposeWith(disposables);

            this.OneWayBind(ViewModel, bind => bind.Form.Attachments, view => view.FileList.ItemsSource)
                .DisposeWith(disposables);

            this.OneWayBind(
                ViewModel,
                bind => bind.Form.Completion,
                view => view.CompletionView.Value,
                value => (value * 100)
            ).DisposeWith(disposables);

            this.OneWayBind(
                ViewModel,
                bind => bind.Form.Completion,
                view => view.CompletionText.Text,
                value => localizer["page.ticket.upsert.form.caption", (value * 100).ToString("N0")]
            ).DisposeWith(disposables);
        });
    }

#endregion

#region Internals

    private void SetupFormInputBindings(
        CompositeDisposable disposables
    ) {
        this.Bind(ViewModel, bind => bind.Form.Heading, view => view.HeadingInput.Text)
            .DisposeWith(disposables);

        this.Bind(ViewModel, bind => bind.Form.Content, view => view.ContentInput.Text)
            .DisposeWith(disposables);

        this.Bind(ViewModel, bind => bind.Form.Category, view => view.DepartmentInput.SelectedValue)
            .DisposeWith(disposables);
    }


    private void SetupFormErrorBindings(
        CompositeDisposable disposables
    ) {
        var errorBindings = new[] {
            (Schema: ViewModel.Form, nameof(ViewModel.Form.Heading), HeadingInputErrorText),
            (Schema: ViewModel.Form, nameof(ViewModel.Form.Content), ContentInputErrorText),
            (Schema: ViewModel.Form, nameof(ViewModel.Form.Category), DepartmentInputErrorText)
        };

        foreach (var (form, field, view) in errorBindings) {
            form.WhenAnyValue(x => x.Errors)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(errors => view.Text = errors.TryGetValue(field, out var e) ? e : string.Empty)
                .DisposeWith(disposables);
        }
    }


    private void SetupStateBindings(
        CompositeDisposable disposables
    ) {
        this.OneWayBind(ViewModel, bind => bind.State.BreadcrumbText, view => view.BreadcrumbText.Text)
            .DisposeWith(disposables);

        this.OneWayBind(ViewModel, bind => bind.State.FormPage1Visibility, view => view.FormContainer1.Visibility)
            .DisposeWith(disposables);

        this.OneWayBind(ViewModel, bind => bind.State.FormPage2Visibility, view => view.FormContainer2.Visibility)
            .DisposeWith(disposables);

        this.OneWayBind(ViewModel, bind => bind.State.PositiveButtonText, view => view.IncrementFormButton.Content)
            .DisposeWith(disposables);

        this.OneWayBind(ViewModel, bind => bind.State.NegativeButtonText, view => view.DecrementFormButton.Content)
            .DisposeWith(disposables);

        this.BindCommand(ViewModel, bind => bind.PositiveCommand, view => view.IncrementFormButton)
            .DisposeWith(disposables);

        this.BindCommand(ViewModel, bind => bind.NegativeCommand, view => view.DecrementFormButton)
            .DisposeWith(disposables);
    }

#endregion
}
