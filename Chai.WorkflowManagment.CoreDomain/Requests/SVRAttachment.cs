using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class SVRAttachment : IEntity
    {
        public int Id { get; set; }
        public string FilePath { get; set; }
       
        public virtual SoleVendorRequest SoleVendorRequest { get; set; }
    }
}
