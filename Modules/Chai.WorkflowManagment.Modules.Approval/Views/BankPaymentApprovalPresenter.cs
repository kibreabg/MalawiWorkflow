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
    public class BankPaymentApprovalPresenter : Presenter<IBankPaymentApprovalView>
    {
        private ApprovalController _controller;
        private RequestController _requestController;
        private BankPaymentRequest _BankPaymentRequest;
        private SettingController _settingController;
        private AdminController _adminController;

        public BankPaymentApprovalPresenter([CreateNew] ApprovalController controller, RequestController requestController, SettingController settingController, AdminController adminController)
        {
            _controller = controller;
            _requestController = requestController;
            _settingController = settingController;
            _adminController = adminController;
        }

        public override void OnViewLoaded()
        {
            if (View.GetBankPaymentRequestId > 0)
            {
                _controller.CurrentObject = _requestController.GetBankPaymentRequest(View.GetBankPaymentRequestId);
            }
            CurrentBankPaymentRequest = _controller.CurrentObject as BankPaymentRequest;
        }

        public override void OnViewInitialized()
        {
            if (_BankPaymentRequest == null)
            {
                _controller.CurrentObject = new BankPaymentRequest();
            }
        }
        public BankPaymentRequest CurrentBankPaymentRequest
        {
            get
            {
                if (_BankPaymentRequest == null)
                {
                    int id = View.GetBankPaymentRequestId;
                    if (id > 0)
                        _BankPaymentRequest = _requestController.GetBankPaymentRequest(id);
                    else
                        _BankPaymentRequest = new BankPaymentRequest();
                }
                return _BankPaymentRequest;
            }
            set { _BankPaymentRequest = value; }
        }
        public AppUser GetUser(int UserId)
        {
            return _adminController.GetUser(UserId);
        }
        public void SaveOrUpdateBankPaymentRequest(BankPaymentRequest BankPaymentRequest)
        {
            _controller.SaveOrUpdateEntity(BankPaymentRequest);
        }
        public BankPaymentRequest GetBankPaymentRequest(int reqId)
        {
            return _requestController.GetBankPaymentRequest(reqId);
        }
        public IList<BankPaymentRequest> ListBankPaymentRequests(string RequestNo, string RequestDate, string ProgressStatus)
        {
            return _controller.ListBankPaymentRequests(RequestNo, RequestDate, ProgressStatus);
        }
        public CPRAttachment GetAttachment(int attachmentId)
        {
            return _requestController.GetCPRAttachment(attachmentId);
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




