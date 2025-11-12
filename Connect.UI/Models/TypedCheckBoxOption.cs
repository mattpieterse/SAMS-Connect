using ReactiveUI.SourceGenerators;

namespace Connect.UI.Models;

public partial class TypedCheckBoxOption<T>(T value, string displayText)
    : TypedReactiveOption<T>(value, displayText)
{
#region Variables

    [Reactive]
    private bool _isSelected;

#endregion
}
