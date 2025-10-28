using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Connect.UI.Models.Data.Validations;

public sealed class TicketDtoValidator
    : AbstractValidator<TicketDto>
{
#region Lifecycle

    public TicketDtoValidator(
        IStringLocalizer localizer
    ) {
        RuleFor(o => o.Category)
            .NotNull().WithMessage(localizer[ErrorKeys.Required]);

        const int headingMinLength = 5;
        const int headingMaxLength = 100;
        RuleFor(o => o.Heading)
            .NotEmpty().WithMessage(localizer[ErrorKeys.Required])
            .MinimumLength(headingMinLength).WithMessage(localizer[ErrorKeys.MinLength, headingMinLength])
            .MaximumLength(headingMaxLength).WithMessage(localizer[ErrorKeys.MaxLength, headingMaxLength]);

        const int contentMinLength = 50;
        const int contentMaxLength = 500;
        RuleFor(o => o.Content)
            .NotEmpty().WithMessage(localizer[ErrorKeys.Required])
            .MinimumLength(contentMinLength).WithMessage(localizer[ErrorKeys.MinLength, contentMinLength])
            .MaximumLength(contentMaxLength).WithMessage(localizer[ErrorKeys.MaxLength, contentMaxLength]);
    }

#endregion

#region Constants

    private static class ErrorKeys
    {
        public const string Required = "form.validation.required";
        public const string MinLength = "form.validation.string.min.length";
        public const string MaxLength = "form.validation.string.max.length";
    }

#endregion
}
