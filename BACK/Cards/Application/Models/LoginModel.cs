namespace Cards.Application.Models;
public record Request(string Login,string Password);
public record Response(string Token);
