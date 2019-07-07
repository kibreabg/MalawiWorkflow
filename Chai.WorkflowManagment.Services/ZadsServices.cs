using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Chai.WorkflowManagment.CoreDomain;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.CoreDomain.Admins;
using Chai.WorkflowManagment.CoreDomain.DataAccess;
using Chai.WorkflowManagment.CoreDomain.Infrastructure;

namespace Chai.WorkflowManagment.Services
{
    public static class ZadsServices
    {
        private static IWorkspace _workspace;
        public static IWorkspace Workspace
        {
            get { return _workspace ?? (_workspace = WorkspaceFactory.Create()); }
            set { _workspace = value; }
        }

        private static AdminServices _adminServices;
        public static AdminServices AdminServices
        {
            get { return _adminServices ?? (_adminServices = new AdminServices()); }
            set { _adminServices = value; }
        }
    }
}
