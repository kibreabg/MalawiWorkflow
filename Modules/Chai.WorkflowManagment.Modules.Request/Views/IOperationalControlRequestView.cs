using Chai.WorkflowManagment.CoreDomain.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public interface IOperationalControlRequestView
    {
        int GetOperationalControlRequestId { get; }
        string GetRequestNo { get; }
        int GetBankAccountId { get; }
        //string GetPayee { get; }
        string GetDescription { get; }
        int GetBeneficiaryId { get; }
        string GetBranchCode { get; }
        string GetBankName { get; }
        string GetVoucherNo { get; }

        string GetPaymentMethod { get; }
        string GetPageType { get; }
    }
}




