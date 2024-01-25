using RPS.Domain.Entities;

namespace RPS.Domain.Repositories.Abstractions;

public interface IRoomRepository
{
    public Task<IQueryable<Room>> GetAllAsync(CancellationToken cancellationToken);
    public Task AddAsync(Room room);
    public Task<Room?> GetByRoomIdAsync(string roomId);
    public Task JoinRoomByIdAsync(string roomId, User player);
}