using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.DAL.EF.Models.DB
{
    public class TableBase : ITrackable
    {
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public int? LastUpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
