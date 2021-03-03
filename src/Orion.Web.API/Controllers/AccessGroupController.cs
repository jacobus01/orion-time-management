using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orion.DAL.Repository.Interfaces;
using System;

namespace Orion.Web.API.Controllers
{
    //You must use attribute routing for any controllers that you want represented in your Swagger document(s):
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