using Connect.Data.Models.Abstract;

namespace Connect.Data.Models;

public interface ICitizenClaim
    : ISingletonEntity, IAuditableEntity, IStashableEntity
{
#region Contract

    public string Heading { get; set; }
    public string Content { get; set; }
    public MunicipalDepartment? Category { get; set; }
    public MunicipalLocation? Province { get; set; }

#endregion
}
