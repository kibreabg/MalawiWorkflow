using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

using Chai.WorkflowManagment.CoreDomain.Admins;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.CoreDomain.DataAccess;
using Chai.WorkflowManagment.CoreDomain.Infrastructure;
using Chai.WorkflowManagment.CoreDomain;

namespace Chai.WorkflowManagment.Services
{
    public class AdminServices
    {
        public AdminServices()
        {
        }

        private IEnumerable<AppUser> _users;
        public IEnumerable<AppUser> GetAllUsers { get { return _users ?? (_users = Dao.Query<AppUser>(x => x.AppUserRoles)); } }

        public AppUser GetUserByUserName(string username)
        {
            return  GetAllUsers.Single(x => x.UserName == username);
        }
                
        public IEnumerable<AppUser> SearchUsers(string username)
        {
            
            return Dao.Query<AppUser>(x => x.UserName.StartsWith(username));
        }

        private IEnumerable<Node> _nodes;
        public IEnumerable<Node> GetAllNodes { get { return _nodes ?? (_nodes = Dao.Query<Node>(x => x.NodeRoles.Select(y => y.Role))); } }

        public Node ActiveNode(string pageid)
        {
            return Dao.Single<Node>(x => x.PageId == pageid, x => x.NodeRoles.Select(y => y.Role));
        }
        
        public void MoveTabUp(Tab tab)
        {
            UpdateTabPosition(tab.Id, 1);
        }

        public void MoveTabDown(Tab tab)
        {
            UpdateTabPosition(tab.Id, 2);
        }
        public void MoveUpTaskPan(int panid)
        {
            UpdateTaskpanPosition(panid, 1);
        }

        public void MoveDownTaskPan(int panid)
        {
            UpdateTaskpanPosition(panid, 2);
        }
        public void MoveUpPanNode(int id)
        {
            UpdateTaskpanNodePosition(id,1);
        }
        public void MoveDownPanNode(int id)
        {
            UpdateTaskpanNodePosition(id,2);
        }

        public int GetMaxTabPosition()
        {
            int? val = Dao.Select<Tab, int?>(x => x.Position, null).Max();

           if (val.HasValue)
               return val.Value + 1;

           return 1;
        }

        private void UpdateTabPosition(int tabId, int direction)
        {
            var tabIdParameter = new ObjectParameter("TabId", tabId);
            var directionParameter = new ObjectParameter("Direction", direction);

            Dao.ExecuteFunction("UpdateTabPosition", tabIdParameter, directionParameter);
        }

        private void UpdateTaskpanNodePosition(int panNodeId, int direction)
        {
            var panNodeIdParameter = new ObjectParameter("PanNodeId", panNodeId);
            var directionParameter = new ObjectParameter("Direction", direction);

            Dao.ExecuteFunction("UpdateTaskpanNodePosition", panNodeIdParameter, directionParameter);
        }

        private void UpdateTaskpanPosition(int panId, int direction)
        {
            var panIdParameter = new ObjectParameter("PanId", panId);
            var directionParameter = new ObjectParameter("Direction", direction);

            Dao.ExecuteFunction("UpdateTaskpanPosition", panIdParameter, directionParameter);
        }

    }
}
