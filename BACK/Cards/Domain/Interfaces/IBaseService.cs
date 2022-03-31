namespace Cards.Domain.Interfaces;

using FluentValidation;
using Cards.Domain.Entities;
using System.Collections.Generic;
using System;

public interface IBaseService<TEntity> where TEntity : BaseEntity
{
    TEntity AddOrUpdate<TValidator>(TEntity obj) where TValidator : AbstractValidator<TEntity>;

    IList<TEntity> Delete(Guid id);

    IList<TEntity> Get();

    TEntity GetById(Guid id);
}