namespace MassLib.Identity.Domain.Interfaces;

public interface IEncrypter
{
    string Encrypt(string password);
    bool Verify(string password, string hash);
}
