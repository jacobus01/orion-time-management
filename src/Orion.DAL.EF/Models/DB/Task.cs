using System;
using System.Collections.Generic;

namespace Orion.DAL.EF.Models.DB
{
    public partial class Task : TableBase
    {
        public Task()
        {
            CapturedTime = new HashSet<CapturedTime>();
        }

        public int Id { get; set; }
        public string TaskName { get; set; }
        public decimal? Duration { get; set; }

        public virtual ICollection<CapturedTime> CapturedTime { get; set; }
    }
}
