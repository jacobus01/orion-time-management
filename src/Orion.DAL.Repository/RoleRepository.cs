using Microsoft.EntityFrameworkCore;
using Orion.DAL.EF.Models.DB;
using Orion.DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orion.DAL.Repository
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(OrionContext context) : base(context)
        {
        }
        public OrionContext OrionContext
        {
            get { return Context as OrionContext; }
        }
        public Role GetRolePerUserId(int userId)
        {
            return OrionContext.User.Where<User>(u => u.Id == userId && u.IsDeleted == false).Include(r => r.Role).FirstOrDefault().Role;
        }
    }
}
