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
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared.MailSender;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public class BankPaymentRequestPresenter : Presenter<IBankPaymentRequestView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private RequestController _controller;
        private AdminController _adminController;
        private SettingController _settingController;
        private BankPaymentRequest _BankPaymentRequest;
        public BankPaymentRequestPresenter([CreateNew] RequestController controller, AdminController adminController, SettingController settingController)
        {
            _controller = controller;
            _adminController = adminController;
            _settingController = settingController;
        }
        public override void OnViewLoaded()
        {
            if (View.GetBankPaymentRequestId > 0)
            {
                _controller.CurrentObject = _controller.GetBankPaymentRequest(View.GetBankPaymentRequestId);
            }
            CurrentBankPaymentRequest = _controller.CurrentObject as BankPaymentRequest;
        }
        public override void OnViewInitialized()
        {
            if (_BankPaymentRequest == null)
            {
                int id = View.GetBankPaymentRequestId;
                if (id > 0)
                    _controller.CurrentObject = _controller.GetBankPaymentRequest(id);
                else
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
                        _BankPaymentRequest = _controller.GetBankPaymentRequest(id);
                    else
                        _BankPaymentRequest = new BankPaymentRequest();
                }
                return _BankPaymentRequest;
            }
            set { _BankPaymentRequest = value; }


        }
        public IList<BankPaymentRequest> GetBankPaymentRequests()
        {
            return _controller.GetBankPaymentRequests();
        }
        private void SaveBankPaymentRequestStatus()
        {
            if (GetApprovalSetting(RequestType.BankPayment_Request.ToString().Replace('_', ' '), 0) != null)
            {
                int i = 1;
                foreach (ApprovalLevel AL in GetApprovalSetting(RequestType.BankPayment_Request.ToString().Replace('_', ' '), 0).ApprovalLevels)
                {
                    BankPaymentRequestStatus CPRS = new BankPaymentRequestStatus();
                    CPRS.BankPaymentRequest = CurrentBankPaymentRequest;
                    //All Approver positions must be entered into the database before the approval workflow could run effectively!
                    if (AL.EmployeePosition.PositionName == "Superviser/Line Manager")
                    {
                        if (CurrentUser().Superviser != 0)
                            CPRS.Approver = CurrentUser().Superviser.Value;
                        else
                        {
                            CPRS.ApprovalStatus = ApprovalStatus.Approved.ToString();
                            CPRS.Date = Convert.ToDateTime(DateTime.Today.Date.ToShortDateString());
                        }
                    }
                    else
                    {
                        if (Approver(AL.EmployeePosition.Id) != null)
                            CPRS.Approver = Approver(AL.EmployeePosition.Id).Id;
                        else
                            CPRS.Approver = 0;
                    }
                    CPRS.WorkflowLevel = i;
                    i++;
                    CurrentBankPaymentRequest.BankPaymentRequestStatuses.Add(CPRS);
                }
            }
        }
        private void GetCurrentApprover()
        {
            if (CurrentBankPaymentRequest.BankPaymentRequestStatuses != null)
            {
                foreach (BankPaymentRequestStatus CPRS in CurrentBankPaymentRequest.BankPaymentRequestStatuses)
                {
                    if (CPRS.ApprovalStatus == null)
                    {
                        SendEmail(CPRS);
                        CurrentBankPaymentRequest.CurrentApprover = CPRS.Approver;
                        CurrentBankPaymentRequest.CurrentLevel = CPRS.WorkflowLevel;
                        CurrentBankPaymentRequest.CurrentStatus = CPRS.ApprovalStatus;
                        break;
                    }
                }
            }
        }
        public void SaveOrUpdateBankPaymentRequest()
        {
            BankPaymentRequest BankPaymentRequest = CurrentBankPaymentRequest;
            BankPaymentRequest.RequestNo = View.GetRequestNo;
            BankPaymentRequest.ProcessDate = Convert.ToDateTime(DateTime.Today.ToShortDateString());
            BankPaymentRequest.ProgressStatus = ProgressStatus.InProgress.ToString();
            BankPaymentRequest.AppUser = _adminController.GetUser(CurrentUser().Id);
            BankPaymentRequest.PaymentMethod = View.GetPaymentMethod;
            if (CurrentBankPaymentRequest.BankPaymentRequestStatuses.Count == 0)
                SaveBankPaymentRequestStatus();
            GetCurrentApprover();

            _controller.SaveOrUpdateEntity(BankPaymentRequest);
        }
        public void SaveOrUpdateBankPaymentRequest(BankPaymentRequest BankPaymentRequest)
        {
            _controller.SaveOrUpdateEntity(BankPaymentRequest);
        }
        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Request/Default.aspx?{0}=3", AppConstants.TABID));
        }
        public IList<Account> ListAccounts()
        {
            return _settingController.GetAccounts();
        }
        public Account GetAccount(int id)
        {
            return _settingController.GetAccount(id);
        }
        public void DeleteBankPaymentRequest(BankPaymentRequest BankPaymentRequest)
        {
            _controller.DeleteEntity(BankPaymentRequest);
        }
        public void DeleteBankPaymentRequestDetail(BankPaymentRequestDetail BankPaymentRequestDetail)
        {
            _controller.DeleteEntity(BankPaymentRequestDetail);
        }
        public BankPaymentRequest GetBankPaymentRequest(int id)
        {
            return _controller.GetBankPaymentRequest(id);
        }
        public int GetLastBankPaymentRequestId()
        {
            return _controller.GetLastBankPaymentRequestId();
        }
        public IList<BankPaymentRequest> ListBankPaymentRequests(string RequestNo, string RequestDate)
        {
            return _controller.ListBankPaymentRequests(RequestNo, RequestDate);
        }
        public BankPaymentRequestDetail GetBankPaymentRequestDetail(int CPRDId)
        {
            return _controller.GetBankPaymentRequestDetail(CPRDId);
        }
        public AppUser Approver(int Position)
        {
            return _controller.Approver(Position);
        }
        public IList<AppUser> GetUsers()
        {
            return _adminController.GetUsers();
        }
        public AppUser GetUser(int id)
        {
            return _adminController.GetUser(id);
        }
        public AppUser CurrentUser()
        {
            return _controller.GetCurrentUser();
        }
        public AppUser GetSuperviser(int superviser)
        {
            return _controller.GetSuperviser(superviser);
        }
        public ApprovalSetting GetApprovalSetting(string RequestType, decimal value)
        {
            return _settingController.GetApprovalSettingforProcess(RequestType, value);
        }
        private void SendEmail(BankPaymentRequestStatus CPRS)
        {
            if (GetSuperviser(CPRS.Approver).IsAssignedJob != true)
            {
                EmailSender.Send(GetSuperviser(CPRS.Approver).Email, "Bank Payment Request", (CurrentBankPaymentRequest.AppUser.FullName).ToUpper() + "' Requests for bank payment");
            }
            else
            {
                EmailSender.Send(GetSuperviser(_controller.GetAssignedJobbycurrentuser(CPRS.Approver).AssignedTo).Email, "Bank Payment Request", (CurrentBankPaymentRequest.AppUser.FullName).ToUpper() + "' Requests for bank payment");
            }
        }
        public void Commit()
        {
            _controller.Commit();
        }
    }
}




