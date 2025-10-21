using System.Windows.Controls;
using Connect.UI.Shells;
using JetBrains.Annotations;
using ReactiveUI;
using Wpf.Ui.Abstractions.Controls;

namespace Connect.UI.Views.Forum;

public sealed partial class ForumView
    : IViewFor<ForumViewModel>, INavigableView<ForumViewModel>
{
#region Variables

    public ForumViewModel ViewModel { get; set; }
    object IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (ForumViewModel) value;
    }

#endregion

#region Lifecycle

    public ForumView(
        ForumViewModel model
    ) {
        ViewModel = model;
        DataContext = ViewModel;
        InitializeComponent();
    }

#endregion
}
