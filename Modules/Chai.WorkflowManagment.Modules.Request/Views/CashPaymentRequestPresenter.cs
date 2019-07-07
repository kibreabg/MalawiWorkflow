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
    public class CashPaymentRequestPresenter : Presenter<ICashPaymentRequestView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private RequestController _controller;
        private AdminController _adminController;
        private SettingController _settingController;
        private CashPaymentRequest _CashPaymentRequest;
        public CashPaymentRequestPresenter([CreateNew] RequestController controller, AdminController adminController, SettingController settingController)
        {
            _controller = controller;
            _adminController = adminController;
            _settingController = settingController;
        }
        public override void OnViewLoaded()
        {
            if (View.GetCashPaymentRequestId > 0)
            {
                _controller.CurrentObject = _controller.GetCashPaymentRequest(View.GetCashPaymentRequestId);
            }
            CurrentCashPaymentRequest = _controller.CurrentObject as CashPaymentRequest;
        }
        public override void OnViewInitialized()
        {
            if (_CashPaymentRequest == null)
            {
                int id = View.GetCashPaymentRequestId;
                if (id > 0)
                    _controller.CurrentObject = _controller.GetCashPaymentRequest(id);
                else
                    _controller.CurrentObject = new CashPaymentRequest();
            }
        }
        public CashPaymentRequest CurrentCashPaymentRequest
        {
            get
            {
                if (_CashPaymentRequest == null)
                {
                    int id = View.GetCashPaymentRequestId;
                    if (id > 0)
                        _CashPaymentRequest = _controller.GetCashPaymentRequest(id);
                    else
                        _CashPaymentRequest = new CashPaymentRequest();
                }
                return _CashPaymentRequest;
            }
            set { _CashPaymentRequest = value; }


        }
        public IList<CashPaymentRequest> GetCashPaymentRequests()
        {
            return _controller.GetCashPaymentRequests();
        }
        private void SaveCashPaymentRequestStatus()
        {
            if (GetApprovalSetting(RequestType.CashPayment_Request.ToString().Replace('_', ' '), CurrentCashPaymentRequest.TotalAmount) != null)
            {
                int i = 1;
                foreach (ApprovalLevel AL in GetApprovalSetting(RequestType.CashPayment_Request.ToString().Replace('_', ' '), CurrentCashPaymentRequest.TotalAmount).ApprovalLevels)
                {
                    CashPaymentRequestStatus CPRS = new CashPaymentRequestStatus();
                    CPRS.CashPaymentRequest = CurrentCashPaymentRequest;
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
                    else if (AL.EmployeePosition.PositionName == "Program Manager")
                    {
                        if (CurrentCashPaymentRequest.CashPaymentRequestDetails[0].Project.Id != 0)
                        {
                            CPRS.Approver = GetProject(CurrentCashPaymentRequest.CashPaymentRequestDetails[0].Project.Id).AppUser.Id;
                        }
                    }
                    else
                    {
                        if (Approver(AL.EmployeePosition.Id) != null)
                        {
                            if (AL.EmployeePosition.PositionName == "Finance Officer")
                            {
                                CPRS.ApproverPosition = AL.EmployeePosition.Id; //So that we can entertain more than one finance manager to handle the request
                            }
                            else
                            {
                                CPRS.Approver = Approver(AL.EmployeePosition.Id).Id;
                            }
                        }
                        else
                            CPRS.Approver = 0;
                    }
                    CPRS.WorkflowLevel = i;
                    i++;
                    CurrentCashPaymentRequest.CashPaymentRequestStatuses.Add(CPRS);
                }
            }
        }
        private void GetCurrentApprover()
        {
            if (CurrentCashPaymentRequest.CashPaymentRequestStatuses != null)
            {
                foreach (CashPaymentRequestStatus CPRS in CurrentCashPaymentRequest.CashPaymentRequestStatuses)
                {
                    if (CPRS.ApprovalStatus == null)
                    {
                        SendEmail(CPRS);
                        CurrentCashPaymentRequest.CurrentApprover = CPRS.Approver;
                        CurrentCashPaymentRequest.CurrentLevel = CPRS.WorkflowLevel;
                        CurrentCashPaymentRequest.CurrentStatus = CPRS.ApprovalStatus;
                        break;
                    }
                }
            }
        }
        public void SaveOrUpdateCashPaymentRequest()
        {
            CashPaymentRequest CashPaymentRequest = CurrentCashPaymentRequest;
            if (CashPaymentRequest.Id <= 0)
            {
                CashPaymentRequest.RequestNo = View.GetRequestNo;
                CashPaymentRequest.VoucherNo = View.GetVoucherNo;
            }
            CashPaymentRequest.RequestDate = Convert.ToDateTime(DateTime.Today.ToShortDateString());
            CashPaymentRequest.Payee = "";
            CashPaymentRequest.Description = View.GetDescription;
            CashPaymentRequest.AmountType = View.GetAmountType;
            CashPaymentRequest.PaymentMethod = View.GetPaymentMethod;
            CashPaymentRequest.ProgressStatus = ProgressStatus.InProgress.ToString();
            CashPaymentRequest.AppUser = _adminController.GetUser(CurrentUser().Id);
            CashPaymentRequest.Supplier = _settingController.GetSupplier(View.GetPayee);
            if (View.GetAmountType != "Actual Amount")
            {
                CashPaymentRequest.PaymentReimbursementStatus = "Not Retired";
            }
            else
            {
                CashPaymentRequest.PaymentReimbursementStatus = "Retired";
                CashPaymentRequest.TotalActualExpendture = CashPaymentRequest.TotalAmount;
            }

            CashPaymentRequest.ExportStatus = "Not Exported";
            if (CurrentCashPaymentRequest.CashPaymentRequestStatuses.Count == 0)
                SaveCashPaymentRequestStatus();

            GetCurrentApprover();

            _controller.SaveOrUpdateEntity(CashPaymentRequest);


        }
        public void SaveOrUpdateCashPaymentRequest(CashPaymentRequest CashPaymentRequest)
        {
            _controller.SaveOrUpdateEntity(CashPaymentRequest);
            _controller.CurrentObject = null;
        }
        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Request/Default.aspx?{0}=3", AppConstants.TABID));
        }
        public void DeleteCashPaymentRequest(CashPaymentRequest CashPaymentRequest)
        {
            _controller.DeleteEntity(CashPaymentRequest);
        }
        public void DeleteCashPaymentRequestDetail(CashPaymentRequestDetail CashPaymentRequestDetail)
        {
            _controller.DeleteEntity(CashPaymentRequestDetail);
        }
        public CashPaymentRequest GetCashPaymentRequest(int id)
        {
            return _controller.GetCashPaymentRequest(id);
        }
        public int GetLastCashPaymentRequestId()
        {
            return _controller.GetLastCashPaymentRequestId();
        }
        public IList<CashPaymentRequest> ListCashPaymentRequests(string RequestNo, string RequestDate)
        {
            return _controller.ListCashPaymentRequests(RequestNo, RequestDate);
        }
        public CashPaymentRequestDetail GetCashPaymentRequestDetail(int CPRDId)
        {
            return _controller.GetCashPaymentRequestDetail(CPRDId);
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
        public ItemAccount GetItemAccount(int ItemAccountId)
        {
            return _settingController.GetItemAccount(ItemAccountId);
        }
        public Project GetProject(int ProjectId)
        {
            return _settingController.GetProject(ProjectId);
        }
        public Grant GetGrant(int GrantId)
        {
            return _settingController.GetGrant(GrantId);
        }
        public IList<Supplier> GetSuppliers()
        {
            return _settingController.GetSuppliers();
        }
        public IList<Project> ListProjects()
        {
            return _settingController.GetProjects();
        }
        public IList<Grant> ListGrants()
        {
            return _settingController.GetGrants();
        }
        public IList<Grant> GetGrantbyprojectId(int projectId)
        {
            return _settingController.GetProjectGrantsByprojectId(projectId);

        }
        public IList<ItemAccount> ListItemAccounts()
        {
            return _settingController.GetItemAccounts();
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
        private void SendEmail(CashPaymentRequestStatus CPRS)
        {
            if (GetSuperviser(CPRS.Approver).IsAssignedJob != true)
            {
                EmailSender.Send(GetSuperviser(CPRS.Approver).Email, "Cash Payment Request", (CurrentCashPaymentRequest.AppUser.FullName).ToUpper() + "Requests for Cash Payment with Request No. - '" + (CurrentCashPaymentRequest.RequestNo).ToUpper() + "'");
            }
            else
            {
                EmailSender.Send(GetSuperviser(_controller.GetAssignedJobbycurrentuser(CPRS.Approver).AssignedTo).Email, "Cash Payment Request", (CurrentCashPaymentRequest.AppUser.FullName).ToUpper() + " Requests for Cash Payment with Request No. - '" + (CurrentCashPaymentRequest.RequestNo).ToUpper() + "'");
            }
        }
        public void Commit()
        {
            _controller.Commit();
        }

    }
}




