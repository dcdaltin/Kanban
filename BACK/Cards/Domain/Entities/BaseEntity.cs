namespace Cards.Domain.Entities;
using System;

public abstract class BaseEntity
{
    public virtual Guid Id { get; init; }
}