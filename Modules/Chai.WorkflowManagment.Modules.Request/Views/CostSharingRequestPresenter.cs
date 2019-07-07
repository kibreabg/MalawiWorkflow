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
    public class CostSharingRequestPresenter : Presenter<ICostSharingRequestView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private RequestController _controller;
        private AdminController _adminController;
        private SettingController _settingController;
        private CostSharingRequest _CostSharingRequest;
        public CostSharingRequestPresenter([CreateNew] RequestController controller, AdminController adminController, SettingController settingController)
        {
            _controller = controller;
            _adminController = adminController;
            _settingController = settingController;
        }
        public override void OnViewLoaded()
        {
            if (View.GetCostSharingRequestId > 0)
            {
                _controller.CurrentObject = _controller.GetCostSharingRequest(View.GetCostSharingRequestId);
            }
            CurrentCostSharingRequest = _controller.CurrentObject as CostSharingRequest;
        }
        public override void OnViewInitialized()
        {
            if (_CostSharingRequest == null)
            {
                int id = View.GetCostSharingRequestId;
                if (id > 0)
                    _controller.CurrentObject = _controller.GetCostSharingRequest(id);
                else
                    _controller.CurrentObject = new CostSharingRequest();
            }
        }
        public CostSharingRequest CurrentCostSharingRequest
        {
            get
            {
                if (_CostSharingRequest == null)
                {
                    int id = View.GetCostSharingRequestId;
                    if (id > 0)
                        _CostSharingRequest = _controller.GetCostSharingRequest(id);
                    else
                        _CostSharingRequest = new CostSharingRequest();
                }
                return _CostSharingRequest;
            }
            set { _CostSharingRequest = value; }


        }
        public IList<CostSharingRequest> GetCostSharingRequests()
        {
            return _controller.GetCostSharingRequests();
        }
        private void SaveCostSharingRequestStatus()
        {
            if (GetApprovalSetting(RequestType.CostSharing_Request.ToString().Replace('_', ' '),0) != null)
            {
                int i = 1;
                foreach (ApprovalLevel AL in GetApprovalSetting(RequestType.CostSharing_Request.ToString().Replace('_', ' '), 0).ApprovalLevels)
                {
                    CostSharingRequestStatus CSRS = new CostSharingRequestStatus();
                    CSRS.CostSharingRequest = CurrentCostSharingRequest;
                    //All Approver positions must be entered into the database before the approval workflow could run effectively!
                    if (AL.EmployeePosition.PositionName == "Superviser/Line Manager")
                    {
                        if (CurrentUser().Superviser != 0)
                            CSRS.Approver = CurrentUser().Superviser.Value;
                        else
                        {
                            CSRS.ApprovalStatus = ApprovalStatus.Approved.ToString();
                            CSRS.Date = Convert.ToDateTime(DateTime.Today.Date.ToShortDateString());
                        }
                    }
                    else if (AL.EmployeePosition.PositionName == "Program Manager")
                    {
                        if (CurrentCostSharingRequest.CostSharingRequestDetails[0].Project.Id != 0)
                        {
                            CSRS.Approver = GetProject(CurrentCostSharingRequest.CostSharingRequestDetails[0].Project.Id).AppUser.Id;
                        }
                    }
                    else
                    {
                        if (Approver(AL.EmployeePosition.Id) != null)
                        {
                            if (AL.EmployeePosition.PositionName == "Finance Officer")
                            {
                                CSRS.ApproverPosition = AL.EmployeePosition.Id; //So that we can entertain more than one finance manager to handle the request
                            }
                            else
                            {
                                CSRS.Approver = Approver(AL.EmployeePosition.Id).Id;
                            }
                        }
                        else
                            CSRS.Approver = 0;
                    }
                    CSRS.WorkflowLevel = i;
                    i++;
                    CurrentCostSharingRequest.CostSharingRequestStatuses.Add(CSRS);
                }
            }
        }
        private void GetCurrentApprover()
        {
            if (CurrentCostSharingRequest.CostSharingRequestStatuses != null)
            {
                foreach (CostSharingRequestStatus CPRS in CurrentCostSharingRequest.CostSharingRequestStatuses)
                {
                    if (CPRS.ApprovalStatus == null)
                    {
                        SendEmail(CPRS);
                        CurrentCostSharingRequest.CurrentApprover = CPRS.Approver;
                        CurrentCostSharingRequest.CurrentLevel = CPRS.WorkflowLevel;
                        CurrentCostSharingRequest.CurrentStatus = CPRS.ApprovalStatus;
                        break;
                    }
                }
            }
        }
        public void SaveOrUpdateCostSharingRequest()
        {
            CostSharingRequest CostSharingRequest = CurrentCostSharingRequest;
            CostSharingRequest.RequestNo = View.GetRequestNo;
            CostSharingRequest.RequestDate = Convert.ToDateTime(DateTime.Today.ToShortDateString());
            CostSharingRequest.Payee = View.GetPayee;
            CostSharingRequest.Description = View.GetDescription;
            CostSharingRequest.EstimatedTotalAmount = View.EstimatedTotalAmount;
            CostSharingRequest.ItemAccount = _settingController.GetItemAccount(View.ItemAccountId);
            CostSharingRequest.VoucherNo = View.GetVoucherNo;
           CostSharingRequest.PaymentMethod = View.GetPaymentMethod;
            CostSharingRequest.ProgressStatus = ProgressStatus.InProgress.ToString();
            CostSharingRequest.AppUser = _adminController.GetUser(CurrentUser().Id);
            if (View.GetAmountType != "Actual Amount")
            {
                CostSharingRequest.PaymentReimbursementStatus = "Not Retired";
            }
            else
            {
                CostSharingRequest.PaymentReimbursementStatus = "Retired";
                CostSharingRequest.ActualTotalAmount = CostSharingRequest.EstimatedTotalAmount;
            }
          
            CostSharingRequest.ExportStatus = "Not Exported";
            if (CurrentCostSharingRequest.CostSharingRequestStatuses.Count == 0)
                SaveCostSharingRequestStatus();

            SaveCostSharingDetail();
           

           
        }
        public void SaveOrUpdateCostSharingRequest(CostSharingRequest CostSharingRequest)
        { 
            GetCurrentApprover();
            _controller.SaveOrUpdateEntity(CostSharingRequest);
            _controller.CurrentObject = null;
        }
        public void SaveCostSharingDetail()
        {
            RemoveCostSharingDetail();
            IList<CostSharingSetting> costsharingsettings = _settingController.GetCostSharingSettings();
            if (costsharingsettings != null)
            {
                foreach (CostSharingSetting CSS in costsharingsettings)
                {
                    if (CSS.Percentage != 0)
                    {
                        CostSharingRequestDetail detail = new CostSharingRequestDetail();
                        detail.CostSharingRequest = CurrentCostSharingRequest;
                        detail.Project = CSS.Project;
                        detail.Grant = CSS.Grant;
                        detail.SharedAmount = (CSS.Percentage / 100) * CurrentCostSharingRequest.EstimatedTotalAmount;
                        CurrentCostSharingRequest.CostSharingRequestDetails.Add(detail);
                    }

                }
            }
        }        
        public void RemoveCostSharingDetail()
        {
            foreach (CostSharingRequestDetail CSRD in CurrentCostSharingRequest.CostSharingRequestDetails)
            {
                
                CurrentCostSharingRequest.RemoveCostSharingRequestDetails(CSRD.Id);
                if (CSRD.Id > 0)
                {
                    _controller.DeleteEntity(CSRD);
                }
            }
        }
        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Request/Default.aspx?{0}=3", AppConstants.TABID));
        }
        public void DeleteCostSharingRequest(CostSharingRequest CostSharingRequest)
        {
            _controller.DeleteEntity(CostSharingRequest);
        }
        public void DeleteCostSharingRequestDetail(CostSharingRequestDetail CostSharingRequestDetail)
        {
            _controller.DeleteEntity(CostSharingRequestDetail);
        }
        public CostSharingRequest GetCostSharingRequest(int id)
        {
            return _controller.GetCostSharingRequest(id);
        }
        public int GetLastCostSharingRequestId()
        {
            return _controller.GetLastCostSharingRequestId();
        }
        public IList<CostSharingRequest> ListCostSharingRequests(string RequestNo, string RequestDate)
        {
            return _controller.ListCostSharingRequests(RequestNo, RequestDate);
        }
        public IList<Supplier> GetSuppliers()
        {
            return _settingController.GetSuppliers();
        }
        public CostSharingRequestDetail GetCostSharingRequestDetail(int CPRDId)
        {
            return _controller.GetCostSharingRequestDetail(CPRDId);
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
        private void SendEmail(CostSharingRequestStatus CPRS)
        {
            if (GetSuperviser(CPRS.Approver).IsAssignedJob != true)
            {
                EmailSender.Send(GetSuperviser(CPRS.Approver).Email, "Cost Sharing Request", (CurrentCostSharingRequest.AppUser.FullName).ToUpper() + " Requests for Cost Sharing with Request No. - '" + (CurrentCostSharingRequest.RequestNo).ToUpper() + "'");
            }
            else
            {
                EmailSender.Send(GetSuperviser(_controller.GetAssignedJobbycurrentuser(CPRS.Approver).AssignedTo).Email, "Cost Sharing Request", (CurrentCostSharingRequest.AppUser.FullName).ToUpper() + " Requests for Cost Sharing with Request No. - '" + (CurrentCostSharingRequest.RequestNo).ToUpper() + "'");
            }
        }
        public void Commit()
        {
            _controller.Commit();
        }

    }
}




