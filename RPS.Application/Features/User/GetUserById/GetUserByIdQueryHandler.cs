using Microsoft.AspNetCore.Identity;
using RPS.Application.Dto.MediatR;
using RPS.Application.Services.Abstractions.Cqrs.Queries;

namespace RPS.Application.Features.User.GetUserById;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, Domain.Entities.User?>
{
    private readonly UserManager<Domain.Entities.User> _userManager;

    public GetUserByIdQueryHandler(UserManager<Domain.Entities.User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<Domain.Entities.User?>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id);
        return new Result<Domain.Entities.User?>(user, user is null);
    }
}