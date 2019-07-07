using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.CoreDomain.TravelLogs;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Modules.Admin;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public class TravelLogPresenter : Presenter<ITravelLogView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private RequestController _controller;
        private AdminController _adminController;
        public TravelLogPresenter([CreateNew] RequestController controller, AdminController adminController)
        {
            _controller = controller;
            _adminController = adminController;
        }

        public override void OnViewLoaded()
        {
            View.TravelLogs = _controller.ListTravelLogs(View.GetRequestId);
        }

        public override void OnViewInitialized()
        {
            
        }
        public IList<TravelLog> GetTravelLogs()
        {
            return _controller.GetTravelLogs();
        }

        public void SaveOrUpdateTravelLog(TravelLog TravelLog, int VehicleRequestId)
        {
            VehicleRequest VehicleRequest = _controller.GetVehicleRequest(VehicleRequestId);
            TravelLog.VehicleRequest = VehicleRequest;
            TravelLog.RequestNo = VehicleRequest.RequestNo;

            _controller.SaveOrUpdateEntity(TravelLog);
        }

        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Approval/frmVehicleApproval.aspx?{0}=4", AppConstants.TABID));
        }

        public void DeleteTravelLog(TravelLog TravelLog)
        {
            _controller.DeleteEntity(TravelLog);
        }
        public TravelLog GetTravelLogById(int id)
        {
            return _controller.GetTravelLog(id);
        }
        public VehicleRequest GetVehicleRequest(int VRId)
        {
           return _controller.GetVehicleRequest(VRId);
        }
        public IList<TravelLog> ListTravelLogs(int RequestId)
        {
            return _controller.ListTravelLogs(RequestId);          
        }
        public AppUser GetUser(int userId)
        {
            return _adminController.GetUser(userId);
        }
        public void Commit()
        {
            _controller.Commit();
        }
    }
}




