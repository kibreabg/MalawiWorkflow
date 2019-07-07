using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Setting;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class CashPaymentRequestDetail : IEntity
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public decimal ActualExpendture { get; set; }
        public virtual CashPaymentRequest CashPaymentRequest { get; set; }
        public virtual ItemAccount ItemAccount { get; set; }
        public string AccountCode { get; set; }
        public virtual Project Project { get; set; }
        public virtual Grant Grant { get; set; }
    }
}
