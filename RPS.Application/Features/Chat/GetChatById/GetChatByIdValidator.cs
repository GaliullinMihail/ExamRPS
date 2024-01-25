using RPS.Application.Features.Chat.GetChatById;
using FluentValidation;

namespace Application.Chat.GetChatById;

public class GetChatByIdValidator : AbstractValidator<GetChatByIdQuery>
{
    public GetChatByIdValidator()
    {
        RuleFor(q => q.UserId).NotNull();

        RuleFor(q => q.CurrUserId).NotNull();
    }
}