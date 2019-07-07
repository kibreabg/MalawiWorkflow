using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Setting;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class ExpenseLiquidationRequestDetail : IEntity
    {
        public int Id { get; set; }
        public decimal AmountAdvanced { get; set; }
        public decimal ActualExpenditure { get; set; }
        public decimal Variance { get; set; }
        public string RefNo { get; set; }
        public virtual Project Project { get; set; }
        public virtual Grant Grant { get; set; }
        public virtual ItemAccount ItemAccount { get; set; }
        public virtual ExpenseType ExpenseType {get;set;}
        public virtual ExpenseLiquidationRequest ExpenseLiquidationRequest { get; set; }
    }
}
