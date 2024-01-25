using RPS.Application.Dto.PlayerRating;
using RPS.Application.Services.Abstractions.Cqrs.Commands;

namespace RPS.Application.Features.MongoDb.UpdatePlayerRating;

public record UpdatePlayerRatingMongoCommand(UpdatePlayerRatingDto Model) : ICommand;