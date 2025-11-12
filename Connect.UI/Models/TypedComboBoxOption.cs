namespace Connect.UI.Models;

public sealed class TypedComboBoxOption<T>(T value, string displayText)
    : TypedReactiveOption<T>(value, displayText);
