using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.CoreDomain.Setting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public interface ICostSharingRequestView
    {
        int GetCostSharingRequestId { get; }
        string GetRequestNo { get; }
        string GetPayee { get; }
        string GetDescription { get; }
        string GetVoucherNo { get; }

        int ItemAccountId { get; }

        decimal EstimatedTotalAmount { get; }
        string GetAmountType { get;  }
        string GetPaymentMethod { get; }

    }
}




