using CoinFlipper.Notification.Application.Services.Email;
using CoinFlipper.ServiceDefaults.Application.Commands;
using FluentValidation;

namespace CoinFlipper.Notification.Application.Commands.Email.Handlers;

public class SendVerificationEmailHandler(IApplicationEmailSender applicationEmailSender) : ICommandHandler<SendVerificationEmailRequest>
{
    public async Task HandleAsync(SendVerificationEmailRequest command, CancellationToken cancellationToken = default)
    {
        //TODO: verificationUrl
        var verificationUrl = "https://github.com/LessIsMoreMK/CoinFlipper";
        await applicationEmailSender.SendUserVerificationEmailAsync(command.DisplayName, command.Email, verificationUrl);
    }
}

public class SendVerificationEmailRequestValidator : AbstractValidator<SendVerificationEmailRequest>
{
    public SendVerificationEmailRequestValidator()
    {
        RuleFor(request => request.Email)
            .EmailAddress()
            .NotEmpty();
    }
}