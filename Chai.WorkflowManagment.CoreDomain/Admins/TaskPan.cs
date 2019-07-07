
using System;
using System.Collections.Generic;

namespace Chai.WorkflowManagment.CoreDomain.Admins
{
    public class TaskPan : IEntity
    {
        public TaskPan()
        {
            this.TaskPanNodes = new List<TaskPanNode>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public int Position { get; set; }
        public string ImagePath { get; set; }

        public virtual Tab Tab { get; set; }
        public virtual IList<TaskPanNode> TaskPanNodes { get; set; }

        public TaskPanNode GetTaskPanNodeById(int tpnodeid)
        {
            foreach (TaskPanNode tp in TaskPanNodes)
            {
                if (tp.Id == tpnodeid)
                    return tp;
            }
            return null;
        }

        public bool NodeWasAddedToPan(int nodeid)
        {
            foreach (TaskPanNode pm in TaskPanNodes)
            {
                if (pm.Node.Id == nodeid)
                    return true;
            }
            return false;
        }
    }

}
