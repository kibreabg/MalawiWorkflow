using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.Enums
{
    public enum RequestType
    {
        Leave_Request,
        Vehicle_Request,
        Purchase_Request,
        CashPayment_Request,
        PaymentReimbursement_Request,
        OperationalControl_Request,
        TravelAdvance_Request,
        ExpenseLiquidation_Request,
        BankPayment_Request,
        CostSharing_Request,
        SoleVendor_Request,
        Bid_Analysis_Request,
    }
   public enum ProgressStatus
    {
        InProgress,
        Completed,
        

    }
    public enum ApprovalStatus
    {
        Approved,
        Reviewed,
        Authorized,
        Prepared,
        Expensed,
        Pay,
        Completed,
        Issued,
        Bank_Payment,
        Rejected


    }
    public enum Will
    {
        Approve,
        Prepare,
        Review,
        Authorize,
        Expense,
        Pay,
        Issue,
        Complete,
       
    }
   
   
}
