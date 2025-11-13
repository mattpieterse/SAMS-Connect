using System.Text.RegularExpressions;

namespace Connect.Data.Models;

public partial class Ticket
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


    public List<string> FileAttachments { get; set; } = [];

#endregion

#region Schema | Admin

    public TicketStatus Status { get; set; } = TicketStatus.Active;

#endregion

#endregion
#region Extensions

    public string CreatedAtFormatted =>
        $"{CreatedAt:dd MMMM yyyy} • {CreatedAt:HH:mm}";


    public string UpdatedAtFormatted =>
        $"{UpdatedAt:dd MMMM yyyy} • {UpdatedAt:HH:mm}";


    public string CategoryFormatted
    {
        get {
            if (Category is not { } category) {
                return "Unspecified";
            }

            var categoryName = category.ToString();
            var readableString = categoryName.All(char.IsUpper)
                ? string.Join(" ", categoryName.ToCharArray())
                : MunicipalDepartmentToReadableString().Replace(categoryName, "$1 $2");

            return $"Department of {readableString}";
        }
    }

#endregion

#region Expressions

    [
        GeneratedRegex("([a-z])([A-Z])")
    ]
    private static partial Regex MunicipalDepartmentToReadableString();

#endregion
}
