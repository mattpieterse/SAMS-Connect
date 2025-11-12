namespace Connect.Data.Models;

public class ForumLetter
    : IForumBroadcast
{
#region Schema

#region Schema | Audit

    public Guid Id { get; init; } = Guid.NewGuid();


    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

#endregion


#region Schema | Forum

    public string Heading { get; set; } = string.Empty;


    public string Content { get; set; } = string.Empty;


    public MunicipalDepartment? Category { get; set; }


    public MunicipalProvincial? Location { get; set; }

#endregion

#endregion
}
