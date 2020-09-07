using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orion.DAL.Repository.Interfaces;

namespace Orion.Web.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccessGroupController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public AccessGroupController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        [Route("AccessGroups")]
        [Authorize]
        //POST : /api/ApplicationUser/Users
        public IActionResult GetAccessGroupList()
        {

            try
            {
                var result = _uow.AccessGroups.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("AccessGroupByUser")]
        [Authorize]
        //POST : /api/ApplicationUser/Users
        public IActionResult GetAccessGroupByUser([FromBody] int UserId)
        {
            try
            {
                var result = _uow.AccessGroups.GetByUserId(UserId);
                result.User = null;
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
