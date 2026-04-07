using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Wms.Application.Common;
using Wms.Domain.Entities.Common;
using Wms.WebApi.Options;

namespace Wms.Infrastructure.Services.Common;

public sealed class BarcodeDefinitionService : IBarcodeDefinitionService
{
    private readonly IRepository<BarcodeDefinitionRecord> _definitions;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly ILocalizationService _localizationService;
    private readonly IReadOnlyList<BarcodeDefinitionDto> _configuredDefinitions;

    public BarcodeDefinitionService(
        IRepository<BarcodeDefinitionRecord> definitions,
        IUnitOfWork unitOfWork,
        ICurrentUserAccessor currentUserAccessor,
        ILocalizationService localizationService,
        IOptions<BarcodeDefinitionsOptions> options)
    {
        _definitions = definitions;
        _unitOfWork = unitOfWork;
        _currentUserAccessor = currentUserAccessor;
        _localizationService = localizationService;
        var configured = options.Value?.Modules ?? new List<BarcodeModuleDefinitionOptions>();
        _configuredDefinitions = configured
            .Where(x => !string.IsNullOrWhiteSpace(x.ModuleKey))
            .Select(MapConfiguredDefinition)
            .ToList();
    }

    public async Task<BarcodeDefinitionDto?> GetDefinitionAsync(string moduleKey, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(moduleKey))
        {
            return null;
        }

        var branchCode = ResolveBranchCode();
        var normalizedModuleKey = moduleKey.Trim();

        var persisted = await _definitions.Query()
            .Where(x => !x.IsDeleted && x.BranchCode == branchCode && x.ModuleKey == normalizedModuleKey)
            .Select(MapEntityExpression())
            .FirstOrDefaultAsync(cancellationToken);

        if (persisted != null)
        {
            return persisted;
        }

        return _configuredDefinitions.FirstOrDefault(x => string.Equals(x.ModuleKey, normalizedModuleKey, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<BarcodeDefinitionDto?> GetDefinitionByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var branchCode = ResolveBranchCode();
        return await _definitions.Query()
            .Where(x => !x.IsDeleted && x.BranchCode == branchCode && x.Id == id)
            .Select(MapEntityExpression())
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<BarcodeDefinitionDto>> GetDefinitionsAsync(CancellationToken cancellationToken = default)
    {
        var branchCode = ResolveBranchCode();
        var persisted = await _definitions.Query()
            .Where(x => !x.IsDeleted && x.BranchCode == branchCode)
            .Select(MapEntityExpression())
            .ToListAsync(cancellationToken);

        var merged = new Dictionary<string, BarcodeDefinitionDto>(StringComparer.OrdinalIgnoreCase);

        foreach (var definition in _configuredDefinitions)
        {
            merged[definition.ModuleKey] = definition;
        }

        foreach (var definition in persisted)
        {
            merged[definition.ModuleKey] = definition;
        }

        return merged.Values
            .OrderBy(x => x.DisplayName)
            .ThenBy(x => x.ModuleKey)
            .ToList();
    }

    public async Task<ApiResponse<BarcodeDefinitionDto>> CreateAsync(SaveBarcodeDefinitionRequestDto request, CancellationToken cancellationToken = default)
    {
        var validationError = ValidateRequest(request);
        if (validationError != null)
        {
            return validationError;
        }

        var branchCode = ResolveBranchCode();
        var normalizedModuleKey = request.ModuleKey.Trim();
        var existing = await _definitions.Query()
            .Where(x => !x.IsDeleted && x.BranchCode == branchCode && x.ModuleKey == normalizedModuleKey)
            .AnyAsync(cancellationToken);

        if (existing)
        {
            var message = _localizationService.GetLocalizedString("BarcodeDefinitionAlreadyExists");
            return ApiResponse<BarcodeDefinitionDto>.ErrorResult(message, message, 409, errorCode: "BarcodeDefinitionAlreadyExists");
        }

        var normalizedPattern = request.SegmentPattern.Trim();
        var entity = new BarcodeDefinitionRecord
        {
            BranchCode = branchCode,
            ModuleKey = normalizedModuleKey,
            DisplayName = request.DisplayName.Trim(),
            DefinitionType = NormalizeDefinitionType(request.DefinitionType),
            SegmentPattern = normalizedPattern,
            RequiredSegments = NormalizeRequiredSegments(request.RequiredSegments),
            IsActive = request.IsActive,
            AllowMirrorLookup = request.AllowMirrorLookup,
            HintText = string.IsNullOrWhiteSpace(request.HintText) ? normalizedPattern : request.HintText.Trim()
        };

        await _definitions.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = MapEntity(entity);
        var successMessage = _localizationService.GetLocalizedString("BarcodeDefinitionCreatedSuccessfully");
        return ApiResponse<BarcodeDefinitionDto>.SuccessResult(dto, successMessage);
    }

    public async Task<ApiResponse<BarcodeDefinitionDto>> UpdateAsync(long id, SaveBarcodeDefinitionRequestDto request, CancellationToken cancellationToken = default)
    {
        var validationError = ValidateRequest(request);
        if (validationError != null)
        {
            return validationError;
        }

        var branchCode = ResolveBranchCode();
        var entity = await _definitions.Query(tracking: true)
            .Where(x => !x.IsDeleted && x.BranchCode == branchCode && x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            var message = _localizationService.GetLocalizedString("BarcodeDefinitionRecordNotFound");
            return ApiResponse<BarcodeDefinitionDto>.ErrorResult(message, message, 404, errorCode: "BarcodeDefinitionRecordNotFound");
        }

        var normalizedModuleKey = request.ModuleKey.Trim();
        var duplicate = await _definitions.Query()
            .Where(x => !x.IsDeleted && x.BranchCode == branchCode && x.ModuleKey == normalizedModuleKey && x.Id != id)
            .AnyAsync(cancellationToken);

        if (duplicate)
        {
            var message = _localizationService.GetLocalizedString("BarcodeDefinitionAlreadyExists");
            return ApiResponse<BarcodeDefinitionDto>.ErrorResult(message, message, 409, errorCode: "BarcodeDefinitionAlreadyExists");
        }

        var normalizedPattern = request.SegmentPattern.Trim();
        entity.ModuleKey = normalizedModuleKey;
        entity.DisplayName = request.DisplayName.Trim();
        entity.DefinitionType = NormalizeDefinitionType(request.DefinitionType);
        entity.SegmentPattern = normalizedPattern;
        entity.RequiredSegments = NormalizeRequiredSegments(request.RequiredSegments);
        entity.IsActive = request.IsActive;
        entity.AllowMirrorLookup = request.AllowMirrorLookup;
        entity.HintText = string.IsNullOrWhiteSpace(request.HintText) ? normalizedPattern : request.HintText.Trim();

        _definitions.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = MapEntity(entity);
        var successMessage = _localizationService.GetLocalizedString("BarcodeDefinitionUpdatedSuccessfully");
        return ApiResponse<BarcodeDefinitionDto>.SuccessResult(dto, successMessage);
    }

    public async Task<ApiResponse<bool>> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var branchCode = ResolveBranchCode();
        var entity = await _definitions.Query(tracking: true)
            .Where(x => !x.IsDeleted && x.BranchCode == branchCode && x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            var message = _localizationService.GetLocalizedString("BarcodeDefinitionRecordNotFound");
            return ApiResponse<bool>.ErrorResult(message, message, 404, errorCode: "BarcodeDefinitionRecordNotFound");
        }

        entity.MarkAsDeleted(_currentUserAccessor.UserId);
        _definitions.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var successMessage = _localizationService.GetLocalizedString("BarcodeDefinitionDeletedSuccessfully");
        return ApiResponse<bool>.SuccessResult(true, successMessage);
    }

    private ApiResponse<BarcodeDefinitionDto>? ValidateRequest(SaveBarcodeDefinitionRequestDto request)
    {
        if (request == null
            || string.IsNullOrWhiteSpace(request.ModuleKey)
            || string.IsNullOrWhiteSpace(request.DisplayName))
        {
            var message = _localizationService.GetLocalizedString("BarcodeDefinitionFieldsRequired");
            return ApiResponse<BarcodeDefinitionDto>.ErrorResult(message, message, 400, errorCode: "BarcodeDefinitionFieldsRequired");
        }

        if (NormalizeDefinitionType(request.DefinitionType) == BarcodeDefinitionTypes.Pattern
            && string.IsNullOrWhiteSpace(request.SegmentPattern))
        {
            var message = _localizationService.GetLocalizedString("BarcodeDefinitionPatternRequired");
            return ApiResponse<BarcodeDefinitionDto>.ErrorResult(message, message, 400, errorCode: "BarcodeDefinitionPatternRequired");
        }

        return null;
    }

    private string ResolveBranchCode()
    {
        return string.IsNullOrWhiteSpace(_currentUserAccessor.BranchCode) ? "0" : _currentUserAccessor.BranchCode!.Trim();
    }

    private static string NormalizeDefinitionType(string? definitionType)
    {
        return string.IsNullOrWhiteSpace(definitionType) ? BarcodeDefinitionTypes.Pattern : definitionType.Trim().ToLowerInvariant();
    }

    private static string NormalizeRequiredSegments(string? requiredSegments)
    {
        if (string.IsNullOrWhiteSpace(requiredSegments))
        {
            return string.Empty;
        }

        return string.Join(",",
            requiredSegments
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase));
    }

    private static BarcodeDefinitionDto MapConfiguredDefinition(BarcodeModuleDefinitionOptions options)
    {
        var segmentPattern = options.SegmentPattern?.Trim() ?? string.Empty;
        return new BarcodeDefinitionDto
        {
            ModuleKey = options.ModuleKey.Trim(),
            DisplayName = string.IsNullOrWhiteSpace(options.DisplayName) ? options.ModuleKey.Trim() : options.DisplayName.Trim(),
            DefinitionType = NormalizeDefinitionType(options.DefinitionType),
            SegmentPattern = segmentPattern,
            RequiredSegments = NormalizeRequiredSegments(options.RequiredSegments),
            IsActive = options.IsActive,
            AllowMirrorLookup = options.AllowMirrorLookup,
            HintText = string.IsNullOrWhiteSpace(options.HintText) ? segmentPattern : options.HintText.Trim(),
            Source = "config",
            IsEditable = false,
            BranchCode = "0"
        };
    }

    private static BarcodeDefinitionDto MapEntity(BarcodeDefinitionRecord entity)
    {
        return new BarcodeDefinitionDto
        {
            Id = entity.Id,
            ModuleKey = entity.ModuleKey,
            DisplayName = entity.DisplayName,
            DefinitionType = entity.DefinitionType,
            SegmentPattern = entity.SegmentPattern,
            RequiredSegments = entity.RequiredSegments,
            IsActive = entity.IsActive,
            AllowMirrorLookup = entity.AllowMirrorLookup,
            HintText = entity.HintText,
            Source = "database",
            IsEditable = true,
            BranchCode = entity.BranchCode
        };
    }

    private static System.Linq.Expressions.Expression<Func<BarcodeDefinitionRecord, BarcodeDefinitionDto>> MapEntityExpression()
    {
        return entity => new BarcodeDefinitionDto
        {
            Id = entity.Id,
            ModuleKey = entity.ModuleKey,
            DisplayName = entity.DisplayName,
            DefinitionType = entity.DefinitionType,
            SegmentPattern = entity.SegmentPattern,
            RequiredSegments = entity.RequiredSegments,
            IsActive = entity.IsActive,
            AllowMirrorLookup = entity.AllowMirrorLookup,
            HintText = entity.HintText,
            Source = "database",
            IsEditable = true,
            BranchCode = entity.BranchCode
        };
    }
}
