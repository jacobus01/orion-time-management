using System;
using System.Collections.Generic;

namespace Orion.DAL.EF.Models.DB
{
    public partial class Role : TableBase
    {
        public Role()
        {
            User = new HashSet<User>();
        }

        public int Id { get; set; }
        public string RoleName { get; set; }
        public decimal? Rate { get; set; }

        public virtual ICollection<User> User { get; set; }
    }
}
