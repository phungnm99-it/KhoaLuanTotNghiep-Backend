using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WebAPI.Model;
using WebAPI.Repository.Interface;

namespace WebAPI.Repository
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected PTStoreContext RepositoryContext { get; set; }
        public GenericRepository(PTStoreContext repositoryContext)
        {
            this.RepositoryContext = repositoryContext;
        }
        public IQueryable<T> FindAll()
        {
            return this.RepositoryContext.Set<T>().AsNoTracking();
        }
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            try
            {
                var result = this.RepositoryContext.Set<T>()
                .Where(expression).AsNoTracking();
                return result;
            }
            catch(Exception ex)
            {
                return null;
            }


        }
        public void Create(T entity)
        {
            this.RepositoryContext.Set<T>().Add(entity);
        }
        public void Update(T entity)
        {
            this.RepositoryContext.Set<T>().Update(entity);
        }
        public void Delete(T entity)
        {
            this.RepositoryContext.Set<T>().Update(entity);
        }
    }
}
