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
    public class BillingMilestoneDivideController : ApiController
    {

        private IBillingMilestoneRepo _repo;

        public BillingMilestoneDivideController(IBillingMilestoneRepo repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public IHttpActionResult Divide([FromBody] BillingMilestoneDivideDTO item)
        {
            //if (item == null)
            //{
            //    return BadRequest();
            //}
            _repo.DivideHito(item);
            _repo.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { Id = item.BillingMilestone.Id }, item);
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

