using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.DAL.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        IAccessGroupRepository AccessGroups { get; }
        ICapturedTimeRepository CapturedTimes { get; }
        IRoleRepository Roles { get; }
        ITaskRepository Tasks { get; }
        IUserRepository Users { get; }
        int Complete();
    }
}
