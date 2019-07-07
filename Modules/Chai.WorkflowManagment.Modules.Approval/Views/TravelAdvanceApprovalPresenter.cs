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

namespace Chai.WorkflowManagment.Modules.Approval.Views
{
    public class TravelAdvanceApprovalPresenter : Presenter<ITravelAdvanceApprovalView>
    {
        private ApprovalController _controller;
        private RequestController _requestController;
        private TravelAdvanceRequest _TravelAdvanceRequest;
        private SettingController _settingController;
        private AdminController _adminController;

        public TravelAdvanceApprovalPresenter([CreateNew] ApprovalController controller, RequestController requestController, SettingController settingController, AdminController adminController)
        {
            _controller = controller;
            _requestController = requestController;
            _settingController = settingController;
            _adminController = adminController;
        }

        public override void OnViewLoaded()
        {
            if (View.GetTravelAdvanceRequestId > 0)
            {
                _controller.CurrentObject = _requestController.GetTravelAdvanceRequest(View.GetTravelAdvanceRequestId);
            }
            CurrentTravelAdvanceRequest = _controller.CurrentObject as TravelAdvanceRequest;
        }

        public override void OnViewInitialized()
        {
            if (_TravelAdvanceRequest == null)
            {
                int id = View.GetTravelAdvanceRequestId;
                if (id > 0)
                    _controller.CurrentObject = _requestController.GetTravelAdvanceRequest(id);
                else
                    _controller.CurrentObject = new TravelAdvanceRequest();
            }
        }
        public TravelAdvanceRequest CurrentTravelAdvanceRequest
        {
            get
            {
                if (_TravelAdvanceRequest == null)
                {
                    int id = View.GetTravelAdvanceRequestId;
                    if (id > 0)
                        _TravelAdvanceRequest = _requestController.GetTravelAdvanceRequest(id);
                    else
                        _TravelAdvanceRequest = new TravelAdvanceRequest();
                }
                return _TravelAdvanceRequest;
            }
            set { _TravelAdvanceRequest = value; }
        }
        public AssignJob GetAssignedJobbycurrentuser()
        {
            return _controller.GetAssignedJobbycurrentuser();
        }
        public AppUser GetUser(int UserId)
        {
            return _adminController.GetUser(UserId);
        }
        public AppUser GetSuperviser(int superviser)
        {
            return _controller.GetSuperviser(superviser);
        }
        public TravelAdvanceRequest GetTravelAdvanceRequest(int reqId)
        {
            return _requestController.GetTravelAdvanceRequest(reqId);
        }
        public void SaveOrUpdateTravelAdvanceRequest(TravelAdvanceRequest TravelAdvanceRequest)
        {
            _controller.SaveOrUpdateEntity(TravelAdvanceRequest);
        }
        public IList<TravelAdvanceRequest> ListTravelAdvanceRequests(string RequestNo, string RequestDate, string ProgressStatus)
        {
            return _controller.ListTravelAdvanceRequests(RequestNo, RequestDate, ProgressStatus);
        }
        public AppUser CurrentUser()
        {
            return _controller.GetCurrentUser();
        }
        public IList<AppUser> GetAppUsersByEmployeePosition(int employeePosition)
        {
            return _settingController.GetAppUsersByEmployeePosition(employeePosition);
        }
        public void navigate(string url)
        {
            _controller.Navigate(url);
        }
        public ApprovalSetting GetApprovalSettingforProcess(string Requesttype, decimal value)
        {
            return _settingController.GetApprovalSettingforProcess(Requesttype, value);
        }
        public IList<Account> GetAccounts()
        {
            return _settingController.GetAccounts();
        }
        public Account GetAccount(int Id)
        {
            return _settingController.GetAccount(Id);
        }
        public AssignJob GetAssignedJobbycurrentuser(int userId)
        {
            return _controller.GetAssignedJobbycurrentuser(userId);
        }
        public int GetAssignedUserbycurrentuser()
        {
            return _controller.GetAssignedUserbycurrentuser();
        }
      
        public void Commit()
        {
            _controller.Commit();
        }

        
    }
}




