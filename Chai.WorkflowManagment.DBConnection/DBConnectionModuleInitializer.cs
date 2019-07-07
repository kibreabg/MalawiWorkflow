using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Services;
using Microsoft.Practices.CompositeWeb.Configuration;
using Microsoft.Practices.CompositeWeb.EnterpriseLibrary.Services;

namespace Chai.POC.DBConnection
{
    public class DBConnectionModuleInitializer : ModuleInitializer
    {
        public override void Load(CompositionContainer container)
        {
            base.Load(container);

            AddGlobalServices(container.Services);
        }

        protected virtual void AddGlobalServices(IServiceCollection globalServices)
        {
            // TODO: add a service that will be visible to any module
        }

        public override void Configure(IServiceCollection services, System.Configuration.Configuration moduleConfiguration)
        {
        }
    }
}
