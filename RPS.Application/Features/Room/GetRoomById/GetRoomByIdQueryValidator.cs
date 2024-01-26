using FluentValidation;

namespace RPS.Application.Features.Room.GetRoomById;

public class GetRoomByIdQueryValidator : AbstractValidator<GetRoomByIdQuery>
{
    public GetRoomByIdQueryValidator()
    {
        RuleFor(q => q.RoomId).NotNull();
    }
}