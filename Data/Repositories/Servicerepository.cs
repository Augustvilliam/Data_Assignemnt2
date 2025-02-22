using Data.Context;
using Data.Entities;

namespace Data.Repositories;

public class Servicerepository : GenericRepository<ServiceEntity>
{
    public Servicerepository(DataDbContext context) : base(context) { }
}

 