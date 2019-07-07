using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;

using Chai.WorkflowManagment.CoreDomain.Admins;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.CoreDomain.Infrastructure;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.CoreDomain.TravelLogs;
using Chai.WorkflowManagment.CoreDomain.Request;
using Chai.WorkflowManagment.CoreDomain.Approval;


namespace Chai.WorkflowManagment.CoreDomain.DataAccess
{
    public class WorkflowManagmentDbContext : BaseDbContext
    {
        public WorkflowManagmentDbContext(bool disableProxy)
            : base("WorkflowManagmentTest")
        {
            if (disableProxy)
                ObjContext().ContextOptions.ProxyCreationEnabled = false;

        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AssignJob> AssignJobs { get; set; }
        public DbSet<Node> Nodes { get; set; }
        public DbSet<NodeRole> NodeRoles { get; set; }
        public DbSet<PocModule> PocModules { get; set; }
        public DbSet<PopupMenu> PopupMenus { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Tab> Tabs { get; set; }
        public DbSet<TabRole> TabRoles { get; set; }
        public DbSet<TaskPan> TaskPans { get; set; }
        public DbSet<TaskPanNode> TaskPanNodes { get; set; }
        public DbSet<AppUserRole> AppUserRoles { get; set; }
        public DbSet<Grant> Grants { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectGrant> ProjectGrants { get; set; }

        public DbSet<EmployeePosition> EmployeePositions { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<ExpenseType> ExpenseTypes { get; set; }
        public DbSet<EmployeeLeave> EmployeeLeaves { get; set; }
        public DbSet<ItemAccount> ItemAccounts { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierType> SupplierTypes { get; set; }
        public DbSet<CarRental> CarRentals { get; set; }
        public DbSet<Beneficiary> Beneficiaries { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<ApprovalSetting> ApprovalSettings { get; set; }
        public DbSet<ApprovalLevel> ApprovalLevels { get; set; }
        public DbSet<CostSharingSetting> CostSharingSetting { get; set; }
        public DbSet<VehicleRequest> VehicleRequests { get; set; }
        public DbSet<TravelLog> TraveLogs { get; set; }
        public DbSet<VehicleRequestStatus> VehicleRequestStatuses { get; set; }
        public DbSet<VehicleRequestDetail> VehicleRequestDetails { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<CashPaymentRequest> CashPaymentRequests { get; set; }
        public DbSet<CashPaymentRequestDetail> CashPaymentRequestDetails { get; set; }
        public DbSet<CashPaymentRequestStatus> CashPaymentRequestStatuses { get; set; }
        public DbSet<CPRAttachment> CPRAttachments { get; set; }

        public DbSet<CostSharingRequest> CostSharingRequests { get; set; }
        public DbSet<CostSharingRequestDetail> CostSharingRequestDetails { get; set; }
        public DbSet<CostSharingRequestStatus> CostSharingRequestStatuses { get; set; }
        public DbSet<CSRAttachment> CSRAttachments { get; set; }
        public DbSet<PaymentReimbursementRequest> PaymentReimbursementRequests { get; set; }
        public DbSet<PaymentReimbursementRequestDetail> PaymentReimbursementRequestDetails { get; set; }
        public DbSet<PaymentReimbursementRequestStatus> PaymentReimbursementRequestStatuses { get; set; }

        public DbSet<OperationalControlRequest> OperationalControlRequests { get; set; }
        public DbSet<OperationalControlRequestDetail> OperationalControlRequestDetails { get; set; }
        public DbSet<OperationalControlRequestStatus> OperationalControlRequestStatuses { get; set; }
        public DbSet<OCRAttachment> OCRAttachments { get; set; }

        public DbSet<BankPaymentRequest> BankPaymentRequests { get; set; }
        public DbSet<BankPaymentRequestDetail> BankPaymentRequestDetails { get; set; }
        public DbSet<BankPaymentRequestStatus> BankPaymentRequestStatuses { get; set; }

        public DbSet<TravelAdvanceRequest> TravelAdvanceRequests { get; set; }
        public DbSet<TravelAdvanceRequestDetail> TravelAdvanceRequestDetails { get; set; }
        public DbSet<TravelAdvanceRequestStatus> TravelAdvanceRequestStatuses { get; set; }
        public DbSet<TravelAdvanceCost> TravelAdvanceCosts { get; set; }
        public DbSet<ExpenseLiquidationRequest> ExpenseLiquidationRequests { get; set; }
        public DbSet<ExpenseLiquidationRequestStatus> ExpenseLiquidationRequestStatuses { get; set; }
        public DbSet<ExpenseLiquidationRequestDetail> ExpenseLiquidationRequestDetails { get; set; }        
        public DbSet<ELRAttachment> ELRAttachments { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<LeaveRequestStatus> LeaveRequestStatuses { get; set; }

        public DbSet<SoleVendorRequestStatus> SoleVendorRequestStatuses { get; set; }
        public DbSet<SoleVendorRequest> SoleVendorRequests { get; set; }
        public DbSet<BidAnalysisRequestStatus> BidAnalysisRequestStatuses { get; set; }
        public DbSet<BidAnalysisRequest> BidAnalysisRequests { get; set; }
        public DbSet<PurchaseRequest> PurchaseRequests { get; set; }
        public DbSet<PurchaseRequestDetail> PurchaseRequestDetail { get; set; }
        public DbSet<PurchaseRequestStatus> PurchaseRequestStatuses { get; set; }
        public DbSet<BidAnalysisRequestDetail> BidAnalysisRequestDetail { get; set; }
        public DbSet<BAAttachment> BAAttachments { get; set; }
        public DbSet<Bidder> Bidders { get; set; }
        public DbSet<BidderItemDetail> BidderItemDetails { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        public DbSet<PurchaseOrderSoleVendor> PurchaseOrderSoleVendors { get; set; }
        public DbSet<PurchaseOrderSoleVendorDetail> PurchaseOrderSoleVendorDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<AppUser>().HasMany(p => p.AppUserRoles).WithMany();
            //modelBuilder.Entity<Node>().HasMany(p => p.NodeRoles).WithMany();
            //modelBuilder.Entity<Tab>().HasMany(p => p.TabRoles).WithMany();
            //modelBuilder.Entity<Tab>().HasMany(p => p.TaskPans).WithMany();
            //modelBuilder.Entity<TaskPan>().HasMany(p => p.TaskPanNodes).WithMany();


            base.OnModelCreating(modelBuilder);
        }
    }
}