using Chai.WorkflowManagment.CoreDomain.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Approval.Views
{
    public interface ICashPaymentApprovalView
    {
        //    VehicleRequest VehicleRequests { get; set; }
        int GetCashPaymentRequestId { get; }
        int GetAccountId { get; }
        //    int GetRequestNo { get; }        
        //    DateTime GetDepartureDate { get; }
        //    DateTime GetReturningDate { get; }
        //    string GetPurposeOfTravel { get; }
        //    int GetNoOfPassengers { get; }
        //    string GetProgressStatus { get; }
        //    int GetRequesterId { get; }
        //    int GetProgramId { get; }
        //    int GetRequestHandlerId { get; }

    }
}




