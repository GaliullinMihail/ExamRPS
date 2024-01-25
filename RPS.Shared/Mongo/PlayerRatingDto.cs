using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RPS.Shared.Mongo;

public class PlayerRatingDto{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Key { get; set; }
    public int Rating { get; set; }
};