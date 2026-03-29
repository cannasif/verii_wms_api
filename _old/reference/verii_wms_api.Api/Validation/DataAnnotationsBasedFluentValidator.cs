using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace WMS_WEBAPI.Validation;

public sealed class DataAnnotationsBasedFluentValidator<T> : AbstractValidator<T> where T : class
{
    public DataAnnotationsBasedFluentValidator()
    {
        RuleFor(x => x).Custom((instance, context) =>
        {
            if (instance is null)
            {
                return;
            }

            var results = new List<ValidationResult>();
            var dataAnnotationsContext = new ValidationContext(instance);

            // Uses attribute-based rules ([Required], [EmailAddress], etc.) to produce FluentValidation failures.
            // This is a transitional approach so we can centralize validation pipeline in FluentValidation.
            Validator.TryValidateObject(
                instance,
                dataAnnotationsContext,
                results,
                validateAllProperties: true);

            foreach (var result in results)
            {
                var message = result.ErrorMessage ?? "Validation error";
                if (result.MemberNames is { } members && members.Any())
                {
                    foreach (var member in members)
                    {
                        context.AddFailure(member, message);
                    }
                }
                else
                {
                    context.AddFailure(message);
                }
            }
        });
    }
}

