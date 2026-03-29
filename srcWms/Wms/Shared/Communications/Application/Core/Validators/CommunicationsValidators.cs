using FluentValidation;
using Wms.Application.Communications.Dtos;

namespace Wms.Application.Communications.Validators;

public sealed class UpdateSmtpSettingsDtoValidator : AbstractValidator<UpdateSmtpSettingsDto>
{
    public UpdateSmtpSettingsDtoValidator()
    {
        RuleFor(x => x.Host).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Port).InclusiveBetween(1, 65535);
        RuleFor(x => x.Username).MaximumLength(200);
        RuleFor(x => x.Password).MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.Password));
        RuleFor(x => x.FromEmail).NotEmpty().MaximumLength(200).EmailAddress();
        RuleFor(x => x.FromName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Timeout).InclusiveBetween(1, 300);
    }
}

public sealed class SendTestMailDtoValidator : AbstractValidator<SendTestMailDto>
{
    public SendTestMailDtoValidator()
    {
        RuleFor(x => x.To).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.To));
    }
}

public sealed class CreateNotificationDtoValidator : AbstractValidator<CreateNotificationDto>
{
    public CreateNotificationDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Message).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.Channel).IsInEnum();
        RuleFor(x => x.RelatedEntityType).MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.RelatedEntityType));
        RuleFor(x => x.ActionUrl).MaximumLength(250).When(x => !string.IsNullOrWhiteSpace(x.ActionUrl));
        RuleFor(x => x.TerminalActionCode).MaximumLength(50).When(x => !string.IsNullOrWhiteSpace(x.TerminalActionCode));
    }
}
