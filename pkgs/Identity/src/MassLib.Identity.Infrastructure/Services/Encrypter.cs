using MassLib.Identity.Domain.Interfaces;
using BCrypt.Net;

namespace MassLib.Identity.Infrastructure.Services;

public class Encrypter : IEncrypter
{
    public string Encrypt(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool Verify(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
