using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;

using Chai.WorkflowManagment.CoreDomain.Infrastructure;
using Chai.WorkflowManagment.Shared.Settings;

namespace Chai.WorkflowManagment.CoreDomain.DataAccess
{
    public static class WorkspaceFactory
    {
        private static string _connectionString = TechnicalSettings.ConnectionString;

        static WorkspaceFactory()
        {
            Database.DefaultConnectionFactory = new SqlConnectionFactory(_connectionString);            
        }

        public static IWorkspace Create()
        {
            return new EFWorkspace(new WorkflowManagmentDbContext(false));
        }

        public static IReadOnlyWorkspace CreateReadOnly()
        {
            return new ReadOnlyEFWorkspace(new WorkflowManagmentDbContext(true));
        }
        
    }

}
