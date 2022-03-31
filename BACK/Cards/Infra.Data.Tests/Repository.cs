namespace Cards.Infra.Data.Tests;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Cards.Infra.Data.Repository;
using Cards.Infra.Data.Context;
using Cards.Domain.Entities;

public class BaseRepositoryTest
{
    private readonly DbContextOptions<CardContext> _contextOptions;

    public BaseRepositoryTest()
    {
        _contextOptions = new DbContextOptionsBuilder<CardContext>()
        .UseInMemoryDatabase("Cards")
        .EnableSensitiveDataLogging()
        .Options;
    }

    [Fact]
    public void CanInsertCard()
    {
        var markdownContext = @"### [\<ins>](https://github.com/markdown-it/markdown-it-ins) ++Inserted text++";
        var card = new Card()
        { Title = "A title", List = "ToDo", Content = markdownContext };

        using var context = CreateContext();
        var repository = new BaseRepository<Card>(context);
        repository.InsertOrUpdate(card);
        var storedCard = context.Find<Card>(card.Id);

        Assert.NotNull(storedCard);
        Assert.Equal(card.Id, storedCard?.Id);
        Assert.Equal(markdownContext, storedCard?.Content);
    }

    [Fact]
    public void CanNotInsertCardWithoutTitle()
    {
        var markdownContext = @"### [\<ins>](https://github.com/markdown-it/markdown-it-ins) ++Inserted text++";
        var card = new Card
        { List = "ToDo", Content = markdownContext };

        using var context = CreateContext();
        var repository = new BaseRepository<Card>(context);

        Assert.ThrowsAny<DbUpdateException>(() => repository.InsertOrUpdate(card));

    }

    [Fact]
    public void CanNotInsertCardWithoutList()
    {
        var markdownContext = @"### [\<ins>](https://github.com/markdown-it/markdown-it-ins) ++Inserted text++";
        var card = new Card
        { Title = "A title", Content = markdownContext };

        using var context = CreateContext();
        var repository = new BaseRepository<Card>(context);

        Assert.ThrowsAny<DbUpdateException>(() => repository.InsertOrUpdate(card));

    }

    [Fact]
    public void CanNotInsertCardWithoutContent()
    {
        var card = new Card
        { Title = "A title", List = "ToDo" };

        using var context = CreateContext();
        var repository = new BaseRepository<Card>(context);

        Assert.ThrowsAny<DbUpdateException>(() => repository.InsertOrUpdate(card));

    }

    [Fact]
    public void CanUpdateCard()
    {
        var card = new Card() { Title = "A title", List = "ToDo", Content = "### Title" };
        var updatedCard = new Card(card.Id) { Title = "Another title", List = "ToDo", Content = "### Title" };

        using var context = CreateContext();
        var repository = new BaseRepository<Card>(context);

        repository.InsertOrUpdate(card);
        repository.InsertOrUpdate(updatedCard);

        Assert.Equal(card.Id, updatedCard.Id);
        Assert.Equal("Another title", context.Find<Card>(updatedCard.Id)?.Title);

    }

    [Fact]
    public void CanDeleteCard()
    {
        var card = new Card() { Title = "A title", List = "ToDo", Content = "### Title" };

        using var context = CreateContext();
        var repository = new BaseRepository<Card>(context);

        repository.InsertOrUpdate(card);
        Assert.Equal(card.Id, context.Find<Card>(card.Id)?.Id);

        repository.Delete(card.Id);
        Assert.Null(context.Find<Card>(card.Id));
    }

    [Fact]
    public void CanSelectCard()
    {
        var card1 = new Card() { Title = "A title", List = "ToDo", Content = "### Title"  };
        var card2 = new Card() { Title = "Another title", List = "ToDo", Content = "### Title"  };

        using var context = CreateContext();
        var repository = new BaseRepository<Card>(context);

        repository.InsertOrUpdate(card1);
        repository.InsertOrUpdate(card2);
        var storedCard1 = repository.Select(card1.Id);
        var storedCard2 = repository.Select(card2.Id);
        var allCards = repository.Select();

        Assert.NotNull(storedCard1);
        Assert.NotNull(storedCard2);
        Assert.Contains(storedCard1, allCards);
        Assert.Contains(storedCard2, allCards);
    }

    CardContext CreateContext() => new CardContext(_contextOptions);
}