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
using Task = Orion.DAL.EF.Models.DB.Task;

namespace Orion.Web.API.Controllers
{
    //You must use attribute routing for any controllers that you want represented in your Swagger document(s):
    [Route("[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        public TaskController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost]
        [Route("CreateUpdateTask")]
        [Authorize]
        //POST : /api/ApplicationUser/CreateUser
        public IActionResult PostTask(TaskModel model)
        {
            _uow.SetActiveUserId(Int32.Parse(Request.Headers["CurrentUserId"]));
            //We need to determine if this is an add or update action


            var task = model.Id != 0 ? _uow.Tasks.SingleOrDefault(u => u.Id == model.Id) : new Task();


            task.TaskName = model.TaskName;
            task.Duration = model.Duration;

            try
            {
                if (model.Id == 0)
                    _uow.Tasks.Add(task);
                _uow.Complete();
                return Ok();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        [Route("Tasks")]
        [Authorize]
        //POST : /api/Task/Tasks
        public IActionResult GetTaskList()
        {

            try
            {
                var result = _uow.Tasks.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("Task")]
        [Authorize]
        //POST : /api/ApplicationUser/Users
        public IActionResult GetTask([FromBody] int Id)
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
    }
}
