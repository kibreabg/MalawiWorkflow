using Chai.WorkflowManagment.CoreDomain.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class BidderItemDetail : IEntity
    {

        public BidderItemDetail()
        { 
        
        }

        public int Id { get; set; }
        public virtual Bidder Bidder { get; set; }
        public virtual ItemAccount ItemAccount { get; set; }

        public string ItemDescription { get; set; }
        public int Qty { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
        
    }
}
