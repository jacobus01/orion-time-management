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
        [Route("CreateUpdateCapturedTime")]
        [Authorize]
        //POST : /api/Task/CreateTask
        public IActionResult PostCapturedTime(CapturedTimeModel model)
        {

            var capturedTime = model.Id.HasValue ? _uow.CapturedTimes.SingleOrDefault(u => u.Id == model.Id) : new CapturedTime();
            capturedTime.Rate = model.Rate;
            capturedTime.TaskId = model.TaskId;
            capturedTime.UserId = model.UserId;
            capturedTime.StartTime= model.StartTime;
            capturedTime.EndTime = model.EndTime;
            try
            {
                //TODO: Validate user has not worked more than 10 hours for the given date
                if (!model.Id.HasValue)
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
