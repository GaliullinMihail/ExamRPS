using RPS.Application.Features.Chat.AddChat;
using FluentValidation;

namespace Application.Chat.AddChat;

public class AddChatCommandValidator : AbstractValidator<AddChatCommand>
{
    public AddChatCommandValidator()
    {
        RuleFor(c => c.FirstUserId).NotNull();

        RuleFor(c => c.SecondUserId).NotNull();
    }
}