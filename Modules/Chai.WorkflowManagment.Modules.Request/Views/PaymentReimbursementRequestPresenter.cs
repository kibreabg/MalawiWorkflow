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
    public class PaymentReimbursementRequestPresenter : Presenter<IPaymentReimbursementRequestView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private RequestController _controller;
        private AdminController _adminController;
        private SettingController _settingController;
        private CashPaymentRequest _CashPaymentRequest;
        public PaymentReimbursementRequestPresenter([CreateNew] RequestController controller, AdminController adminController, SettingController settingController)
        {
            _controller = controller;
            _adminController = adminController;
            _settingController = settingController;
        }
        public override void OnViewLoaded()
        {
            if (View.GetTARequestId > 0)
            {
                _controller.CurrentObject = _controller.GetCashPaymentRequest(View.GetTARequestId);
            }
            CurrentCashPaymentRequest = _controller.CurrentObject as CashPaymentRequest;
            if (CurrentCashPaymentRequest.PaymentReimbursementRequest == null)
            { CurrentCashPaymentRequest.PaymentReimbursementRequest = new PaymentReimbursementRequest(); }
        }
        public override void OnViewInitialized()
        {
            //if (_CashPaymentRequest == null)
            //{
            //    int id = View.GetTARequestId;
            //    if (id > 0)
            //        _controller.CurrentObject = _controller.GetCashPaymentRequest(id);
            //    else
            //        _controller.CurrentObject = new CashPaymentRequest();
            //}
        }
        public CashPaymentRequest CurrentCashPaymentRequest
        {
            get
            {
                if (_CashPaymentRequest == null)
                {
                    int id = View.GetTARequestId;
                    if (id > 0)
                        _CashPaymentRequest = _controller.GetCashPaymentRequest(id);
                    else
                        _CashPaymentRequest = new CashPaymentRequest();
                }
                return _CashPaymentRequest;
            }
            set
            {
                _CashPaymentRequest = value;
            }
        }
        public IList<PaymentReimbursementRequest> GetPaymentReimbursementRequests()
        {
            return _controller.GetPaymentReimbursementRequests();
        }
        private void SavePaymentReimbursementRequestStatus()
        {
            if (GetApprovalSetting(RequestType.ExpenseLiquidation_Request.ToString().Replace('_', ' '), 0) != null)
            {
                int i = 1;
                foreach (ApprovalLevel AL in GetApprovalSetting(RequestType.ExpenseLiquidation_Request.ToString().Replace('_', ' '), 0).ApprovalLevels)
                {
                    PaymentReimbursementRequestStatus ELRS = new PaymentReimbursementRequestStatus();
                    ELRS.PaymentReimbursementRequest = _CashPaymentRequest.PaymentReimbursementRequest;

                    if (Approver(AL.EmployeePosition.Id) != null)
                        ELRS.Approver = Approver(AL.EmployeePosition.Id).Id;
                    else
                        ELRS.Approver = 0;
                    ELRS.WorkflowLevel = i;
                    i++;
                    _CashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestStatuses.Add(ELRS);
                }
            }
        }
        private void GetCurrentApprover()
        {
            if (_CashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestStatuses != null)
            {
                foreach (PaymentReimbursementRequestStatus ELRS in _CashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestStatuses)
                {
                    if (ELRS.ApprovalStatus == null)
                    {
                        SendEmail(ELRS);
                        CurrentCashPaymentRequest.PaymentReimbursementRequest.CurrentApprover = ELRS.Approver;
                        CurrentCashPaymentRequest.PaymentReimbursementRequest.CurrentLevel = ELRS.WorkflowLevel;
                        CurrentCashPaymentRequest.PaymentReimbursementRequest.CurrentStatus = ELRS.ApprovalStatus;
                        break;
                    }
                }
            }
        }
        public void SaveOrUpdatePaymentReimbursementRequest(int tarId)
        {

            CurrentCashPaymentRequest.PaymentReimbursementRequest.RequestDate = Convert.ToDateTime(DateTime.Today.ToShortDateString());
            CurrentCashPaymentRequest.PaymentReimbursementRequest.Comment = View.GetComment;
            CurrentCashPaymentRequest.PaymentReimbursementRequest.ExpenseType = View.GetExpenseType;
            CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentMethod = View.GetPaymentMethod;
            CurrentCashPaymentRequest.PaymentReimbursementRequest.ProgressStatus = ProgressStatus.InProgress.ToString();

            CurrentCashPaymentRequest.PaymentReimbursementRequest.CashPaymentRequest = _controller.GetCashPaymentRequest(tarId);

            if (CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestStatuses.Count == 0)
                SavePaymentReimbursementRequestStatus();
            GetCurrentApprover();
            _controller.SaveOrUpdateEntity(CurrentCashPaymentRequest);
        }
        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Request/Default.aspx?{0}=3", AppConstants.TABID));
        }
        public void DeletePaymentReimbursementRequest(PaymentReimbursementRequest PaymentReimbursementRequest)
        {
            _controller.DeleteEntity(PaymentReimbursementRequest);
        }
        public PaymentReimbursementRequest GetPaymentReimbursementRequest(int id)
        {
            return _controller.GetPaymentReimbursementRequest(id);
        }
        public IList<PaymentReimbursementRequest> ListPaymentReimbursementRequests(string RequestDate)
        {
            return _controller.ListPaymentReimbursementRequests(RequestDate);
        }
        public IList<CashPaymentRequest> ListCashPaymentsNotExpensed()
        {
            return _controller.ListCashPaymentsNotExpensed();
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

        public ApprovalSetting GetApprovalSetting(string RequestType, int value)
        {
            return _settingController.GetApprovalSettingforProcess(RequestType, value);
        }
        private void SendEmail(PaymentReimbursementRequestStatus ELRS)
        {
            if (GetSuperviser(ELRS.Approver).IsAssignedJob != true)
            {
                EmailSender.Send(GetSuperviser(ELRS.Approver).Email, "Payment Reimbursement Request", (CurrentCashPaymentRequest.AppUser.FullName).ToUpper() + " Requests for Payment Reimbursement with Voucher No. - '" + (CurrentCashPaymentRequest.VoucherNo).ToUpper() + "'");
            }
            else
            {
                EmailSender.Send(GetSuperviser(_controller.GetAssignedJobbycurrentuser(ELRS.Approver).AssignedTo).Email, "Payment Reimbursement Request", (CurrentCashPaymentRequest.AppUser.FullName).ToUpper() + " Requests for Payment Reimbursement with Voucher No. - '" + (CurrentCashPaymentRequest.VoucherNo).ToUpper() + "'");
            }
        }
        public void Commit()
        {
            _controller.Commit();
        }
    }
}




