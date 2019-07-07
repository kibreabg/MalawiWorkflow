using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class CostSharingRequest : IEntity
    {
        public CostSharingRequest()
        {
            this.CostSharingRequestDetails = new List<CostSharingRequestDetail>();
            this.CostSharingRequestStatuses = new List<CostSharingRequestStatus>();
            this.CSRAttachments = new List<CSRAttachment>();
        }
        public int Id { get; set; }
        public string RequestNo { get; set; }
        public Nullable<DateTime> RequestDate { get; set; }
        public string Payee { get; set; }
        public string Description { get; set; }
        public string VoucherNo { get; set; }
        public decimal EstimatedTotalAmount { get; set; }
        public decimal ActualTotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public int CurrentApprover { get; set; }
        public int CurrentApproverPosition { get; set; }
        public int CurrentLevel { get; set; }
        public string CurrentStatus { get; set; }
       
        public string ProgressStatus { get; set; }
        public string PaymentReimbursementStatus { get; set; }
        public string ExportStatus { get; set; }
        public string AmountType { get; set; }
        public virtual ItemAccount ItemAccount { get; set; }
        
        public virtual AppUser AppUser { get; set; }
        public virtual IList<CostSharingRequestDetail> CostSharingRequestDetails { get; set; }
        public virtual IList<CostSharingRequestStatus> CostSharingRequestStatuses { get; set; }
        public virtual IList<CSRAttachment> CSRAttachments { get; set; }
        #region CostSharingStatus
        public virtual CostSharingRequestStatus GetCostSharingRequestStatus(int Id)
        {
            foreach (CostSharingRequestStatus CSRS in CostSharingRequestStatuses)
            {
                if (CSRS.Id == Id)
                    return CSRS;
            }
            return null;
        }
        public virtual CostSharingRequestStatus GetCashPaymentRequestStatusworkflowLevel(int workflowLevel)
        {
            foreach (CostSharingRequestStatus CSRS in CostSharingRequestStatuses)
            {
                if (CSRS.WorkflowLevel == workflowLevel)
                    return CSRS;
            }
            return null;
        }
        public virtual IList<CostSharingRequestStatus> GetCostSharingRequestStatusByRequestId(int RequestId)
        {
            IList<CostSharingRequestStatus> LRS = new List<CostSharingRequestStatus>();
            foreach (CostSharingRequestStatus CSRS in CostSharingRequestStatuses)
            {
                if (CSRS.CostSharingRequest.Id == RequestId)
                    LRS.Add(CSRS);
            }
            return LRS;
        }
        public virtual void RemoveCashPaymentRequestStatus(int Id)
        {
            foreach (CostSharingRequestStatus CSRS in CostSharingRequestStatuses)
            {
                if (CSRS.Id == Id)
                    CostSharingRequestStatuses.Remove(CSRS);
                break;
            }
        }
        #endregion
        #region CashPaymentRequestDetail
        public virtual CostSharingRequestDetail GetCostSharingRequestDetails(int Id)
        {

            foreach (CostSharingRequestDetail CSRD in CostSharingRequestDetails)
            {
                if (CSRD.Id == Id)
                    return CSRD;
            }
            return null;
        }

        public virtual IList<CostSharingRequestDetail> GetCostSharingRequestDetailsByPurchaseId(int PurchaseId)
        {
            IList<CostSharingRequestDetail> CSD = new List<CostSharingRequestDetail>();
            foreach (CostSharingRequestDetail CSRD in CostSharingRequestDetails)
            {
                if (CSRD.CostSharingRequest.Id == PurchaseId)
                    CSD.Add(CSRD);
            }
            return CSD;
        }
        public virtual void RemoveCostSharingRequestDetails(int Id)
        {
            foreach (CostSharingRequestDetail CSRD in CostSharingRequestDetails)
            {
                if (CSRD.Id == Id)
                    CostSharingRequestDetails.Remove(CSRD);
                break;
            }
        }
        #endregion
        #region CSAttachment

        public virtual void RemoveCSAttachment(string FilePath)
        {
            foreach (CSRAttachment cpa in CSRAttachments)
            {
                if (cpa.FilePath == FilePath)
                {
                    CSRAttachments.Remove(cpa);
                    break;
                }
            }
        }
        #endregion
    }
}
