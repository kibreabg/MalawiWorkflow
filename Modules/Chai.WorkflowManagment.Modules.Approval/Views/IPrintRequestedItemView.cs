using Chai.WorkflowManagment.CoreDomain.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Approval.Views
{
    public interface IPrintRequestedItemView
    {
        BidAnalysisRequest BidAnalysisRequest { get; set; }
        string RequestNo { get; }
        string RequestDate { get; }
        int BidAnalysisRequestId { get; }
    }
}




