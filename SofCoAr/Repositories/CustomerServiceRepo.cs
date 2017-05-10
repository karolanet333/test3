
using SofCoAr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace SofCoAr.Repositories
{
    public class CustomerServiceRepo : BaseRepo<CustomerService>, ICustomerServiceRepo
    {
        public CustomerServiceRepo(SofcoContext context = null) : base(context)
        {
        }

        public IEnumerable<CustomerService> GetByIdCustomer(int id)
        {
            IQueryable<CustomerService> query = base._context.Set<CustomerService>().Include(c => c.Customer);

            query = query.Where(s => s.IdCustomer == id).OrderBy(s => s.Name);

            return query.ToList();
        }
    }
}
