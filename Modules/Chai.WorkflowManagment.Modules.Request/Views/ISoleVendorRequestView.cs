using Chai.WorkflowManagment.CoreDomain.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public interface ISoleVendorRequestView
    {
        IList<SoleVendorRequest> SoleVendorRequests { get; set; }
        int GetSoleVendorRequestId { get; }
        string GetRequestNo { get; }        
        DateTime GetRequestDate { get; }
        string GetContactPersonNumber { get; }
        decimal GetProposedPurchasedPrice { get; }
        int GetPurchaseRequestId { get; }
        int GetProposedSupplier { get; }
        string GetComment { get; }
        string GetSoleSourceJustificationPreparedBy { get; }
        string GetSoleVendorJustificationType { get; }
        string GetPaymentMethod { get; }
        int GetProjectId { get; }
        int GetGrantId { get; }
     
    }
}




