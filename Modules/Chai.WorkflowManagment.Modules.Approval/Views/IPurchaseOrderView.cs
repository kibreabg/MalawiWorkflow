using Chai.WorkflowManagment.CoreDomain.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Approval.Views
{
    public interface IPurchaseOrderView
    {
        BidAnalysisRequest BidAnalysisRequest { get; set; }

       
        string RequestType { get;  }
        string RequestNo { get; }
        string RequestDate { get; }
        int BidAnalysisRequestId { get; }

       
    }
}




