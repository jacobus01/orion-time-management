using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Orion.DAL.Repository.Interfaces
{
    public interface IRepository<T> : IDisposable where T : class
    {
        IUnitOfWork _unitOfWork { get; }
        IEnumerable<T> GetAll();
        T Get(T entity);
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}
