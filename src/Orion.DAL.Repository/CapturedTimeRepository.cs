using Microsoft.EntityFrameworkCore;
using Orion.DAL.EF.Models.DB;
using Orion.DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orion.DAL.Repository
{
    public class CapturedTimeRepository: Repository<CapturedTime>, ICapturedTimeRepository
    {
        public CapturedTimeRepository(OrionContext context) : base(context)
        {
        }
        public OrionContext OrionContext
        {
            get { return Context as OrionContext; }
        }

        public IEnumerable<CapturedTime> GetByUserIdAndDates(DateTime startdate, DateTime endDate, int userId)
        {
            return OrionContext.CapturedTime.Where<CapturedTime>(c => c.StartTime >= startdate && c.EndTime <= endDate && c.UserId == userId && c.IsDeleted == false).Include(t => t.Task);
        }
        public decimal GetTotalHoursPerDateRange(DateTime startdate, DateTime endDate)
        {
            decimal totals = 0.0M;
            var records = OrionContext.CapturedTime.Where<CapturedTime>(c => c.StartTime >= startdate && c.EndTime <= endDate && c.IsDeleted == false).ToList<CapturedTime>();
            foreach(var time in records)
            {
                TimeSpan diffResult = time.EndTime - time.StartTime;
                totals += decimal.Parse(diffResult.TotalHours.ToString());
            }
            return totals;
        }
        public decimal GetTotalHoursPerDateRangePerUserID(DateTime startdate, DateTime endDate, int userId)
        {
            decimal totals = 0.0M;
            var records = OrionContext.CapturedTime.Where<CapturedTime>(c => c.StartTime >= startdate && c.EndTime <= endDate && c.UserId == userId && c.IsDeleted == false).ToList<CapturedTime>();
            foreach(var time in records)
            {
                TimeSpan diffResult = time.EndTime - time.StartTime;
                totals += decimal.Parse(diffResult.TotalHours.ToString());
            }
            return totals;
        }
        public decimal GetTotalPayPerDateRange(DateTime startdate, DateTime endDate)
        {
            decimal totals = 0.00M;
            var records = OrionContext.CapturedTime.Where<CapturedTime>(c => c.StartTime >= startdate && c.EndTime <= endDate && c.IsDeleted == false).ToList<CapturedTime>();
            foreach (var time in records)
            {
                TimeSpan diffResult = time.EndTime - time.StartTime;
                totals += decimal.Parse((diffResult.TotalHours * double.Parse(time.Rate.ToString())).ToString());
            }
            return totals;
        }
        public decimal GetTotalPayPerDateRangePerUserId(DateTime startdate, DateTime endDate, int userId)
        {
            decimal totals = 0.00M;
            var records = OrionContext.CapturedTime.Where<CapturedTime>(c => c.StartTime >= startdate && c.EndTime <= endDate && c.UserId == userId && c.IsDeleted == false).ToList<CapturedTime>();
            foreach (var time in records)
            {
                TimeSpan diffResult = time.EndTime - time.StartTime;
                totals += decimal.Parse((diffResult.TotalHours * double.Parse(time.Rate.ToString())).ToString());
            }
            return totals;
        }
        public IEnumerable<CapturedTime> GetAllActive()
        {
            return OrionContext.CapturedTime.Where<CapturedTime>(c => c.IsDeleted == false);
        }
        public decimal GetCapturefTimePerUserPerDate(DateTime date,int userId)
        {
            decimal totalHours = 0.0M;
            DateTime startOfDay = new DateTime(date.Year, date.Month, date.Day);
            DateTime nextDay = startOfDay.AddDays(1);
            var capturedTimes = OrionContext.CapturedTime.Where<CapturedTime>(c => c.StartTime >= startOfDay && c.EndTime < nextDay && c.UserId == userId && c.IsDeleted == false).ToList();
            foreach(var time in capturedTimes)
            {
                TimeSpan dateDiff = time.EndTime - time.StartTime;
                totalHours += Decimal.Parse(dateDiff.TotalHours.ToString());
            }
            return totalHours;
        }
    }
}
