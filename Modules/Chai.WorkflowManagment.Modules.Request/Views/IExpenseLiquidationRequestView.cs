using Chai.WorkflowManagment.CoreDomain.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public interface IExpenseLiquidationRequestView
    {
        int GetTARequestId { get; }
        string GetExpenseType { get; }
        string GetComment { get; }
        string GetAdditionalComment { get; }
        string GetTravelAdvReqDate { get; }

        string GetPaymentMethod { get; }



    }
}




