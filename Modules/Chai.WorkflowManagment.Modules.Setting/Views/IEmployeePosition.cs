using Chai.WorkflowManagment.CoreDomain.Setting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public interface IEmployeePositionView
    {
        IList<EmployeePosition> EmployeePosition { get; set; }
       
    }
}




