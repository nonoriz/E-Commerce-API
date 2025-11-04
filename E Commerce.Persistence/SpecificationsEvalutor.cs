using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistence
{
    public class SpecificationsEvalutor
    {
        public static IQueryable<TEntity> CreteQuery<TEntity, TKey>(IQueryable<TEntity>
           EntryPoint, ISpecifications<TEntity, TKey> specifications) where TEntity : BaseEntity<TKey>
        {
            var query = EntryPoint;
            if(specifications is not null)
            {
                if(specifications.Criteria is not null)
                {
                    query= query.Where(specifications.Criteria);
                }
                if (specifications.IncludeExpression is not null && specifications.IncludeExpression.Any())
                {
                  query= specifications.IncludeExpression
                        .Aggregate(query, (currentQuery, includeExp) => currentQuery.Include(includeExp));
                }

                if(specifications.OrderBy is not null)
                {
                    query= query.OrderBy(specifications.OrderBy);
                }
                if(specifications.OrderByDescending is not null)
                {
                    query= query.OrderByDescending(specifications.OrderByDescending);
                }

                if(specifications.IsPaginated)
                {
                    query= query.Skip(specifications.Skip).Take(specifications.Take);
                }

            }
            return query;

        }
    }
}
