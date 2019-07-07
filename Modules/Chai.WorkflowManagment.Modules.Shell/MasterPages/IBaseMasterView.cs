using System;
using System.Collections.Generic;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.Modules.Shell.MasterPages
{
    public interface IBaseMasterView
    {
        string TabId { get; }
        AppUser CurrentUser { set; get; }
    }
}
