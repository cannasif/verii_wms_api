using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;
using System.Linq;

namespace WMS_WEBAPI.Services
{
    public class SrtTerminalLineService : ISrtTerminalLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        public SrtTerminalLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IRequestCancellationAccessor requestCancellationAccessor)
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


        public async Task<ApiResponse<IEnumerable<SrtTerminalLineDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
            try
            {
                var entities = await _unitOfWork.SrtTerminalLines.Query().ToListAsync(requestCancellationToken);
                var dtos = _mapper.Map<IEnumerable<SrtTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<SrtTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<SrtTerminalLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
            try
            {
                request ??= new PagedRequest();
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 20;

                var allResult = await GetAllAsync();
                if (!allResult.Success || allResult.Data == null)
                {
                    return ApiResponse<PagedResponse<SrtTerminalLineDto>>.ErrorResult(
                        allResult.Message,
                        allResult.ExceptionMessage,
                        allResult.StatusCode);
                }

                var query = allResult.Data.AsQueryable();
                query = query.ApplyFilters(request.Filters, request.FilterLogic);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = query.Count();
                var items = query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToList();

                var result = new PagedResponse<SrtTerminalLineDto>(items, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResponse<SrtTerminalLineDto>>.SuccessResult(result, allResult.Message);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<SrtTerminalLineDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("Error_GetAll"),
                    ex.Message ?? string.Empty,
                    500);
            }
        }


        public async Task<ApiResponse<SrtTerminalLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
            try
            {
                var entity = await _unitOfWork.SrtTerminalLines.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync(requestCancellationToken);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<SrtTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("SrtTerminalLineNotFound"), _localizationService.GetLocalizedString("SrtTerminalLineNotFound"), 404);
                }
                var dto = _mapper.Map<SrtTerminalLineDto>(entity);
                return ApiResponse<SrtTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("SrtTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SrtTerminalLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
            try
            {
                var entities = await _unitOfWork.SrtTerminalLines.Query().Where(x => x.HeaderId == headerId).ToListAsync(requestCancellationToken);
                var dtos = _mapper.Map<IEnumerable<SrtTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<SrtTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SrtTerminalLineDto>>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
            try
            {
                var entities = await _unitOfWork.SrtTerminalLines.Query().Where(x => x.TerminalUserId == userId).ToListAsync(requestCancellationToken);
                var dtos = _mapper.Map<IEnumerable<SrtTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<SrtTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SrtTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SrtTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }


        public async Task<ApiResponse<SrtTerminalLineDto>> CreateAsync(CreateSrtTerminalLineDto createDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
            try
            {
                var entity = _mapper.Map<SrtTerminalLine>(createDto);
                await _unitOfWork.SrtTerminalLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync(requestCancellationToken);
                var dto = _mapper.Map<SrtTerminalLineDto>(entity);
                return ApiResponse<SrtTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtTerminalLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("SrtTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<SrtTerminalLineDto>> UpdateAsync(long id, UpdateSrtTerminalLineDto updateDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
            try
            {
                var entity = await _unitOfWork.SrtTerminalLines.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync(requestCancellationToken);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<SrtTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("SrtTerminalLineNotFound"), _localizationService.GetLocalizedString("SrtTerminalLineNotFound"), 404);
                }
                _mapper.Map(updateDto, entity);
                _unitOfWork.SrtTerminalLines.Update(entity);
                await _unitOfWork.SaveChangesAsync(requestCancellationToken);
                var dto = _mapper.Map<SrtTerminalLineDto>(entity);
                return ApiResponse<SrtTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtTerminalLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SrtTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("SrtTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
            try
            {
                await _unitOfWork.SrtTerminalLines.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync(requestCancellationToken);
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SrtTerminalLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SrtTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
