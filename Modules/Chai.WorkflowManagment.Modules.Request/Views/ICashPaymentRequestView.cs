using Chai.WorkflowManagment.CoreDomain.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public interface ICashPaymentRequestView
    {
        int GetCashPaymentRequestId { get; }
        string GetRequestNo { get; }
        int GetPayee { get; }
        string GetDescription { get; }
        string GetVoucherNo { get; }
        string GetAmountType { get; }
        string GetPaymentMethod { get; }
    }
}




