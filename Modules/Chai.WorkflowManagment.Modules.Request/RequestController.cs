using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Utility;
using Microsoft.Practices.ObjectBuilder;

using Chai.WorkflowManagment.CoreDomain;
using Chai.WorkflowManagment.CoreDomain.DataAccess;
using Chai.WorkflowManagment.CoreDomain.Admins;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Services;
using Chai.WorkflowManagment.Shared.Navigation;


using System.Data;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.CoreDomain.TravelLogs;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.CoreDomain.Request;

namespace Chai.WorkflowManagment.Modules.Request
{
    public class RequestController : ControllerBase
    {
        private IWorkspace _workspace;

        [InjectionConstructor]
        public RequestController([ServiceDependency] IHttpContextLocatorService httpContextLocatorService, [ServiceDependency]INavigationService navigationService)
            : base(httpContextLocatorService, navigationService)
        {
            _workspace = ZadsServices.Workspace;
        }
        public AppUser GetSuperviser(int superviser)
        {
            return _workspace.Single<AppUser>(x => x.Id == superviser);
        }
        public AppUser CurrentUser()
        {
            return GetCurrentUser();
        }
        public AppUser Approver(int position)
        {
            return _workspace.SqlQuery<AppUser>("SELECT * FROM AppUsers WHERE EmployeePosition_Id = " + position).ToList().Last<AppUser>();
        }

        #region CurrenrObject
        public object CurrentObject
        {
            get
            {
                return GetCurrentContext().Session["CurrentObject"];
            }
            set
            {
                GetCurrentContext().Session["CurrentObject"] = value;
            }
        }
        #endregion
        #region Travel Log
        public IList<TravelLog> GetTravelLogs()
        {
            return WorkspaceFactory.CreateReadOnly().Query<TravelLog>(null).ToList();
        }
        public TravelLog GetTravelLog(int TravelLogId)
        {
            return _workspace.Single<TravelLog>(x => x.Id == TravelLogId);
        }
        public IList<TravelLog> ListTravelLogs(int RequestId)
        {
            string filterExpression = "";

            filterExpression = "SELECT * FROM TravelLogs Where 1 = Case when '" + RequestId + "' = '' Then 1 When TravelLogs.VehicleRequest_Id = '" + RequestId + "' Then 1 END ";

            return _workspace.SqlQuery<TravelLog>(filterExpression).ToList();

        }
        #endregion
        #region Vehicle Requests
        public IList<VehicleRequest> GetVehicleRequests()
        {
            return WorkspaceFactory.CreateReadOnly().Query<VehicleRequest>(null).ToList();
        }
        public VehicleRequest GetVehicleRequest(int id)
        {
            return _workspace.Single<VehicleRequest>(x => x.Id == id);
        }
        public IList<VehicleRequest> ListVehicleRequests(string RequestNo, string RequestDate)
        {
            string filterExpression = "";

            filterExpression = "SELECT * FROM VehicleRequests Where 1 = Case when '" + RequestNo + "' = '' Then 1 When VehicleRequests.RequestNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When VehicleRequests.RequestDate = '" + RequestDate + "'  Then 1 END And VehicleRequests.AppUser_Id='" + GetCurrentUser().Id + "' order by VehicleRequests.Id Desc ";

            return _workspace.SqlQuery<VehicleRequest>(filterExpression).ToList();

        }
        public int GetLastVehicleRequestId()
        {
            if (_workspace.Last<VehicleRequest>() != null)
            {
                return _workspace.Last<VehicleRequest>().Id;
            }
            else { return 0; }
        }
        public VehicleRequestDetail GetAssignedVehicleById(int id)
        {
            return _workspace.Single<VehicleRequestDetail>(x => x.Id == id);
        }
        #endregion
        #region Cash Payment
        public CashPaymentRequest GetCashPaymentRequest(int RequestId)
        {
            return _workspace.Single<CashPaymentRequest>(x => x.Id == RequestId);
        }
        public IList<CashPaymentRequest> ListCashPaymentRequests(string RequestNo, string RequestDate)
        {
            string filterExpression = "";

            filterExpression = "SELECT * FROM CashPaymentRequests LEFT JOIN Suppliers on CashPaymentRequests.Supplier_Id = Suppliers.Id Where 1 = Case when '" + RequestNo + "' = '' Then 1 When CashPaymentRequests.VoucherNo = '" + RequestNo + "' Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When CashPaymentRequests.RequestDate = '" + RequestDate + "'  Then 1 END And CashPaymentRequests.AppUser_Id='" + GetCurrentUser().Id + "' ORDER BY CashPaymentRequests.Id Desc";
           // return WorkspaceFactory.CreateReadOnly().Queryable<CashPaymentRequest>(filterExpression).ToList();
            return _workspace.SqlQuery<CashPaymentRequest>(filterExpression).ToList();
        }
        public IList<CashPaymentRequest> GetCashPaymentRequests()
        {
            return WorkspaceFactory.CreateReadOnly().Query<CashPaymentRequest>(null).ToList();
        }
        public IList<CashPaymentRequest> ListCashPaymentsNotExpensed()
        {
            int currentUserId = GetCurrentUser().Id;
            return WorkspaceFactory.CreateReadOnly().Query<CashPaymentRequest>(x => x.PaymentReimbursementStatus == "Completed" && x.PaymentReimbursementRequest == null && currentUserId == x.AppUser.Id).ToList();
        }
        public CashPaymentRequestDetail GetCashPaymentRequestDetail(int CPRDId)
        {
            return _workspace.Single<CashPaymentRequestDetail>(x => x.Id == CPRDId);
        }
        public int GetLastCashPaymentRequestId()
        {
            if (_workspace.Last<CashPaymentRequest>() != null)
            {
                return _workspace.Last<CashPaymentRequest>().Id;
            }
            else { return 0; }
        }
       
        public CPRAttachment GetCPRAttachment(int attachmentId)
        {
            return _workspace.Single<CPRAttachment>(x => x.Id == attachmentId);
        }
        #endregion
        #region Cost Sharing
        public CostSharingRequest GetCostSharingRequest(int RequestId)
        {
            return _workspace.Single<CostSharingRequest>(x => x.Id == RequestId);
        }
        public IList<CostSharingRequest> ListCostSharingRequests(string RequestNo, string RequestDate)
        {
            string filterExpression = "";

            filterExpression = "SELECT * FROM CostSharingRequests Where 1 = Case when '" + RequestNo + "' = '' Then 1 When CostSharingRequests.VoucherNo = '" + RequestNo + "' Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When CostSharingRequests.RequestDate = '" + RequestDate + "'  Then 1 END And CostSharingRequests.AppUser_Id='" + GetCurrentUser().Id + "' ORDER BY CostSharingRequests.Id Desc";

            return _workspace.SqlQuery<CostSharingRequest>(filterExpression).ToList();
        }
        public IList<CostSharingRequest> GetCostSharingRequests()
        {
            return WorkspaceFactory.CreateReadOnly().Query<CostSharingRequest>(null).ToList();
        }
        public IList<CostSharingRequest> ListCostSharingRequestsNotExpensed()
        {
            int currentUserId = GetCurrentUser().Id;
            return WorkspaceFactory.CreateReadOnly().Query<CostSharingRequest>(x => x.PaymentReimbursementStatus == "Completed" && currentUserId == x.AppUser.Id).ToList();
        }
        public CostSharingRequestDetail GetCostSharingRequestDetail(int CSRDId)
        {
            return _workspace.Single<CostSharingRequestDetail>(x => x.Id == CSRDId);
        }
        public int GetLastCostSharingRequestId()
        {
            if (_workspace.Last<CostSharingRequest>() != null)
            {
                return _workspace.Last<CostSharingRequest>().Id;
            }
            else { return 0; }
        }
        public CSRAttachment GetCSRAttachment(int attachmentId)
        {
            return _workspace.Single<CSRAttachment>(x => x.Id == attachmentId);
        }
        #endregion
        #region Payment Reimbursement
        public IList<PaymentReimbursementRequest> ListPaymentReimbursementRequests(string RequestDate)
        {
            string filterExpression = "";

            filterExpression = "SELECT * FROM PaymentReimbursementRequests Where 1 = Case when '" + RequestDate + "' = '' Then 1 When PaymentReimbursementRequests.RequestDate = '" + RequestDate + "'  Then 1 END ORDER BY PaymentReimbursementRequests.Id Desc ";

            return _workspace.SqlQuery<PaymentReimbursementRequest>(filterExpression).ToList();
        }
        public PaymentReimbursementRequest GetPaymentReimbursementRequest(int RequestId)
        {
            return _workspace.Single<PaymentReimbursementRequest>(x => x.Id == RequestId);
        }
        public IList<PaymentReimbursementRequest> GetPaymentReimbursementRequests()
        {
            return WorkspaceFactory.CreateReadOnly().Query<PaymentReimbursementRequest>(null).ToList();
        }
        //public ELRAttachment GetELRAttachment(int attachmentId)
        //{
        //    return _workspace.Single<ELRAttachment>(x => x.Id == attachmentId);
        //}
        #endregion
        #region Bank Payment
        public BankPaymentRequest GetBankPaymentRequest(int RequestId)
        {
            return _workspace.Single<BankPaymentRequest>(x => x.Id == RequestId);
        }
        public IList<BankPaymentRequest> GetBankPaymentRequests()
        {
            return WorkspaceFactory.CreateReadOnly().Query<BankPaymentRequest>(null).ToList();
        }
        public int GetLastBankPaymentRequestId()
        {
            if (_workspace.Last<BankPaymentRequest>() != null)
            {
                return _workspace.Last<BankPaymentRequest>().Id;
            }
            else { return 1; }
        }
        public IList<BankPaymentRequest> ListBankPaymentRequests(string RequestNo, string ProcessDate)
        {
            string filterExpression = "";

            filterExpression = "SELECT * FROM BankPaymentRequests Where 1 = Case when '" + RequestNo + "' = '' Then 1 When BankPaymentRequests.RequestNo = '" + RequestNo + "' Then 1 END And  1 = Case when '" + ProcessDate + "' = '' Then 1 When BankPaymentRequests.ProcessDate = '" + ProcessDate + "'  Then 1 END ORDER BY BankPaymentRequests.Id Desc";

            return _workspace.SqlQuery<BankPaymentRequest>(filterExpression).ToList();
        }
        public BankPaymentRequestDetail GetBankPaymentRequestDetail(int BPRDId)
        {
            return _workspace.Single<BankPaymentRequestDetail>(x => x.Id == BPRDId);
        }
        #endregion
        #region Operational Control
        public OperationalControlRequest GetOperationalControlRequest(int RequestId)
        {
            return _workspace.Single<OperationalControlRequest>(x => x.Id == RequestId);
        }
        public IList<OperationalControlRequest> ListOperationalControlRequests(string RequestNo, string RequestDate)
        {
            string filterExpression = "";

            filterExpression = "SELECT * FROM OperationalControlRequests Where 1 = Case when '" + RequestNo + "' = '' Then 1 When OperationalControlRequests.VoucherNo = '" + RequestNo + "' Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When OperationalControlRequests.RequestDate = '" + RequestDate + "'  Then 1 END ORDER BY OperationalControlRequests.Id Desc";

            return _workspace.SqlQuery<OperationalControlRequest>(filterExpression).ToList();
        }
        public IList<OperationalControlRequest> GetOperationalControlRequests()
        {
            return WorkspaceFactory.CreateReadOnly().Query<OperationalControlRequest>(null).ToList();
        }
        public IList<OperationalControlRequest> ListOperationalControlsNotExpensed()
        {
            //int currentUserId = GetCurrentUser().Id;
            //return WorkspaceFactory.CreateReadOnly().Query<OperationalControlRequest>(x => x.PaymentReimbursementStatus == "Completed" && x.PaymentReimbursementRequest == null && currentUserId == x.AppUser.Id).ToList();
            return null;
        }
        public OperationalControlRequestDetail GetOperationalControlRequestDetail(int OCRDId)
        {
            return _workspace.Single<OperationalControlRequestDetail>(x => x.Id == OCRDId);
        }
        public int GetLastOperationalControlRequestId()
        {
            if (_workspace.Last<OperationalControlRequest>() != null)
            {
                return _workspace.Last<OperationalControlRequest>().Id;
            }
            else { return 0; }
        }
        public OCRAttachment GetOCRAttachment(int attachmentId)
        {
            return _workspace.Single<OCRAttachment>(x => x.Id == attachmentId);
        }
        #endregion
        #region Travel Advance Request
        public TravelAdvanceRequest GetTravelAdvanceRequest(int TravelAdvanceRequestId)
        {
            return _workspace.Single<TravelAdvanceRequest>(x => x.Id == TravelAdvanceRequestId);
        }
        public IList<TravelAdvanceRequest> GetTravelAdvanceRequests()
        {
            return WorkspaceFactory.CreateReadOnly().Query<TravelAdvanceRequest>(null).ToList();
        }
        public int GetLastTravelAdvanceRequestId()
        {
            if (_workspace.Last<TravelAdvanceRequest>() != null)
            {
                return _workspace.Last<TravelAdvanceRequest>().Id;
            }
            else { return 0; }
        }
        public IList<TravelAdvanceRequest> ListTravelAdvanceRequests(string RequestNo, string RequestDate)
        {
            string filterExpression = "";

            filterExpression = "SELECT * FROM TravelAdvanceRequests Where 1 = Case when '" + RequestNo + "' = '' Then 1 When TravelAdvanceRequests.TravelAdvanceNo = '" + RequestNo + "' Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When TravelAdvanceRequests.RequestDate = '" + RequestDate + "'  Then 1 END And TravelAdvanceRequests.AppUser_Id='" + GetCurrentUser().Id + "' ORDER BY TravelAdvanceRequests.Id Desc ";

            return _workspace.SqlQuery<TravelAdvanceRequest>(filterExpression).ToList();
        }
        public IList<TravelAdvanceRequest> ListTravelAdvancesNotExpensed()
        {
            int currentUserId = GetCurrentUser().Id;
            return WorkspaceFactory.CreateReadOnly().Query<TravelAdvanceRequest>(x => x.ExpenseLiquidationStatus == "Completed" && x.ExpenseLiquidationRequest.ExpenseLiquidationRequestStatuses.Count == 0 && x.AppUser.Id == currentUserId).ToList();
        }
        public TravelAdvanceRequestDetail GetTravelAdvanceRequestDetail(int id)
        {
            return _workspace.Single<TravelAdvanceRequestDetail>(x => x.Id == id);
        }
        public TravelAdvanceCost GetTravelAdvanceCost(int id)
        {
            return _workspace.Single<TravelAdvanceCost>(x => x.Id == id);
        }
        #endregion
        #region Expense Liquidation
        public IList<ExpenseLiquidationRequest> ListExpenseLiquidationRequests(string ExpenseType, string RequestDate)
        {
            string filterExpression = "";

            filterExpression = "SELECT * FROM ExpenseLiquidationRequests INNER JOIN TravelAdvanceRequests ON TravelAdvanceRequests.Id = ExpenseLiquidationRequests.Id Where 1 = Case when '" + ExpenseType + "' = '' Then 1 When ExpenseLiquidationRequests.ExpenseType = '" + ExpenseType + "' Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When ExpenseLiquidationRequests.RequestDate = '" + RequestDate + "'  Then 1 END AND TravelAdvanceRequests.AppUser_Id='" + GetCurrentUser().Id + "' ORDER BY ExpenseLiquidationRequests.Id Desc ";

            return _workspace.SqlQuery<ExpenseLiquidationRequest>(filterExpression).ToList();
        }
        public ExpenseLiquidationRequest GetExpenseLiquidationRequest(int RequestId)
        {
            return _workspace.Single<ExpenseLiquidationRequest>(x => x.Id == RequestId);
        }
        public IList<ExpenseLiquidationRequest> GetExpenseLiquidationRequests()
        {
            return WorkspaceFactory.CreateReadOnly().Query<ExpenseLiquidationRequest>(null).ToList();
        }
        public ELRAttachment GetELRAttachment(int attachmentId)
        {
            return _workspace.Single<ELRAttachment>(x => x.Id == attachmentId);
        }
        #endregion
        #region LeaveRequest

        public IList<LeaveRequest> GetLeaveRequests()
        {
            return WorkspaceFactory.CreateReadOnly().Query<LeaveRequest>(null).ToList();
        }
        public LeaveRequest GetLeaveRequest(int LeaveRequestId)
        {
            return _workspace.Single<LeaveRequest>(x => x.Id == LeaveRequestId);
        }

        public IList<LeaveRequest> ListLeaveRequests(string RequestNo, string RequestDate)
        {
            string filterExpression = "";

            filterExpression = "SELECT  *  FROM LeaveRequests Where 1 = Case when '" + RequestNo + "' = '' Then 1 When LeaveRequests.RequestNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When LeaveRequests.RequestedDate = '" + RequestDate + "'  Then 1 END And LeaveRequests.Requester='" + GetCurrentUser().Id + "'order by LeaveRequests.Id DESC ";

            return _workspace.SqlQuery<LeaveRequest>(filterExpression).ToList();

        }
        public int GetLastLeaveRequestId()
        {
            if (_workspace.Last<LeaveRequest>() != null)
            {
                return _workspace.Last<LeaveRequest>().Id;
            }
            else { return 0; }
        }
        #endregion
        #region AssignJob
        public AssignJob GetAssignedJobbycurrentuser()
        {
            int userId = GetCurrentUser().Id;
            return _workspace.Single<AssignJob>(x => x.AppUser.Id == userId && x.Status == true);
        }
        public AssignJob GetAssignedJobbycurrentuser(int UserId)
        {
            //int userId = GetCurrentUser().Id;
            return _workspace.Single<AssignJob>(x => x.AppUser.Id == UserId && x.Status == true);
        }
        public int GetAssignedUserbycurrentuser()
        {
            int userId = GetCurrentUser().Id;
            IList<AssignJob> AJ = _workspace.All<AssignJob>(x => x.AssignedTo == userId && x.Status == true).ToList();
            if (AJ.Count != 0)
            { return AJ[0].AssignedTo; }
            else
                return 0;
        }
        #endregion
        #region PurchaseRequest

        public IList<PurchaseRequest> GetPurchaseRequests()
        {
            return WorkspaceFactory.CreateReadOnly().Query<PurchaseRequest>(null).ToList();
        }
        public PurchaseRequest GetPurchaseRequest(int PurchaseRequestId)
        {
            return _workspace.Single<PurchaseRequest>(x => x.Id == PurchaseRequestId, x => x.PurchaseRequestDetails.Select(y => y.ItemAccount), x => x.PurchaseRequestDetails.Select(z => z.Project));
        }
        public PurchaseRequestDetail GetPurchaseRequestDetail(int PurchaseRequestDetailId)
        {
            return _workspace.Single<PurchaseRequestDetail>(x => x.Id == PurchaseRequestDetailId);
        }
        public IList<PurchaseRequest> ListPurchaseRequests(string RequestNo, string RequestDate)
        {
            string filterExpression = "";

            filterExpression = "SELECT  *  FROM PurchaseRequests Where 1 = Case when '" + RequestNo + "' = '' Then 1 When PurchaseRequests.RequestNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When PurchaseRequests.RequestedDate = '" + RequestDate + "'  Then 1 END and PurchaseRequests.Requester='" + GetCurrentUser().Id + "' order by PurchaseRequests.Id Desc ";

            return _workspace.SqlQuery<PurchaseRequest>(filterExpression).ToList();

        }
        public IList<PurchaseRequest> ListPurchaseRequestForBids(string RequestNo, string RequestDate, string ProgressStatus)
        {
            string filterExpression = "";

            if (ProgressStatus != "Completed")
            {
                filterExpression = " SELECT  *  FROM PurchaseRequests INNER JOIN AppUsers on AppUsers.Id=PurchaseRequests.CurrentApprover  Left JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 Where 1 = Case when '" + RequestNo + "' = '' Then 1 When PurchaseRequests.RequestNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When PurchaseRequests.RequestedDate = '" + RequestDate + "'  Then 1 END AND PurchaseRequests.ProgressStatus='" + ProgressStatus + "' " +
                                       " AND  ((PurchaseRequests.CurrentApprover = '" + CurrentUser().Id + "') or (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) order by PurchaseRequests.Id DESC";
            }
            else
            {
                filterExpression = " SELECT  *  FROM PurchaseRequests INNER JOIN AppUsers on AppUsers.Id=PurchaseRequests.CurrentApprover INNER JOIN PurchaseRequestStatuses on PurchaseRequestStatuses.PurchaseRequest_Id = PurchaseRequests.Id Left JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 Where 1 = Case when '" + RequestNo + "' = '' Then 1 When PurchaseRequests.RequestNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When PurchaseRequests.RequestedDate = '" + RequestDate + "'  Then 1 END AND PurchaseRequests.ProgressStatus='" + ProgressStatus + "' AND " +
                                           "   (PurchaseRequestStatuses.ApprovalStatus Is not null AND (PurchaseRequestStatuses.Approver = '" + CurrentUser().Id + "') or (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) order by PurchaseRequests.Id DESC ";
            }
            return _workspace.SqlQuery<PurchaseRequest>(filterExpression).ToList();

        }
        public int GetLastPurchaseRequestId()
        {
            if (_workspace.Last<PurchaseRequest>() != null)
            {
                return _workspace.Last<PurchaseRequest>().Id;
            }
            else
            { return 0; }
        }

        #endregion
        #region Sole Vendor Requests
        public IList<SoleVendorRequest> GetSoleVendorRequests()
        {
            return WorkspaceFactory.CreateReadOnly().Query<SoleVendorRequest>(null).ToList();
        }
        public SoleVendorRequest GetSoleVendorRequest(int id)
        {
            return _workspace.Single<SoleVendorRequest>(x => x.Id == id);
        }
        public IList<SoleVendorRequest> ListSoleVendorRequests(string RequestNo, string RequestDate)
        {
            string filterExpression = "";

            filterExpression = "SELECT * FROM SoleVendorRequests Where 1 = Case when '" + RequestNo + "' = '' Then 1 When SoleVendorRequests.RequestNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When SoleVendorRequests.RequestDate = '" + RequestDate + "'  Then 1 END And SoleVendorRequests.AppUser_Id='" + GetCurrentUser().Id + "' order by SoleVendorRequests.Id Desc ";

            return _workspace.SqlQuery<SoleVendorRequest>(filterExpression).ToList();

        }
        public SoleVendorRequestDetail GetSoleVendorRequestDetail(int id)
        {
            return _workspace.Single<SoleVendorRequestDetail>(x => x.Id == id);
        }
        public int GetLastSoleVendorRequestId()
        {
            if (_workspace.Last<SoleVendorRequest>() != null)
            {
                return _workspace.Last<SoleVendorRequest>().Id;
            }
            else { return 0; }
        }

        #endregion
        #region Bid Analysis Requests
        public IList<BidAnalysisRequest> GetBidAnalysisRequests()
        {
            return WorkspaceFactory.CreateReadOnly().Query<BidAnalysisRequest>(null).ToList();
        }
        public BidAnalysisRequest GetBidAnalysisRequest(int id)
        {
            return _workspace.Single<BidAnalysisRequest>(x => x.Id == id);
        }
        public BidderItemDetail GetBiderItem(int id)
        {
            return _workspace.Single<BidderItemDetail>(x => x.Id == id);
        }
        public IList<BidAnalysisRequest> ListBidAnalysisRequests(string RequestNo, string RequestDate)
        {
            string filterExpression = "";

            filterExpression = "SELECT * FROM BidAnalysisRequests Where 1 = Case when '" + RequestNo + "' = '' Then 1 When BidAnalysisRequests.RequestNo = '" + RequestNo + "'  Then 1 END And  1 = Case when '" + RequestDate + "' = '' Then 1 When BidAnalysisRequests.RequestDate = '" + RequestDate + "'  Then 1 END And BidAnalysisRequests.AppUser_Id='" + GetCurrentUser().Id + "' order by BidAnalysisRequests.Id Desc ";

            return _workspace.SqlQuery<BidAnalysisRequest>(filterExpression).ToList();

        }
        public int GetLastBidAnalysisRequestId()
        {
            if (_workspace.Last<BidAnalysisRequest>() != null)
            {
                return _workspace.Last<BidAnalysisRequest>().Id;
            }
            else { return 0; }
        }

        #endregion

        #region Entity Manipulation
        public void SaveOrUpdateEntity<T>(T item) where T : class
        {
            IEntity entity = (IEntity)item;
            if (entity.Id == 0)
                _workspace.Add<T>(item);
            else
                _workspace.Update<T>(item);

            _workspace.CommitChanges();
            _workspace.Refresh(item);
        }
        public void DeleteEntity<T>(T item) where T : class
        {
            _workspace.Delete<T>(item);
            _workspace.CommitChanges();
            _workspace.Refresh(item);
        }

        public void Commit()
        {
            _workspace.CommitChanges();
        }
        #endregion               
    }
}
