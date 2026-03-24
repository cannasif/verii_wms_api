using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class ShTerminalLineService : IShTerminalLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        public ShTerminalLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IRequestCancellationAccessor requestCancellationAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _requestCancellationAccessor = requestCancellationAccessor;
        }
        private CancellationToken ResolveCancellationToken(CancellationToken token = default)
        {
            return _requestCancellationAccessor.Get(token);
        }
        private CancellationToken RequestCancellationToken => ResolveCancellationToken();


        public async Task<ApiResponse<IEnumerable<ShTerminalLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
            try
            {
                var entities = await _unitOfWork.ShTerminalLines.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<ShTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<ShTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("ShTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<ShTerminalLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
            try
            {
                request ??= new PagedRequest();
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 20;

                var query = _unitOfWork.ShTerminalLines.Query();
                query = query.ApplyFilters(request.Filters, request.FilterLogic);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync(requestCancellationToken);
                var entities = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync(requestCancellationToken);

                var dtos = _mapper.Map<List<ShTerminalLineDto>>(entities);
                var result = new PagedResponse<ShTerminalLineDto>(dtos, totalCount, request.PageNumber, request.PageSize);

                return ApiResponse<PagedResponse<ShTerminalLineDto>>.SuccessResult(
                    result,
                    _localizationService.GetLocalizedString("ShTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<ShTerminalLineDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("ShTerminalLineErrorOccurred"),
                    ex.Message ?? string.Empty,
                    500);
            }
        }


        public async Task<ApiResponse<ShTerminalLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
            try
            {
                var entity = await _unitOfWork.ShTerminalLines.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync(requestCancellationToken);
                if (entity == null) { var nf = _localizationService.GetLocalizedString("ShTerminalLineNotFound"); return ApiResponse<ShTerminalLineDto>.ErrorResult(nf, nf, 404); }
                var dto = _mapper.Map<ShTerminalLineDto>(entity);
                return ApiResponse<ShTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("ShTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<ShTerminalLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
            try
            {
                var entities = await _unitOfWork.ShTerminalLines.FindAsync(x => x.HeaderId == headerId);
                var dtos = _mapper.Map<IEnumerable<ShTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<ShTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("ShTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<ShTerminalLineDto>>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
            try
            {
                var entities = await _unitOfWork.ShTerminalLines.FindAsync(x => x.TerminalUserId == userId);
                var dtos = _mapper.Map<IEnumerable<ShTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<ShTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("ShTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }


        public async Task<ApiResponse<ShTerminalLineDto>> CreateAsync(CreateShTerminalLineDto createDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
            try
            {
                var entity = _mapper.Map<ShTerminalLine>(createDto);
                var created = await _unitOfWork.ShTerminalLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync(requestCancellationToken);
                var dto = _mapper.Map<ShTerminalLineDto>(created);
                return ApiResponse<ShTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShTerminalLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("ShTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShTerminalLineDto>> UpdateAsync(long id, UpdateShTerminalLineDto updateDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
            try
            {
                var existing = await _unitOfWork.ShTerminalLines.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync(requestCancellationToken);
                if (existing == null) { var nf = _localizationService.GetLocalizedString("ShTerminalLineNotFound"); return ApiResponse<ShTerminalLineDto>.ErrorResult(nf, nf, 404); }
                var entity = _mapper.Map(updateDto, existing);
                _unitOfWork.ShTerminalLines.Update(entity);
                await _unitOfWork.SaveChangesAsync(requestCancellationToken);
                var dto = _mapper.Map<ShTerminalLineDto>(entity);
                return ApiResponse<ShTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShTerminalLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("ShTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
            try
            {
                await _unitOfWork.ShTerminalLines.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync(requestCancellationToken);
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("ShTerminalLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("ShTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
