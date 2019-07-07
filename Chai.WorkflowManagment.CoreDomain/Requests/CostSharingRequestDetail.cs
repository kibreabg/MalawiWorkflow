using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Setting;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class CostSharingRequestDetail : IEntity
    {
        public int Id { get; set; }
        public decimal SharedAmount { get; set; }
        public virtual CostSharingRequest CostSharingRequest { get; set; }
        public virtual Project Project { get; set; }
        public virtual Grant Grant { get; set; }
   
    }
}
