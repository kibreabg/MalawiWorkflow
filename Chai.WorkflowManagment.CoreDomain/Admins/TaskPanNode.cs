
using System;
using System.Collections.Generic;

namespace Chai.WorkflowManagment.CoreDomain.Admins
{
    public class TaskPanNode : IEntity
    {
        public int Id { get; set; }
        public int Position { get; set; }

        public virtual Node Node { get; set; }
        public virtual TaskPan TaskPan { get; set; }
    }

}
