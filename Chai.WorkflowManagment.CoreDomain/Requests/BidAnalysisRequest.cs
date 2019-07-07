using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;
using System.ComponentModel.DataAnnotations;
using Chai.WorkflowManagment.CoreDomain.Approval;


namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class BidAnalysisRequest : IEntity
    {
        public BidAnalysisRequest()
        {
            this.BidAnalysisRequestStatuses = new List<BidAnalysisRequestStatus>();
            this.BAAttachments = new List<BAAttachment>();
            this.Bidders = new List<Bidder>();
            this.BidAnalysisRequestDetails = new List<BidAnalysisRequestDetail>();

        }
        public int Id { get; set; }
    
     
      
      
        public string RequestNo { get; set; }
        public Nullable<DateTime> RequestDate { get; set; }
        public DateTime AnalyzedDate { get; set; }
  
        public string SpecialNeed { get; set; }
        
        public virtual Supplier Supplier { get; set; }
        public decimal TotalPrice { get; set; }
        public string ReasonforSelection { get; set; }
        public int SelectedBy { get; set; }
        public string Status { get; set; }
        public string PaymentMethod { get; set; }
        public int CurrentApprover { get; set; }
        public Nullable<int> CurrentLevel { get; set; }
        public string  CurrentStatus { get; set; }
        public string ProgressStatus { get; set; }        
        public virtual AppUser AppUser { get; set; }
        public virtual Project Project { get; set; }
        public virtual Grant Grant { get; set; }
        [Required]
        public virtual PurchaseRequest PurchaseRequest { get; set; }
      
        public virtual IList<BidAnalysisRequestStatus> BidAnalysisRequestStatuses { get; set; }
        public virtual PurchaseOrder PurchaseOrders { get; set; }        
        public virtual IList<Bidder> Bidders { get; set; }
        public virtual IList<BAAttachment> BAAttachments { get; set; }
        public virtual IList<BidAnalysisRequestDetail> BidAnalysisRequestDetails { get; set; }
        #region Bidder
        public virtual Bidder GetBidder(int Id)
        {

            foreach (Bidder bidder in Bidders)
            {
                if (bidder.Id == Id)
                    return bidder;

            }
            return null;
        }
        public virtual Bidder GetBidderbyRank()
        {

            foreach (Bidder bidder in Bidders)
            {
                if (bidder.Rank == 1)
                    return bidder;

            }
            return null;
        }
        public  IList<Bidder> GetBidderbyRankone()
        {
            IList<Bidder> Bidders = new List<Bidder>();
            foreach (Bidder bidder in Bidders)
            {
                if (bidder.Rank == 1)
                    Bidders.Add(bidder);

            }
            return Bidders;
        }
        public virtual IList<Bidder> GetBidderByBidAnalysisId(int AnalisisId)
        {
            IList<Bidder> Bidders = new List<Bidder>();
            foreach (Bidder bidder in Bidders)
            {
                if (bidder.BidAnalysisRequest.Id == AnalisisId)
                    Bidders.Add(bidder);

            }
            return Bidders;
        }
        public virtual void RemoveBidder(int Id)
        {

            foreach (Bidder bidder in Bidders)
            {
                if (bidder.Id == Id)
                    Bidders.Remove(bidder);
                break;
            }

        }

        public IList<BidderItemDetail> GetAllBidderItemDetails()
        {
            IList<BidderItemDetail> details = new List<BidderItemDetail>();
            foreach (Bidder b in Bidders)
            {
                foreach (BidderItemDetail det in b.BidderItemDetails)
                {
                    details.Add(det);
                }
            }
            return details;
        }
        #endregion
        #region BidAnalysisRequestDetail
        public virtual BidAnalysisRequestDetail GetBidAnalysisRequestDetail(int Id)
        {

            foreach (BidAnalysisRequestDetail PRS in BidAnalysisRequestDetails)
            {
                if (PRS.Id == Id)
                    return PRS;

            }
            return null;
        }

        public virtual IList<BidAnalysisRequestDetail> GetBidAnalysisRequestDetailByPurchaseId(int PurchaseId)
        {
            IList<BidAnalysisRequestDetail> LRS = new List<BidAnalysisRequestDetail>();
            foreach (BidAnalysisRequestDetail AR in BidAnalysisRequestDetails)
            {
                if (AR.BidAnalysisRequest.Id == PurchaseId)
                    LRS.Add(AR);

            }
            return LRS;
        }
        public virtual void RemoveBidAnalysisRequestDetail(int Id)
        {

            foreach (BidAnalysisRequestDetail PRS in BidAnalysisRequestDetails)
            {
                if (PRS.Id == Id)
                    BidAnalysisRequestDetails.Remove(PRS);
                break;
            }

        }

        #endregion
        #region BidAnalysisRequestStatus
        public virtual BidAnalysisRequestStatus GetBidAnalysisRequestStatus(int Id)
        {

            foreach (BidAnalysisRequestStatus SVRS in BidAnalysisRequestStatuses)
            {
                if (SVRS.Id == Id)
                    return SVRS;

            }
            return null;
        }
        public virtual BidAnalysisRequestStatus GetBidAnalysisRequestStatusworkflowLevel(int workflowLevel)
        {

            foreach (BidAnalysisRequestStatus LRS in BidAnalysisRequestStatuses)
            {
                if (LRS.WorkflowLevel == workflowLevel)
                    return LRS;

            }
            return null;
        }
        public virtual IList<BidAnalysisRequestStatus> GetBidAnalysisRequestStatusByRequestId(int RequestId)
        {
            IList<BidAnalysisRequestStatus> VRS = new List<BidAnalysisRequestStatus>();
            foreach (BidAnalysisRequestStatus VR in BidAnalysisRequestStatuses)
            {
                if (VR.BidAnalysisRequest.Id == RequestId)
                    VRS.Add(VR);

            }
            return VRS;
        }
        public virtual void RemoveBidAnalysisRequestStatus(int Id)
        {

            foreach (BidAnalysisRequestStatus VRS in BidAnalysisRequestStatuses)
            {
                if (VRS.Id == Id)
                    BidAnalysisRequestStatuses.Remove(VRS);
                break;
            }

        }
        #endregion
        #region BAAttachment
        public virtual void RemoveBAAttachment(string FilePath)
        {
            foreach (BAAttachment cpa in BAAttachments)
            {
                if (cpa.FilePath == FilePath)
                {
                    BAAttachments.Remove(cpa);
                    break;
                }
            }
        }
        #endregion
    }
}
