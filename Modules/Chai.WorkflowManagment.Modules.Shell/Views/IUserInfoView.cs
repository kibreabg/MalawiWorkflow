using System;
using System.Collections.Generic;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Users;
namespace Chai.WorkflowManagment.Modules.Shell.Views
{
    public interface IUserInfoView
    {
        AppUser user { set; }
    }
}




