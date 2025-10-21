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
        HomeViewModel model
    ) {
        ViewModel = model;
        DataContext = ViewModel;
        InitializeComponent();
    }

#endregion
}
