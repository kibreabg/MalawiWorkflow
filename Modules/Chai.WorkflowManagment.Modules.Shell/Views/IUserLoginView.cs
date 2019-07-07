using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Shell.Views
{
    public interface IUserLoginView
    {
        string GetUserName { get; }
        string GetPassword { get; }
        bool PersistLogin { get; }
    }
}




