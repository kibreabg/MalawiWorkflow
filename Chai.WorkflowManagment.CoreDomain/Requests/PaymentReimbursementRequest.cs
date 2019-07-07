using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class PaymentReimbursementRequest : IEntity
    {
        public PaymentReimbursementRequest()
        {
            this.PaymentReimbursementRequestStatuses = new List<PaymentReimbursementRequestStatus>();
            this.PaymentReimbursementRequestDetails = new List<PaymentReimbursementRequestDetail>();
       
        }
        public int Id { get; set; }
        public Nullable<DateTime> RequestDate { get; set; }
        public string ExpenseType { get; set; }        
        public string Comment { get; set; }
        public string PaymentMethod { get; set; }
        public int CurrentApprover { get; set; }
        public int CurrentLevel { get; set; }
        public string CurrentStatus { get; set; }
        public string ProgressStatus { get; set; }

        [Required]
        public virtual CashPaymentRequest CashPaymentRequest { get; set; }
        public virtual IList<PaymentReimbursementRequestDetail> PaymentReimbursementRequestDetails { get; set; }
        public virtual IList<PaymentReimbursementRequestStatus> PaymentReimbursementRequestStatuses { get; set; }


        #region ReimbursementDetails
        public virtual PaymentReimbursementRequestDetail GetPaymentReimbursementRequestDetail(int Id)
        {

            foreach (PaymentReimbursementRequestDetail PaymentReimbursementRequestDetail in PaymentReimbursementRequestDetails)
            {
                if (PaymentReimbursementRequestDetail.Id == Id)
                    return PaymentReimbursementRequestDetail;
            }
            return null;
        }

        public virtual IList<PaymentReimbursementRequestDetail> GetPaymentReimbursementRequestDetailByLiquidationId(int liquidationId)
        {
            IList<PaymentReimbursementRequestDetail> LiquidationDetails = new List<PaymentReimbursementRequestDetail>();
            foreach (PaymentReimbursementRequestDetail PaymentReimbursementRequestDetail in PaymentReimbursementRequestDetails)
            {
                if (PaymentReimbursementRequestDetail.PaymentReimbursementRequest.Id == liquidationId)
                    LiquidationDetails.Add(PaymentReimbursementRequestDetail);
            }
            return LiquidationDetails;
        }
        public virtual void RemovePaymentReimbursementRequestDetail(int Id)
        {
            foreach (PaymentReimbursementRequestDetail PaymentReimbursementRequestDetail in PaymentReimbursementRequestDetails)
            {
                if (PaymentReimbursementRequestDetail.Id == Id)
                    PaymentReimbursementRequestDetails.Remove(PaymentReimbursementRequestDetail);
                break;
            }
        }

        #endregion
    }
}
