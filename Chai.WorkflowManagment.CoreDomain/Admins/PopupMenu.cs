
using System;
using System.Collections.Generic;

namespace Chai.WorkflowManagment.CoreDomain.Admins
{
    public class PopupMenu : IEntity
    {
        public int Id { get; set; }
        public int Position { get; set; }

        public virtual Node Node { get; set; }
        public virtual Tab Tab { get; set; }
    }

}
