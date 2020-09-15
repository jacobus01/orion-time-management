using Orion.DAL.EF.Models.DB;
using Orion.DAL.Repository.Interfaces;
using System.Collections.Generic;

namespace Orion.DAL.Repository.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {

        User GetByEmail(string email);

        bool CheckPassword(User user, string password);
        User GetById(int id);

        IEnumerable<User> GetAllNotDeleted();

        bool IsUnusedUserName(string userName);

        int GetTotal();

        IEnumerable<User> GetAllWithSubs();
    }
}