using RPS.Application.Dto.MediatR;
using RPS.Application.Services.Abstractions;
using RPS.Application.Services.Abstractions.Cqrs.Commands;

namespace RPS.Application.Features.MongoDb.SavePlayerRating;

public class SavePlayerRatingMongoCommandHandler: ICommandHandler<SavePlayerRatingMongoCommand>
{
    private readonly IMongoDbClient _mongoClient;

    public SavePlayerRatingMongoCommandHandler(IMongoDbClient mongoClient)
    {
        _mongoClient = mongoClient;
    }

    public Task<Result> Handle(SavePlayerRatingMongoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _mongoClient.CreateAsync(request.PlayerRating);
            return Task.FromResult(new Result(true));
        }
        catch (Exception e)
        {
            return Task.FromResult(new Result(false, e.Message));
        }
    }
}