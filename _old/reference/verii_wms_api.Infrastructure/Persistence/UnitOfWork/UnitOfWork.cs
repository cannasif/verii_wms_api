using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using WMS_WEBAPI.Data;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.Models.UserPermissions;
using WMS_WEBAPI.Repositories;

namespace WMS_WEBAPI.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WmsDbContext _context;
        private readonly ICurrentUserService _executionContextAccessor;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;
        private bool _disposed = false;

        // Repository instances
        private IGenericRepository<User>? _users;
        private IGenericRepository<UserDetail>? _userDetails;
        private IGenericRepository<UserSession>? _userSessions;
        private IGenericRepository<PasswordResetRequest>? _passwordResetRequests;
        private IGenericRepository<BaseEntity>? _baseEntities;
        private IGenericRepository<BaseHeaderEntity>? _baseHeaderEntities;
        
        // GoodReceipt repository instances
        private IGenericRepository<GrHeader>? _grHeaders;
        private IGenericRepository<GrLine>? _grLines;
        private IGenericRepository<GrImportDocument>? _grImportDocuments;
        private IGenericRepository<GrImportLine>? _grImportLines;
        private IGenericRepository<GrLineSerial>? _grLineSerials;
        private IGenericRepository<GrRoute>? _grRoutes;
        private IGenericRepository<GrTerminalLine>? _grTerminalLines;
        
        // User and Authority repository instances
        private IGenericRepository<UserAuthority>? _userAuthorities;
        private IGenericRepository<SmtpSetting>? _smtpSettings;
        private IGenericRepository<PermissionDefinition>? _permissionDefinitions;
        private IGenericRepository<PermissionGroup>? _permissionGroups;
        private IGenericRepository<PermissionGroupPermission>? _permissionGroupPermissions;
        private IGenericRepository<UserPermissionGroup>? _userPermissionGroups;
        
        private IGenericRepository<PlatformPageGroup>? _platformPageGroups;
        private IGenericRepository<PlatformUserGroupMatch>? _platformUserGroupMatches;
        
        private IGenericRepository<MobilePageGroup>? _mobilePageGroups;
        private IGenericRepository<MobileUserGroupMatch>? _mobileUserGroupMatches;
        
        // WarehouseTransfer repository instances
        private IGenericRepository<WtLine>? _wtLines;
        private IGenericRepository<WtHeader>? _wtHeaders;
        private IGenericRepository<WtRoute>? _wtRoutes;
        private IGenericRepository<WtTerminalLine>? _wtTerminalLines;
        private IGenericRepository<WtImportLine>? _wtImportLines;
        private IGenericRepository<WtLineSerial>? _wtLineSerials;

        // ProductTransfer repository instances
        private IGenericRepository<PtHeader>? _ptHeaders;
        private IGenericRepository<PtLine>? _ptLines;
        private IGenericRepository<PtImportLine>? _ptImportLines;
        private IGenericRepository<PtRoute>? _ptRoutes;
        private IGenericRepository<PtTerminalLine>? _ptTerminalLines;
        private IGenericRepository<PtLineSerial>? _ptLineSerials;

        // Production repository instances
        private IGenericRepository<PrHeader>? _prHeaders;
        private IGenericRepository<PrLine>? _prLines;
        private IGenericRepository<PrImportLine>? _prImportLines;
        private IGenericRepository<PrRoute>? _prRoutes;
        private IGenericRepository<PrTerminalLine>? _prTerminalLines;
        private IGenericRepository<PrLineSerial>? _prLineSerials;
        private IGenericRepository<PrHeaderSerial>? _prHeaderSerials;

        // SubcontractingIssueTransfer repository instances
        private IGenericRepository<SitHeader>? _sitHeaders;
        private IGenericRepository<SitLine>? _sitLines;
        private IGenericRepository<SitImportLine>? _sitImportLines;
        private IGenericRepository<SitRoute>? _sitRoutes;
        private IGenericRepository<SitTerminalLine>? _sitTerminalLines;
        private IGenericRepository<SitLineSerial>? _sitLineSerials;

        // SubcontractingReceiptTransfer repository instances
        private IGenericRepository<SrtHeader>? _srtHeaders;
        private IGenericRepository<SrtLine>? _srtLines;
        private IGenericRepository<SrtImportLine>? _srtImportLines;
        private IGenericRepository<SrtRoute>? _srtRoutes;
        private IGenericRepository<SrtTerminalLine>? _srtTerminalLines;
        private IGenericRepository<SrtLineSerial>? _srtLineSerials;
        private IGenericRepository<WoHeader>? _woHeaders;
        private IGenericRepository<WoLine>? _woLines;
        private IGenericRepository<WoImportLine>? _woImportLines;
        private IGenericRepository<WoRoute>? _woRoutes;
        private IGenericRepository<WoTerminalLine>? _woTerminalLines;
        private IGenericRepository<WoLineSerial>? _woLineSerials;
        private IGenericRepository<WiHeader>? _wiHeaders;
        private IGenericRepository<WiLine>? _wiLines;
        private IGenericRepository<WiImportLine>? _wiImportLines;
        private IGenericRepository<WiRoute>? _wiRoutes;
        private IGenericRepository<WiTerminalLine>? _wiTerminalLines;
        private IGenericRepository<WiLineSerial>? _wiLineSerials;
        private IGenericRepository<ShHeader>? _shHeaders;
        private IGenericRepository<ShLine>? _shLines;
        private IGenericRepository<ShImportLine>? _shImportLines;
        private IGenericRepository<ShRoute>? _shRoutes;
        private IGenericRepository<ShTerminalLine>? _shTerminalLines;
        private IGenericRepository<ShLineSerial>? _shLineSerials;

        // InventoryCount repository instances
        private IGenericRepository<IcHeader>? _icHeaders;
        private IGenericRepository<IcImportLine>? _icImportLines;
        private IGenericRepository<IcRoute>? _icRoutes;
        private IGenericRepository<IcTerminalLine>? _icTerminalLines;
        private IGenericRepository<Notification>? _notifications;
        private IGenericRepository<Customer>? _customers;
        private IGenericRepository<Stock>? _stocks;
        private IGenericRepository<JobFailureLog>? _jobFailureLogs;

        // Package repository instances
        private IGenericRepository<PHeader>? _pHeaders;
        private IGenericRepository<PPackage>? _pPackages;
        private IGenericRepository<PLine>? _pLines;

        // Parameter repository instances
        private IGenericRepository<GrParameter>? _grParameters;
        private IGenericRepository<WtParameter>? _wtParameters;
        private IGenericRepository<WoParameter>? _woParameters;
        private IGenericRepository<WiParameter>? _wiParameters;
        private IGenericRepository<ShParameter>? _shParameters;
        private IGenericRepository<SrtParameter>? _srtParameters;
        private IGenericRepository<SitParameter>? _sitParameters;
        private IGenericRepository<PtParameter>? _ptParameters;
        private IGenericRepository<PrParameter>? _prParameters;
        private IGenericRepository<IcParameter>? _icParameters;
        private IGenericRepository<PParameter>? _pParameters;

        public UnitOfWork(
            WmsDbContext context,
            ICurrentUserService executionContextAccessor,
            IRequestCancellationAccessor requestCancellationAccessor)
        {
            _context = context;
            _executionContextAccessor = executionContextAccessor;
            _requestCancellationAccessor = requestCancellationAccessor;
        }

        // Repository properties

        public IGenericRepository<User> Users =>
            _users ??= new GenericRepository<User>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<UserDetail> UserDetails =>
            _userDetails ??= new GenericRepository<UserDetail>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<UserSession> UserSessions =>
            _userSessions ??= new GenericRepository<UserSession>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<PasswordResetRequest> PasswordResetRequests =>
            _passwordResetRequests ??= new GenericRepository<PasswordResetRequest>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<BaseEntity> BaseEntities =>
            _baseEntities ??= new GenericRepository<BaseEntity>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<BaseHeaderEntity> BaseHeaderEntities =>
            _baseHeaderEntities ??= new GenericRepository<BaseHeaderEntity>(_context, _executionContextAccessor, _requestCancellationAccessor);

        // GoodReceipt repository properties
        public IGenericRepository<GrHeader> GrHeaders =>
            _grHeaders ??= new GenericRepository<GrHeader>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<GrLine> GrLines =>
            _grLines ??= new GenericRepository<GrLine>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<GrImportDocument> GrImportDocuments =>
            _grImportDocuments ??= new GenericRepository<GrImportDocument>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<GrImportLine> GrImportLines =>
            _grImportLines ??= new GenericRepository<GrImportLine>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<GrLineSerial> GrLineSerials =>
            _grLineSerials ??= new GenericRepository<GrLineSerial>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<GrRoute> GrRoutes =>
            _grRoutes ??= new GenericRepository<GrRoute>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<GrTerminalLine> GrTerminalLines =>
            _grTerminalLines ??= new GenericRepository<GrTerminalLine>(_context, _executionContextAccessor, _requestCancellationAccessor);
        
        // User and Authority repository properties
        public IGenericRepository<UserAuthority> UserAuthorities =>
            _userAuthorities ??= new GenericRepository<UserAuthority>(_context, _executionContextAccessor, _requestCancellationAccessor);
        public IGenericRepository<SmtpSetting> SmtpSettings =>
            _smtpSettings ??= new GenericRepository<SmtpSetting>(_context, _executionContextAccessor, _requestCancellationAccessor);
        public IGenericRepository<PermissionDefinition> PermissionDefinitions =>
            _permissionDefinitions ??= new GenericRepository<PermissionDefinition>(_context, _executionContextAccessor, _requestCancellationAccessor);
        public IGenericRepository<PermissionGroup> PermissionGroups =>
            _permissionGroups ??= new GenericRepository<PermissionGroup>(_context, _executionContextAccessor, _requestCancellationAccessor);
        public IGenericRepository<PermissionGroupPermission> PermissionGroupPermissions =>
            _permissionGroupPermissions ??= new GenericRepository<PermissionGroupPermission>(_context, _executionContextAccessor, _requestCancellationAccessor);
        public IGenericRepository<UserPermissionGroup> UserPermissionGroups =>
            _userPermissionGroups ??= new GenericRepository<UserPermissionGroup>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<PlatformPageGroup> PlatformPageGroups =>
            _platformPageGroups ??= new GenericRepository<PlatformPageGroup>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<PlatformUserGroupMatch> PlatformUserGroupMatches =>
            _platformUserGroupMatches ??= new GenericRepository<PlatformUserGroupMatch>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<MobilePageGroup> MobilePageGroups =>
            _mobilePageGroups ??= new GenericRepository<MobilePageGroup>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<MobileUserGroupMatch> MobileUserGroupMatches =>
            _mobileUserGroupMatches ??= new GenericRepository<MobileUserGroupMatch>(_context, _executionContextAccessor, _requestCancellationAccessor);

        // WarehouseTransfer repository properties
        public IGenericRepository<WtLine> WtLines =>
            _wtLines ??= new GenericRepository<WtLine>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<WtHeader> WtHeaders =>
            _wtHeaders ??= new GenericRepository<WtHeader>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<WtRoute> WtRoutes =>
            _wtRoutes ??= new GenericRepository<WtRoute>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<WtTerminalLine> WtTerminalLines =>
            _wtTerminalLines ??= new GenericRepository<WtTerminalLine>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<WtImportLine> WtImportLines =>
            _wtImportLines ??= new GenericRepository<WtImportLine>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<WtLineSerial> WtLineSerials =>
            _wtLineSerials ??= new GenericRepository<WtLineSerial>(_context, _executionContextAccessor, _requestCancellationAccessor);

        // ProductTransfer repository properties
        public IGenericRepository<PtHeader> PtHeaders =>
            _ptHeaders ??= new GenericRepository<PtHeader>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<PtLine> PtLines =>
            _ptLines ??= new GenericRepository<PtLine>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<PtImportLine> PtImportLines =>
            _ptImportLines ??= new GenericRepository<PtImportLine>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<PtRoute> PtRoutes =>
            _ptRoutes ??= new GenericRepository<PtRoute>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<PtTerminalLine> PtTerminalLines =>
            _ptTerminalLines ??= new GenericRepository<PtTerminalLine>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<PtLineSerial> PtLineSerials =>
            _ptLineSerials ??= new GenericRepository<PtLineSerial>(_context, _executionContextAccessor, _requestCancellationAccessor);

        // Production repository properties
        public IGenericRepository<PrHeader> PrHeaders =>
            _prHeaders ??= new GenericRepository<PrHeader>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<PrLine> PrLines =>
            _prLines ??= new GenericRepository<PrLine>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<PrImportLine> PrImportLines =>
            _prImportLines ??= new GenericRepository<PrImportLine>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<PrRoute> PrRoutes =>
            _prRoutes ??= new GenericRepository<PrRoute>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<PrTerminalLine> PrTerminalLines =>
            _prTerminalLines ??= new GenericRepository<PrTerminalLine>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<PrLineSerial> PrLineSerials =>
            _prLineSerials ??= new GenericRepository<PrLineSerial>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<PrHeaderSerial> PrHeaderSerials =>
            _prHeaderSerials ??= new GenericRepository<PrHeaderSerial>(_context, _executionContextAccessor, _requestCancellationAccessor);

        // SubcontractingIssueTransfer repository properties
        public IGenericRepository<SitHeader> SitHeaders =>
            _sitHeaders ??= new GenericRepository<SitHeader>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<SitLine> SitLines =>
            _sitLines ??= new GenericRepository<SitLine>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<SitImportLine> SitImportLines =>
            _sitImportLines ??= new GenericRepository<SitImportLine>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<SitRoute> SitRoutes =>
            _sitRoutes ??= new GenericRepository<SitRoute>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<SitTerminalLine> SitTerminalLines =>
            _sitTerminalLines ??= new GenericRepository<SitTerminalLine>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<SitLineSerial> SitLineSerials =>
            _sitLineSerials ??= new GenericRepository<SitLineSerial>(_context, _executionContextAccessor, _requestCancellationAccessor);

        // SubcontractingReceiptTransfer repository properties
        public IGenericRepository<SrtHeader> SrtHeaders =>
            _srtHeaders ??= new GenericRepository<SrtHeader>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<SrtLine> SrtLines =>
            _srtLines ??= new GenericRepository<SrtLine>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<SrtImportLine> SrtImportLines =>
            _srtImportLines ??= new GenericRepository<SrtImportLine>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<SrtRoute> SrtRoutes =>
            _srtRoutes ??= new GenericRepository<SrtRoute>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<SrtTerminalLine> SrtTerminalLines =>
            _srtTerminalLines ??= new GenericRepository<SrtTerminalLine>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<SrtLineSerial> SrtLineSerials =>
            _srtLineSerials ??= new GenericRepository<SrtLineSerial>(_context, _executionContextAccessor, _requestCancellationAccessor);

        // WarehouseOutbound repository properties
        public IGenericRepository<WoHeader> WoHeaders =>
            _woHeaders ??= new GenericRepository<WoHeader>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<WoLine> WoLines =>
            _woLines ??= new GenericRepository<WoLine>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<WoImportLine> WoImportLines =>
            _woImportLines ??= new GenericRepository<WoImportLine>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<WoRoute> WoRoutes =>
            _woRoutes ??= new GenericRepository<WoRoute>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<WoTerminalLine> WoTerminalLines =>
            _woTerminalLines ??= new GenericRepository<WoTerminalLine>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<WoLineSerial> WoLineSerials =>
            _woLineSerials ??= new GenericRepository<WoLineSerial>(_context, _executionContextAccessor, _requestCancellationAccessor);

        // WarehouseInbound repository properties
        public IGenericRepository<WiHeader> WiHeaders =>
            _wiHeaders ??= new GenericRepository<WiHeader>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<WiLine> WiLines =>
            _wiLines ??= new GenericRepository<WiLine>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<WiImportLine> WiImportLines =>
            _wiImportLines ??= new GenericRepository<WiImportLine>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<WiRoute> WiRoutes =>
            _wiRoutes ??= new GenericRepository<WiRoute>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<WiTerminalLine> WiTerminalLines =>
            _wiTerminalLines ??= new GenericRepository<WiTerminalLine>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<WiLineSerial> WiLineSerials =>
            _wiLineSerials ??= new GenericRepository<WiLineSerial>(_context, _executionContextAccessor, _requestCancellationAccessor);

        // Shipping repository properties
        public IGenericRepository<ShHeader> ShHeaders =>
            _shHeaders ??= new GenericRepository<ShHeader>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<ShLine> ShLines =>
            _shLines ??= new GenericRepository<ShLine>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<ShImportLine> ShImportLines =>
            _shImportLines ??= new GenericRepository<ShImportLine>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<ShRoute> ShRoutes =>
            _shRoutes ??= new GenericRepository<ShRoute>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<ShTerminalLine> ShTerminalLines =>
            _shTerminalLines ??= new GenericRepository<ShTerminalLine>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<ShLineSerial> ShLineSerials =>
            _shLineSerials ??= new GenericRepository<ShLineSerial>(_context, _executionContextAccessor, _requestCancellationAccessor);

        // InventoryCount repository properties
        public IGenericRepository<IcHeader> ICHeaders =>
            _icHeaders ??= new GenericRepository<IcHeader>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<IcImportLine> IcImportLines =>
            _icImportLines ??= new GenericRepository<IcImportLine>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<IcRoute> IcRoutes =>
            _icRoutes ??= new GenericRepository<IcRoute>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<IcTerminalLine> IcTerminalLines =>
            _icTerminalLines ??= new GenericRepository<IcTerminalLine>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<Notification> Notifications =>
            _notifications ??= new GenericRepository<Notification>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<Customer> Customers =>
            _customers ??= new GenericRepository<Customer>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<Stock> Stocks =>
            _stocks ??= new GenericRepository<Stock>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<JobFailureLog> JobFailureLogs =>
            _jobFailureLogs ??= new GenericRepository<JobFailureLog>(_context, _executionContextAccessor, _requestCancellationAccessor);

        // Package repository properties
        public IGenericRepository<PHeader> PHeaders =>
            _pHeaders ??= new GenericRepository<PHeader>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<PPackage> PPackages =>
            _pPackages ??= new GenericRepository<PPackage>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<PLine> PLines =>
            _pLines ??= new GenericRepository<PLine>(_context, _executionContextAccessor, _requestCancellationAccessor);

        // Parameter repository properties
        public IGenericRepository<GrParameter> GrParameters =>
            _grParameters ??= new GenericRepository<GrParameter>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<WtParameter> WtParameters =>
            _wtParameters ??= new GenericRepository<WtParameter>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<WoParameter> WoParameters =>
            _woParameters ??= new GenericRepository<WoParameter>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<WiParameter> WiParameters =>
            _wiParameters ??= new GenericRepository<WiParameter>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<ShParameter> ShParameters =>
            _shParameters ??= new GenericRepository<ShParameter>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<SrtParameter> SrtParameters =>
            _srtParameters ??= new GenericRepository<SrtParameter>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<SitParameter> SitParameters =>
            _sitParameters ??= new GenericRepository<SitParameter>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<PtParameter> PtParameters =>
            _ptParameters ??= new GenericRepository<PtParameter>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<PrParameter> PrParameters =>
            _prParameters ??= new GenericRepository<PrParameter>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<IcParameter> IcParameters =>
            _icParameters ??= new GenericRepository<IcParameter>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IGenericRepository<PParameter> PParameters =>
            _pParameters ??= new GenericRepository<PParameter>(_context, _executionContextAccessor, _requestCancellationAccessor);

        public IQueryable<TEntity> SqlQuery<TEntity>(string sql, params object[] parameters) where TEntity : class
        {
            return _context.Set<TEntity>()
                .FromSqlRaw(sql, parameters)
                .AsNoTracking();
        }

        public async Task<long> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(_requestCancellationAccessor.Get(cancellationToken));
        }

        public long SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Database.BeginTransactionAsync(_requestCancellationAccessor.Get(cancellationToken));
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            var tx = _context.Database.CurrentTransaction;
            if (tx != null)
            {
                await tx.CommitAsync(_requestCancellationAccessor.Get(cancellationToken));
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            var tx = _context.Database.CurrentTransaction;
            if (tx != null)
            {
                await tx.RollbackAsync(_requestCancellationAccessor.Get(cancellationToken));
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
