namespace Cards.Domain.Entities;

public class Card : BaseEntity
{
    public Card(Guid id){Id = id;}
    public Card(){Id = Guid.NewGuid();}

    public string Title { get; init; }

    public string Content { get; init; }

    public string List { get; init; }
}