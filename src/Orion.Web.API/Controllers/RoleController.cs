using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orion.DAL.EF.Models.DB;
using Orion.DAL.Repository.Interfaces;
using Orion.Web.API.Models;

namespace Orion.Web.API.Controllers
{
    //You must use attribute routing for any controllers that you want represented in your Swagger document(s):
    [Route("[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public RoleController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        [Route("Roles")]
        [Authorize]
        //POST : /api/ApplicationUser/Users
        public IActionResult GetRoleList()
        {
            System.Threading.Thread.Sleep(3000);
            try
            {
                var result = _uow.Roles.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("Role")]
        [Authorize]
        //POST : /api/ApplicationUser/Users
        public IActionResult GetRole([FromBody] int Id)
        {
            try
            {
                var result = _uow.Roles.SingleOrDefault(r => r.Id == Id);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("RolePerUser")]
        [Authorize]
        //POST : /api/ApplicationUser/Users
        public IActionResult GetRolePerUser([FromBody] int UserId)
        {
            try
            {
                var result = _uow.Roles.GetRolePerUserId(UserId);
                return Ok(new { result.Id, result.RoleName,result.Rate});
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("CreateUpdateRole")]
        [Authorize]
        //POST : /api/ApplicationUser/CreateUser
        public IActionResult PostTask(RoleModel model)
        {
            _uow.SetActiveUserId(Int32.Parse(Request.Headers["CurrentUserId"]));
            //We need to determine if this is an add or update action


            var role = model.Id != 0 ? _uow.Roles.SingleOrDefault(u => u.Id == model.Id) : new Role();


            role.RoleName = model.RoleName;
            role.Rate = model.Rate;

            try
            {
                if (model.Id == 0)
                    _uow.Roles.Add(role);
                _uow.Complete();
                return Ok();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
