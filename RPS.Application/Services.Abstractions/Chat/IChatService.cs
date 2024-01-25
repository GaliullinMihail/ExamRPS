using RPS.Domain.Entities;

namespace RPS.Application.Services.Abstractions.Chat;

public interface IChatService
{
    public Task<Room> GetChatById(string curUserId, string userId);
}