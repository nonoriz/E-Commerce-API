using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using E_Commerce.Persistence.Data.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext dbContext;
        private readonly Dictionary<Type, object> repositories = [];

        public UnitOfWork(StoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IGenericRepository<TEntity, TKey> GenericRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
           var EntityType= typeof(TEntity);
            if (repositories.TryGetValue(EntityType, out object? Reporsitory))
            {
                return (IGenericRepository<TEntity, TKey>)Reporsitory;
            }
            var newRepo = new GenericRepository<TEntity, TKey>(dbContext);
            repositories[EntityType]=newRepo;
            return newRepo;
        }

        public async Task<int> SaveChangesAsync()=>await dbContext.SaveChangesAsync();

    }
}
