using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Shared;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public class SupplierTypePresenter : Presenter<ISupplierTypeView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private Chai.WorkflowManagment.Modules.Setting.SettingController _controller;
        public SupplierTypePresenter([CreateNew] Chai.WorkflowManagment.Modules.Setting.SettingController controller)
        {
            _controller = controller;
        }

        public override void OnViewLoaded()
        {
            View.SupplierType = _controller.ListSupplierTypes(View.SupplierTypeEmail);
        }

        public override void OnViewInitialized()
        {
            
        }
        public IList<SupplierType> GetSupplierTypes()
        {
            return _controller.GetSupplierTypes();
        }

        public void SaveOrUpdateSupplierType(SupplierType SupplierType)
        {
            _controller.SaveOrUpdateEntity(SupplierType);
        }

        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Setting/Default.aspx?{0}=3", AppConstants.TABID));
        }

        public void DeleteSupplierType(SupplierType SupplierType)
        {
            _controller.DeleteEntity(SupplierType);
        }
        public SupplierType GetSupplierTypeById(int id)
        {
            return _controller.GetSupplierType(id);
        }

        public IList<SupplierType> ListSupplierTypes(string SupplierTypeName)
        {
            return _controller.ListSupplierTypes(SupplierTypeName);
          
        }
        public void Commit()
        {
            _controller.Commit();
        }
    }
}




