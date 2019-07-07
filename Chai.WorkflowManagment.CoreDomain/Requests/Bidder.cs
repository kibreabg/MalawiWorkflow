using Chai.WorkflowManagment.CoreDomain.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class Bidder : IEntity
    {

        public Bidder()
        {
            this.BidderItemDetails = new List<BidderItemDetail>();
        }

        public int Id { get; set; }
        public virtual BidAnalysisRequest BidAnalysisRequest { get; set; }
        public string ContactDetails { get; set; }
        public string LeadTimefromSupplier { get; set; }
       
        public string SpecialTermsDelivery { get; set; }
        public int Rank { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual SupplierType SupplierType { get; set; }
        [NotMapped]
        public string GetSelectionReason
        {
            get
            {
                if (Rank == 1)
                {
                    return BidAnalysisRequest.ReasonforSelection;

                }
                else
                {
                    return "";
                }
            }
        }
        //   public virtual ItemAccount ItemAccount { get; set; }

        //    public int Qty { get; set; }

        //  public decimal UnitCost { get; set; }
        //  public decimal TotalCost { get; set; }    

        public virtual IList<BidderItemDetail> BidderItemDetails { get; set; }

        #region Bidder
        public virtual BidderItemDetail GetBidderItemDetail(int Id)
        {

            foreach (BidderItemDetail BidderItemDetail in BidderItemDetails)
            {
                if (BidderItemDetail.Id == Id)
                    return BidderItemDetail;

            }
            return null;
        }
        
        public virtual IList<BidderItemDetail> GetBidderItemDetailByBidderId(int bidderId)
        {
            IList<BidderItemDetail> Bidders = new List<BidderItemDetail>();
            foreach (BidderItemDetail BidderItemDetail in BidderItemDetails)
            {
                if (BidderItemDetail.Bidder.Id == bidderId)
                    Bidders.Add(BidderItemDetail);

            }
            return Bidders;
        }
        public virtual void RemoveBidderItemDetail(int Id)
        {

            foreach (BidderItemDetail BidderItemDetail in BidderItemDetails)
            {
                if (BidderItemDetail.Id == Id)
                    BidderItemDetails.Remove(BidderItemDetail);
                break;
            }

        }

        #endregion
    }
}
