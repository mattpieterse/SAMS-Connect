using System.Diagnostics.CodeAnalysis;
using System.Reactive.Disposables.Fluent;
using ReactiveUI;
using Wpf.Ui.Abstractions.Controls;

namespace Connect.UI.Views.Forum;

public sealed partial class ForumView
    : IViewFor<ForumViewModel>, INavigableView<ForumViewModel>
{
#region Variables

    [AllowNull]
    public ForumViewModel ViewModel { get; set; }


    [NotNullIfNotNull(nameof(ViewModel))]
    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (ForumViewModel?) value;
    }

#endregion

#region Lifecycle

    public ForumView(
        ForumViewModel model
    ) {
        ViewModel = model;
        DataContext = ViewModel;

        InitializeComponent();
        this.WhenActivated(disposables => {
            this.Bind(
                ViewModel,
                bind => bind.SearchText,
                view => view.SearchBox.Text
            ).DisposeWith(disposables);

            this.OneWayBind(
                ViewModel,
                bind => bind.SearchBroadcasts,
                view => view.SearchBox.ItemsSource
            ).DisposeWith(disposables);

            this.OneWayBind(
                ViewModel,
                bind => bind.CachedBroadcasts,
                view => view.ForumList.ItemsSource
            ).DisposeWith(disposables);
        });
    }

#endregion
}
