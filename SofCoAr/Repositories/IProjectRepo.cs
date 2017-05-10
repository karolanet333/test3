using SofCoAr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofCoAr.Repositories
{
    public interface IProjectRepo : IBaseRepo<Project>
    {
        IEnumerable<Project> GetByService(int idCustomer, int idService);
    }
}
