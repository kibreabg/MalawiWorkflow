using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Shared;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public class VehiclePresenter : Presenter<IVehicleView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private SettingController _controller;
        public VehiclePresenter([CreateNew] SettingController controller)
        {
            _controller = controller;
        }

        public override void OnViewLoaded()
        {
            View.Vehicles = _controller.ListVehicles(View.GetPlateNo);
        }

        public override void OnViewInitialized()
        {
            
        }
        public IList<Vehicle> GetVehicles()
        {
            return _controller.GetVehicles();
        }

        public void SaveOrUpdateVehicle(Vehicle Vehicle)
        {
            _controller.SaveOrUpdateEntity(Vehicle);
        }

        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Setting/Default.aspx?{0}=3", AppConstants.TABID));
        }

        public void DeleteVehicle(Vehicle Vehicle)
        {
            _controller.DeleteEntity(Vehicle);
        }
        public Vehicle GetVehicleById(int id)
        {
            return _controller.GetVehicle(id);
        }

        public IList<Vehicle> ListVehicles(string PlateNo)
        {
            return _controller.ListVehicles(PlateNo);
          
        }
        public void Commit()
        {
            _controller.Commit();
        }
    }
}




