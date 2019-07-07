using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class BankPaymentRequest : IEntity
    {
        public BankPaymentRequest()
        {
            this.BankPaymentRequestDetails = new List<BankPaymentRequestDetail>();
            this.BankPaymentRequestStatuses = new List<BankPaymentRequestStatus>();            
        }
        public int Id { get; set; }
        public string RequestNo { get; set; }
        public Nullable<DateTime> ProcessDate { get; set; }
        public string PaymentMethod { get; set; }
        public int CurrentApprover { get; set; }
        public int CurrentLevel { get; set; }
        public string CurrentStatus { get; set; }
        public string ProgressStatus { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual IList<BankPaymentRequestDetail> BankPaymentRequestDetails { get; set; }
        public virtual IList<BankPaymentRequestStatus> BankPaymentRequestStatuses { get; set; }        

        #region BankPaymentRequestStatus
        public virtual BankPaymentRequestStatus GetBankPaymentRequestStatus(int Id)
        {
            foreach (BankPaymentRequestStatus CPRS in BankPaymentRequestStatuses)
            {
                if (CPRS.Id == Id)
                    return CPRS;
            }
            return null;
        }
        public virtual BankPaymentRequestStatus GetBankPaymentRequestStatusworkflowLevel(int workflowLevel)
        {
            foreach (BankPaymentRequestStatus CPRS in BankPaymentRequestStatuses)
            {
                if (CPRS.WorkflowLevel == workflowLevel)
                    return CPRS;
            }
            return null;
        }
        public virtual IList<BankPaymentRequestStatus> GetBankPaymentRequestStatusByRequestId(int RequestId)
        {
            IList<BankPaymentRequestStatus> LRS = new List<BankPaymentRequestStatus>();
            foreach (BankPaymentRequestStatus CPRS in BankPaymentRequestStatuses)
            {
                if (CPRS.BankPaymentRequest.Id == RequestId)
                    LRS.Add(CPRS);
            }
            return LRS;
        }
        public virtual void RemoveBankPaymentRequestStatus(int Id)
        {
            foreach (BankPaymentRequestStatus CPRS in BankPaymentRequestStatuses)
            {
                if (CPRS.Id == Id)
                    BankPaymentRequestStatuses.Remove(CPRS);
                break;
            }
        }
        #endregion
        #region BankPaymentRequestDetail
        public virtual BankPaymentRequestDetail GetBankPaymentRequestDetail(int Id)
        {

            foreach (BankPaymentRequestDetail CPRS in BankPaymentRequestDetails)
            {
                if (CPRS.Id == Id)
                    return CPRS;
            }
            return null;
        }

        public virtual IList<BankPaymentRequestDetail> GetBankPaymentRequestDetailByPurchaseId(int PurchaseId)
        {
            IList<BankPaymentRequestDetail> LRS = new List<BankPaymentRequestDetail>();
            foreach (BankPaymentRequestDetail CPRS in BankPaymentRequestDetails)
            {
                if (CPRS.BankPaymentRequest.Id == PurchaseId)
                    LRS.Add(CPRS);
            }
            return LRS;
        }
        public virtual void RemoveBankPaymentRequestDetail(int Id)
        {
            foreach (BankPaymentRequestDetail CPRS in BankPaymentRequestDetails)
            {
                if (CPRS.Id == Id)
                    BankPaymentRequestDetails.Remove(CPRS);
                break;
            }
        }
        #endregion
    }
}
