using Microsoft.EntityFrameworkCore;
using Orion.DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.DAL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public DbContext Context { get; }
        public UnitOfWork(DbContext dbContext)
        {
            Context = dbContext;
        }

        public void BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public bool Commit()
        {
            throw new NotImplementedException();
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }
    }
}
