using MediatR;
using RPS.Application.Dto.MediatR;

namespace RPS.Application.Services.Abstractions.Cqrs.Queries;

public interface IQuery<TResponse>: IRequest<Result<TResponse>>
{
}