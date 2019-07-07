using Chai.WorkflowManagment.CoreDomain.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public interface ITravelAdvanceRequestView
    {
        int GetTARequestId { get; }
        string GetRequestNo { get; }
        DateTime GetRequestDate { get; }
        string GetComments { get; }
        string GetPurposeOfTravel { get; }
        string GetVisitingTeam { get; }
        int GetProjectId { get; }
        int GetGrantId { get; }

        string GetPaymentMethod { get; }

    }
}




