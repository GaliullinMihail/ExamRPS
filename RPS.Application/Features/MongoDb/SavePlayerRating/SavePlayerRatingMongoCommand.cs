using RPS.Application.Services.Abstractions.Cqrs.Commands;
using RPS.Shared.Mongo;

namespace RPS.Application.Features.MongoDb.SavePlayerRating;

public record SavePlayerRatingMongoCommand(PlayerRatingDto PlayerRating): ICommand;