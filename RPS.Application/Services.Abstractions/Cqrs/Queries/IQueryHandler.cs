using MediatR;
using RPS.Application.Dto.MediatR;

namespace RPS.Application.Services.Abstractions.Cqrs.Queries;

public interface IQueryHandler<in TQuery, TResponse>: IRequestHandler<TQuery, Result<TResponse>> where TQuery: IQuery<TResponse> 
{
}
