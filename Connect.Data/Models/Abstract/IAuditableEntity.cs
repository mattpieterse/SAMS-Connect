namespace Connect.Data.Models.Abstract;

public interface IAuditableEntity
{
#region Contract

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

#endregion

#region Defaults

    /// <summary>
    /// Determines whether the entity has been modified.
    /// </summary>
    public bool IsModified => (UpdatedAt != CreatedAt);


    /// <summary>
    /// Informs the entity that it was updated at the current
    /// <see cref="DateTime.UtcNow"/>.
    /// </summary>
    public void Touch() {
        UpdatedAt = DateTime.UtcNow;
    }

#endregion
}
