using RPS.Application.Services.Abstractions.Cqrs.Queries;

namespace RPS.Application.Features.User.GetUserById;

public record GetUserByIdQuery(string Id) : IQuery<Domain.Entities.User?>;