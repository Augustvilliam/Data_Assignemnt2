

using Data.Context;
using Data.Entities;

namespace Data.Repositories;

public class RoleRepository : GenericRepository<RoleEntity>
{
    public RoleRepository(DataDbContext context) : base(context) { }
}
