using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Setting
{
    public partial class Supplier : IEntity 
    {
        public Supplier()
        { 
        
        }
        public int Id { get; set; }
        public string SupplierName { get; set; }
        public string SupplierAddress { get; set; }
        public string SupplierContact { get; set; }
        public string Email { get; set; }
        public string ContactPhone { get; set; }
        public string Status { get; set; }
        public virtual SupplierType SupplierType { get; set; }
       
    }
}
