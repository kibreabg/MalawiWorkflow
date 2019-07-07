using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Setting
{
    public partial class ExpenseType : IEntity 
    {
        public ExpenseType()
        { 
        
        }
        public int Id { get; set; }
        public string ExpenseTypeName { get; set; }
        public string Status { get; set; }
        
       
    }
}
