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

    public Task<IQueryable<Room>> GetAllAsync(CancellationToken cancellationToken)
    {
        return  Task.FromResult(_dbContext.Rooms.AsQueryable());
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

    public async Task JoinRoomByIdAsync(string roomId, User player)
    {
        var room = await _dbContext.Rooms.FirstOrDefaultAsync(r => r.Id == roomId);
        
        if (room is null)
            return;

        room.SecondPlayerId = player.Id;
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveRoomByIdAsync(string roomId)
    {
        var room = await GetByRoomIdAsync(roomId);
        _dbContext.Rooms.Remove(room!);
        await _dbContext.SaveChangesAsync();
    }
}