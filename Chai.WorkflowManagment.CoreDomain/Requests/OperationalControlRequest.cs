using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class OperationalControlRequest : IEntity
    {
        public OperationalControlRequest()
        {
            this.OperationalControlRequestDetails = new List<OperationalControlRequestDetail>();
            this.OperationalControlRequestStatuses = new List<OperationalControlRequestStatus>();
            this.OCRAttachments = new List<OCRAttachment>();
        }
        public int Id { get; set; }
        public string RequestNo { get; set; }
        public Nullable<DateTime> RequestDate { get; set; }
        public string Payee { get; set; }
        public string Description { get; set; }
        public string VoucherNo { get; set; }
        public string PageType { get; set; }
        public string BranchCode { get; set; }
        public string BankName { get; set; }
        public string PaymentMethod { get; set; }
        public int CurrentApprover { get; set; }
        public int CurrentLevel { get; set; }
        public string CurrentStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalActualExpendture { get; set; }
        public string ProgressStatus { get; set; }
        public string PaymentReimbursementStatus { get; set; }
        public string ExportStatus { get; set; }
        public virtual Beneficiary Beneficiary { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual Account Account { get; set; }
        public virtual IList<OperationalControlRequestDetail> OperationalControlRequestDetails { get; set; }
        public virtual IList<OperationalControlRequestStatus> OperationalControlRequestStatuses { get; set; }
        public virtual IList<OCRAttachment> OCRAttachments { get; set; }

        #region OperationalControlRequestStatus
        public virtual OperationalControlRequestStatus GetOperationalControlRequestStatus(int Id)
        {
            foreach (OperationalControlRequestStatus CPRS in OperationalControlRequestStatuses)
            {
                if (CPRS.Id == Id)
                    return CPRS;
            }
            return null;
        }
        public virtual OperationalControlRequestStatus GetOperationalControlRequestStatusworkflowLevel(int workflowLevel)
        {
            foreach (OperationalControlRequestStatus CPRS in OperationalControlRequestStatuses)
            {
                if (CPRS.WorkflowLevel == workflowLevel)
                    return CPRS;
            }
            return null;
        }
        public virtual IList<OperationalControlRequestStatus> GetOperationalControlRequestStatusByRequestId(int RequestId)
        {
            IList<OperationalControlRequestStatus> LRS = new List<OperationalControlRequestStatus>();
            foreach (OperationalControlRequestStatus CPRS in OperationalControlRequestStatuses)
            {
                if (CPRS.OperationalControlRequest.Id == RequestId)
                    LRS.Add(CPRS);
            }
            return LRS;
        }
        public virtual void RemoveOperationalControlRequestStatus(int Id)
        {
            foreach (OperationalControlRequestStatus CPRS in OperationalControlRequestStatuses)
            {
                if (CPRS.Id == Id)
                    OperationalControlRequestStatuses.Remove(CPRS);
                break;
            }
        }
        #endregion
        #region OperationalControlRequestDetail
        public virtual OperationalControlRequestDetail GetOperationalControlRequestDetail(int Id)
        {
            foreach (OperationalControlRequestDetail CPRS in OperationalControlRequestDetails)
            {
                if (CPRS.Id == Id)
                    return CPRS;
            }
            return null;
        }

        public virtual IList<OperationalControlRequestDetail> GetOperationalControlRequestDetailByPurchaseId(int PurchaseId)
        {
            IList<OperationalControlRequestDetail> LRS = new List<OperationalControlRequestDetail>();
            foreach (OperationalControlRequestDetail CPRS in OperationalControlRequestDetails)
            {
                if (CPRS.OperationalControlRequest.Id == PurchaseId)
                    LRS.Add(CPRS);
            }
            return LRS;
        }
        public virtual void RemoveOperationalControlRequestDetail(int Id)
        {
            foreach (OperationalControlRequestDetail CPRS in OperationalControlRequestDetails)
            {
                if (CPRS.Id == Id)
                    OperationalControlRequestDetails.Remove(CPRS);
                break;
            }
        }
        #endregion
        #region CPAttachment

        public virtual void RemoveOCAttachment(string FilePath)
        {
            foreach (OCRAttachment cpa in OCRAttachments)
            {
                if (cpa.FilePath == FilePath)
                {
                    OCRAttachments.Remove(cpa);
                    break;
                }
            }
        }
        #endregion
    }
}
