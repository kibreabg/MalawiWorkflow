using System;
using System.Web.SessionState;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb.Web;
using System.Linq;
using System.Linq.Expressions;

using Chai.WorkflowManagment.CoreDomain;
using Chai.WorkflowManagment.CoreDomain.DataAccess;
using Chai.WorkflowManagment.CoreDomain.Admins;
using Chai.WorkflowManagment.Shared.Navigation;
using Chai.WorkflowManagment.Services;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.CoreDomain.Request;
using Chai.WorkflowManagment.CoreDomain.Users;
using System.Collections.Generic;
using Chai.WorkflowManagment.Enums;

namespace Chai.WorkflowManagment.Modules.Shell
{
    public class ShellController : ControllerBase
    {
        private IWorkspace _workspace;
        private int currentUser;
        [InjectionConstructor]
        public ShellController([ServiceDependency] IHttpContextLocatorService httpContextLocatorService,
           [ServiceDependency] INavigationService navigationService)
            : base(httpContextLocatorService,navigationService)
        {
            _workspace = ZadsServices.Workspace;
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
        public AssignJob GetAssignedJobbycurrentuser()
        {
            int userId = GetCurrentUser().Id;
            IList<AssignJob> AJ = _workspace.All<AssignJob>(x => x.AssignedTo == userId && x.Status == true).ToList();
                    return AJ[0]; 
           

        }
        public AppUser GetUserByUserName(string userName)
        {
            return _workspace.Single<AppUser>(x => x.UserName == userName, x => x.AppUserRoles.Select(y => y.Role));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        public Node ActiveNode(int nodeid)
        {
            using (var vr = WorkspaceFactory.CreateReadOnly())
            {
                return vr.Single<Node>(x => x.Id == nodeid, x => x.NodeRoles.Select(y => y.Role));     
            }
        }
        public Tab ActiveTab(int tabid)
        {
            using (var vr = WorkspaceFactory.CreateReadOnly())
            {
                return vr.Single<Tab>(x => x.Id == tabid, x => x.PocModule, x => x.TabRoles.Select(z => z.Role), x => x.TaskPans.Select(y => y.TaskPanNodes.Select(w => w.Node.PocModule)), x => x.TaskPans.Select(y => y.TaskPanNodes.Select(w => w.Node.NodeRoles.Select(a => a.Role) )));
            }
        }
        #region ReimbersmentStatus

        public int GetCashPaymentReimbersment()
        {
            currentUser = GetCurrentUser().Id;
            int Count = 0;
            Count = WorkspaceFactory.CreateReadOnly().Count<CashPaymentRequest>(x => x.PaymentReimbursementStatus == "Not Retired" && x.ProgressStatus == "Completed");
            if (Count != 0)
                return Count;
            else
                return 0;

        }
        public int GetBankPaymentReimbersment()
        {
            currentUser = GetCurrentUser().Id;
            int Count = 0;
            Count = WorkspaceFactory.CreateReadOnly().Count<OperationalControlRequest>(x => x.PaymentReimbursementStatus == "Not Retired" && x.ProgressStatus == "Completed");
            if (Count != 0)
                return Count;
            else
                return 0;

        }
        public int GetCostSharingPaymentReimbersment()
        {
            currentUser = GetCurrentUser().Id;
            int Count = 0;
            Count = WorkspaceFactory.CreateReadOnly().Count<CostSharingRequest>(x => x.PaymentReimbursementStatus == "Not Retired" && x.ProgressStatus == "Completed");
            if (Count != 0)
                return Count;
            else
                return 0;

        }
        #endregion
        #region MyTasks
        public int GetLeaveTasks()
        {
            currentUser = GetCurrentUser().Id;
            string filterExpression = "";
            filterExpression = " SELECT  *  FROM LeaveRequests INNER JOIN AppUsers on AppUsers.Id=LeaveRequests.CurrentApprover Left JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 Where LeaveRequests.ProgressStatus='InProgress' " +
                                   " AND  ((LeaveRequests.CurrentApprover = '" + currentUser + "') or (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) order by LeaveRequests.Id ";

            return _workspace.SqlQuery<LeaveRequest>(filterExpression).Count();
        }
        public int GetVehicleTasks()
        {
            string filterExpression = "";

            filterExpression = " SELECT  *  FROM VehicleRequests INNER JOIN AppUsers on AppUsers.Id=VehicleRequests.CurrentApprover Left JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1  Where VehicleRequests.ProgressStatus='InProgress' " +
                                   " AND  (VehicleRequests.CurrentApprover = '" + currentUser + "') or (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "') order by VehicleRequests.Id ";

            return _workspace.SqlQuery<VehicleRequest>(filterExpression).Count();
        }       
        public int GetCashPaymentRequestTasks()
        {
            currentUser = GetCurrentUser().Id;
            string filterExpression = "";

            filterExpression = " SELECT * FROM CashPaymentRequests " +
                                    " LEFT JOIN AppUsers on AppUsers.Id = CashPaymentRequests.CurrentApprover " + 
                                    " LEFT JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 " +
                                    " WHERE CashPaymentRequests.ProgressStatus = 'InProgress'" + 
                                        " AND ((CashPaymentRequests.CurrentApprover = '" + currentUser + "')" +
                                        " OR (CashPaymentRequests.CurrentApproverPosition = '" + GetCurrentUser().EmployeePosition.Id + "')" +
                                        " OR (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "'))" +
                                        " ORDER BY CashPaymentRequests.Id";

            return _workspace.SqlQuery<CashPaymentRequest>(filterExpression).Count();
        }
        public int GetCostSharingRequestTasks()
        {
            currentUser = GetCurrentUser().Id;
            string filterExpression = "";
            
            filterExpression = " SELECT * FROM CostSharingRequests " +
                                    " LEFT JOIN AppUsers on AppUsers.Id = CostSharingRequests.CurrentApprover " +
                                    " LEFT JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 " +
                                    " WHERE CostSharingRequests.ProgressStatus = 'InProgress'" +
                                        " AND ((CostSharingRequests.CurrentApprover = '" + currentUser + "')" +
                                        " OR (CostSharingRequests.CurrentApproverPosition = '" + GetCurrentUser().EmployeePosition.Id + "')" +
                                        " OR (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "'))" +
                                        " ORDER BY CostSharingRequests.Id";

            return _workspace.SqlQuery<CostSharingRequest>(filterExpression).Count();
        }
        public int GetTravelAdvanceRequestTasks()
        {
            currentUser = GetCurrentUser().Id;
            string filterExpression = "";
            
            filterExpression = " SELECT * FROM TravelAdvanceRequests " +
                                    " LEFT JOIN AppUsers on AppUsers.Id = TravelAdvanceRequests.CurrentApprover " +
                                    " LEFT JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 " +
                                    " WHERE TravelAdvanceRequests.ProgressStatus = 'InProgress'" +
                                        " AND ((TravelAdvanceRequests.CurrentApprover = '" + currentUser + "')" +
                                        " OR (TravelAdvanceRequests.CurrentApproverPosition = '" + GetCurrentUser().EmployeePosition.Id + "')" +
                                        " OR (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "'))" +
                                        " ORDER BY TravelAdvanceRequests.RequestDate";
            return _workspace.SqlQuery<TravelAdvanceRequest>(filterExpression).Count();
        }
        public int GetPurchaseRequestsTasks()
        {
            currentUser = GetCurrentUser().Id;
            string filterExpression = "";

            filterExpression = " SELECT  *  FROM PurchaseRequests INNER JOIN AppUsers on AppUsers.Id=PurchaseRequests.CurrentApprover Left JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 Where PurchaseRequests.ProgressStatus='InProgress' " +
                                   " AND  ((PurchaseRequests.CurrentApprover = '" + currentUser + "') or (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) order by PurchaseRequests.Id ";

            return _workspace.SqlQuery<PurchaseRequest>(filterExpression).Count();

        }
        public int GetReviewExpenseLiquidationRequestsTasks()
        {
            currentUser = GetCurrentUser().Id;
            string filterExpression = "";

            filterExpression = " SELECT * FROM ExpenseLiquidationRequests INNER JOIN AppUsers on AppUsers.Id = ExpenseLiquidationRequests.CurrentApprover Left JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 Where ExpenseLiquidationRequests.ProgressStatus='InProgress' " +
                                   " AND (ExpenseLiquidationRequests.CurrentStatus != 'Rejected' OR ExpenseLiquidationRequests.CurrentStatus IS NULL) AND ((ExpenseLiquidationRequests.CurrentApprover = '" + currentUser + "') or (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) order by ExpenseLiquidationRequests.Id ";

            return _workspace.SqlQuery<ExpenseLiquidationRequest>(filterExpression).Count();
        }       
        public int GetExpenseLiquidationRequestsTasks()
        {
            currentUser = GetCurrentUser().Id;
            int Count = 0;
            Count = WorkspaceFactory.CreateReadOnly().Count<TravelAdvanceRequest>(x => x.AppUser.Id == currentUser && x.ExpenseLiquidationStatus == "Completed");
            if (Count != 0)
                return Count;
            else
                return 0;

        }
        public int GetBankPaymentTasks()
        {
            currentUser = GetCurrentUser().Id;
            string filterExpression = "";

            filterExpression = " SELECT * FROM OperationalControlRequests INNER JOIN AppUsers on AppUsers.Id = OperationalControlRequests.CurrentApprover Left JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 Where OperationalControlRequests.ProgressStatus='InProgress' " +
                                  " AND  ((OperationalControlRequests.CurrentApprover = '" + currentUser+ "') or (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) order by OperationalControlRequests.Id ";

            return _workspace.SqlQuery<OperationalControlRequest>(filterExpression).Count();
        }


        public int GetBidAnalysisTasks()
        {
            currentUser = GetCurrentUser().Id;
            string filterExpression = "";

            filterExpression = " SELECT * FROM BidAnalysisRequests INNER JOIN AppUsers on AppUsers.Id = BidAnalysisRequests.CurrentApprover Left JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 Where BidAnalysisRequests.ProgressStatus='InProgress' " +
                                  " AND  ((BidAnalysisRequests.CurrentApprover = '" + currentUser + "') or (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) order by BidAnalysisRequests.Id ";

            return _workspace.SqlQuery<BidAnalysisRequest>(filterExpression).Count();
        }

        public int GetSoleVendorTasks()
        {
            currentUser = GetCurrentUser().Id;
            string filterExpression = "";

            filterExpression = " SELECT * FROM SoleVendorRequests INNER JOIN AppUsers on AppUsers.Id = SoleVendorRequests.CurrentApprover Left JOIN AssignJobs on AssignJobs.AppUser_Id = AppUsers.Id AND AssignJobs.Status = 1 Where SoleVendorRequests.ProgressStatus='InProgress' " +
                                  " AND  ((SoleVendorRequests.CurrentApprover = '" + currentUser + "') or (AssignJobs.AssignedTo = '" + GetAssignedUserbycurrentuser() + "')) order by SoleVendorRequests.Id ";

            return _workspace.SqlQuery<SoleVendorRequest>(filterExpression).Count();
        }

    
        #endregion
        #region MyRequests
        public int GetLeaveMyRequest()
        {
            currentUser = GetCurrentUser().Id;
            int Count = 0;
            Count = WorkspaceFactory.CreateReadOnly().Count<LeaveRequest>(x => x.Requester == currentUser && x.ProgressStatus == "InProgress");
            if (Count != 0)
                return Count;
            else
                return 0;

        }
        public int GetVehicleMyRequest()
        {
            currentUser = GetCurrentUser().Id;
            int Count = 0;
            Count = WorkspaceFactory.CreateReadOnly().Count<VehicleRequest>(x => x.AppUser.Id == currentUser && x.ProgressStatus == "InProgress");
            if (Count != 0)
                return Count;
            else
                return 0;

        }
        public int GetCashPaymentRequestMyRequests()
        {
            currentUser = GetCurrentUser().Id;
            int Count = 0;
            Count = WorkspaceFactory.CreateReadOnly().Count<CashPaymentRequest>(x => x.AppUser.Id == currentUser && x.ProgressStatus == "InProgress");
            if (Count != 0)
                return Count;
            else
                return 0;

        }        
        public int GetCostSharingRequestMyRequests()
        {
            currentUser = GetCurrentUser().Id;
            int Count = 0;
            Count = WorkspaceFactory.CreateReadOnly().Count<CostSharingRequest>(x => x.AppUser.Id == currentUser && x.ProgressStatus == "InProgress");
            if (Count != 0)
                return Count;
            else
                return 0;

        }
        public int GetTravelAdvanceRequestMyRequest()
        {
            currentUser = GetCurrentUser().Id;
            int Count = 0;
            Count = WorkspaceFactory.CreateReadOnly().Count<TravelAdvanceRequest>(x => x.AppUser.Id == currentUser && x.ProgressStatus == "InProgress");
            if (Count != 0)
                return Count;
            else
                return 0;

        }
        public int GetPurchaseRequestsMyRequest()
        {
            currentUser = GetCurrentUser().Id;
            int Count = 0;
            Count = WorkspaceFactory.CreateReadOnly().Count<PurchaseRequest>(x => x.Requester == currentUser && x.ProgressStatus == "InProgress");
            if (Count != 0)
                return Count;
            else
                return 0;

        }
        public int GetBankRequestsMyRequest()
        {
            currentUser = GetCurrentUser().Id;
            int Count = 0;
            Count = WorkspaceFactory.CreateReadOnly().Count<OperationalControlRequest>(x => x.AppUser.Id == currentUser && x.ProgressStatus == "InProgress");
            if (Count != 0)
                return Count;
            else
                return 0;

        }

        public int GetBidAnalysisRequestsMyRequest()
        {
            currentUser = GetCurrentUser().Id;
            int Count = 0;
            Count = WorkspaceFactory.CreateReadOnly().Count<BidAnalysisRequest>(x => x.AppUser.Id == currentUser && x.ProgressStatus == "InProgress");
            if (Count != 0)
                return Count;
            else
                return 0;

        }
        public int GetSoleVendorRequestsMyRequest()
        {
            currentUser = GetCurrentUser().Id;
            int Count = 0;
            Count = WorkspaceFactory.CreateReadOnly().Count<SoleVendorRequest>(x => x.AppUser.Id == currentUser && x.ProgressStatus == "InProgress");
            if (Count != 0)
                return Count;
            else
                return 0;

        }
        #endregion
        #region MyProgresses
        public IList<VehicleRequest> GetVehicleInProgress()
        {
            currentUser = GetCurrentUser().Id;
            IList<VehicleRequest> vehicleRequests = WorkspaceFactory.CreateReadOnly().Query<VehicleRequest>(x => x.AppUser.Id == currentUser && x.ProgressStatus == "InProgress").ToList();
            return vehicleRequests;
        }

        public IList<CashPaymentRequest> GetCashPaymentsInProgress()
        {
            currentUser = GetCurrentUser().Id;
            IList<CashPaymentRequest> cashPaymentRequests = WorkspaceFactory.CreateReadOnly().Query<CashPaymentRequest>(x => x.AppUser.Id == currentUser && x.ProgressStatus == "InProgress").ToList();
            return cashPaymentRequests;
        }

        public IList<CostSharingRequest> GetCostSharingInProgress()
        {
            currentUser = GetCurrentUser().Id;
            IList<CostSharingRequest> costSharingRequests = WorkspaceFactory.CreateReadOnly().Query<CostSharingRequest>(x => x.AppUser.Id == currentUser && x.ProgressStatus == "InProgress").ToList();
            return costSharingRequests;
        }

        public IList<TravelAdvanceRequest> GetTravelAdvanceInProgress()
        {
            currentUser = GetCurrentUser().Id;
            IList<TravelAdvanceRequest> travelAdvanceRequests = WorkspaceFactory.CreateReadOnly().Query<TravelAdvanceRequest>(x => x.AppUser.Id == currentUser && x.ProgressStatus == "InProgress").ToList();
            return travelAdvanceRequests;
        }

        public IList<PurchaseRequest> GetPurchaseInProgress()
        {
            currentUser = GetCurrentUser().Id;
            IList<PurchaseRequest> purchaseRequests = WorkspaceFactory.CreateReadOnly().Query<PurchaseRequest>(x => x.Requester == currentUser && x.ProgressStatus == "InProgress").ToList();
            return purchaseRequests;
        }

        public IList<BankPaymentRequest> GetBankPaymentInProgress()
        {
            currentUser = GetCurrentUser().Id;
            IList<BankPaymentRequest> bankPaymentRequests = WorkspaceFactory.CreateReadOnly().Query<BankPaymentRequest>(x => x.AppUser.Id == currentUser && x.ProgressStatus == "InProgress").ToList();
            return bankPaymentRequests;
        }

        public IList<BidAnalysisRequest> GetBidAnalysisInProgress()
        {
            currentUser = GetCurrentUser().Id;
            IList<BidAnalysisRequest> bidAnalyisRequests = WorkspaceFactory.CreateReadOnly().Query<BidAnalysisRequest>(x => x.AppUser.Id == currentUser && x.ProgressStatus == "InProgress").ToList();
            return bidAnalyisRequests;
        }
        public IList<SoleVendorRequest> GetSoleVendorInProgress()
        {
            currentUser = GetCurrentUser().Id;
            IList<SoleVendorRequest> soleVendorRequests = WorkspaceFactory.CreateReadOnly().Query<SoleVendorRequest>(x => x.AppUser.Id == currentUser && x.ProgressStatus == "InProgress").ToList();
            return soleVendorRequests;
        }

        public IList<LeaveRequest> GetLeaveInProgress()
        {
            currentUser = GetCurrentUser().Id;
            IList<LeaveRequest> leaveRequests = WorkspaceFactory.CreateReadOnly().Query<LeaveRequest>(x => x.Requester == currentUser && x.ProgressStatus == "InProgress").ToList();
            return leaveRequests;
        }
        #endregion
        public AppUser GetUser(int userid)
        {
            return _workspace.Single<AppUser>(x => x.Id == userid, x => x.AppUserRoles.Select(y => y.Role));
        }
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
    }
}
