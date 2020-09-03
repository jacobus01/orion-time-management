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
            return OrionContext.User.SingleOrDefault(e => e.Email == email);
        }

        public User GetById(int id)
        {
            return OrionContext.User.SingleOrDefault(e => e.Id == id);
        }

        //TODO: This check should not happen here and needs to be put in a helper class
        public bool CheckPassword(User user, string password)
        {
            string decryptedPassword = Cipher.Decrypt(user.PasswordHash, Cipher.orionSalt);
            return password == decryptedPassword;
        }

        public OrionContext OrionContext
        {
            get { return Context as OrionContext; }
        }
    }
}
