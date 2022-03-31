namespace Cards.Infra.Data.Context;
using Cards.Domain.Entities;
using Cards.Infra.Data.Mapping;
using Microsoft.EntityFrameworkCore;

public class CardContext : DbContext
{
    public CardContext(DbContextOptions<CardContext> options) : base(options)
    {

    }

    public DbSet<Card> Cards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Card>(new CardMap().Configure);
    }
}