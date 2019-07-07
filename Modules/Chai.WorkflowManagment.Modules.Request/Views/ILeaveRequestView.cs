using Chai.WorkflowManagment.CoreDomain.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public interface ILeaveRequestView
    {
        LeaveRequest LeaveRequest { get; set; }
        string RequestNo { get; }
        string RequestDate { get; }
        int LeaveRequestId { get; }
    }
}




