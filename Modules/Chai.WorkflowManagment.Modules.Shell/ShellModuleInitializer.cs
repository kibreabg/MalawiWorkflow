using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Services;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Configuration;
using Microsoft.Practices.CompositeWeb.Authorization;
using Microsoft.Practices.CompositeWeb.EnterpriseLibrary.Services;
using Chai.WorkflowManagment.Services;
using Chai.WorkflowManagment.Shared.Navigation;

namespace Chai.WorkflowManagment.Modules.Shell
{
    public class ShellModuleInitializer : ModuleInitializer
    {        
        public override void Load(CompositionContainer container)
        {
            base.Load(container);

            AddGlobalServices(container.Parent.Services);
            AddModuleServices(container.Services);
        }

        protected virtual void AddGlobalServices(IServiceCollection globalServices)
        {
            globalServices.AddNew<RedirectNavigationService, INavigationService>();
        }

        protected virtual void AddModuleServices(IServiceCollection moduleServices)
        {
            // TODO: register services that can be accesed only by the Shell module
        }
    }
}
