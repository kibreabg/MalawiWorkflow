using Chai.WorkflowManagment.CoreDomain.Request;
using Chai.WorkflowManagment.CoreDomain.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Approval.Views
{
    public interface ISoleVendorApprovalView
    {
        SoleVendorRequest SoleVendorRequest { get; set; }
        string RequestNo { get; }
        string RequestDate { get; }
        int SoleVendorRequestId { get; }
    }
}




