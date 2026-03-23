using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class CustomerMirrorService : ICustomerMirrorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerMirrorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<PagedResponse<CustomerPagedDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                request ??= new PagedRequest();
                request.Filters ??= new List<Filter>();

                var columnMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { "code", nameof(Models.Customer.CustomerCode) },
                    { "name", nameof(Models.Customer.CustomerName) }
                };

                var query = _unitOfWork.Customers.Query()
                    .ApplySearch(
                        request.Search,
                        nameof(Models.Customer.CustomerCode),
                        nameof(Models.Customer.CustomerName),
                        nameof(Models.Customer.TaxOffice),
                        nameof(Models.Customer.TaxNumber),
                        nameof(Models.Customer.TcknNumber),
                        nameof(Models.Customer.SalesRepCode),
                        nameof(Models.Customer.GroupCode),
                        nameof(Models.Customer.Email),
                        nameof(Models.Customer.Website),
                        nameof(Models.Customer.Phone1),
                        nameof(Models.Customer.Phone2),
                        nameof(Models.Customer.Address),
                        nameof(Models.Customer.City),
                        nameof(Models.Customer.District),
                        nameof(Models.Customer.CountryCode))
                    .ApplyFilters(request.Filters, request.FilterLogic, columnMapping)
                    .ApplySorting(request.SortBy ?? nameof(Models.Customer.Id), string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase), columnMapping);

                var totalCount = await query.CountAsync();
                var items = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .Select(x => new CustomerPagedDto
                    {
                        Id = x.Id,
                        CustomerCode = x.CustomerCode,
                        CustomerName = x.CustomerName,
                        TaxOffice = x.TaxOffice,
                        TaxNumber = x.TaxNumber,
                        TcknNumber = x.TcknNumber,
                        SalesRepCode = x.SalesRepCode,
                        GroupCode = x.GroupCode,
                        CreditLimit = x.CreditLimit,
                        BranchCode = x.BranchCode,
                        BusinessUnitCode = x.BusinessUnitCode,
                        Email = x.Email,
                        Website = x.Website,
                        Phone1 = x.Phone1,
                        Phone2 = x.Phone2,
                        Address = x.Address,
                        City = x.City,
                        District = x.District,
                        CountryCode = x.CountryCode,
                        IsErpIntegrated = x.IsErpIntegrated,
                        ErpIntegrationNumber = x.ErpIntegrationNumber,
                        LastSyncDate = x.LastSyncDate,
                        CreatedDate = x.CreatedDate
                    })
                    .ToListAsync();

                var paged = new PagedResponse<CustomerPagedDto>(items, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResponse<CustomerPagedDto>>.SuccessResult(paged, "Customer mirror records retrieved successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<CustomerPagedDto>>.ErrorResult("Customer mirror records could not be retrieved.", ex.Message, 500);
            }
        }
    }
}
