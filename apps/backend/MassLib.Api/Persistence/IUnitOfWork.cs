namespace MassLib.Api.Persistence;

public interface IUnitOfWork
{
    Task CommitAsync();
}
