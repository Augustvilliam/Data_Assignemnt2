using Data.Context;
using Data.Entities;

namespace Data.Repositories;

public class EmployeeRepository : GenericRepository<EmployeeEntity>
{
    public EmployeeRepository(DataDbContext context) : base(context) { }
}

