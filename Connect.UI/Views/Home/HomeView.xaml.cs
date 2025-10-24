using System.Reactive.Disposables.Fluent;
using ReactiveUI;
using Wpf.Ui.Abstractions.Controls;

namespace Connect.UI.Views.Home;

public sealed partial class HomeView
    : IViewFor<HomeViewModel>, INavigableView<HomeViewModel>
{
#region Variables

    public HomeViewModel ViewModel { get; set; }
    object IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (HomeViewModel) value;
    }

#endregion

#region Lifecycle

    public HomeView(
        HomeViewModel viewModel
    ) {
        ViewModel = viewModel;
        DataContext = ViewModel;

        InitializeComponent();
        this.WhenActivated(disposables => {
            this.BindCommand(
                ViewModel,
                model => model.NavigateToEventsViewCommand,
                view => view.NavigateToEventsButton
            ).DisposeWith(disposables);

            this.BindCommand(
                ViewModel,
                model => model.NavigateToTicketViewCommand,
                view => view.NavigateToIssuesButton
            ).DisposeWith(disposables);

            this.BindCommand(
                ViewModel,
                model => model.NavigateToUpsertViewCommand,
                view => view.NavigateToIssueUpsertButton
            ).DisposeWith(disposables);
        });
    }

#endregion
}
