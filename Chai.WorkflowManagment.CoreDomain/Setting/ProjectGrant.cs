using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Setting
{
    public partial class ProjectGrant : IEntity 
    {
        public ProjectGrant()
        { 
        
        }
        public int Id { get; set; }
        public Grant Grant { get; set; }
        public Project project { get; set; }
        public Nullable<DateTime> GrantDate { get; set; }
      
       
    }
}
