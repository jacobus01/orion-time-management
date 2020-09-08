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
    }
}
