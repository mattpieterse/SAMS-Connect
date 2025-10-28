namespace Connect.Data.Models;

public class Ticket
    : ICitizenClaim
{
#region Schema

#region Schema | Audit

    public Guid Id { get; init; } = Guid.NewGuid();


    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

#endregion

#region Schema | Stash

    public DateTime? StashedAt { get; set; } = null;

#endregion

#region Schema | Claim

    public string Heading { get; set; } = string.Empty;


    public string Content { get; set; } = string.Empty;


    public MunicipalDepartment? Category { get; set; }

#endregion

#endregion
}
