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
    public class VehicleApprovalPresenter : Presenter<IVehicleApprovalView>
    {
        private ApprovalController _controller;
        private RequestController _requestController;
        private VehicleRequest _vehicleRequest;
        private SettingController _settingController;
        private AdminController _adminController;

        public VehicleApprovalPresenter([CreateNew] ApprovalController controller, RequestController requestController, SettingController settingController, AdminController adminController)
        {
            _controller = controller;
            _requestController = requestController;
            _settingController = settingController;
            _adminController = adminController;
        }

        public override void OnViewLoaded()
        {
            if (View.GetVehicleRequestId > 0)
            {
                _controller.CurrentObject = _requestController.GetVehicleRequest(View.GetVehicleRequestId);
            }
            CurrentVehicleRequest = _controller.CurrentObject as VehicleRequest;
        }
        public override void OnViewInitialized()
        {
            if (_vehicleRequest == null)
            {
                int id = View.GetVehicleRequestId;
                if (id > 0)
                    _controller.CurrentObject = _requestController.GetVehicleRequest(id);
                else
                    _controller.CurrentObject = new VehicleRequest();
            }
        }
        public VehicleRequest CurrentVehicleRequest
        {
            get
            {
                if (_vehicleRequest == null)
                {
                    int id = View.GetVehicleRequestId;
                    if (id > 0)
                        _vehicleRequest = _requestController.GetVehicleRequest(id);
                    else
                        _vehicleRequest = new VehicleRequest();
                }
                return _vehicleRequest;
            }
            set { _vehicleRequest = value; }
        }
        public IList<AppUser> GetDrivers()
        {
            return _adminController.GetDrivers();
        }
        public AppUser GetAssignDriver(int Id)
        {
            return _adminController.GetAssignDriver(Id);
        }
       
        public IList<CarRental> GetCarRentals()
        {
            return _settingController.GetCarRentals();
        }
        public CarRental GetCarRental(int Id)
        {
            return _settingController.GetCarRental(Id);
        }
        public IList<Vehicle> GetVehicles()
        {
            return _settingController.GetVehicles();
        }
        public Vehicle GetVehicle(int Id)
        {
            return _settingController.GetVehicle(Id);
        }
        public AppUser GetUser(int UserId)
        {
            return _adminController.GetUser(UserId);
        }
        public VehicleRequestDetail GetVehicleById(int id)
        {
            return _requestController.GetAssignedVehicleById(id);
        }
        public AssignJob GetAssignedJobbycurrentuser()
        {
            return _controller.GetAssignedJobbycurrentuser();
        }
        public void SaveOrUpdateVehicleRequest(VehicleRequest VehicleRequest)
        {
            _controller.SaveOrUpdateEntity(VehicleRequest);
            _controller.CurrentObject = null;
        }
        public IList<VehicleRequest> ListVehicleRequests(string RequestNo, string RequestDate, string ProgressStatus)
        {
            return _controller.ListVehicleRequests(RequestNo, RequestDate, ProgressStatus);
        }
        public AppUser CurrentUser()
        {
            return _controller.GetCurrentUser();
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
        public void DeleteVehicles(VehicleRequestDetail Vehicle)
        {
            _controller.DeleteEntity(Vehicle);
        }
        public void Commit()
        {
            _controller.Commit();
        }               
    }
}




