using RPS.Shared.Mongo;

namespace RPS.Application.Services.Abstractions;

public interface IMongoDbClient
{
    public Task<List<PlayerRatingDto>> GetAsync();

    public Task<PlayerRatingDto> GetAsync(string key);

    public Task CreateAsync(PlayerRatingDto newPlayerRating);

    public Task UpdateAsync(string key, PlayerRatingDto updatedPlayerRating);

    public Task RemoveAsync(string key);
}