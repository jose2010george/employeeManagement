using EmployManagementSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace EmployManagementSystem.Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        void Delete(TEntity entityToDelete);
        void Delete(long id);
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");
        Task<TEntity> GetByID(long id); 
        void Insert(TEntity entity);
        void Update(TEntity entityToUpdate);
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }


}
