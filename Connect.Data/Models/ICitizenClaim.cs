using System.Text.RegularExpressions;
using Connect.Data.Models.Abstract;

namespace Connect.Data.Models;

public partial interface ICitizenClaim
    : ISingletonEntity, IAuditableEntity, IStashableEntity
{
#region Contract

    public string Heading { get; set; }
    public string Content { get; set; }
    public MunicipalDepartment? Category { get; set; }

#endregion
}
