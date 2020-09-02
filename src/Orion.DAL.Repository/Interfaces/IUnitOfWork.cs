using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.DAL.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        DbContext Context { get; }
        void BeginTransaction();
        void SaveChanges();
        bool Commit();
        void Rollback();
    }
}
