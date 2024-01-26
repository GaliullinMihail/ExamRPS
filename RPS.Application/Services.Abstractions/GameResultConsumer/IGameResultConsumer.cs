using RPS.Shared.GameResult;
using MassTransit;

namespace RPS.Application.Services.Abstractions.GameResultConsumer;

public interface IGameResultConsumer : IConsumer<GameResultDto>
{
}