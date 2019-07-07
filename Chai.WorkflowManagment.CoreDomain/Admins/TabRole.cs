
using System;
using System.Collections;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.CoreDomain.Admins
{
    public class TabRole : IEntity
	{
        public int Id { get; set; }
        public bool ViewAllowed { get; set; }

        public virtual Role Role { get; set; }
        public virtual Tab Tab { get; set; }
	}

}
