namespace MassLib.Shared.Persistence;

public interface IUnitOfWork
{
    Task CommitAsync();
}
