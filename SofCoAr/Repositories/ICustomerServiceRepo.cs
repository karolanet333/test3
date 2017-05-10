using SofCoAr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofCoAr.Repositories
{
    public interface ICustomerServiceRepo : IBaseRepo<CustomerService>
    {
        IEnumerable<CustomerService> GetByIdCustomer(int id);
    }
}
