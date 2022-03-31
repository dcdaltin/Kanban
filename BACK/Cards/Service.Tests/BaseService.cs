namespace Cards.Service.Tests;
using Xunit;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Cards.Infra.Data.Context;
using Cards.Domain.Entities;
using Cards.Service.Services;
using Cards.Service.Validators;
using Cards.Infra.Data.Repository;

public class BaseServiceTest
{
    private readonly BaseRepository<Card> _baseRepository;

    public BaseServiceTest()
    {
        var contextOptions = new DbContextOptionsBuilder<CardContext>()
        .UseInMemoryDatabase("Cards")
        .Options;

        var context = new CardContext(contextOptions);
        _baseRepository = new BaseRepository<Card>(context);
    }

    [Fact]
    public void CanAddCard()
    {
        var markdownContext = @"### [\<ins>](https://github.com/markdown-it/markdown-it-ins) ++Inserted text++";
        var service = new BaseService<Card>(_baseRepository);
        var card = new Card() { Title = "A title", List = "ToDo", Content = markdownContext };

        service.AddOrUpdate<CardValidator>(card);
        var storedCard = _baseRepository.Select(card.Id);

        Assert.NotNull(storedCard);
        Assert.Equal(card.Id, storedCard.Id);
        Assert.Equal(markdownContext, storedCard.Content);
    }

    [Fact]
    public void CanUpdateCard()
    {
        var markdownContext = @"### [\<ins>](https://github.com/markdown-it/markdown-it-ins) ++Inserted text++";
        var service = new BaseService<Card>(_baseRepository);
        var card = new Card() { Title = "A title", List = "ToDo", Content = markdownContext };
        var updatedCard = new Card(card.Id) { Title = "Another title", List = "ToDo", Content = markdownContext };

        service.AddOrUpdate<CardValidator>(card);
        var storedCard = _baseRepository.Select(card.Id);

        service.AddOrUpdate<CardValidator>(updatedCard);
        var storedUpdatedCard = _baseRepository.Select(updatedCard.Id);

        Assert.NotNull(storedUpdatedCard);
        Assert.Equal(storedCard.Id, storedUpdatedCard.Id);
        Assert.Equal("Another title", storedUpdatedCard.Title);
    }

    [Fact]
    public void TitleValidation()
    {
        var markdownContext = @"### [\<ins>](https://github.com/markdown-it/markdown-it-ins) ++Inserted text++";
        var service = new BaseService<Card>(_baseRepository);
        var card = new Card() { List = "ToDo", Content = markdownContext };

        Assert.Throws<FluentValidation.ValidationException>(() => service.AddOrUpdate<CardValidator>(card));
    }

    [Fact]
    public void ListValidation()
    {
        var markdownContext = @"### [\<ins>](https://github.com/markdown-it/markdown-it-ins) ++Inserted text++";
        var service = new BaseService<Card>(_baseRepository);
        var card = new Card() { Title = "A title", Content = markdownContext };

        Assert.Throws<FluentValidation.ValidationException>(() => service.AddOrUpdate<CardValidator>(card));
    }

    [Fact]
    public void ContentValidation()
    {
        var service = new BaseService<Card>(_baseRepository);
        var card = new Card() { Title = "A title", List = "ToDo" };

        Assert.Throws<FluentValidation.ValidationException>(() => service.AddOrUpdate<CardValidator>(card));
    }
}