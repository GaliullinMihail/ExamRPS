using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RPS.Shared.Mongo;

public class PlayerRatingDto
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = default!;

    public string Key { get; set; } = default!;
    
    public int Rating { get; set; }
};