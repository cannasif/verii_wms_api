namespace WMS_WEBAPI.Security
{
    public static class PermissionCatalog
    {
        public static IReadOnlyCollection<PermissionDefinitionSeedModel> All { get; } =
        [
            new("access-control.user-management.view", "User Management View", "Read users, roles and user permission assignments."),
            new("access-control.user-management.create", "User Management Create", "Create users, roles and related records."),
            new("access-control.user-management.update", "User Management Update", "Update or delete users, roles and related records."),

            new("access-control.permission-definitions.view", "Permission Definitions View", "Read permission definitions."),
            new("access-control.permission-definitions.create", "Permission Definitions Create", "Create permission definitions."),
            new("access-control.permission-definitions.update", "Permission Definitions Update", "Update or delete permission definitions."),

            new("access-control.permission-groups.view", "Permission Groups View", "Read permission groups."),
            new("access-control.permission-groups.create", "Permission Groups Create", "Create permission groups."),
            new("access-control.permission-groups.update", "Permission Groups Update", "Update or delete permission groups and their permissions."),

            new("access-control.mail-settings.view", "Mail Settings View", "Read SMTP and mail settings."),
            new("access-control.mail-settings.update", "Mail Settings Update", "Update SMTP and mail settings or send test mail."),

            new("access-control.hangfire-monitoring.view", "Hangfire Monitoring View", "Read Hangfire monitoring and failure logs."),

            new("wms.goods-receipt.view", "Goods Receipt View", "Read goods receipt records."),
            new("wms.goods-receipt.create", "Goods Receipt Create", "Create goods receipt records."),
            new("wms.goods-receipt.update", "Goods Receipt Update", "Update or delete goods receipt records."),

            new("wms.transfer.view", "Transfer View", "Read warehouse transfer records."),
            new("wms.transfer.create", "Transfer Create", "Create warehouse transfer records."),
            new("wms.transfer.update", "Transfer Update", "Update or delete warehouse transfer records."),

            new("wms.subcontracting.issue.view", "Subcontracting Issue View", "Read subcontracting issue transfer records."),
            new("wms.subcontracting.issue.create", "Subcontracting Issue Create", "Create subcontracting issue transfer records."),
            new("wms.subcontracting.issue.update", "Subcontracting Issue Update", "Update or delete subcontracting issue transfer records."),

            new("wms.subcontracting.receipt.view", "Subcontracting Receipt View", "Read subcontracting receipt transfer records."),
            new("wms.subcontracting.receipt.create", "Subcontracting Receipt Create", "Create subcontracting receipt transfer records."),
            new("wms.subcontracting.receipt.update", "Subcontracting Receipt Update", "Update or delete subcontracting receipt transfer records."),

            new("wms.warehouse.inbound.view", "Warehouse Inbound View", "Read warehouse inbound records."),
            new("wms.warehouse.inbound.create", "Warehouse Inbound Create", "Create warehouse inbound records."),
            new("wms.warehouse.inbound.update", "Warehouse Inbound Update", "Update or delete warehouse inbound records."),

            new("wms.warehouse.outbound.view", "Warehouse Outbound View", "Read warehouse outbound records."),
            new("wms.warehouse.outbound.create", "Warehouse Outbound Create", "Create warehouse outbound records."),
            new("wms.warehouse.outbound.update", "Warehouse Outbound Update", "Update or delete warehouse outbound records."),

            new("wms.shipment.view", "Shipment View", "Read shipment records."),
            new("wms.shipment.create", "Shipment Create", "Create shipment records."),
            new("wms.shipment.update", "Shipment Update", "Update or delete shipment records."),

            new("wms.package.view", "Package View", "Read package records."),
            new("wms.package.create", "Package Create", "Create package records."),
            new("wms.package.update", "Package Update", "Update or delete package records."),

            new("wms.parameters.gr.view", "Goods Receipt Parameters View", "Read goods receipt parameters."),
            new("wms.parameters.gr.update", "Goods Receipt Parameters Update", "Update goods receipt parameters."),
            new("wms.parameters.wt.view", "Transfer Parameters View", "Read transfer parameters."),
            new("wms.parameters.wt.update", "Transfer Parameters Update", "Update transfer parameters."),
            new("wms.parameters.wo.view", "Warehouse Outbound Parameters View", "Read warehouse outbound parameters."),
            new("wms.parameters.wo.update", "Warehouse Outbound Parameters Update", "Update warehouse outbound parameters."),
            new("wms.parameters.wi.view", "Warehouse Inbound Parameters View", "Read warehouse inbound parameters."),
            new("wms.parameters.wi.update", "Warehouse Inbound Parameters Update", "Update warehouse inbound parameters."),
            new("wms.parameters.sh.view", "Shipment Parameters View", "Read shipment parameters."),
            new("wms.parameters.sh.update", "Shipment Parameters Update", "Update shipment parameters."),
            new("wms.parameters.srt.view", "Subcontracting Receipt Parameters View", "Read subcontracting receipt parameters."),
            new("wms.parameters.srt.update", "Subcontracting Receipt Parameters Update", "Update subcontracting receipt parameters."),
            new("wms.parameters.sit.view", "Subcontracting Issue Parameters View", "Read subcontracting issue parameters."),
            new("wms.parameters.sit.update", "Subcontracting Issue Parameters Update", "Update subcontracting issue parameters."),
            new("wms.parameters.pt.view", "Production Transfer Parameters View", "Read production transfer parameters."),
            new("wms.parameters.pt.update", "Production Transfer Parameters Update", "Update production transfer parameters."),
            new("wms.parameters.pr.view", "Production Parameters View", "Read production parameters."),
            new("wms.parameters.pr.update", "Production Parameters Update", "Update production parameters."),
            new("wms.parameters.ic.view", "Inventory Count Parameters View", "Read inventory count parameters."),
            new("wms.parameters.ic.update", "Inventory Count Parameters Update", "Update inventory count parameters."),
            new("wms.parameters.p.view", "Package Parameters View", "Read package parameters."),
            new("wms.parameters.p.update", "Package Parameters Update", "Update package parameters."),

            new("wms.production.view", "Production View", "Read production records."),
            new("wms.production.create", "Production Create", "Create production records."),
            new("wms.production.update", "Production Update", "Update or delete production records."),

            new("wms.production-transfer.view", "Production Transfer View", "Read production transfer records."),
            new("wms.production-transfer.create", "Production Transfer Create", "Create production transfer records."),
            new("wms.production-transfer.update", "Production Transfer Update", "Update or delete production transfer records."),

            new("wms.inventory-count.view", "Inventory Count View", "Read inventory count records."),
            new("wms.inventory-count.create", "Inventory Count Create", "Create inventory count records."),
            new("wms.inventory-count.update", "Inventory Count Update", "Update or delete inventory count records.")
        ];
    }
}
