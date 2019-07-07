using Chai.WorkflowManagment.CoreDomain.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public interface IBankPaymentRequestView
    {
        int GetBankPaymentRequestId { get; }
        string GetRequestNo { get; }
        string GetPaymentMethod { get; }
    }
}




