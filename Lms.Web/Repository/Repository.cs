using Kys.Web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Kys.Web.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<int> InsertAsync(T entity);
        T Insert(T entity);
        IEnumerable<T> Insert(IEnumerable<T> entities);
        Task<int> UpdateAsync(T entity);
        T Update(T entity);
        IEnumerable<T> Update(IEnumerable<T> entities);
        Task<int> DeleteAsync(T entity);
        int Delete(T entity);
        Task<IEnumerable<T>> SelectNoTrackingAsync();
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(object id);
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync();
        Task<IEnumerable<T>> FromSqlAsync(string sqlQuery);
        Task<IQueryable<T>> FindByAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate);
        IQueryable<T> SelectIncludeMany(params Expression<Func<T, object>>[] includes);
        IQueryable<T> SelectInclude(Expression<Func<T, object>> include);
        IQueryable<T> SelectNoTracking();
        IQueryable<T> GetAll();
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        T GetById(object id);
        T Find(Expression<Func<T, bool>> predicate);
        int Count();
        IEnumerable<T> FromSql(string sqlQuery);
    }

    public class Repository<T> : IRepository<T> where T :  class
    {
        internal KutuphaneDbContext _dbContext { get; private set; }

        public Repository(KutuphaneDbContext context)
        {
            _dbContext = context;
        }

        #region Kaydet
        public T Insert(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            this._dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();
            return entity;
        }
        public IEnumerable<T> Insert(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            foreach (var entity in entities)
                this._dbContext.Set<T>().Add(entity);

            this._dbContext.SaveChanges();
            return entities;
        }
        public async Task<int> InsertAsync(T entity)
        {
            this._dbContext.Set<T>().Add(entity);
            return await this._dbContext.SaveChangesAsync();
        }
        #endregion  Kaydet

        #region Guncelle
        public T Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            this._dbContext.Set<T>().Update(entity);
            this._dbContext.SaveChanges();
            return entity;
        }
        public async Task<int> UpdateAsync(T entity)
        {
            this._dbContext.Set<T>().Update(entity);
            return await this._dbContext.SaveChangesAsync();

        }
        public IEnumerable<T> Update(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            foreach (var entity in entities)
                this._dbContext.Set<T>().Update(entity);
            this._dbContext.SaveChanges();
            return entities;
        }
        #endregion

        #region Sil

        public int Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            this._dbContext.Set<T>().Remove(entity);
            return this._dbContext.SaveChanges();
        }
        public async Task<int> DeleteAsync(T entity)
        {
            this._dbContext.Set<T>().Remove(entity);
            return await this._dbContext.SaveChangesAsync();
        }

        #endregion Sil

        #region Async

        public async Task<IEnumerable<T>> SelectNoTrackingAsync() => await this._dbContext.Set<T>().AsNoTracking().ToListAsync();
        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await this._dbContext.Set<T>().Where(predicate).ToListAsync();
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await this._dbContext.Set<T>().ToListAsync();
        }
        public async Task<T> GetByIdAsync(object id)
        {
            return await this._dbContext.Set<T>().FindAsync(id);
        }
        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await this._dbContext.Set<T>().SingleOrDefaultAsync(predicate);
        }

        public async Task<int> CountAsync()
        {
            return await this._dbContext.Set<T>().CountAsync();
        }
        public async Task<IEnumerable<T>> FromSqlAsync(string sqlQuery)
        {
            return await this._dbContext.Set<T>().FromSqlRaw<T>(sqlQuery).ToListAsync();
        }

        #endregion

        #region Sync

        public IQueryable<T> SelectIncludeMany(params Expression<Func<T, object>>[] includes)
        {
            return this._dbContext.Set<T>().IncludeMultiple(includes);

        }
        public IQueryable<T> SelectInclude(Expression<Func<T, object>> include)
        {
            return this._dbContext.Set<T>().Include(include);
        }

        public IQueryable<T> SelectNoTracking() => this._dbContext.Set<T>().AsNoTracking();

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate)
        {
            return this._dbContext.Set<T>().Where(predicate);
        }

        public IQueryable<T> GetAll()
        {
            return this._dbContext.Set<T>();
        }
        public T GetById(object id)
        {
            return this._dbContext.Set<T>().Find(id);
        }

        public T Find(Expression<Func<T, bool>> predicate)
        {
            return this._dbContext.Set<T>().SingleOrDefault(predicate);
        }
        public int Count()
        {
            return this._dbContext.Set<T>().Count();
        }
        public IEnumerable<T> FromSql(string sqlQuery)
        {
            return this._dbContext.Set<T>().FromSqlRaw<T>(sqlQuery);
        }

        public async Task<IQueryable<T>> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            var data = await this._dbContext.Set<T>().Where(predicate).ToListAsync();
            return data.AsQueryable();
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return this._dbContext.Set<T>().Where(predicate);
        }

        #endregion
    }
}
