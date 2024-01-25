using RPS.Application.Dto.MediatR;
using RPS.Application.Services.Abstractions;
using RPS.Application.Services.Abstractions.Cqrs.Queries;
using RPS.Shared.Mongo;

namespace RPS.Application.Features.MongoDb.GetPlayerRating;

public class GetPlayerRatingMongoQueryHandler: IQueryHandler<GetPlayerRatingMongoQuery, PlayerRatingDto>
{
    private readonly IMongoDbClient _mongoDb;

    public GetPlayerRatingMongoQueryHandler(IMongoDbClient mongoDb)
    {
        _mongoDb = mongoDb;
    }

    public async Task<Result<PlayerRatingDto>> Handle(GetPlayerRatingMongoQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var metadata = await _mongoDb.GetAsync(request.Key);
            return new Result<PlayerRatingDto>(metadata, true);
        }
        catch (Exception e)
        {
            return new Result<PlayerRatingDto>(null, false, e.Message);
        }
    }
}