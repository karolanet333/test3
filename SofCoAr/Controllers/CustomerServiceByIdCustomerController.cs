using SofCoAr.Models;
using SofCoAr.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SofCoAr.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CustomerServiceByIdCustomerController : ApiController
    {

        private ICustomerServiceRepo _repo;

        public CustomerServiceByIdCustomerController(ICustomerServiceRepo repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IEnumerable<CustomerService> GetByIdCustomer(int id)
        {
            return _repo.GetByIdCustomer(id);
        }

        private bool Exists(int id)
        {
            bool rpta = true;
            var obj = _repo.GetById(id);
            if (obj == null)
            {
                rpta = false;
            }
            return rpta;
        }

        private new void Dispose()
        {
            if (_repo != null)
            {
                _repo.Dispose();
            }
            base.Dispose();
        }
    }
}

