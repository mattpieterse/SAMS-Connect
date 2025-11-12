using System.Text.RegularExpressions;
using Connect.Data.Models.Abstract;

namespace Connect.Data.Models;

public partial interface IForumBroadcast
    : ISingletonEntity, IAuditableEntity
{
#region Contract

    public string Heading { get; set; }
    public string Content { get; set; }
    public MunicipalDepartment? Category { get; set; }
    public MunicipalProvincial? Location { get; set; }

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
