namespace Connect.UI.Models.Annotations;

/// <summary>
/// Flags a property that is initialized late via a delegate subscription.
///
/// This attribute does not generate any sources, but should be used to mark any
/// property that is promised by `= null!` and then definitively initialized for
/// the purpose of justification.
/// </summary>
[AttributeUsage(
    AttributeTargets.Property | AttributeTargets.Field
)]
public sealed class LateinitAttribute : Attribute;
