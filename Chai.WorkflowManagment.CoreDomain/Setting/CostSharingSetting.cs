using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Setting
{
    public partial class CostSharingSetting : IEntity
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public Nullable<int> GrantId { get; set; }
        public decimal Percentage { get; set; }
        public virtual Project Project { get; set; }
        public virtual Grant Grant {get;set;}
    }
}
