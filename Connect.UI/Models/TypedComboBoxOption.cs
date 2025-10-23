namespace Connect.UI.Models;

public sealed class TypedComboBoxOption<T>(
    T value,
    string displayText
)
{
    public T Value { get; } = value;
    public string DisplayText { get; } = displayText;
    public override string ToString() => DisplayText;
}
