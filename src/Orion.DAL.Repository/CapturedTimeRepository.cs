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
        public IEnumerable<CapturedTime> GetAllActive()
        {
            return OrionContext.CapturedTime.Where<CapturedTime>(c => c.IsDeleted == false);
        }
    }
}
