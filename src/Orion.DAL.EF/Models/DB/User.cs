using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Orion.DAL.EF.Models.DB
{
    public partial class User : TableBase
    {
        public User()
        {
            CapturedTime = new HashSet<CapturedTime>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool? ChangePasswordOnNextLogin { get; set; }
        public string EmployeeNumber { get; set; }
        public bool? IsActive { get; set; }
        public bool? LockoutEnabled { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public int? RoleId { get; set; }

        public int? AccessGroupId { get; set; }
        public byte[] ProfilePicture { get; set; }

        public virtual Role Role { get; set; }
        public virtual AccessGroup AccessGroup { get; set; }
        public virtual ICollection<CapturedTime> CapturedTime { get; set; }
    }
}
