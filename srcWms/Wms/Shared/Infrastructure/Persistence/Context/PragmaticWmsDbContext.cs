using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Wms.Domain.Entities.AccessControl;
using Wms.Domain.Entities.Communications;
using Wms.Domain.Entities.Common;
using Wms.Domain.Entities.Customer;
using Wms.Domain.Entities.Definitions;
using Wms.Domain.Entities.GoodsReceipt;
using Wms.Domain.Entities.Identity;
using Wms.Domain.Entities.InventoryCount;
using Wms.Domain.Entities.Package;
using Wms.Domain.Entities.Production;
using Wms.Domain.Entities.ProductionTransfer;
using Wms.Domain.Entities.ServiceAllocation;
using Wms.Domain.Entities.Shipping;
using Wms.Domain.Entities.Stock;
using Wms.Domain.Entities.SubcontractingIssueTransfer;
using Wms.Domain.Entities.SubcontractingReceiptTransfer;
using Wms.Domain.Entities.Warehouse;
using Wms.Domain.Entities.WarehouseInbound;
using Wms.Domain.Entities.WarehouseOutbound;
using Wms.Domain.Entities.WarehouseTransfer;
using Wms.Domain.Entities.YapKod;

namespace Wms.Infrastructure.Persistence.Context;

/// <summary>
/// İlk pragmatik vertical slice için sadece definitions/parameter tablolarını açar.
/// `_old` WmsDbContext davranışını modül modül taşıma stratejisinin ilk adımıdır.
/// </summary>
public sealed class WmsDbContext : DbContext
{
    public WmsDbContext(DbContextOptions<WmsDbContext> options)
        : base(options)
    {
    }

    public DbSet<GrParameter> GrParameters => Set<GrParameter>();
    public DbSet<IcParameter> IcParameters => Set<IcParameter>();
    public DbSet<IcHeader> IcHeaders => Set<IcHeader>();
    public DbSet<IcScope> IcScopes => Set<IcScope>();
    public DbSet<IcLine> IcLines => Set<IcLine>();
    public DbSet<IcCountEntry> IcCountEntries => Set<IcCountEntry>();
    public DbSet<IcAdjustment> IcAdjustments => Set<IcAdjustment>();
    public DbSet<IcImportLine> IcImportLines => Set<IcImportLine>();
    public DbSet<IcRoute> IcRoutes => Set<IcRoute>();
    public DbSet<IcTerminalLine> IcTerminalLines => Set<IcTerminalLine>();
    public DbSet<PParameter> PParameters => Set<PParameter>();
    public DbSet<PrParameter> PrParameters => Set<PrParameter>();
    public DbSet<PtParameter> PtParameters => Set<PtParameter>();
    public DbSet<ShParameter> ShParameters => Set<ShParameter>();
    public DbSet<SitParameter> SitParameters => Set<SitParameter>();
    public DbSet<SrtParameter> SrtParameters => Set<SrtParameter>();
    public DbSet<WiParameter> WiParameters => Set<WiParameter>();
    public DbSet<WoParameter> WoParameters => Set<WoParameter>();
    public DbSet<WtParameter> WtParameters => Set<WtParameter>();
    public DbSet<UserAuthority> UserAuthorities => Set<UserAuthority>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserSession> UserSessions => Set<UserSession>();
    public DbSet<UserDetail> UserDetails => Set<UserDetail>();
    public DbSet<PasswordResetRequest> PasswordResetRequests => Set<PasswordResetRequest>();
    public DbSet<PermissionDefinition> PermissionDefinitions => Set<PermissionDefinition>();
    public DbSet<PermissionGroup> PermissionGroups => Set<PermissionGroup>();
    public DbSet<PermissionGroupPermission> PermissionGroupPermissions => Set<PermissionGroupPermission>();
    public DbSet<UserPermissionGroup> UserPermissionGroups => Set<UserPermissionGroup>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<SmtpSetting> SmtpSettings => Set<SmtpSetting>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<GrHeader> GrHeaders => Set<GrHeader>();
    public DbSet<GrLine> GrLines => Set<GrLine>();
    public DbSet<GrImportLine> GrImportLines => Set<GrImportLine>();
    public DbSet<GrRoute> GrRoutes => Set<GrRoute>();
    public DbSet<GrLineSerial> GrLineSerials => Set<GrLineSerial>();
    public DbSet<GrTerminalLine> GrTerminalLines => Set<GrTerminalLine>();
    public DbSet<GrImportDocument> GrImportDocuments => Set<GrImportDocument>();
    public DbSet<PHeader> PHeaders => Set<PHeader>();
    public DbSet<PPackage> PPackages => Set<PPackage>();
    public DbSet<PLine> PLines => Set<PLine>();
    public DbSet<WtHeader> WtHeaders => Set<WtHeader>();
    public DbSet<WtLine> WtLines => Set<WtLine>();
    public DbSet<WtImportLine> WtImportLines => Set<WtImportLine>();
    public DbSet<WtRoute> WtRoutes => Set<WtRoute>();
    public DbSet<WtLineSerial> WtLineSerials => Set<WtLineSerial>();
    public DbSet<WtTerminalLine> WtTerminalLines => Set<WtTerminalLine>();
    public DbSet<WoHeader> WoHeaders => Set<WoHeader>();
    public DbSet<WoLine> WoLines => Set<WoLine>();
    public DbSet<WoImportLine> WoImportLines => Set<WoImportLine>();
    public DbSet<WoRoute> WoRoutes => Set<WoRoute>();
    public DbSet<WoLineSerial> WoLineSerials => Set<WoLineSerial>();
    public DbSet<WoTerminalLine> WoTerminalLines => Set<WoTerminalLine>();
    public DbSet<WiHeader> WiHeaders => Set<WiHeader>();
    public DbSet<WiLine> WiLines => Set<WiLine>();
    public DbSet<WiImportLine> WiImportLines => Set<WiImportLine>();
    public DbSet<WiRoute> WiRoutes => Set<WiRoute>();
    public DbSet<WiLineSerial> WiLineSerials => Set<WiLineSerial>();
    public DbSet<WiTerminalLine> WiTerminalLines => Set<WiTerminalLine>();
    public DbSet<ShHeader> ShHeaders => Set<ShHeader>();
    public DbSet<ShLine> ShLines => Set<ShLine>();
    public DbSet<ShImportLine> ShImportLines => Set<ShImportLine>();
    public DbSet<ShRoute> ShRoutes => Set<ShRoute>();
    public DbSet<ShLineSerial> ShLineSerials => Set<ShLineSerial>();
    public DbSet<ShTerminalLine> ShTerminalLines => Set<ShTerminalLine>();
    public DbSet<PrHeader> PrHeaders => Set<PrHeader>();
    public DbSet<PrHeaderAssignment> PrHeaderAssignments => Set<PrHeaderAssignment>();
    public DbSet<PrOrder> PrOrders => Set<PrOrder>();
    public DbSet<PrOrderDependency> PrOrderDependencies => Set<PrOrderDependency>();
    public DbSet<PrOrderAssignment> PrOrderAssignments => Set<PrOrderAssignment>();
    public DbSet<PrOrderOutput> PrOrderOutputs => Set<PrOrderOutput>();
    public DbSet<PrOrderConsumption> PrOrderConsumptions => Set<PrOrderConsumption>();
    public DbSet<PrOperation> PrOperations => Set<PrOperation>();
    public DbSet<PrOperationLine> PrOperationLines => Set<PrOperationLine>();
    public DbSet<PrOperationEvent> PrOperationEvents => Set<PrOperationEvent>();
    public DbSet<PrLine> PrLines => Set<PrLine>();
    public DbSet<PrImportLine> PrImportLines => Set<PrImportLine>();
    public DbSet<PrRoute> PrRoutes => Set<PrRoute>();
    public DbSet<PrLineSerial> PrLineSerials => Set<PrLineSerial>();
    public DbSet<PrTerminalLine> PrTerminalLines => Set<PrTerminalLine>();
    public DbSet<PtHeader> PtHeaders => Set<PtHeader>();
    public DbSet<PtLine> PtLines => Set<PtLine>();
    public DbSet<PtImportLine> PtImportLines => Set<PtImportLine>();
    public DbSet<PtRoute> PtRoutes => Set<PtRoute>();
    public DbSet<PtLineSerial> PtLineSerials => Set<PtLineSerial>();
    public DbSet<PtTerminalLine> PtTerminalLines => Set<PtTerminalLine>();
    public DbSet<ServiceCase> ServiceCases => Set<ServiceCase>();
    public DbSet<ServiceCaseLine> ServiceCaseLines => Set<ServiceCaseLine>();
    public DbSet<OrderAllocationLine> OrderAllocationLines => Set<OrderAllocationLine>();
    public DbSet<BusinessDocumentLink> BusinessDocumentLinks => Set<BusinessDocumentLink>();
    public DbSet<SitHeader> SitHeaders => Set<SitHeader>();
    public DbSet<SitLine> SitLines => Set<SitLine>();
    public DbSet<SitImportLine> SitImportLines => Set<SitImportLine>();
    public DbSet<SitRoute> SitRoutes => Set<SitRoute>();
    public DbSet<SitLineSerial> SitLineSerials => Set<SitLineSerial>();
    public DbSet<SitTerminalLine> SitTerminalLines => Set<SitTerminalLine>();
    public DbSet<SrtHeader> SrtHeaders => Set<SrtHeader>();
    public DbSet<SrtLine> SrtLines => Set<SrtLine>();
    public DbSet<SrtImportLine> SrtImportLines => Set<SrtImportLine>();
    public DbSet<SrtRoute> SrtRoutes => Set<SrtRoute>();
    public DbSet<SrtLineSerial> SrtLineSerials => Set<SrtLineSerial>();
    public DbSet<SrtTerminalLine> SrtTerminalLines => Set<SrtTerminalLine>();
    public DbSet<JobFailureLog> JobFailureLogs => Set<JobFailureLog>();
    public DbSet<PrHeaderSerial> PrHeaderSerials => Set<PrHeaderSerial>();
    public DbSet<Stock> Stocks => Set<Stock>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<YapKod> YapKodlar => Set<YapKod>();
    public DbSet<BarcodeDefinitionRecord> BarcodeDefinitions => Set<BarcodeDefinitionRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WmsDbContext).Assembly);
    }

    public override int SaveChanges()
    {
        ApplyBranchCodes();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ApplyBranchCodes();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyBranchCodes();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        ApplyBranchCodes();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void ApplyBranchCodes()
    {
        var entries = ChangeTracker.Entries()
            .Where(x => x.State is EntityState.Added or EntityState.Modified)
            .Where(x => x.Entity is IBranchScopedEntity)
            .ToList();

        foreach (var entry in entries)
        {
            if (entry.Entity is BaseHeaderEntity headerEntity)
            {
                headerEntity.BranchCode = NormalizeBranchCode(headerEntity.BranchCode);
                continue;
            }

            var scopedEntity = (IBranchScopedEntity)entry.Entity;
            var resolvedBranchCode = ResolveBranchCodeFromHeader(entry) ?? scopedEntity.BranchCode;
            scopedEntity.BranchCode = NormalizeBranchCode(resolvedBranchCode);
        }
    }

    private string? ResolveBranchCodeFromHeader(EntityEntry entry)
    {
        var headerNavigation = entry.Metadata.GetNavigations()
            .FirstOrDefault(x => typeof(BaseHeaderEntity).IsAssignableFrom(x.TargetEntityType.ClrType));

        if (headerNavigation != null)
        {
            var navigationEntry = entry.Navigation(headerNavigation.Name);
            if (navigationEntry.CurrentValue is BaseHeaderEntity trackedHeader)
            {
                return trackedHeader.BranchCode;
            }
        }

        var headerIdProperty = entry.Metadata.FindProperty("HeaderId");
        if (headerIdProperty?.ClrType != typeof(long) && headerIdProperty?.ClrType != typeof(long?))
        {
            return null;
        }

        var headerIdValue = entry.Property("HeaderId").CurrentValue;
        if (headerIdValue == null)
        {
            return null;
        }

        var headerEntityType = headerNavigation?.TargetEntityType.ClrType;
        if (headerEntityType == null)
        {
            return null;
        }

        var trackedHeaderEntry = ChangeTracker.Entries()
            .FirstOrDefault(x => x.Metadata.ClrType == headerEntityType && x.Property("Id").CurrentValue?.Equals(headerIdValue) == true);

        if (trackedHeaderEntry?.Entity is BaseHeaderEntity trackedHeaderEntity)
        {
            return trackedHeaderEntity.BranchCode;
        }

        if (Find(headerEntityType, headerIdValue) is BaseHeaderEntity persistedHeaderEntity)
        {
            return persistedHeaderEntity.BranchCode;
        }

        return null;
    }

    private static string NormalizeBranchCode(string? branchCode)
    {
        return string.IsNullOrWhiteSpace(branchCode) ? "0" : branchCode.Trim();
    }
}
