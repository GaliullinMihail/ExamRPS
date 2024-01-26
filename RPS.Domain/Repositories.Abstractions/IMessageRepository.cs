using RPS.Domain.Entities;

namespace RPS.Domain.Repositories.Abstractions;

public interface IMessageRepository
{
    public Task<IEnumerable<Message>> GetAllAsync(CancellationToken cancellationToken);
    public Task AddAsync(Message message);
    public Task<Message?> GetByMessageIdAsync(string messageId);
}