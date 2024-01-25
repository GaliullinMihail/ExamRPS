namespace RPS.Domain.Repositories.Abstractions
{
    public interface IRepositoryManager
    {
        IRoomRepository RoomRepository { get; }
        IMessageRepository MessageRepository { get; }
    }
}
