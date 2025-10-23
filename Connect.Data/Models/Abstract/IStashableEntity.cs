namespace Connect.Data.Models.Abstract;

public interface IStashableEntity
{
#region Contract

    public DateTime? StashedAt { get; set; }

#endregion

#region Defaults

    /// <summary>
    /// Determines whether the entity is stashed (soft-deleted).
    /// </summary>
    /// <seealso cref="Conceal"/>
    /// <seealso cref="Hydrate"/>
    public bool IsStashed => (StashedAt != null);


    /// <summary>
    /// Determines whether the entity is visible (not-stashed).
    /// </summary>
    /// <seealso cref="Conceal"/>
    /// <seealso cref="Hydrate"/>
    public bool IsVisible => (StashedAt == null);


    /// <summary>
    /// Attempts to safely soft-delete the entity by flagging it as stashed at
    /// the current <see cref="DateTime.UtcNow"/> timestamp.
    /// </summary>
    /// <seealso cref="Hydrate"/>
    public void Conceal() {
        if (IsVisible) StashedAt = DateTime.UtcNow;
    }


    /// <summary>
    /// Attempts to safely soft-restore the entity by clearing the entity's
    /// <see cref="StashedAt"/> soft-deletion property flag.
    /// </summary>
    /// <seealso cref="Conceal"/>
    public void Hydrate() {
        if (IsStashed) StashedAt = null;
    }

#endregion
}
