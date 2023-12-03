using CoinFlipper.Notification.Application.Services.Email;
using CoinFlipper.ServiceDefaults.Application.Commands;
using FluentValidation;

namespace CoinFlipper.Notification.Application.Commands.Email.Handlers;

public class SendFeedbackEmailHandler(
    IApplicationEmailSender applicationEmailSender
    ) : ICommandHandler<SendFeedbackEmailRequest>
{
    public async Task HandleAsync(SendFeedbackEmailRequest command, CancellationToken cancellationToken = default)
    {
        await applicationEmailSender.SendFeedbackEmailAsync(command.DisplayName, command.Email, command.FeedbackContent);
    }
}

public class SendFeedbackEmailRequestValidator : AbstractValidator<SendFeedbackEmailRequest>
{
    public SendFeedbackEmailRequestValidator()
    {
        RuleFor(request => request.Email)
            .EmailAddress();

        RuleFor(request => request.FeedbackContent)
            .NotEmpty();
    }
}