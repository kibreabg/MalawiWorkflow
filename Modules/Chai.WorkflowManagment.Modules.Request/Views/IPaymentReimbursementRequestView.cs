using Chai.WorkflowManagment.CoreDomain.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public interface IPaymentReimbursementRequestView
    {
        int GetTARequestId { get; }
        string GetExpenseType { get; }
        string GetComment { get; }
        string GetPaymentMethod { get; }
    }
}




