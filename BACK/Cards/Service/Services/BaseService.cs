namespace Cards.Service.Services;
using FluentValidation;
using Cards.Domain.Entities;
using Cards.Domain.Interfaces;
using System;
using System.Collections.Generic;

public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : BaseEntity
{
    private readonly IBaseRepository<TEntity> _baseRepository;

    public BaseService(IBaseRepository<TEntity> baseRepository)
    {
        _baseRepository = baseRepository;
    }

    public TEntity AddOrUpdate<TValidator>(TEntity obj) where TValidator : AbstractValidator<TEntity>
    {
        Validate(obj, Activator.CreateInstance<TValidator>());
        _baseRepository.InsertOrUpdate(obj);
        return obj;
    }

    public IList<TEntity> Delete(Guid id)
    {
        _baseRepository.Delete(id);
        return Get();
    }

    public IList<TEntity> Get() => _baseRepository.Select();

    public TEntity GetById(Guid id) => _baseRepository.Select(id);
    
    private void Validate(TEntity obj, AbstractValidator<TEntity> validator)
    {
        if (obj == null)
            throw new Exception("Registros não detectados!");

        validator.ValidateAndThrow(obj);
    }
}
