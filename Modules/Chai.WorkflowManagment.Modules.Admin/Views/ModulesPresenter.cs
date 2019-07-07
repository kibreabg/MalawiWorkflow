using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.Services;
using Chai.WorkflowManagment.CoreDomain.Admins;

namespace Chai.WorkflowManagment.Modules.Admin.Views
{
    public class ModulesPresenter : Presenter<IModulesView>
    {
        private AdminController _controller;
        
        [InjectionConstructor]
        public ModulesPresenter([CreateNew] AdminController controller)
        {
            _controller = controller;
        }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public IList<PocModule> GetListOfModules()
        {
            return _controller.GetListOfAllPocModules();
        }

       
    }
}




