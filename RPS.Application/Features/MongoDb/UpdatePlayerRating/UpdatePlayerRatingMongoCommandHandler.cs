using RPS.Application.Dto.MediatR;
using RPS.Application.Services.Abstractions;
using RPS.Application.Services.Abstractions.Cqrs.Commands;

namespace RPS.Application.Features.MongoDb.UpdatePlayerRating;

public class UpdatePlayerRatingMongoCommandHandler 
    : ICommandHandler<UpdatePlayerRatingMongoCommand>
{
    private readonly IMongoDbClient _mongoClient;

    public UpdatePlayerRatingMongoCommandHandler(IMongoDbClient mongoClient)
    {
        _mongoClient = mongoClient;
    }

    public Task<Result> Handle(
        UpdatePlayerRatingMongoCommand request, 
        CancellationToken cancellationToken)
    {
        try
        {
            request.Model.PlayerRating.Rating += request.Model.RatingDifference;
            _mongoClient.UpdateAsync(request.Model.PlayerRating.Key, request.Model.PlayerRating);
            return Task.FromResult(new Result(true));
        }
        catch (Exception e)
        {
            return Task.FromResult(new Result(false, e.Message));
        }
    }
}