using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Services;
using Microsoft.Practices.CompositeWeb.Configuration;
using Microsoft.Practices.CompositeWeb.EnterpriseLibrary.Services;
using Chai.POC.Shared.Navigation;

namespace Chai.POC.Shared
{
    public class SharedModuleInitializer : ModuleInitializer
    {
        public override void Load(CompositionContainer container)
        {
            base.Load(container);

            AddGlobalServices(container.Services);
        }

        protected virtual void AddGlobalServices(IServiceCollection globalServices)
        {
              
        }

        public override void Configure(IServiceCollection services, System.Configuration.Configuration moduleConfiguration)
        {
        }
    }
}
