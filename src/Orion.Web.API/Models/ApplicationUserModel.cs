using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orion.Web.API.Models
{
    public class ApplicationUserModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeNumber { get; set; }
        public bool? IsActive { get; set; }
        public bool? LockoutEnabled { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public int? RoleId { get; set; }
        public byte[] ProfilePicture { get; set; }
    }
}
