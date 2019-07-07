using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Utility;
using Microsoft.Practices.ObjectBuilder;

using Chai.WorkflowManagment.CoreDomain;
using Chai.WorkflowManagment.CoreDomain.DataAccess;
using Chai.WorkflowManagment.CoreDomain.Admins;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Services;
using Chai.WorkflowManagment.Shared.Navigation;
using Chai.WorkflowManagment.CoreDomain.Setting;


namespace Chai.WorkflowManagment.Modules.Admin
{
    public class AdminController : ControllerBase
    {
        private IWorkspace _workspace;

        [InjectionConstructor]
        public AdminController([ServiceDependency] IHttpContextLocatorService httpContextLocatorService, [ServiceDependency]INavigationService navigationService)
            : base(httpContextLocatorService, navigationService)
        {
            _workspace = ZadsServices.Workspace;
        }

        #region Security And Administration
        public IList<Role> GetRoles
        {
            get
            {
                return WorkspaceFactory.CreateReadOnly().Query<Role>(null).ToList();
            }
        }
        public IList<AppUser> GetUsers()
        {
            return WorkspaceFactory.CreateReadOnly().Query<AppUser>(null).ToList();
        }
        public Role GetRoleById(int roleid)
        {
            return _workspace.Single<Role>(x => x.Id == roleid);
        }
        public AppUser GetUser(int userid)
        {
            return _workspace.Single<AppUser>(x => x.Id == userid, x => x.AppUserRoles.Select(y => y.Role));
        }
        public IList<AppUser> SearchUsers(string username)
        {
            return ZadsServices.AdminServices.SearchUsers(username).ToList();
        }
        public void SaveOrUpdateUser(AppUser user)
        {
            if (user.Id <= 0)
            {
                user.DateCreated = DateTime.Now;
                user.DateModified = DateTime.Now;

                using (var wr = WorkspaceFactory.CreateReadOnly())
                {
                    if (wr.Single<AppUser>(x => x.UserName == user.UserName) != null)
                        throw new Exception("User name already exists");
                }
            }
            else
            {
                foreach (AppUserRole r in user.AppUserRoles)
                {
                    if (r.Id == 0)
                        _workspace.Add(r);
                }
            }

            SaveOrUpdateEntity<AppUser>(user);
        }
        public void RemoveListOfObjects<T>(T[] items) where T : class
        {
            for (int i = 0; i < items.Length; i++)
            {
                _workspace.Delete<T>(items[i]);
            }
        }
        public Node GetNodeById(int nodeid)
        {
            return _workspace.Single<Node>(x => x.Id == nodeid, x => x.NodeRoles.Select(y => y.Role));
        }
        public IList<Node> GetListOfNodeByModuleId(int modid)
        {
            return WorkspaceFactory.CreateReadOnly().Query<Node>(x => x.PocModule.Id == modid).ToList();
        }
        public IList<Node> GetListOfAllNodes()
        {
            return WorkspaceFactory.CreateReadOnly().Query<Node>(null).ToList();
        }
        public PocModule GetModuleById(int modid)
        {
            return _workspace.Single<PocModule>(x => x.Id == modid);
        }
        public IList<PocModule> GetListOfAllPocModules()
        {
            return WorkspaceFactory.CreateReadOnly().Query<PocModule>(null).ToList();
        }
        public Tab GetTabById(int tabid)
        {
            return _workspace.Single<Tab>(x => x.Id == tabid, x => x.PocModule, x => x.TabRoles.Select(y => y.Role), x => x.TaskPans.Select(z => z.TaskPanNodes.Select(w => w.Node)));
        }
        public IEnumerable<Tab> GetListOfAllTab()
        {
            return WorkspaceFactory.CreateReadOnly().Query<Tab>(null);
        }
        public void MoveTabUp(Tab tab)
        {
            ZadsServices.AdminServices.MoveTabUp(tab);
            _workspace.Refresh(tab);
        }
        public void MoveTabDown(Tab tab)
        {
            ZadsServices.AdminServices.MoveTabDown(tab);
        }
        public void MoveUpTaskPan(int panid)
        {
            ZadsServices.AdminServices.MoveUpTaskPan(panid);
        }
        public void MoveDownTaskPan(int panid)
        {
            ZadsServices.AdminServices.MoveDownTaskPan(panid);
        }
        public void MoveUpPanNode(int id)
        {
            ZadsServices.AdminServices.MoveUpPanNode(id);
        }
        public void MoveDownPanNode(int id)
        {
            ZadsServices.AdminServices.MoveDownPanNode(id);
        }
        public int GetMaxTabPosition()
        {
            return ZadsServices.AdminServices.GetMaxTabPosition();
        }
        #endregion
        #region Setting
        public IList<EmployeePosition> GetEmployeePositions()
        {
            return WorkspaceFactory.CreateReadOnly().Query<EmployeePosition>(null).ToList();
        }
        public EmployeePosition GetEmployeePosition(int EmployeePositionId)
        {
            return _workspace.Single<EmployeePosition>(x => x.Id == EmployeePositionId);
        }
        public IList<Project> GetProjects()
        {
            return WorkspaceFactory.CreateReadOnly().Query<Project>(null).ToList();
        }
        public Project GetProject(int Project)
        {
            return _workspace.Single<Project>(x => x.Id == Project);
        }
        #endregion
        #region Assignjob
        public IList<AssignJob> GetAssignJobs()
        {
            return WorkspaceFactory.CreateReadOnly().Query<AssignJob>(null).ToList();
        }
        public AssignJob GetAssignJob(int AssignJobId)
        {
            return _workspace.Single<AssignJob>(x => x.Id == AssignJobId && x.Status == true);
        }
        public AssignJob GetAssignedJobbycurrentuser()
        {
            int userId = GetCurrentUser().Id;
            return _workspace.Single<AssignJob>(x => x.AppUser.Id == userId && x.Status == true);
        }
        public IList<AppUser> GetApprovers()
        {
            int userId = GetCurrentUser().Id;
            return _workspace.SqlQuery<AppUser>("SELECT DISTINCT(AppUsers.Id), AppUsers.* FROM AppUsers INNER JOIN AppUserRoles ON AppUserRoles.AppUser_Id = AppUsers.Id INNER JOIN Roles ON AppUserRoles.Role_Id = Roles.Id WHERE (Roles.Name != 'Requester' AND Roles.Name != 'Administrator' AND Roles.Name != 'Preparer' AND AppUsers.Id != '" + userId + "') ORDER BY AppUsers.Id;").ToList();
        }
        #endregion
        #region Drivers
        public IList<AppUser> GetDrivers()
        {
            return WorkspaceFactory.CreateReadOnly().Query<AppUser>(x => x.EmployeePosition.PositionName == "Driver" || x.EmployeePosition.PositionName == "Admin/HR Assisitance (Driver)").ToList();
        }

        public AppUser GetAssignDriver(int Id)
        {
            return _workspace.Single<AppUser>(x => x.Id == Id);
        }
       
        #endregion

        #region Entity Manipulation
        public void SaveOrUpdateEntity<T>(T item) where T : class
        {
            IEntity entity = (IEntity)item;
            if (entity.Id == 0)
                _workspace.Add<T>(item);
            else
                _workspace.Update<T>(item);

            _workspace.CommitChanges();
            _workspace.Refresh(item);
        }
        public void DeleteEntity<T>(T item) where T : class
        {
            _workspace.Delete<T>(item);
            _workspace.CommitChanges();
            _workspace.Refresh(item);
        }
        #endregion

    }
}
