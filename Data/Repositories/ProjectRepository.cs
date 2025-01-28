using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Context;
using Data.Entities;

namespace Data.Repositories;

public class ProjectRepository : GenericRepository<ProjectEntity>
{
    public ProjectRepository(DataDbContext context) : base(context) { }
}

