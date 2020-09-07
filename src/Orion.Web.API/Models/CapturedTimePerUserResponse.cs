using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.Web.API.Models
{
    public class CapturedTimePerUserResponse
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int UserId { get; set; }
    }
}
