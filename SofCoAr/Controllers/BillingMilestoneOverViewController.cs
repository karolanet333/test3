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
    public class BillingMilestoneOverViewController : ApiController
    {

        private IBillingMilestoneRepo _repo;

        public BillingMilestoneOverViewController(IBillingMilestoneRepo repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IEnumerable<BillingMilestone> GetAll(int idCustomer, int idService, int idProject)
        {
            return _repo.GetAllFiltered(idCustomer, idService, idProject);
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

