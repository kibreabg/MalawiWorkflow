using System;
using System.Collections.Generic;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.Modules.Shell.MasterPages
{
    public interface IHomeMasterView
    {
        AppUser user { set; get; }
    }
}




