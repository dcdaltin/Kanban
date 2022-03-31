namespace Cards.Domain.Interfaces;
using Cards.Domain.Entities;

public interface ITokenService
{
    string BuildToken(string key, string issuer, string identifier);
}