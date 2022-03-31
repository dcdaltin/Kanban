namespace Cards.Application.Logging;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Globalization;
using Cards.Domain.Entities;
using Cards.Domain.Interfaces;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private enum Actions { Removed, Updated };

    private void Log(Card? card, Actions action)
    {
        var culture = new CultureInfo("br-BR");
        var localDateTime = DateTime.Now.ToString(culture);
        var message = "{localDateTime} - Card {Id} - {Title} - {Action}";
        _logger.LogInformation(message, localDateTime, card?.Id, card?.Title, action);
    }

    public RequestLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _next = next;
        _logger = loggerFactory.CreateLogger<RequestLoggingMiddleware>();
    }

    public async Task Invoke(HttpContext context, IBaseService<Card> service)
    {
        string? idFromQuery = context.Request?.Query["id"].SingleOrDefault();
        Card? card = null;
        if (!String.IsNullOrEmpty(idFromQuery))
        {
            var id = Guid.Parse(idFromQuery);
            card = service.GetById(id);
        }

        try
        {
            await _next(context);
        }
        finally
        {
            switch (context.Request?.Method)
            {
                case "PUT":
                    Log(card, Actions.Updated);
                    break;
                case "DELETE":
                    Log(card, Actions.Removed);
                    break;
            }
        }
    }
}