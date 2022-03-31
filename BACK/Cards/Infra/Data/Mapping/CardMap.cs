namespace Cards.Infra.Data.Mapping;
using Cards.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CardMap : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.HasKey(prop => prop.Id);

        builder.Property(prop => prop.Title)
            .HasConversion(prop => prop.ToString(), prop => prop)
            .IsRequired();

        builder.Property(prop => prop.Content)
           .HasConversion(prop => prop.ToString(), prop => prop)
           .IsRequired();

        builder.Property(prop => prop.List)
            .HasConversion(prop => prop.ToString(), prop => prop)
            .IsRequired();
    }
}