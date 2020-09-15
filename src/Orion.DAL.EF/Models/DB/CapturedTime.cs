using System;
using System.Collections.Generic;

namespace Orion.DAL.EF.Models.DB
{
    public partial class CapturedTime : TableBase
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TaskId { get; set; }
        public int Color { get; set; }
        public decimal Rate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public virtual Task Task { get; set; }
        public virtual User User { get; set; }
    }
}
