using ReactiveUI;

namespace Connect.UI.Models;

/// <summary>
/// Defines a wrapper around a value type in which a displayable text can be
/// attached. This is used in instances such as selections whereby the model
/// requires access to the selected instance directly.
/// </summary>
/// <remarks>
/// Should be used for types that do not have a suitable, displayable string.
/// Derives from <see cref="ReactiveObject"/> to provide reactivity in the case
/// of implementing extended value configurations in children classes.
/// </remarks>
/// <seealso cref="TypedComboBoxOption{T}"/>
/// <seealso cref="TypedCheckBoxOption{T}"/>
public abstract class TypedReactiveOption<T>(T value, string displayText)
    : ReactiveObject
{
#region Contract

    /// <summary>
    /// The value of the selection.
    /// </summary>
    public T Value { get; } = value;

    public string DisplayText { get; } = displayText;
    public override string ToString() => DisplayText;

#endregion
}
