using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public class ProjectPresenter : Presenter<IProjectView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private Chai.WorkflowManagment.Modules.Setting.SettingController _controller;
        public ProjectPresenter([CreateNew] Chai.WorkflowManagment.Modules.Setting.SettingController controller)
        {
            _controller = controller;
        }

        public override void OnViewLoaded()
        {
            View.Projects = _controller.GetProjects();
        }

        public override void OnViewInitialized()
        {

        }
        public IList<Project> GetProjects()
        {
            return _controller.GetProjects();
        }

        public void SaveOrUpdateProject(Project project)
        {
            _controller.SaveOrUpdateEntity(project);
        }
        public Project GetProject(int ProjectId)
        {
            return _controller.GetProject(ProjectId);
        }
        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Setting/Default.aspx?{0}=3", AppConstants.TABID));
        }

        public void DeleteProject(Project project)
        {
            _controller.DeleteEntity(project);
        }
        public void DeleteProjectGrant(ProGrant ProjectGrant)
        {
            _controller.DeleteEntity(ProjectGrant);
        }
        public ProGrant GetProjectGrant(int Id)
        {
            return _controller.GetProjectGrant(Id);
        }
        public IList<Project> ListProjects(string ProjectCode)
        {
            return _controller.ListProjects(ProjectCode);

        }
        public IList<Grant> ListGrant()
        {
            return _controller.GetGrants();

        }
        public Grant GetGrant(int GrantId)
        {
            return _controller.GetGrant(GrantId);

        }
        public IList<AppUser> GetProgramManagers()
        {
            return _controller.GetProgramManagers();
        }
        public AppUser GetUser(int userid)
        {
            return _controller.GetUser(userid);
        }
        public void Commit()
        {
            _controller.Commit();
        }
    }
}




