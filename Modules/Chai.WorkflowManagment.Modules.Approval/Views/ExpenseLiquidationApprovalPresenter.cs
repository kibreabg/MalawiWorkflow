using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.Modules.Admin;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Modules.Setting;
using Chai.WorkflowManagment.Modules.Request;
using System.Data;

namespace Chai.WorkflowManagment.Modules.Approval.Views
{
    public class ExpenseLiquidationApprovalPresenter : Presenter<IExpenseLiquidationApprovalView>
    {
        private ApprovalController _controller;
        private RequestController _requestController;
        private ExpenseLiquidationRequest _expenseLiquidationRequest;
        private SettingController _settingController;
        private AdminController _adminController;

        public ExpenseLiquidationApprovalPresenter([CreateNew] ApprovalController controller, RequestController requestController, SettingController settingController, AdminController adminController)
        {
            _controller = controller;
            _requestController = requestController;
            _settingController = settingController;
            _adminController = adminController;
        }
        public override void OnViewLoaded()
        {
            if (View.GetExpenseLiquidationRequestId > 0)
            {
                _controller.CurrentObject = _requestController.GetExpenseLiquidationRequest(View.GetExpenseLiquidationRequestId);
            }
            CurrentExpenseLiquidationRequest = _controller.CurrentObject as ExpenseLiquidationRequest;
        }
        public override void OnViewInitialized()
        {

        }
        public ExpenseLiquidationRequest CurrentExpenseLiquidationRequest
        {
            get
            {
                if (_expenseLiquidationRequest == null)
                {
                    int id = View.GetExpenseLiquidationRequestId;
                    if (id > 0)
                        _expenseLiquidationRequest = _requestController.GetExpenseLiquidationRequest(id);
                    else
                        _expenseLiquidationRequest = new ExpenseLiquidationRequest();
                }
                return _expenseLiquidationRequest;
            }
            set { _expenseLiquidationRequest = value; }
        }
        public IList<AppUser> GetDrivers()
        {
            return _adminController.GetDrivers();
        }
        public IList<CarRental> GetCarRentals()
        {
            return _settingController.GetCarRentals();
        }
        public CarRental GetCarRental(int Id)
        {
            return _settingController.GetCarRental(Id);
        }
        public ELRAttachment GetAttachment(int attachmentId)
        {
            return _requestController.GetELRAttachment(attachmentId);
        }
        public AppUser GetUser(int UserId)
        {
            return _adminController.GetUser(UserId);
        }
        public void SaveOrUpdateExpenseLiquidationRequest(ExpenseLiquidationRequest ExpenseLiquidationRequest)
        {
            _controller.SaveOrUpdateEntity(ExpenseLiquidationRequest);
        }
        public ExpenseLiquidationRequest GetExpenseLiquidationRequest(int liqID)
        {
            return _requestController.GetExpenseLiquidationRequest(liqID);
        }
        public IList<ExpenseLiquidationRequest> ListExpenseLiquidationRequests(string ExpenseType, string RequestDate, string ProgressStatus)
        {
            return _controller.ListExpenseLiquidationRequests(ExpenseType, RequestDate, ProgressStatus);
        }
        public ItemAccount GetItemAccount(int id)
        {
            return _settingController.GetItemAccount(id);
        }
        public Project GetProject(int id)
        {
            return _settingController.GetProject(id);
        }
        public IList<ItemAccount> GetItemAccountList()
        {
            return _settingController.GetItemAccounts();
        }
        public IList<Project> GetProjectList()
        {
            return _settingController.GetProjects();
        }
        public TravelAdvanceRequest GetTravelAdvanceRequest(int id)
        {
            return _requestController.GetTravelAdvanceRequest(id);
        }
        public AppUser CurrentUser()
        {
            return _controller.GetCurrentUser();
        }
        public AssignJob GetAssignedJobbycurrentuser()
        {
            return _controller.GetAssignedJobbycurrentuser();
        }
        public ApprovalSetting GetApprovalSettingforProcess(string Requesttype, decimal value)
        {
            return _settingController.GetApprovalSettingforProcess(Requesttype, value);
        }
        public void navigate(string url)
        {
            _controller.Navigate(url);
        }
        public AssignJob GetAssignedJobbycurrentuser(int userId)
        {
            return _controller.GetAssignedJobbycurrentuser(userId);
        }
        public int GetAssignedUserbycurrentuser()
        {
            return _controller.GetAssignedUserbycurrentuser();
        }
        public DataSet ExportTravelAdvance(int LiquidationId)
        {
            return _controller.ExportTravelAdvance(LiquidationId);
        }
        public void Commit()
        {
            _controller.Commit();
        }

    }
}




