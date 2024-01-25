using MediatR;
using RPS.Application.Dto.MediatR;

namespace RPS.Application.Services.Abstractions.Cqrs.Commands;

public interface ICommand: IRequest<Result>
{
}

public interface ICommand<TResponse>: IRequest<Result<TResponse>>
{
}