using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Setting;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class TravelAdvanceCost : IEntity
    {
        public TravelAdvanceCost()
        {
        }
        public int Id { get; set; }
        public string AccountCode { get; set; }
        public int Days { get; set; }
        public decimal UnitCost { get; set; }
        public int NoOfUnits { get; set; }
        public decimal Total { get; set; }
        public virtual TravelAdvanceRequestDetail TravelAdvanceRequestDetail { get; set; }
        public virtual ItemAccount ItemAccount { get; set; }
        public virtual ExpenseType ExpenseType { get; set; }
    }
}
