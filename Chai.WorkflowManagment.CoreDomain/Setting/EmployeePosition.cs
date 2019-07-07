using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Setting
{
    public partial class EmployeePosition : IEntity 
    {
        public EmployeePosition()
        { 
          
        }
        public int Id { get; set; }
        public string PositionName { get; set; }
        public string Status { get; set; }
       
    }
}
