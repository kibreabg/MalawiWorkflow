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
    public class CostSharingApprovalPresenter : Presenter<ICostSharingApprovalView>
    {
        private ApprovalController _controller;
        private RequestController _requestController;
        private CostSharingRequest _costSharingRequest;
        private SettingController _settingController;
        private AdminController _adminController;

        public CostSharingApprovalPresenter([CreateNew] ApprovalController controller, RequestController requestController, SettingController settingController, AdminController adminController)
        {
            _controller = controller;
            _requestController = requestController;
            _settingController = settingController;
            _adminController = adminController;
        }

        public override void OnViewLoaded()
        {
            if (View.GetCostSharingRequestId > 0)
            {
                _controller.CurrentObject = _requestController.GetCashPaymentRequest(View.GetCostSharingRequestId);
            }
            CurrentCostSharingRequest = _controller.CurrentObject as CostSharingRequest;
        }
        public override void OnViewInitialized()
        {
            if (_costSharingRequest == null)
            {
                _controller.CurrentObject = new CostSharingRequest();
            }
        }
        public IList<CostSharingSetting> GetCostSharingSettings()
        {
            return _settingController.GetCostSharingSettings();
        }
        public void DeleteCostSharingsetting(CostSharingRequestDetail CostSharingRequestDetail)
        {
            _controller.DeleteEntity(CostSharingRequestDetail);
        }
        public CostSharingRequest CurrentCostSharingRequest
        {
            get
            {
                if (_costSharingRequest == null)
                {
                    int id = View.GetCostSharingRequestId;
                    if (id > 0)
                        _costSharingRequest = _requestController.GetCostSharingRequest(id);
                    else
                        _costSharingRequest = new CostSharingRequest();
                }
                return _costSharingRequest;
            }
            set { _costSharingRequest = value; }
        }
        public AppUser GetUser(int UserId)
        {
            return _adminController.GetUser(UserId);
        }
        public void SaveOrUpdateCostSharingRequest(CostSharingRequest CostSharingRequest)
        {
            _controller.SaveOrUpdateEntity(CostSharingRequest);
            _controller.CurrentObject = null;
        }
        public CostSharingRequest GetCostSharingRequest(int reqId)
        {
            return _requestController.GetCostSharingRequest(reqId);
        }
        public IList<CostSharingRequest> ListCostSharingRequests(string RequestNo, string RequestDate, string ProgressStatus)
        {
            return _controller.ListCostSharingRequests(RequestNo, RequestDate, ProgressStatus);
        }
        public CSRAttachment GetAttachment(int attachmentId)
        {
            return _requestController.GetCSRAttachment(attachmentId);
        }
        public Account GetAccount(int AccountId)
        {
            return _settingController.GetAccount(AccountId);
        }
        public IList<Account> GetAccounts()
        {
            return _settingController.GetAccounts();
        }
        public AppUser CurrentUser()
        {
            return _controller.GetCurrentUser();
        }
        public IList<AppUser> GetAppUsersByEmployeePosition(int employeePosition)
        {
            return _settingController.GetAppUsersByEmployeePosition(employeePosition);
        }
        public IList<Project> ListProjects()
        {
            return _settingController.GetProjects();
        }
        public Project GetProject(int ProjectId)
        {
            return _settingController.GetProject(ProjectId);
        }
        public Grant GetGrant(int GrantId)
        {
            return _settingController.GetGrant(GrantId);
        }
        public IList<Grant> GetGrantbyprojectId(int projectId)
        {
            return _settingController.GetProjectGrantsByprojectId(projectId);
        }
        public AssignJob GetAssignedJobbycurrentuser()
        {
            return _controller.GetAssignedJobbycurrentuser();
        }
        public AssignJob GetAssignedJobbycurrentuser(int userId)
        {
            return _controller.GetAssignedJobbycurrentuser(userId);
        }
        public ItemAccount GetItemAccount(int ItemAccountId)
        {
            return _settingController.GetItemAccount(ItemAccountId);
        }
        public IList<ItemAccount> ListItemAccounts()
        {
            return _settingController.GetItemAccounts();
        }
        public void navigate(string url)
        {
            _controller.Navigate(url);
        }
        public ApprovalSetting GetApprovalSettingforProcess(string Requesttype, decimal value)
        {
            return _settingController.GetApprovalSettingforProcess(Requesttype, value);
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




