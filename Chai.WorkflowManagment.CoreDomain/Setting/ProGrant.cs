using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Setting
{
    [Table("ProGrants")]
    public partial class ProGrant : IEntity 
    {
        public ProGrant()
        { 
        
        }
        public int Id { get; set; }
        public Grant Grant { get; set; }
        public Project Project { get; set; }
        public Nullable<DateTime> GrantDate { get; set; }
      
       
    }
}
