using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orion.DAL.EF.Models.DB;
using Orion.DAL.Repository.Interfaces;

namespace Orion.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CapturedTimeController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        public CapturedTimeController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost]
        [Route("CreateCapturedTime")]
        [Authorize]
        //POST : /api/Task/CreateTask
        public Object PostCapturedTime(Object model)
        {
            var capturedTime = new CapturedTime();
            dynamic res = model;
            capturedTime.Rate = res.Rate;
            capturedTime.TaskId = res.TaskId;
            capturedTime.UserId = res.UserId;
            capturedTime.StartTime= res.StartTime;
            capturedTime.EndTime = res.EndTime;
            try
            {
                //TODO: Validate user has not worked more than 10 hours for the given date
                _uow.CapturedTimes.Add(capturedTime);
                _uow.Complete();
                return Ok();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        [Route("CapturedTimes")]
        [Authorize]
        //POST : /api/Task/Tasks
        public IActionResult GetTaskList()
        {

            try
            {
                var result = _uow.CapturedTimes.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
