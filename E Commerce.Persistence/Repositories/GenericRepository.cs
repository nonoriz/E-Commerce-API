using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using E_Commerce.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreDbContext dbContext;

        public GenericRepository(StoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task AddAsync(TEntity entity) => await dbContext.Set<TEntity>().AddAsync(entity);


        public async Task<IEnumerable<TEntity>> GetAllAsync()=>await dbContext.Set<TEntity>().ToListAsync();


        public async Task<TEntity?> GetByIdAsync(TKey id)=>await dbContext.Set<TEntity>().FindAsync(id);

        public void Remove(TEntity entity)=> dbContext.Set<TEntity>().Remove(entity);

        public void Update(TEntity entity)=> dbContext.Set<TEntity>().Update(entity);

    }
}
