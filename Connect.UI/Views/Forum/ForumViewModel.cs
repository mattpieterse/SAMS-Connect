using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;

namespace Connect.UI.Views.Forum;

public sealed partial class ForumViewModel
    : ObservableObject, IActivatableViewModel
{
#region Variables

    public ViewModelActivator Activator { get; } = new();

#endregion
}
