using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WiTerminalLineService : IWiTerminalLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public WiTerminalLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<WiTerminalLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.WiTerminalLines.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<WiTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<WiTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WiTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WiTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WiTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<WiTerminalLineDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                request ??= new PagedRequest();
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 20;

                var query = _unitOfWork.WiTerminalLines.AsQueryable().Where(x => !x.IsDeleted);
                query = query.ApplyFilters(request.Filters, request.FilterLogic);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var entities = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = _mapper.Map<List<WiTerminalLineDto>>(entities);
                var result = new PagedResponse<WiTerminalLineDto>(dtos, totalCount, request.PageNumber, request.PageSize);

                return ApiResponse<PagedResponse<WiTerminalLineDto>>.SuccessResult(
                    result,
                    _localizationService.GetLocalizedString("WiTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<WiTerminalLineDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("WiTerminalLineErrorOccurred"),
                    ex.Message ?? string.Empty,
                    500);
            }
        }


        public async Task<ApiResponse<WiTerminalLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.WiTerminalLines.GetByIdAsync(id);
                if (entity == null) return ApiResponse<WiTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("WiTerminalLineNotFound"), _localizationService.GetLocalizedString("WiTerminalLineNotFound"), 404);
                var dto = _mapper.Map<WiTerminalLineDto>(entity);
                return ApiResponse<WiTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WiTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("WiTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WiTerminalLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.WiTerminalLines.FindAsync(x => x.HeaderId == headerId);
                var dtos = _mapper.Map<IEnumerable<WiTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<WiTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WiTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WiTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WiTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WiTerminalLineDto>>> GetByUserIdAsync(long userId)
        {
            try
            {
                var entities = await _unitOfWork.WiTerminalLines.FindAsync(x => x.TerminalUserId == userId);
                var dtos = _mapper.Map<IEnumerable<WiTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<WiTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WiTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WiTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("WiTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }


        public async Task<ApiResponse<WiTerminalLineDto>> CreateAsync(CreateWiTerminalLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<WiTerminalLine>(createDto);
                var created = await _unitOfWork.WiTerminalLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<WiTerminalLineDto>(created);
                return ApiResponse<WiTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiTerminalLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WiTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("WiTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WiTerminalLineDto>> UpdateAsync(long id, UpdateWiTerminalLineDto updateDto)
        {
            try
            {
                var existing = await _unitOfWork.WiTerminalLines.GetByIdAsync(id);
                if (existing == null) return ApiResponse<WiTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("WiTerminalLineNotFound"), _localizationService.GetLocalizedString("WiTerminalLineNotFound"), 404);
                var entity = _mapper.Map(updateDto, existing);
                _unitOfWork.WiTerminalLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<WiTerminalLineDto>(entity);
                return ApiResponse<WiTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WiTerminalLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WiTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("WiTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                await _unitOfWork.WiTerminalLines.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WiTerminalLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WiTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
