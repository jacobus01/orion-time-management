using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.Web.API.Models
{
    public class CapturedTimeModel
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public int? TaskId { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Duration { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
