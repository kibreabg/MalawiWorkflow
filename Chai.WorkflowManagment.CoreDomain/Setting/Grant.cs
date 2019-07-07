using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Setting
{
    public partial class Grant : IEntity 
    {
        public Grant()
        {
            this.ProGrants = new List<ProGrant>();
        }
        public int Id { get; set; }
        public string GrantName { get; set; }
        public string GrantCode { get; set; }
        public string Donor { get; set; }
        public string Status { get; set; }
        public IList<ProGrant> ProGrants { get; set; }
       
    }
}
