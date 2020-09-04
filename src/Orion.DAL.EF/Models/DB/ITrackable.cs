using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.DAL.EF.Models.DB
{
    public interface ITrackable
    {
        DateTime? CreatedAt { get; set; }
        int? CreatedBy { get; set; }
        DateTime? LastUpdatedAt { get; set; }
        int? LastUpdatedBy { get; set; }
        bool IsDeleted { get; set; }

    }
}
