using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Shared;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public class SupplierPresenter : Presenter<ISupplierView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private SettingController _controller;
        public SupplierPresenter([CreateNew] SettingController controller)
        {
            _controller = controller;
        }

        public override void OnViewLoaded()
        {
            View.Supplier = _controller.ListSuppliers(View.SupplierName);
        }

        public override void OnViewInitialized()
        {
            
        }
        public IList<Supplier> GetSuppliers()
        {
            return _controller.GetSuppliers();
        }

        public void SaveOrUpdateSupplier(Supplier Supplier)
        {
            _controller.SaveOrUpdateEntity(Supplier);
        }

        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Setting/Default.aspx?{0}=3", AppConstants.TABID));
        }

        public void DeleteSupplier(Supplier Supplier)
        {
            _controller.DeleteEntity(Supplier);
        }
        public Supplier GetSupplierById(int id)
        {
            return _controller.GetSupplier(id);
        }

        public IList<Supplier> ListSuppliers(string SupplierName)
        {
            return _controller.ListSuppliers(SupplierName);          
        }
        public IList<SupplierType> GetSupplierTypes()
        {
            return _controller.GetSupplierTypes();
        }
        public SupplierType GetSupplierTypeById(int id)
        {
            return _controller.GetSupplierType(id);
        }
        public void Commit()
        {
            _controller.Commit();
        }        
    }
}




