using Chai.WorkflowManagment.CoreDomain.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public interface IPurchaseRequestView
    {
        PurchaseRequest PurchaseRequest { get; set; }
        string RequestNo { get; }
        string RequestDate { get; }
        int PurchaseRequestId { get; }
        string GetPaymentMethod { get; }
    }
}




