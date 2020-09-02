using Orion.DAL.EF.Models.DB;
using Orion.DAL.Repository.Interfaces;

namespace Orion.DAL.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        IUnitOfWork _unitOfWork { get; }

        User GetByEmail(string email);

        bool CheckPassword(User user, string password);
        User GetById(int id);
    }
}