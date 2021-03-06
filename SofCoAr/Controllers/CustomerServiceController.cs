﻿using SofCoAr.Models;
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
    public class CustomerServiceController : ApiController
    {

        private ICustomerServiceRepo _repo;

        public CustomerServiceController(ICustomerServiceRepo repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [Route("api/getById")]
        public CustomerService GetById(int Id)
        {
            var item = _repo.GetById(Id);
            return item;
        }

        [HttpPost]
        public IHttpActionResult Add([FromBody] CustomerService item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            _repo.Add(item);
            _repo.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { Id = item.Id }, item);
        }

        [HttpPut]
        public IHttpActionResult Update([FromBody] CustomerService item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            var obj = _repo.GetById(item.Id);
            if (!Exists(item.Id))
            {
                return NotFound();
            }
            _repo.Edit(item);
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

