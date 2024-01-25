using RPS.Application.Dto.MediatR;
using RPS.Application.Services.Abstractions;
using RPS.Application.Services.Abstractions.Cqrs.Queries;
using RPS.Shared.Mongo;

namespace RPS.Application.Features.MongoDb.GetAllPlayersRating;

public class GetAllPlayersRatingQueryHandler 
    : IQueryHandler<GetAllPlayersRatingQuery, List<PlayerRatingDto>>
{
    private readonly IMongoDbClient _mongoClient;

    public GetAllPlayersRatingQueryHandler(IMongoDbClient mongoClient)
    {
        _mongoClient = mongoClient;
    }

    public async Task<Result<List<PlayerRatingDto>>> Handle(
        GetAllPlayersRatingQuery request, 
        CancellationToken cancellationToken)
    {
        try
        {
            return new Result<List<PlayerRatingDto>>(await _mongoClient.GetAsync(), true);
        }
        catch (Exception e)
        {
            return new Result<List<PlayerRatingDto>>(null, false);
        }
    }
}