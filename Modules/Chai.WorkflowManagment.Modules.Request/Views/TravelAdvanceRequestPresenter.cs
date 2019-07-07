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
    public class TravelAdvanceRequestPresenter : Presenter<ITravelAdvanceRequestView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private RequestController _controller;
        private AdminController _adminController;
        private SettingController _settingController;
        private TravelAdvanceRequest _travelAdvanceRequest;
        public TravelAdvanceRequestPresenter([CreateNew] RequestController controller, AdminController adminController, SettingController settingController)
        {
            _controller = controller;
            _adminController = adminController;
            _settingController = settingController;
        }
        public override void OnViewLoaded()
        {
            if (View.GetTARequestId > 0)
            {
                _controller.CurrentObject = _controller.GetTravelAdvanceRequest(View.GetTARequestId);
            }
            CurrentTravelAdvanceRequest = _controller.CurrentObject as TravelAdvanceRequest;
        }
        public override void OnViewInitialized()
        {
            if (_travelAdvanceRequest == null)
            {
                _controller.CurrentObject = new TravelAdvanceRequest();
            }
        }
        public TravelAdvanceRequest CurrentTravelAdvanceRequest
        {
            get
            {
                if (_travelAdvanceRequest == null)
                {
                    int id = View.GetTARequestId;
                    if (id > 0)
                        _travelAdvanceRequest = _controller.GetTravelAdvanceRequest(id);
                    else
                        _travelAdvanceRequest = new TravelAdvanceRequest();
                }
                return _travelAdvanceRequest;
            }
            set
            {
                _travelAdvanceRequest = value;
            }
        }
        public IList<TravelAdvanceRequest> GetTravelAdvanceRequests()
        {
            return _controller.GetTravelAdvanceRequests();
        }
        private void SaveTravelAdvanceRequestStatus()
        {
            if (GetApprovalSetting(RequestType.TravelAdvance_Request.ToString().Replace('_', ' '), 0) != null)
            {
                int i = 1;
                foreach (ApprovalLevel AL in GetApprovalSetting(RequestType.TravelAdvance_Request.ToString().Replace('_', ' '), 0).ApprovalLevels)
                {
                    TravelAdvanceRequestStatus TARS = new TravelAdvanceRequestStatus();
                    TARS.TravelAdvanceRequest = CurrentTravelAdvanceRequest;
                    if (AL.EmployeePosition.PositionName == "Superviser/Line Manager")
                    {
                        if (CurrentUser().Superviser != 0)
                            TARS.Approver = CurrentUser().Superviser.Value;
                        else
                        {
                            TARS.ApprovalStatus = ApprovalStatus.Approved.ToString();
                            TARS.Date = Convert.ToDateTime(DateTime.Today.Date.ToShortDateString());
                        }

                    }
                    else if (AL.EmployeePosition.PositionName == "Program Manager")
                    {
                        if (CurrentTravelAdvanceRequest.Project.Id != 0)
                        {
                            TARS.Approver = GetProject(CurrentTravelAdvanceRequest.Project.Id).AppUser.Id;
                        }
                    }
                    else
                    {
                        if (Approver(AL.EmployeePosition.Id) != null)
                        {
                            if (AL.EmployeePosition.PositionName == "Finance Officer")
                            {
                                TARS.ApproverPosition = AL.EmployeePosition.Id; //So that we can entertain more than one finance manager to handle the request
                            }
                            else
                            {
                                TARS.Approver = Approver(AL.EmployeePosition.Id).Id;
                            }
                        }
                        else
                            TARS.Approver = 0;
                    }
                    TARS.WorkflowLevel = i;
                    i++;
                    CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses.Add(TARS);
                }
            }
        }
        private void GetCurrentApprover()
        {
            if (CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses != null)
            {
                foreach (TravelAdvanceRequestStatus VRS in CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses)
                {
                    if (VRS.ApprovalStatus == null)
                    {
                        SendEmail(VRS);
                        CurrentTravelAdvanceRequest.CurrentApprover = VRS.Approver;
                        CurrentTravelAdvanceRequest.CurrentLevel = VRS.WorkflowLevel;
                        CurrentTravelAdvanceRequest.ProgressStatus = ProgressStatus.InProgress.ToString();
                        break;
                    }
                }
            }
        }
        public void SaveOrUpdateTARequest()
        {
            TravelAdvanceRequest TravelAdvanceRequest = CurrentTravelAdvanceRequest;
            TravelAdvanceRequest.TravelAdvanceNo = View.GetRequestNo;
            TravelAdvanceRequest.RequestDate = View.GetRequestDate;
            TravelAdvanceRequest.VisitingTeam = View.GetVisitingTeam;
            TravelAdvanceRequest.PurposeOfTravel = View.GetPurposeOfTravel;
            TravelAdvanceRequest.Comments = View.GetComments;
            TravelAdvanceRequest.PaymentMethod = View.GetPaymentMethod;
            TravelAdvanceRequest.ProgressStatus = ProgressStatus.InProgress.ToString();
            TravelAdvanceRequest.ExportStatus = "Not Exported";
            TravelAdvanceRequest.AppUser = _adminController.GetUser(CurrentUser().Id);

            if (View.GetProjectId != 0)
                TravelAdvanceRequest.Project = _settingController.GetProject(View.GetProjectId);
            if (View.GetGrantId != 0)
                TravelAdvanceRequest.Grant = _settingController.GetGrant(View.GetGrantId);

            if (CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses.Count == 0)
                SaveTravelAdvanceRequestStatus();
            GetCurrentApprover();

            _controller.SaveOrUpdateEntity(TravelAdvanceRequest);
            _controller.CurrentObject = null;
        }
        public void SaveOrUpdateTARequest(TravelAdvanceRequest TravelAdvanceRequest)
        {
            _controller.SaveOrUpdateEntity(TravelAdvanceRequest);
        }
        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Request/Default.aspx?{0}=3", AppConstants.TABID));
        }
        public void DeleteTravelAdvanceRequest(TravelAdvanceRequest TravelAdvanceRequest)
        {
            _controller.DeleteEntity(TravelAdvanceRequest);
        }
        public TravelAdvanceRequest GetTravelAdvanceRequest(int id)
        {
            return _controller.GetTravelAdvanceRequest(id);
        }
        public TravelAdvanceCost GetTravelAdvanceCost(int id)
        {
            return _controller.GetTravelAdvanceCost(id);
        }
        public IList<TravelAdvanceRequest> ListTravelAdvanceRequests(string RequestNo, string ReqestDate)
        {
            return _controller.ListTravelAdvanceRequests(RequestNo, ReqestDate);
        }
        public TravelAdvanceRequestDetail GetTravelAdvanceRequestDetail(int id)
        {
            return _controller.GetTravelAdvanceRequestDetail(id);
        }
        public void DeleteTravelAdvanceRequestDetail(TravelAdvanceRequestDetail travelAdvanceRequestDetail)
        {
            _controller.DeleteEntity(travelAdvanceRequestDetail);
        }
        public void DeleteTravelAdvanceCost(TravelAdvanceCost travelAdvanceCost)
        {
            _controller.DeleteEntity(travelAdvanceCost);
        }
        public AppUser Approver(int Position)
        {
            return _controller.Approver(Position);
        }
        public IList<AppUser> GetDrivers()
        {
            return _adminController.GetDrivers();
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
        public Project GetProject(int Id)
        {
            return _settingController.GetProject(Id);
        }
        public IList<Project> GetProjects()
        {
            return _settingController.GetProjects();
        }
        public IList<Grant> GetGrants()
        {
            return _settingController.GetGrants();
        }
        public IList<Grant> GetGrantbyprojectId(int projectId)
        {
            return _settingController.GetProjectGrantsByprojectId(projectId);
        }
        public ItemAccount GetItemAccount(int itemAccountId)
        {
            return _settingController.GetItemAccount(itemAccountId);
        }
        public ItemAccount GetDefaultItemAccount()
        {
            return _settingController.GetDefaultItemAccount();
        }
        public IList<ItemAccount> GetItemAccounts()
        {
            return _settingController.GetItemAccounts();
        }
        public ExpenseType GetExpenseType(int Id)
        {
            return _settingController.GetExpenseType(Id);
        }
        public IList<ExpenseType> GetExpenseTypes()
        {
            return _settingController.GetExpenseTypes();
        }
        public ApprovalSetting GetApprovalSetting(string RequestType, decimal value)
        {
            return _settingController.GetApprovalSettingforProcess(RequestType, value);
        }
        public int GetLastTravelAdvanceRequestId()
        {
            return _controller.GetLastTravelAdvanceRequestId();
        }
        private void SendEmail(TravelAdvanceRequestStatus VRS)
        {
            if (GetSuperviser(VRS.Approver).IsAssignedJob != true)
            {
                EmailSender.Send(GetSuperviser(VRS.Approver).Email, "Travel Advance Request", (CurrentTravelAdvanceRequest.AppUser.FullName).ToUpper() + " Requests for Travel Advance with Travel Advance No. - " + (CurrentTravelAdvanceRequest.TravelAdvanceNo).ToUpper() + "'");
            }
            else
            {
                EmailSender.Send(GetSuperviser(_controller.GetAssignedJobbycurrentuser(VRS.Approver).AssignedTo).Email, "Travel Advance Request", (CurrentTravelAdvanceRequest.AppUser.FullName).ToUpper() + " Requests for Travel Advance with Travel Advance No. - " + (CurrentTravelAdvanceRequest.TravelAdvanceNo).ToUpper() + "'");
            }
        }
        public void Commit()
        {
            _controller.Commit();
        }
    }
}




