using Microsoft.EntityFrameworkCore;
using Orion.DAL.EF.Models.DB;
using Orion.DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orion.DAL.Repository
{
    public class Repository<T> : IRepository<T>  where T : class
    {
        public IUnitOfWork _unitOfWork { get; }

        public Repository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<T> GetAll()
        {
            return _unitOfWork.Context.Set<T>().ToList<T>();
        }

        public T Get(T entity)
        {
            return _unitOfWork.Context.Set<T>().FirstOrDefault<T>(e => e == entity);
        }

        public void Add(T entity)
        {
            _unitOfWork.Context.Set<T>().Add(entity);

        }

        public void Delete(T entity)
        {
            T existing = _unitOfWork.Context.Set<T>().Find(entity);
            if (existing != null) _unitOfWork.Context.Set<T>().Remove(existing);
        }

        public void Update(T entity)
        {
            _unitOfWork.Context.Entry(entity).State = EntityState.Modified;
            _unitOfWork.Context.Update(entity);
        }

        public void Dispose()
        {
  
        }
    }
}
