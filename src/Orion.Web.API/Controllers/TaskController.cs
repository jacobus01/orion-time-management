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
        IRepository<DAL.EF.Models.DB.Task> _repo;
        public TaskController(IRepository<DAL.EF.Models.DB.Task> repo)
        {
            _repo = repo;
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
                _repo.Add(task);
                _repo._unitOfWork.Context.SaveChanges();
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
                var result = _repo.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
