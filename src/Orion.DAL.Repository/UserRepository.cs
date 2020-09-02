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
        public IUnitOfWork _unitOfWork { get; }
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public User GetByEmail(string email)
        {
            return _unitOfWork.Context.Set<User>().FirstOrDefault(e => e.Email == email);
        }

        public User GetById(int id)
        {
            return _unitOfWork.Context.Set<User>().FirstOrDefault(e => e.Id == id);
        }

        public bool CheckPassword(User user, string password)
        {
            string decryptedPassword = Cipher.Decrypt(user.PasswordHash, Cipher.orionSalt);
            return password == decryptedPassword;
        }
    }
}
