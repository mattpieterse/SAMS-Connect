using ReactiveUI;
using Wpf.Ui.Abstractions.Controls;

namespace Connect.UI.Views.Home;

public sealed partial class AboutView
    : IViewFor<AboutViewModel>, INavigableView<AboutViewModel>
{
#region Variables

    public AboutViewModel ViewModel { get; set; }
    object IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (AboutViewModel) value;
    }

#endregion

#region Lifecycle

    public AboutView(
        AboutViewModel model
    ) {
        ViewModel = model;
        DataContext = ViewModel;
        InitializeComponent();
    }

#endregion
}
