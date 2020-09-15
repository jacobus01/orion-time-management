using Microsoft.EntityFrameworkCore;
using Orion.Common.Library.Encryption;
using Orion.DAL.EF.Models.DB;
using Orion.DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;

namespace Orion.DAL.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(OrionContext context) : base(context)
        {
        }

        public User GetByEmail(string email)
        {
            return OrionContext.User.SingleOrDefault(e => e.Email == email && e.IsDeleted == false);
        }

        public User GetById(int id)
        {
            return OrionContext.User.SingleOrDefault(e => e.Id == id && e.IsDeleted == false);
        }

        //TODO: This check should not happen here and needs to be put in a helper class
        public bool CheckPassword(User user, string password)
        {
            string decryptedPassword = Cipher.Decrypt(user.PasswordHash, Cipher.orionSalt);
            return password == decryptedPassword;
        }

        public IEnumerable<User> GetAllNotDeleted()
        {
            return OrionContext.User.Where(u => u.IsDeleted == false);
        }

        public bool IsUnusedUserName(string userName)
        {
            return OrionContext.User.Any(u => u.IsDeleted == false && u.UserName == userName);
        }

        public int GetTotal()
        {
            return OrionContext.User.Count(u => u.IsDeleted == false);
        }

        public IEnumerable<User> GetAllWithSubs()
        {
            return OrionContext.User.Where(u => u.IsDeleted == false).Include(r => r.Role).Include(a => a.AccessGroup);
        }

        public OrionContext OrionContext
        {
            get { return Context as OrionContext; }
        }
    }
}
