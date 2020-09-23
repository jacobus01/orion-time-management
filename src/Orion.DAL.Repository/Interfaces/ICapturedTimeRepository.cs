using Orion.DAL.EF.Models.DB;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.DAL.Repository.Interfaces
{
    public interface ICapturedTimeRepository: IRepository<CapturedTime>
    {
        IEnumerable<CapturedTime> GetByUserIdAndDates(DateTime startdate, DateTime endDate, int userId);

        IEnumerable<CapturedTime> GetAllActive();

        decimal GetTotalHoursPerDateRange(DateTime startdate, DateTime endDate);

        decimal GetTotalHoursPerDateRangePerUserID(DateTime startdate, DateTime endDate, int userId);

        decimal GetTotalPayPerDateRange(DateTime startdate, DateTime endDate);

        decimal GetTotalPayPerDateRangePerUserId(DateTime startdate, DateTime endDate, int userId);

        decimal GetCapturefTimePerUserPerDate(DateTime date, int userId);

        decimal GetCapturefTimePerUserPerDate(DateTime date, int userId, int excludeId);
    }
}
