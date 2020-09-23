using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Orion.DAL.EF.Models.DB;
using Orion.DAL.Repository.Interfaces;
using Orion.Web.API.Models;

namespace Orion.Web.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CapturedTimeController : ControllerBase
    {
        private readonly int[] daysInMonth = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        private readonly IUnitOfWork _uow;
        public CapturedTimeController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        private int GetDaysPerMonth(int year, int month)
        {
            if(month == 2)
            {
                if (year % 100 == 0)
                {
                    //not leap year
                    return daysInMonth[month - 1];
                }
                else if (year % 4 == 0)
                {
                    return 29;
                    //leap year
                }
            }
            return daysInMonth[month - 1];
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
            capturedTime.Color = model.Color.Value;
            try
            {
                if(isValidTime(model, model.Id.Value !=0))
                {
                    if (model.Id == 0)
                        _uow.CapturedTimes.Add(capturedTime);
                    _uow.Complete();
                    return Ok(new { newId = capturedTime.Id });
                }
                else
                {
                    return BadRequest(new { message = "Captured time would exceed maximum of 10 hours a day" });
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private bool isValidTime(CapturedTimeModel model, bool isUpdate)
        {
            var hoursWorked = 0.0M;
            var startDate = DateTime.Parse(model.StartTime);
            var endDate = DateTime.Parse(model.EndTime);
            if (!isUpdate)
                hoursWorked = _uow.CapturedTimes.GetCapturefTimePerUserPerDate(startDate, model.UserId.Value);
            else
                hoursWorked = _uow.CapturedTimes.GetCapturefTimePerUserPerDate(startDate, model.UserId.Value, model.Id.Value);

            TimeSpan dateDiff = endDate - startDate;
            hoursWorked += decimal.Parse(dateDiff.TotalHours.ToString());
            return !(hoursWorked > 10);

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
                    TaskName = time.Task.TaskName,
                    TaskId = time.TaskId,
                    Color = time.Color
                    });
                }
                return Ok(newList);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("TotalHoursPerUser")]
        [Authorize]
        //POST : /api/Task/Tasks
        public IActionResult GetTotalHoursPerUser(TotalHoursResponse model)
        {
            var monthDate = DateTime.Parse(model.Response);
            DateTime startDate = monthDate.AddDays(-1 * monthDate.Day + 1);
            DateTime endDate = monthDate.AddDays(-1 * monthDate.Day + 1).AddDays(GetDaysPerMonth(monthDate.Year, monthDate.Month));

            try
            {
                var totalHours = _uow.CapturedTimes.GetTotalHoursPerDateRangePerUserID(startDate, endDate, model.UserId.Value);
                return Ok(new { TotalHours = totalHours});
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("TotalPayPerUser")]
        [Authorize]
        //POST : /api/Task/Tasks
        public IActionResult GetTotalPayPerUser(TotalPayResponse model)
        {
            var monthDate = DateTime.Parse(model.Response);
            DateTime startDate = monthDate.AddDays(-1 * monthDate.Day + 1);
            DateTime endDate = monthDate.AddDays(-1 * monthDate.Day + 1).AddDays(GetDaysPerMonth(monthDate.Year, monthDate.Month));

            try
            {
                var totalPay = _uow.CapturedTimes.GetTotalPayPerDateRangePerUserId(startDate, endDate, model.UserId.Value);
                return Ok(new { TotalPay = totalPay });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("TotalHours")]
        [Authorize]
        //POST : /api/Task/Tasks
        public IActionResult GetTotalHours(TotalHoursResponse model)
        {
            var monthDate = DateTime.Parse(model.Response);
            DateTime startDate = monthDate.AddDays(-1 * monthDate.Day + 1);
            DateTime endDate = monthDate.AddDays(-1 * monthDate.Day).AddDays(GetDaysPerMonth(monthDate.Year, monthDate.Month));

            try
            {
                var totalHours = _uow.CapturedTimes.GetTotalHoursPerDateRange(startDate, endDate);
                return Ok(new { TotalHours = totalHours });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("TotalPay")]
        [Authorize]
        //POST : /api/Task/Tasks
        public IActionResult GetTotalPay(TotalPayResponse model)
        {
            var monthDate = DateTime.Parse(model.Response);
            DateTime startDate = monthDate.AddDays(-1 * monthDate.Day + 1);
            DateTime endDate = monthDate.AddDays(-1 * monthDate.Day).AddDays(GetDaysPerMonth(monthDate.Year, monthDate.Month));

            try
            {
                var totalPay = _uow.CapturedTimes.GetTotalPayPerDateRange(startDate, endDate);
                return Ok(new { TotalPay = totalPay });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("DeleteCapturedTime")]
        [Authorize]
        //POST : /api/Task/CreateTask
        public IActionResult DeleteCapturedTime([FromBody] int Id)
        {
            _uow.SetActiveUserId(Int32.Parse(Request.Headers["CurrentUserId"]));

            var capturedTime = _uow.CapturedTimes.SingleOrDefault(u => u.Id == Id);
            try
            {

                _uow.CapturedTimes.Remove(capturedTime);
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
