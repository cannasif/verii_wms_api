using FluentValidation;
using Wms.Application.AccessControl.Dtos;

namespace Wms.Application.AccessControl.Validators;

public sealed class CreatePermissionDefinitionDtoValidator : AbstractValidator<CreatePermissionDefinitionDto>
{
    public CreatePermissionDefinitionDtoValidator()
    {
        RuleFor(x => x.Code).NotEmpty().MaximumLength(120);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Description).MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}

public sealed class UpdatePermissionDefinitionDtoValidator : AbstractValidator<UpdatePermissionDefinitionDto>
{
    public UpdatePermissionDefinitionDtoValidator()
    {
        RuleFor(x => x.Code).MaximumLength(120).When(x => !string.IsNullOrWhiteSpace(x.Code));
        RuleFor(x => x.Name).MaximumLength(150).When(x => !string.IsNullOrWhiteSpace(x.Name));
        RuleFor(x => x.Description).MaximumLength(500).When(x => x.Description != null);
    }
}

public sealed class SyncPermissionDefinitionsDtoValidator : AbstractValidator<SyncPermissionDefinitionsDto>
{
    public SyncPermissionDefinitionsDtoValidator()
    {
        RuleFor(x => x.Items).NotNull();
        RuleForEach(x => x.Items).SetValidator(new SyncPermissionDefinitionItemDtoValidator());
    }
}

public sealed class SyncPermissionDefinitionItemDtoValidator : AbstractValidator<SyncPermissionDefinitionItemDto>
{
    public SyncPermissionDefinitionItemDtoValidator()
    {
        RuleFor(x => x.Code).NotEmpty().MaximumLength(120);
        RuleFor(x => x.Name).MaximumLength(150).When(x => !string.IsNullOrWhiteSpace(x.Name));
        RuleFor(x => x.Description).MaximumLength(500).When(x => x.Description != null);
    }
}

public sealed class CreatePermissionGroupDtoValidator : AbstractValidator<CreatePermissionGroupDto>
{
    public CreatePermissionGroupDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}

public sealed class UpdatePermissionGroupDtoValidator : AbstractValidator<UpdatePermissionGroupDto>
{
    public UpdatePermissionGroupDtoValidator()
    {
        RuleFor(x => x.Name).MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.Name));
        RuleFor(x => x.Description).MaximumLength(500).When(x => x.Description != null);
    }
}

public sealed class SetPermissionGroupPermissionsDtoValidator : AbstractValidator<SetPermissionGroupPermissionsDto>
{
    public SetPermissionGroupPermissionsDtoValidator()
    {
        RuleFor(x => x.PermissionDefinitionIds).NotNull();
    }
}

public sealed class SetUserPermissionGroupsDtoValidator : AbstractValidator<SetUserPermissionGroupsDto>
{
    public SetUserPermissionGroupsDtoValidator()
    {
        RuleFor(x => x.PermissionGroupIds).NotNull();
    }
}
