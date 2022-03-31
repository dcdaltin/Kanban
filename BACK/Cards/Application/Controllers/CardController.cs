namespace Cards.Application.Controllers;
using Microsoft.AspNetCore.Mvc;
using Cards.Domain.Entities;
using Cards.Domain.Interfaces;
using Cards.Service.Validators;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("cards")]
[Authorize]
public class CardController : ControllerBase
{
    private readonly ILogger<CardController> _logger;
    private readonly IBaseService<Card> _service;

    public CardController(ILogger<CardController> logger, IBaseService<Card> service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var cardList = _service.Get();
        return Ok(cardList);
    }

    [HttpPost]
    public IActionResult Post(Card card)
    {
        try
        {
            var storedCard = _service.AddOrUpdate<CardValidator>(card);
            return Created(this.Request.Path, storedCard);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    [HttpPut]
    public IActionResult Put(Guid id, Card card)
    {
        var existingEntity = _service.GetById(id);
        if(existingEntity == null) return NotFound();
        try
        {
            var toUpdate = new Card(id){Title = card.Title, Content = card.Content, List = card.List};
            var storedCard = _service.AddOrUpdate<CardValidator>(toUpdate);
            return Created(this.Request.Path, storedCard);
        }
        catch (FluentValidation.ValidationException e)
        {
            return BadRequest(e);
        }
    }

    [HttpDelete]
    public IActionResult Delete(Guid id)
    {
        try
        {
            var existingEntity = _service.GetById(id);
            if(existingEntity == null) return NotFound();
            var remainingCards = _service.Delete(id);
            return Ok(remainingCards);
        }
        catch (System.Exception)
        {
            return BadRequest();
        }       
    }
}