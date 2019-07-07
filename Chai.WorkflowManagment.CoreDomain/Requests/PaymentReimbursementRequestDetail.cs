using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Setting;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class PaymentReimbursementRequestDetail : IEntity
    {
        public int Id { get; set; }
        public decimal AmountAdvanced { get; set; }
        public decimal ActualExpenditure { get; set; }
        public decimal Variance { get; set; }
        public virtual Project Project { get; set; }
        public virtual ItemAccount ItemAccount { get; set; }
        public virtual PaymentReimbursementRequest PaymentReimbursementRequest { get; set; }
    }
}
