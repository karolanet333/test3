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
    public class BillingMilestoneController : ApiController
    {

        private IBillingMilestoneRepo _repo;

        public BillingMilestoneController(IBillingMilestoneRepo repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IEnumerable<BillingMilestone> GetAll(int idCustomer, int idService, int idProject)
        {
            return _repo.GetAllFilteredWithDetails(idCustomer, idService, idProject);
        }

        [HttpGet]
        public BillingMilestone GetById(int Id)
        {
            var item = _repo.GetById(Id);
            return item;
        }

        [HttpPost]
        public IHttpActionResult Add([FromBody] BillingMilestoneEditDTO item)
        {
            _repo.AddHito(item);
            _repo.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { Id = item.BillingMilestone.Id }, item);
        }

        [HttpPut]
        public IHttpActionResult Update([FromBody] BillingMilestoneEditDTO item)
        {
            _repo.EditHito(item.BillingMilestone, item.IdUser);
            _repo.SaveChanges();
            return Ok();

        }

        [HttpDelete]
        public IHttpActionResult Delete(int Id)
        {
            if (!Exists(Id))
            {
                return NotFound();
            }
            _repo.Delete(Id);
            _repo.SaveChanges();
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

