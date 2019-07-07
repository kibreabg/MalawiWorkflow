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
    public class OperationalControlApprovalPresenter : Presenter<IOperationalControlApprovalView>
    {
        private ApprovalController _controller;
        private RequestController _requestController;
        private OperationalControlRequest _OperationalControlRequest;
        private SettingController _settingController;
        private AdminController _adminController;

        public OperationalControlApprovalPresenter([CreateNew] ApprovalController controller, RequestController requestController, SettingController settingController, AdminController adminController)
        {
            _controller = controller;
            _requestController = requestController;
            _settingController = settingController;
            _adminController = adminController;
        }

        public override void OnViewLoaded()
        {
            if (View.GetOperationalControlRequestId > 0)
            {
                _controller.CurrentObject = _requestController.GetOperationalControlRequest(View.GetOperationalControlRequestId);
            }
            CurrentOperationalControlRequest = _controller.CurrentObject as OperationalControlRequest;
        }

        public override void OnViewInitialized()
        {
            if (_OperationalControlRequest == null)
            {
                _controller.CurrentObject = new OperationalControlRequest();
            }
        }
        public OperationalControlRequest CurrentOperationalControlRequest
        {
            get
            {
                if (_OperationalControlRequest == null)
                {
                    int id = View.GetOperationalControlRequestId;
                    if (id > 0)
                        _OperationalControlRequest = _requestController.GetOperationalControlRequest(id);
                    else
                        _OperationalControlRequest = new OperationalControlRequest();
                }
                return _OperationalControlRequest;
            }
            set { _OperationalControlRequest = value; }
        }
        public AppUser GetUser(int UserId)
        {
            return _adminController.GetUser(UserId);
        }
        public void SaveOrUpdateOperationalControlRequest(OperationalControlRequest OperationalControlRequest)
        {
            _controller.SaveOrUpdateEntity(OperationalControlRequest);
        }
        public OperationalControlRequest GetOperationalControlRequest(int reqId)
        {
            return _requestController.GetOperationalControlRequest(reqId);
        }
        public IList<OperationalControlRequest> ListOperationalControlRequests(string RequestNo, string RequestDate, string ProgressStatus)
        {
            return _controller.ListOperationalControlRequests(RequestNo, RequestDate, ProgressStatus);
        }
        public OCRAttachment GetAttachment(int attachmentId)
        {
            return _requestController.GetOCRAttachment(attachmentId);
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




