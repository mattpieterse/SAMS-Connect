using System.Reactive;
using System.Reactive.Linq;
using Connect.UI.Views.Forum;
using Connect.UI.Views.Ticket;
using ReactiveUI;
using Wpf.Ui;

namespace Connect.UI.Views.Home;

public sealed class HomeViewModel
    : ReactiveObject, IActivatableViewModel
{
#region Variables

    public ViewModelActivator Activator { get; } = new();


    public ReactiveCommand<Unit, Unit> NavigateToEventsViewCommand { get; private set; } = null!;
    public ReactiveCommand<Unit, Unit> NavigateToTicketViewCommand { get; private set; } = null!;
    public ReactiveCommand<Unit, Unit> NavigateToUpsertViewCommand { get; private set; } = null!;

#endregion

#region Lifecycle

    private readonly INavigationService _navigationService;


    public HomeViewModel(
        INavigationService navigationService
    ) {
        _navigationService = navigationService;
        Observe();
    }


    private void Observe() {
        var canAlwaysExecute = Observable.Return(true)
            .ObserveOn(RxApp.MainThreadScheduler);

        NavigateToEventsViewCommand = ReactiveCommand.Create(
            () => { _navigationService.Navigate(typeof(ForumView)); },
            canAlwaysExecute
        );

        NavigateToTicketViewCommand = ReactiveCommand.Create(
            () => { _navigationService.Navigate(typeof(TicketControlView)); },
            canAlwaysExecute
        );

        NavigateToUpsertViewCommand = ReactiveCommand.Create(
            () => { _navigationService.Navigate(typeof(TicketUpsertView)); },
            canAlwaysExecute
        );
    }

#endregion
}
