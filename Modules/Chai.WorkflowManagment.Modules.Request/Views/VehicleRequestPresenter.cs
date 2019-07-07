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
    public class VehicleRequestPresenter : Presenter<IVehicleRequestView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private RequestController _controller;
        private AdminController _adminController;
        private SettingController _settingController;
        private VehicleRequest _vehicleRequest;
        public VehicleRequestPresenter([CreateNew] RequestController controller, AdminController adminController, SettingController settingController)
        {
            _controller = controller;
            _adminController = adminController;
            _settingController = settingController;
        }
        public override void OnViewLoaded()
        {
            if (View.GetVehicleRequestId > 0)
            {
                _controller.CurrentObject = _controller.GetVehicleRequest(View.GetVehicleRequestId);
            }
            CurrentVehicleRequest = _controller.CurrentObject as VehicleRequest;
        }
        public override void OnViewInitialized()
        {

        }
        public VehicleRequest CurrentVehicleRequest
        {
            get
            {
                if (_vehicleRequest == null)
                {
                    int id = View.GetVehicleRequestId;
                    if (id > 0)
                        _vehicleRequest = _controller.GetVehicleRequest(id);
                    else
                        _vehicleRequest = new VehicleRequest();
                }
                return _vehicleRequest;
            }
            set
            {
                _vehicleRequest = value;
            }
        }
        public IList<VehicleRequest> GetVehicleRequests()
        {
            return _controller.GetVehicleRequests();
        }
        public int GetLastVehicleRequestId()
        {
            return _controller.GetLastVehicleRequestId();
        }
        private void SaveVehicleRequestStatus()
        {
            if (GetApprovalSetting(RequestType.Vehicle_Request.ToString().Replace('_', ' '), 0) != null)
            {
                int i = 1;
                foreach (ApprovalLevel AL in GetApprovalSetting(RequestType.Vehicle_Request.ToString().Replace('_', ' '), 0).ApprovalLevels)
                {
                    VehicleRequestStatus VRS = new VehicleRequestStatus();
                    VRS.VehicleRequest = CurrentVehicleRequest;
                    if (AL.EmployeePosition.PositionName == "Superviser/Line Manager")
                    {
                        if (CurrentUser().Superviser != 0)
                            VRS.Approver = CurrentUser().Superviser.Value;
                        else
                            VRS.ApprovalStatus = ApprovalStatus.Approved.ToString();
                    }
                    else if (AL.EmployeePosition.PositionName == "Program Manager")
                    {
                        if (CurrentVehicleRequest.Project.Id != 0)
                        {
                            VRS.Approver = GetProject(CurrentVehicleRequest.Project.Id).AppUser.Id;
                        }
                    }
                    else
                    {
                        if (Approver(AL.EmployeePosition.Id) != null)
                            VRS.Approver = Approver(AL.EmployeePosition.Id).Id;
                        else
                            VRS.Approver = 0;
                    }
                    VRS.WorkflowLevel = i;
                    i++;
                    CurrentVehicleRequest.VehicleRequestStatuses.Add(VRS);
                }
            }
        }
        private void GetCurrentApprover()
        {
            if (CurrentVehicleRequest.VehicleRequestStatuses != null)
            {
                foreach (VehicleRequestStatus VRS in CurrentVehicleRequest.VehicleRequestStatuses)
                {
                    if (VRS.ApprovalStatus == null)
                    {
                        SendEmail(VRS);
                        CurrentVehicleRequest.CurrentApprover = VRS.Approver;
                        CurrentVehicleRequest.CurrentLevel = VRS.WorkflowLevel;
                        CurrentVehicleRequest.ProgressStatus = ProgressStatus.InProgress.ToString();
                        break;
                    }
                }
            }
        }
        public void SaveOrUpdateVehicleRequest()
        {
            VehicleRequest VehicleRequest = CurrentVehicleRequest;
            VehicleRequest.RequestNo = View.GetRequestNo;
            VehicleRequest.RequestDate = Convert.ToDateTime(DateTime.Today.ToShortDateString());
            VehicleRequest.DepartureDate = View.GetDepartureDate;
            VehicleRequest.ReturningDate = View.GetReturningDate;
            VehicleRequest.DepartureTime = View.GetDepartureTime;
         
            VehicleRequest.PurposeOfTravel = View.GetPurposeOfTravel;
            VehicleRequest.Destination = View.GetDestination;
            VehicleRequest.Comment = View.GetComment;
            VehicleRequest.NoOfPassengers = View.GetNoOfPassengers;
            VehicleRequest.ProgressStatus = ProgressStatus.InProgress.ToString();
            if (View.GetProjectId != 0)
                VehicleRequest.Project = _settingController.GetProject(View.GetProjectId);
            if (View.GetGrantId != 0)
                VehicleRequest.Grant = _settingController.GetGrant(View.GetGrantId);
            VehicleRequest.AppUser = _adminController.GetUser(CurrentUser().Id);

            if (CurrentVehicleRequest.VehicleRequestStatuses.Count == 0)
                SaveVehicleRequestStatus();
            GetCurrentApprover();

            _controller.SaveOrUpdateEntity(VehicleRequest);
            _controller.CurrentObject = null;
        }
        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Request/Default.aspx?{0}=3", AppConstants.TABID));
        }
        public void DeleteVehicleRequest(VehicleRequest VehicleRequest)
        {
            _controller.DeleteEntity(VehicleRequest);
        }
        public VehicleRequest GetVehicleRequest(int id)
        {
            return _controller.GetVehicleRequest(id);
        }
        public IList<VehicleRequest> ListVehicleRequests(string RequestNo, string RequestDate)
        {
            return _controller.ListVehicleRequests(RequestNo, RequestDate);
        }

        public AppUser Approver(int Position)
        {
            return _controller.Approver(Position);
        }
        public AssignJob GetAssignedJobbycurrentuser()
        {
            return _controller.GetAssignedJobbycurrentuser();
        }
        public AssignJob GetAssignedJobbycurrentuser(int UserId)
        {
            return _controller.GetAssignedJobbycurrentuser(UserId);
        }
        public IList<AppUser> GetUsers()
        {
            return _adminController.GetUsers();
        }
        public AppUser GetUser(int id)
        {
            return _adminController.GetUser(id);
        }
        public AppUser GetSuperviser(int superviser)
        {
            return _controller.GetSuperviser(superviser);
        }
        public AppUser CurrentUser()
        {
            return _controller.GetCurrentUser();
        }
        public Project GetProject(int ProjectId)
        {
            return _settingController.GetProject(ProjectId);
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
        public ApprovalSetting GetApprovalSetting(string RequestType, int value)
        {
            return _settingController.GetApprovalSettingforProcess(RequestType, value);
        }
        private void SendEmail(VehicleRequestStatus VRS)
        {
            if (GetSuperviser(VRS.Approver).IsAssignedJob != true)
            {
                EmailSender.Send(GetSuperviser(VRS.Approver).Email, "Vehicle Request", (CurrentVehicleRequest.AppUser.FullName).ToUpper() + "' Request for Vehicle No '" + (CurrentVehicleRequest.RequestNo).ToUpper() + "'");
            }
            else
            {
                EmailSender.Send(GetSuperviser(_controller.GetAssignedJobbycurrentuser(VRS.Approver).AssignedTo).Email, "Vehicle Request", (CurrentVehicleRequest.AppUser.FullName).ToUpper() + "' Request for Vehicle");
            }
        }
        public void Commit()
        {
            _controller.Commit();
        }


    }
}




