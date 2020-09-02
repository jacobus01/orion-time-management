using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.DAL.EF.Models.DB
{
    public partial class AccessGroup
    {
        public AccessGroup()
        {
            User = new HashSet<User>();
        }

        public int Id { get; set; }
        public string AccessGroupName { get; set; }

        public virtual ICollection<User> User { get; set; }
    }
}
