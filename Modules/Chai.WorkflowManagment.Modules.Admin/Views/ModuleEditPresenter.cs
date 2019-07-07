using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;

using Chai.WorkflowManagment.CoreDomain;
using Chai.WorkflowManagment.CoreDomain.Admins;
using Chai.WorkflowManagment.Shared;

namespace Chai.WorkflowManagment.Modules.Admin.Views
{
    public class ModuleEditPresenter : Presenter<IModuleEditView>
    {
         private AdminController _controller;
         private PocModule _pocModule;

         public ModuleEditPresenter([CreateNew] AdminController controller)
         {
         		_controller = controller;
         }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
        }

        public PocModule CurrentPocModule
        {
            get
            {
                if (_pocModule == null)
                {
                    int id = int.Parse(View.GetModuleId);
                    if (id > 0)
                        _pocModule = _controller.GetModuleById(id);
                    else
                        _pocModule = new PocModule();
                }

                return _pocModule;
            }
        }
        
        public void SaveOrUpdateModule()
        {
            PocModule module = CurrentPocModule;
            module.Name = View.GetName;
            module.FolderPath = View.GetFolderPath;
            _controller.SaveOrUpdateEntity<PocModule>(module);
        }

        public void DeleteModule()
        {
            _controller.DeleteEntity<PocModule>(CurrentPocModule);
        }

        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Admin/Modules.aspx?{0}=0", AppConstants.TABID));
        }
    }
}




