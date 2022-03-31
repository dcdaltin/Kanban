namespace Cards.Domain.Interfaces;
using Cards.Domain.Entities;
using System.Collections.Generic;
using System;

public interface IBaseRepository<TEntity> where TEntity : BaseEntity
{
    void InsertOrUpdate(TEntity obj);

    void Delete(Guid id);

    IList<TEntity> Select();

    TEntity Select(Guid id);
}