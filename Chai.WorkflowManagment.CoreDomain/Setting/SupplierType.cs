using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Setting
{
    public partial class SupplierType : IEntity 
    {
        public SupplierType()
        {
            this.Suppliers = new List<Supplier>();     
        }
        public int Id { get; set; }
        public string SupplierTypeName { get; set; }
        public string Status { get; set; }
        public IList<Supplier> Suppliers { get; set; }
       
    }
}
