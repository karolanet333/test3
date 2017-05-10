using SofCoAr.Models;
using SofCoAr.Models.DTO;
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
    public class BillingMilestoneDetailsController : ApiController
    {

        private IBillingMilestoneRepo _repo;

        public BillingMilestoneDetailsController(IBillingMilestoneRepo repo)
        {
            _repo = repo;
        }


        [HttpPut]
        public IHttpActionResult Update([FromBody] BillingMilestoneAproveRejectDTO item)
        {
            //if (item == null)
            //{
            //    return BadRequest();
            //}
            //var obj = _repo.GetById(item.BillingMilestone.Id);
            //if (obj == null)
            //{
            //    return NotFound();
            //}
            _repo.AproveRejectHito(item.BillingMilestone, item.IdUser, item.Rechazar);
            return Ok();

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

