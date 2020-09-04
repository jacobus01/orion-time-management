using Microsoft.EntityFrameworkCore;
using Orion.DAL.EF.Models.DB;
using Orion.DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.DAL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OrionContext _context;

        //What is so attractive about this pattern is that it injects just a single instance of the context
        //for all the repositories and neatly desposes it when done
        public UnitOfWork(OrionContext context)
        {
            _context = context;
            AccessGroups = new AccessGroupRepository(_context);
            CapturedTimes = new CapturedTimeRepository(_context);
            Roles = new RoleRepository(_context);
            Tasks = new TaskRepository(_context);
            Users = new UserRepository(_context);

        }

        public void SetActiveUserId(int Id)
        {
            _context.CurrentUserId = Id;
        }

        public IAccessGroupRepository AccessGroups { get; private set; }

        public ICapturedTimeRepository CapturedTimes { get; private set; }

        public IRoleRepository Roles { get; private set; }

        public ITaskRepository Tasks { get; private set; }

        public IUserRepository Users { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

