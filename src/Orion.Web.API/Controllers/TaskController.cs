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
    public class TaskController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        public TaskController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost]
        [Route("CreateTask")]
        [Authorize]
        //POST : /api/Task/CreateTask
        public Object PostTask(Object model)
        {
            var task = new DAL.EF.Models.DB.Task();
            dynamic res = model;
            task.TaskName = res.TaskName;
            task.Duration = res.Duration;
            try
            {
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
    }
}
