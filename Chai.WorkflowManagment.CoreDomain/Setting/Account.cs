using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Setting
{
    public partial class Account : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AccountNo { get; set; }
        public string Status { get; set; }
    }
}
