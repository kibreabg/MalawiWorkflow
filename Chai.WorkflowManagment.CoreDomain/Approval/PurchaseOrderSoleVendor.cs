using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.CoreDomain.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Approval
{
    [Table("PurchaseOrderSoleVendors")]
    public partial class PurchaseOrderSoleVendor : IEntity
    {

        public PurchaseOrderSoleVendor()
        {
            this.PurchaseOrderSoleVendorDetails = new List<PurchaseOrderSoleVendorDetail>();
        }

        public int Id { get; set; }
        [Required]
        public virtual SoleVendorRequest SoleVendorRequest { get; set; }
        public DateTime PODate { get; set; }        
        public string PoNumber { get; set; }
        public string Billto { get; set; }
        public string ShipTo { get; set; }
        public decimal DeliveryFees { get; set; }
        public string PaymentTerms { get; set; }
        public string Status { get; set; }
        public decimal TotalPrice { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual IList<PurchaseOrderSoleVendorDetail> PurchaseOrderSoleVendorDetails { get; set; }

        #region PurchaseOrderSoleVendorDetail
        public virtual PurchaseOrderSoleVendorDetail GetPurchaseOrderSoleVendorDetail(int Id)
        {

            foreach (PurchaseOrderSoleVendorDetail detail in PurchaseOrderSoleVendorDetails)
            {
                if (detail.Id == Id)
                    return detail;

            }
            return null;
        }


        public virtual void RemovePurchaseOrderSoleVendorDetail(int Id)
        {

            foreach (PurchaseOrderSoleVendorDetail detail in PurchaseOrderSoleVendorDetails)
            {
                if (detail.Id == Id)
                    PurchaseOrderSoleVendorDetails.Remove(detail);
                break;
            }

        }

        #endregion

    }
}
