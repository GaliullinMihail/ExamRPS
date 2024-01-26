using RPS.Application.Services.Abstractions.Cqrs.Queries;
using RPS.Shared.Mongo;

namespace RPS.Application.Features.MongoDb.GetPlayerRating;

public record GetPlayerRatingMongoQuery(string Key): IQuery<PlayerRatingDto>;