using System;
using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Interfaces;
using System.Linq;

using Chai.WorkflowManagment.CoreDomain.DataAccess;
using Chai.WorkflowManagment.CoreDomain.Admins;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.CoreDomain.Request;
using Chai.WorkflowManagment.CoreDomain.Requests;

namespace Chai.WorkflowManagment.Modules.Shell.MasterPages
{
    public class BaseMasterPresenter : Presenter<IBaseMasterView>
    {
        private ShellController _controller;
        private int _tabId;

        public BaseMasterPresenter()
        {
        }

        public override void OnViewLoaded()
        {
            View.CurrentUser = _controller.GetCurrentUser();
            if (!Int32.TryParse(View.TabId, out _tabId))
                _tabId = -1;
        }

        [CreateNew]
        public ShellController Controller
        {
            get
            {
                return _controller;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this._controller = value;
            }
        }

        public int TabId
        {
            get { return _tabId; }
        }

        
        public IHttpContext CurrentContext
        {
            get { return Controller.GetCurrentContext(); }
        }

        public AppUser CurrentUser
        {
            get
            {
                return Controller.GetCurrentUser();
            }
        }
        
        public bool UserIsAuthenticated
        {
            get { return Controller.UserIsAuthenticated;}
        }

        public void Navigate(string url)
        {
            Controller.Navigate(url);
        }
        
        public Tab ActiveTab
        {
            get { return Controller.ActiveTab(_tabId); }
        }

        public IEnumerable<Tab> GetListOfAllTabs()
        {
            using(var vr = WorkspaceFactory.CreateReadOnly())
            {
                return vr.Query<Tab>(null, x => x.PocModule, x => x.TabRoles.Select(z => z.Role), x => x.TaskPans, x => x.TaskPans.Select(y => y.TaskPanNodes.Select(w => w.Node).Select(e => e.NodeRoles.Select(r => r.Role))), x => x.TaskPans.Select(y => y.TaskPanNodes.Select(w => w.Node).Select(z=>z.PocModule))).ToList();
            }
        }
        #region ReimbersmentStatus

        public int GetCashPaymentReimbersment()
        {
            return _controller.GetCashPaymentReimbersment();
        }
        public int GetBankPaymentReimbersment()
        {
            return _controller.GetBankPaymentReimbersment();
        }
        public int GetCostSharingPaymentReimbersment()
        {
            return _controller.GetCostSharingPaymentReimbersment();
        }
        #endregion
        #region My Tasks
        public int GetLeaveTasks()
        {
            return _controller.GetLeaveTasks();
        }
        public int GetVehicleTasks()
        {
            return _controller.GetVehicleTasks();
        }
        public int GetCashPaymentRequestTasks()
        {
            return _controller.GetCashPaymentRequestTasks();
        }
        public int GetCostSharingRequestTasks()
        {
            return _controller.GetCostSharingRequestTasks();
        }
        public int GetPurchaseRequestsTasks()
        {
            return _controller.GetPurchaseRequestsTasks();
        }
        public int GetTravelAdvanceRequestTasks()
        {
            return _controller.GetTravelAdvanceRequestTasks();
        }
        public int GetReviewExpenseLiquidationRequestsTasks()
        {
            return _controller.GetReviewExpenseLiquidationRequestsTasks();
        }
        public int GetExpenseLiquidationRequestsTasks()
        {
            return _controller.GetExpenseLiquidationRequestsTasks();
        }
        public int GetBankPaymentRequestsTasks()
        {
            return _controller.GetBankPaymentTasks();
        }
        public int GetBidAnalysisRequestsTasks()
        {
            return _controller.GetBidAnalysisTasks();
        }
        public int GetSoleVendorRequestsTasks()
        {
            return _controller.GetSoleVendorTasks();
        }
        #endregion
        #region MyRequests
        public int GetLeaveMyRequest()
        {
            return _controller.GetLeaveMyRequest();

        }
        public int GetVehicleMyRequest()
        {
            return _controller.GetVehicleMyRequest();

        }
        public int GetCashPaymentRequestMyRequests()
        {
            return _controller.GetCashPaymentRequestMyRequests();
        }
        public int GetCostSharingRequestMyRequests()
        {
            return _controller.GetCostSharingRequestMyRequests();
        }
        public int GetTravelAdvanceRequestMyRequest()
        {
            return _controller.GetTravelAdvanceRequestMyRequest();

        }
        public int GetPurchaseRequestsMyRequest()
        {
            return _controller.GetPurchaseRequestsMyRequest();

        }
        public int GetSoleVendorRequestsMyRequest()
        {
            return _controller.GetSoleVendorRequestsMyRequest();

        }
        public int GetBidAnalysisRequestsMyRequest()
        {
            return _controller.GetBidAnalysisRequestsMyRequest();

        }
        public int GetBankRequestsMyRequest()
        {
            return _controller.GetBankRequestsMyRequest();
        }              
        public IList<LeaveRequest> ListLeaveApprovalProgress()
        {
            return _controller.GetLeaveInProgress();
        }
        public IList<VehicleRequest> ListVehicleApprovalProgress()
        {
            return _controller.GetVehicleInProgress();
        }
        public IList<CashPaymentRequest> ListPaymentApprovalProgress()
        {
            return _controller.GetCashPaymentsInProgress();
        }
        public IList<CostSharingRequest> ListCostApprovalProgress()
        {
            return _controller.GetCostSharingInProgress();
        }
        public IList<TravelAdvanceRequest> ListTravelApprovalProgress()
        {
            return _controller.GetTravelAdvanceInProgress();
        }
        public IList<PurchaseRequest> ListPurchaseApprovalProgress()
        {
            return _controller.GetPurchaseInProgress();
        }
        public IList<BankPaymentRequest> ListBankPaymentApprovalProgress()
        {
            return _controller.GetBankPaymentInProgress();
        }
        public IList<BidAnalysisRequest> ListBidAnalysisApprovalProgress()
        {
            return _controller.GetBidAnalysisInProgress();
        }
        public IList<SoleVendorRequest> ListSoleVendorApprovalProgress()
        {
            return _controller.GetSoleVendorInProgress();
        }
        #endregion
        public AppUser GetUser(int UserId)
        {
            return _controller.GetUser(UserId);
        }        
    }
}
