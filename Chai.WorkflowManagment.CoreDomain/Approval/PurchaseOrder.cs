using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.CoreDomain.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Approval
{
    [Table("PurchaseOrders")]
    public partial class PurchaseOrder : IEntity
    {

        public PurchaseOrder()
        {
            this.PurchaseOrderDetails = new List<PurchaseOrderDetail>();
        }

        public int Id { get; set; }
       [Required]
        public virtual BidAnalysisRequest BidAnalysisRequest { get; set; }
      
       
        public Nullable<DateTime> PODate { get; set; }
        public Supplier Supplier { get; set; }
        public string PoNumber { get; set; }
        public string Billto { get; set; }
        public string ShipTo { get; set; }
        public decimal DeliveryFees { get; set; }
        public string PaymentTerms { get; set; }
        public string Status { get; set; }
        public decimal TotalPrice { get; set; }
        public IList<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }

        #region PurchaseOrderDetail
        public virtual PurchaseOrderDetail GetPurchaseOrderDetail(int Id)
        {

            foreach (PurchaseOrderDetail detail in PurchaseOrderDetails)
            {
                if (detail.Id == Id)
                    return detail;

            }
            return null;
        }


        public virtual void RemovePurchaseOrderDetail(int Id)
        {

            foreach (PurchaseOrderDetail detail in PurchaseOrderDetails)
            {
                if (detail.Id == Id)
                    PurchaseOrderDetails.Remove(detail);
                break;
            }

        }

        #endregion

    }
}
