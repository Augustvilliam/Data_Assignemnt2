using Data.Context;
using Data.Entities;

namespace Data.Repositories;

public class CustomerRepository : GenericRepository<CustomerEntity>
{
    public CustomerRepository(DataDbContext context) : base(context) { }
}

