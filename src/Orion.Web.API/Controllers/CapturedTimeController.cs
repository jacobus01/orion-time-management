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
    [Route("[controller]")]
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
            _uow.SetActiveUserId(Int32.Parse(Request.Headers["CurrentUserId"]));

            var capturedTime = model.Id != 0 ? _uow.CapturedTimes.SingleOrDefault(u => u.Id == model.Id) : new CapturedTime();
            capturedTime.Rate = model.Rate.Value;
            capturedTime.TaskId = model.TaskId.Value;
            capturedTime.UserId = model.UserId.Value;
            capturedTime.StartTime= DateTime.Parse(model.StartTime);
            capturedTime.EndTime = DateTime.Parse(model.EndTime);
            try
            {
                //TODO: Validate user has not worked more than 10 hours for the given date
                if (model.Id == 0)
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
        public IActionResult GetCapturedTimesList()
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

        [HttpPost]
        [Route("CapturedTimesPerUser")]
        [Authorize]
        //POST : /api/Task/Tasks
        public IActionResult GetCapturedTimesPerUserList(CapturedTimePerUserResponse model)
        {

            try
            {
                var capturedTimes = _uow.CapturedTimes.GetByUserIdAndDates(DateTime.Parse(model.StartDate), DateTime.Parse(model.EndDate), model.UserId);
                var newList = new List<dynamic>();
                foreach(var time in capturedTimes)
                {
                    newList.Add( new { Id = time.Id,
                    UserId = time.UserId,
                    Rate = time.Rate,
                    StartTime = time.StartTime,
                    EndTime = time.EndTime,
                    TaskName = time.Task.TaskName
                    });
                }
                return Ok(newList);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
