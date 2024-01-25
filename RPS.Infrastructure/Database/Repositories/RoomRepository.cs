using RPS.Domain.Entities;
using RPS.Domain.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace RPS.Infrastructure.Database.Repositories;

public class RoomRepository: IRoomRepository
{
    private readonly ApplicationDbContext _dbContext;

    public RoomRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Room>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Rooms.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Room room)
    {
        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Room?> GetByRoomIdAsync(string roomId)
    {
        return await _dbContext.Rooms.FirstOrDefaultAsync(x => x.Id == roomId);
    }
}