

using Data.Context;
using Data.Entities;

namespace Data.Repositories;

public class RoleRepository : GenericRepository<RoleDto>
{
    public RoleRepository(DataDbContext context) : base(context) { }
}
