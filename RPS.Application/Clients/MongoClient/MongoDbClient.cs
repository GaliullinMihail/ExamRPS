using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RPS.Application.Configs;
using RPS.Application.Services.Abstractions;
using RPS.Shared.Mongo;

namespace RPS.Application.Clients.MongoClient;

public class MongoDbClient: IMongoDbClient
{
    private readonly IMongoCollection<PlayerRatingDto> _metadataCollection;

    public MongoDbClient(IOptions<MongoDbConfig> mongoDbConfig)
    {
        var mongoClient = new MongoDB.Driver.MongoClient(
            mongoDbConfig.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            mongoDbConfig.Value.DatabaseName);

        _metadataCollection = mongoDatabase.GetCollection<PlayerRatingDto>(
            mongoDbConfig.Value.MetadataCollectionName);
    }

    public async Task<List<PlayerRatingDto>> GetAsync() =>
        await _metadataCollection.Find(_ => true).ToListAsync();

    public async Task<PlayerRatingDto> GetAsync(string key) =>
        await _metadataCollection.Find(b => b.Key == key).FirstOrDefaultAsync();

    public async Task CreateAsync(PlayerRatingDto newPlayerRating) =>
        await _metadataCollection.InsertOneAsync(newPlayerRating);

    public async Task UpdateAsync(string key, PlayerRatingDto updatedPlayerRating) =>
        await _metadataCollection.ReplaceOneAsync(b => b.Key == key, updatedPlayerRating);

    public async Task RemoveAsync(string key) =>
        await _metadataCollection.DeleteOneAsync(b => b.Key == key);
}