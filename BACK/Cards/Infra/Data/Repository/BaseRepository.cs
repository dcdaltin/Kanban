namespace Cards.Infra.Data.Repository;
using Cards.Domain.Entities;
using Cards.Domain.Interfaces;
using Cards.Infra.Data.Context;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly DbContext _dbContext;

        public BaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void InsertOrUpdate(TEntity obj)
        {
            var existingEntity = _dbContext.Set<TEntity>().Find(obj.Id);
            if(existingEntity == null) 
            {
                _dbContext.Set<TEntity>().Add(obj);
            }
            else
            {
                _dbContext.Entry(existingEntity).CurrentValues.SetValues(obj);
            }
            _dbContext.SaveChanges();
        }

        public void Delete(Guid id)
        {
            _dbContext.Set<TEntity>().Remove(Select(id));
            _dbContext.SaveChanges();
        }

        public IList<TEntity> Select() =>
            _dbContext.Set<TEntity>().ToList();

        public TEntity Select(Guid id) =>
            _dbContext.Set<TEntity>().Find(id);

    }