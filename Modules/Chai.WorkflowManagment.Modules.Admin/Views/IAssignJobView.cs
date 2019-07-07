using System;
using System.Collections.Generic;
using System.Text;
using Chai.WorkflowManagment.CoreDomain;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.Modules.Admin.Views
{
    public interface IAssignJobView
    {
        int GetId { get; }
        int GetAssisnTo { get; }
        bool Getstatus { get; }



    }
}
