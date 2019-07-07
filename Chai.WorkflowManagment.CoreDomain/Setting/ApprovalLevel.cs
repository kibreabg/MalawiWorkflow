using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Setting
{
    public partial class ApprovalLevel : IEntity 
    {
        public ApprovalLevel()
        { 
        
        }


        public int Id { get; set; }

        public int WorkflowLevel { get; set; }
        public string Will { get; set; }
        public virtual EmployeePosition EmployeePosition { get; set; }
        public virtual ApprovalSetting ApprovalSetting { get; set; }
    }
}
