using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class MobilemenuLineService : IMobilemenuLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public MobilemenuLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<MobilemenuLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.MobilemenuLines.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<MobilemenuLineDto>>(entities);
                return ApiResponse<IEnumerable<MobilemenuLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("MobilemenuLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("MobilemenuLineErrorOccurred");
                return ApiResponse<IEnumerable<MobilemenuLineDto>>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<MobilemenuLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.MobilemenuLines.GetByIdAsync(id);
                if (entity == null)
                {
                    var message = _localizationService.GetLocalizedString("MobilemenuLineNotFound");
                    return ApiResponse<MobilemenuLineDto>.ErrorResult(message, _localizationService.GetLocalizedString("MobilemenuLineNotFound"), 404);
                }

                var dto = _mapper.Map<MobilemenuLineDto>(entity);
                return ApiResponse<MobilemenuLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("MobilemenuLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("MobilemenuLineErrorOccurred");
                return ApiResponse<MobilemenuLineDto>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<MobilemenuLineDto>> GetByItemIdAsync(string itemId)
        {
            try
            {
                var entities = await _unitOfWork.MobilemenuLines.FindAsync(x => x.ItemId == itemId && !x.IsDeleted);
                var entity = entities.FirstOrDefault();
                
                if (entity == null)
                {
                    var message = _localizationService.GetLocalizedString("MobilemenuLineNotFound");
                    return ApiResponse<MobilemenuLineDto>.ErrorResult(message, _localizationService.GetLocalizedString("MobilemenuLineNotFound"), 404);
                }

                var dto = _mapper.Map<MobilemenuLineDto>(entity);
                return ApiResponse<MobilemenuLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("MobilemenuLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("MobilemenuLineErrorOccurred");
                return ApiResponse<MobilemenuLineDto>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<MobilemenuLineDto>>> GetByHeaderIdAsync(int headerId)
        {
            try
            {
                var entities = await _unitOfWork.MobilemenuLines.FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<MobilemenuLineDto>>(entities);
                return ApiResponse<IEnumerable<MobilemenuLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("MobilemenuLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("MobilemenuLineErrorOccurred");
                return ApiResponse<IEnumerable<MobilemenuLineDto>>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<MobilemenuLineDto>>> GetByTitleAsync(string title)
        {
            try
            {
                var entities = await _unitOfWork.MobilemenuLines.FindAsync(x => x.Title.Contains(title) && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<MobilemenuLineDto>>(entities);
                return ApiResponse<IEnumerable<MobilemenuLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("MobilemenuLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("MobilemenuLineErrorOccurred");
                return ApiResponse<IEnumerable<MobilemenuLineDto>>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<MobilemenuLineDto>> CreateAsync(CreateMobilemenuLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<MobilemenuLine>(createDto);
                entity.CreatedDate = DateTime.UtcNow;

                await _unitOfWork.MobilemenuLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<MobilemenuLineDto>(entity);
                return ApiResponse<MobilemenuLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("MobilemenuLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("MobilemenuLineErrorOccurred");
                return ApiResponse<MobilemenuLineDto>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<MobilemenuLineDto>> UpdateAsync(long id, UpdateMobilemenuLineDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.MobilemenuLines.GetByIdAsync(id);
                if (entity == null)
                {
                    var message = _localizationService.GetLocalizedString("MobilemenuLineNotFound");
                    return ApiResponse<MobilemenuLineDto>.ErrorResult(message, _localizationService.GetLocalizedString("MobilemenuLineNotFound"), 404);
                }

                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.UtcNow;

                _unitOfWork.MobilemenuLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<MobilemenuLineDto>(entity);
                return ApiResponse<MobilemenuLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("MobilemenuLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("MobilemenuLineErrorOccurred");
                return ApiResponse<MobilemenuLineDto>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.MobilemenuLines.ExistsAsync(id);
                if (!exists)
                {
                    var message = _localizationService.GetLocalizedString("MobilemenuLineNotFound");
                    return ApiResponse<bool>.ErrorResult(message, _localizationService.GetLocalizedString("MobilemenuLineNotFound"), 404);
                }

                await _unitOfWork.MobilemenuLines.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("MobilemenuLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                var message = _localizationService.GetLocalizedString("MobilemenuLineErrorOccurred");
                return ApiResponse<bool>.ErrorResult(message, ex.Message ?? string.Empty, 500);
            }
        }

    }
}
