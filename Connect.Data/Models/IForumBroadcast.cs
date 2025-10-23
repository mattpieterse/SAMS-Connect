using Connect.Data.Models.Abstract;

namespace Connect.Data.Models;

public interface IForumBroadcast
    : ISingletonEntity, IAuditableEntity
{
#region Contract

    public string Heading { get; set; }
    public string Content { get; set; }
    public MunicipalDepartment? Category { get; set; }
    public MunicipalLocation? Location { get; set; }

#endregion
}
