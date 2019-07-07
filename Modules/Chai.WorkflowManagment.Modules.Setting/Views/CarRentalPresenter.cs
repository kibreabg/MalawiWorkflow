using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Shared;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public class CarRentalPresenter : Presenter<ICarRentalView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private SettingController _controller;
        public CarRentalPresenter([CreateNew] SettingController controller)
        {
            _controller = controller;
        }

        public override void OnViewLoaded()
        {
            View.CarRentals = _controller.ListCarRentals(View.GetName);
        }

        public override void OnViewInitialized()
        {
            
        }
        public IList<CarRental> GetCarRentals()
        {
            return _controller.GetCarRentals();
        }

        public void SaveOrUpdateCarRental(CarRental CarRental)
        {
            _controller.SaveOrUpdateEntity(CarRental);
        }

        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Setting/Default.aspx?{0}=3", AppConstants.TABID));
        }

        public void DeleteCarRental(CarRental CarRental)
        {
            _controller.DeleteEntity(CarRental);
        }
        public CarRental GetCarRentalById(int id)
        {
            return _controller.GetCarRental(id);
        }

        public IList<CarRental> ListCarRentals(string CarRentalName)
        {
            return _controller.ListCarRentals(CarRentalName);
          
        }
        public void Commit()
        {
            _controller.Commit();
        }
    }
}




