using MassTransit;
using MediatR;
using RPS.Application.Dto.PlayerRating;
using RPS.Application.Features.MongoDb.GetPlayerRating;
using RPS.Application.Features.MongoDb.UpdatePlayerRating;
using RPS.Application.Services.Abstractions.GameResultConsumer;
using RPS.Shared.GameResult;
using RPS.Shared.Mongo;
using RPS.Shared.StaticData;

namespace RPS.Application.Services.GameResultConsumer;

public class GameResultConsumer : IGameResultConsumer
{
    private readonly IMediator _mediator;

    public GameResultConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<GameResultDto> context)
    {
        var gameResult = context.Message;
        
        var winnerRating = 
            await _mediator.Send(new GetPlayerRatingMongoQuery(gameResult.WinnerNickName));
        var looserRating= 
            await _mediator.Send(new GetPlayerRatingMongoQuery(gameResult.LooserNickName));
        
        if (!winnerRating.IsSuccess || !looserRating.IsSuccess)
            return;
        
        if (gameResult.Draw)
        {
            await SendUpdateInMongo(winnerRating.Value!, StaticData.PointsForDraw);
            await SendUpdateInMongo(looserRating.Value!, StaticData.PointsForDraw);
            return;
        }
        await SendUpdateInMongo(winnerRating.Value!, StaticData.PointsForWinner);
        await SendUpdateInMongo(looserRating.Value!, StaticData.PointsForLooser);
    }

    private async Task SendUpdateInMongo(PlayerRatingDto rating, int pointDiff)
    {
        var updatePlayerDto = new UpdatePlayerRatingDto
        {
            PlayerRating = rating,
            RatingDifference = pointDiff
        };
        await _mediator.Send(new UpdatePlayerRatingMongoCommand(updatePlayerDto));
    }
}