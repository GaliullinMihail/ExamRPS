using RPS.Domain.Repositories.Abstractions;

namespace RPS.Infrastructure.Database.Repositories;

public sealed class RepositoryManager : IRepositoryManager
{
    private readonly Lazy<IRoomRepository> _lazyRoomRepository;
    private readonly Lazy<IMessageRepository> _lazyMessageRepository;
    public RepositoryManager(ApplicationDbContext dbContext)
    {
        _lazyMessageRepository = new Lazy<IMessageRepository>(() => new MessageRepository(dbContext));
        _lazyRoomRepository = new Lazy<IRoomRepository>(() => new RoomRepository(dbContext));
    }
    public IRoomRepository RoomRepository => _lazyRoomRepository.Value;
    public IMessageRepository MessageRepository => _lazyMessageRepository.Value;
}

