using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Setting
{
    public partial class ItemAccount : IEntity 
    {
        public ItemAccount()
        { 
        
        }
        public int Id { get; set; }
        public string AccountName { get; set; }
        public string AccountCode { get; set; }
        public string Status { get; set; }
      
       
    }
}
