using Hangfire;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services.Jobs
{
    [DisableConcurrentExecution(timeoutInSeconds: 300)]
        [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 60, 120, 300 })]
    public class CustomerSyncJob : ICustomerSyncJob
    {
        private const string RecurringJobId = "erp-customer-sync-job";
        private readonly IErpUnitOfWork _erpUnitOfWork;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CustomerSyncJob> _logger;

        public CustomerSyncJob(
            IErpUnitOfWork erpUnitOfWork,
            IUnitOfWork unitOfWork,
            ILogger<CustomerSyncJob> logger)
        {
            _erpUnitOfWork = erpUnitOfWork;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("Customer sync started.");

            List<RII_VW_CARI> erpCustomers;
            try
            {
                erpCustomers = await _erpUnitOfWork.Query<RII_VW_CARI>()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                await LogRecordFailureAsync("ERP_FETCH", ex);
                _logger.LogWarning(ex, "Customer sync aborted: ERP fetch failed.");
                return;
            }

            if (erpCustomers.Count == 0)
            {
                _logger.LogInformation("Customer sync skipped: no ERP records returned.");
                return;
            }

            var createdCount = 0;
            var updatedCount = 0;
            var reactivatedCount = 0;
            var skippedCount = 0;
            var failedCount = 0;
            var duplicatePayloadCount = 0;
            var processedCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var erpCustomer in erpCustomers)
            {
                var code = erpCustomer.CARI_KOD?.Trim() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(code))
                {
                    skippedCount++;
                    continue;
                }

                if (!processedCodes.Add(code))
                {
                    duplicatePayloadCount++;
                    continue;
                }

                try
                {
                    var customer = await _unitOfWork.Customers.Query(tracking: true, ignoreQueryFilters: true)
                        .Where(x => x.CustomerCode == code)
                        .FirstOrDefaultAsync();

                    var customerName = string.IsNullOrWhiteSpace(erpCustomer.CARI_ISIM) ? code : erpCustomer.CARI_ISIM!.Trim();
                    var taxOffice = EmptyToNull(erpCustomer.VERGI_DAIRESI);
                    var taxNumber = EmptyToNull(erpCustomer.VERGI_NUMARASI);
                    var tcknNumber = EmptyToNull(erpCustomer.TCKIMLIKNO);
                    var email = EmptyToNull(erpCustomer.EMAIL);
                    var website = EmptyToNull(erpCustomer.WEB);
                    var phone1 = EmptyToNull(erpCustomer.CARI_TEL);
                    var phone2 = EmptyToNull(erpCustomer.CARI_TEL2);
                    var address = EmptyToNull(erpCustomer.CARI_ADRES);
                    var city = EmptyToNull(erpCustomer.CARI_IL);
                    var district = EmptyToNull(erpCustomer.CARI_ILCE);
                    var countryCode = EmptyToNull(erpCustomer.ULKE_KODU);
                    var salesRepCode = EmptyToNull(erpCustomer.PLASIYER_KODU);
                    var groupCode = EmptyToNull(erpCustomer.GRUP_KODU);
                    var branchCode = erpCustomer.SUBE_KODU;
                    var businessUnitCode = erpCustomer.ISLETME_KODU;
                    var creditLimit = erpCustomer.RISK_SINIRI;

                    if (customer == null)
                    {
                        await _unitOfWork.Customers.AddAsync(new Customer
                        {
                            CustomerCode = code,
                            CustomerName = customerName,
                            TaxOffice = taxOffice,
                            TaxNumber = taxNumber,
                            TcknNumber = tcknNumber,
                            SalesRepCode = salesRepCode,
                            GroupCode = groupCode,
                            CreditLimit = creditLimit,
                            BranchCode = branchCode,
                            BusinessUnitCode = businessUnitCode,
                            Email = email,
                            Website = website,
                            Phone1 = phone1,
                            Phone2 = phone2,
                            Address = address,
                            City = city,
                            District = district,
                            CountryCode = countryCode,
                            IsErpIntegrated = true,
                            ErpIntegrationNumber = code,
                            LastSyncDate = DateTime.UtcNow,
                            CreatedDate = DateTimeProvider.Now,
                            IsDeleted = false
                        });

                        await _unitOfWork.SaveChangesAsync();
                        createdCount++;
                        continue;
                    }

                    var updated = false;
                    var reactivated = false;

                    updated |= SetIfChanged(customer, x => x.CustomerName, customerName);
                    updated |= SetIfChanged(customer, x => x.TaxOffice, taxOffice);
                    updated |= SetIfChanged(customer, x => x.TaxNumber, taxNumber);
                    updated |= SetIfChanged(customer, x => x.TcknNumber, tcknNumber);
                    updated |= SetIfChanged(customer, x => x.SalesRepCode, salesRepCode);
                    updated |= SetIfChanged(customer, x => x.GroupCode, groupCode);
                    updated |= SetIfChanged(customer, x => x.CreditLimit, creditLimit);
                    updated |= SetIfChanged(customer, x => x.BranchCode, branchCode);
                    updated |= SetIfChanged(customer, x => x.BusinessUnitCode, businessUnitCode);
                    updated |= SetIfChanged(customer, x => x.Email, email);
                    updated |= SetIfChanged(customer, x => x.Website, website);
                    updated |= SetIfChanged(customer, x => x.Phone1, phone1);
                    updated |= SetIfChanged(customer, x => x.Phone2, phone2);
                    updated |= SetIfChanged(customer, x => x.Address, address);
                    updated |= SetIfChanged(customer, x => x.City, city);
                    updated |= SetIfChanged(customer, x => x.District, district);
                    updated |= SetIfChanged(customer, x => x.CountryCode, countryCode);
                    updated |= SetIfChanged(customer, x => x.IsErpIntegrated, true);
                    updated |= SetIfChanged(customer, x => x.ErpIntegrationNumber, code);

                    if (customer.IsDeleted)
                    {
                        customer.IsDeleted = false;
                        customer.DeletedDate = null;
                        customer.DeletedBy = null;
                        updated = true;
                        reactivated = true;
                    }

                    if (!updated)
                    {
                        continue;
                    }

                    customer.UpdatedDate = DateTimeProvider.Now;
                    customer.UpdatedBy = null;
                    customer.LastSyncDate = DateTime.UtcNow;

                    await _unitOfWork.SaveChangesAsync();

                    if (reactivated)
                    {
                        reactivatedCount++;
                    }
                    else
                    {
                        updatedCount++;
                    }
                }
                catch (Exception ex)
                {
                    failedCount++;
                    await LogRecordFailureAsync(code, ex);
                    // create a clean scope on the next iteration via fresh tracked query
                }
            }

            _logger.LogInformation(
                "Customer sync completed. created={Created}, updated={Updated}, reactivated={Reactivated}, failed={Failed}, skipped={Skipped}, duplicatePayload={DuplicatePayload}.",
                createdCount,
                updatedCount,
                reactivatedCount,
                failedCount,
                skippedCount,
                duplicatePayloadCount);
        }

        private async Task LogRecordFailureAsync(string code, Exception ex)
        {
            _logger.LogError(ex, "Customer sync record failed. CustomerCode: {CustomerCode}", code);

            try
            {
                await _unitOfWork.JobFailureLogs.AddAsync(new JobFailureLog
                {
                    JobId = $"{RecurringJobId}:{code}:{DateTime.UtcNow:yyyyMMddHHmmssfff}",
                    JobName = $"{typeof(CustomerSyncJob).FullName}.ExecuteAsync",
                    FailedAt = DateTime.UtcNow,
                    Reason = $"CustomerCode={code}",
                    ExceptionType = ex.GetType().FullName,
                    ExceptionMessage = ex.Message,
                    StackTrace = ex.StackTrace?.Length > 4000 ? ex.StackTrace[..4000] : ex.StackTrace,
                    Queue = "default",
                    RetryCount = 0,
                    CreatedDate = DateTimeProvider.Now,
                    IsDeleted = false
                });

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception logEx)
            {
                _logger.LogWarning(logEx, "Customer sync failure could not be written to RII_JOB_FAILURE_LOG. CustomerCode: {CustomerCode}", code);
            }
        }

        private static string? EmptyToNull(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        private static bool SetIfChanged<TEntity, TValue>(
            TEntity entity,
            System.Linq.Expressions.Expression<Func<TEntity, TValue>> selector,
            TValue value)
        {
            if (selector.Body is not System.Linq.Expressions.MemberExpression member)
            {
                return false;
            }

            if (member.Member is not System.Reflection.PropertyInfo property)
            {
                return false;
            }

            var current = property.GetValue(entity);
            if (Equals(current, value))
            {
                return false;
            }

            property.SetValue(entity, value);
            return true;
        }
    }
}
