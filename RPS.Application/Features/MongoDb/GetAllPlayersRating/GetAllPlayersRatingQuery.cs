using RPS.Application.Services.Abstractions.Cqrs.Queries;
using RPS.Shared.Mongo;

namespace RPS.Application.Features.MongoDb.GetAllPlayersRating;

public record GetAllPlayersRatingQuery() : IQuery<List<PlayerRatingDto>>;