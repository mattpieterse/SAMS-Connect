using Connect.Data.Models;

namespace Connect.UI.Models.Data;

public sealed class TicketDto
{
#region Schema

    public string Heading { get; set; } = string.Empty;


    public string Content { get; set; } = string.Empty;


    public MunicipalDepartment? Category { get; set; }

#endregion

#region Mapper

    public Ticket ToDataObject => new() {
        Category = Category,
        Heading = Heading,
        Content = Content
    };


    public TicketDto ToUiObject => new() {
        Category = Category,
        Heading = Heading,
        Content = Content
    };

#endregion
}
