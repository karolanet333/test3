
using SofCoAr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace SofCoAr.Repositories
{
    public class ProjectRepo : BaseRepo<Project>, IProjectRepo
    {
        public ProjectRepo(SofcoContext context = null) : base(context)
        {
        }

        public IEnumerable<Project> GetByService(int idCustomer, int idService)
        {
            IQueryable<Project> query = _context.Set<Project>();//.Include(c => c.Customer);

            query = query.Where(s =>
                   s.IdCustomer == idCustomer
                && s.IdService == idService).OrderBy(s => s.Name);

            return query.ToList();
        }
    }
}