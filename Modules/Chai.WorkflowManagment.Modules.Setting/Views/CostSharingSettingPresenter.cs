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
    public class CostSharingSettingPresenter : Presenter<ICostSharingSetting>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private Chai.WorkflowManagment.Modules.Setting.SettingController _controller;
        public CostSharingSettingPresenter([CreateNew] Chai.WorkflowManagment.Modules.Setting.SettingController controller)
        {
            _controller = controller;
        }

        public override void OnViewLoaded()
        {
            //View.Projects = _controller.GetProjects();
        }

        public override void OnViewInitialized()
        {

        }
        public IList<Project> GetProjects()
        {
            return _controller.GetProjects();
        }
        public IList<CostSharingSetting> GetCostSharingSettings()
        {

            return _controller.GetCostSharingSettings();
        }
        public IList<Grant> GetGrantbyprojectId(int projectId)
        {
            return _controller.GetProjectGrantsByprojectId(projectId);

        }
        public Grant GetGrant(int GrantId)
        {
            return _controller.GetGrant(GrantId);
        }
        public Project GetProject(int ProjectId)
        {
            return _controller.GetProject(ProjectId);
        }
        public Project GetProjectforCostSharing(int ProjectId)
        {
            return _controller.GetProjectforCostSharing(ProjectId);
        
        }
        public CostSharingSetting GetProjectfromCostSharing(int ProjectId)
        {
            return _controller.GetProjectfromCostSharingSettings(ProjectId);
        }
        public void SaveOrUpdateCostSharing(CostSharingSetting project)
        {
            _controller.SaveOrUpdateEntity(project);
        }
        public void DeleteCostSharingSetting(CostSharingSetting CostSharingSetting)
        {
            _controller.DeleteEntity(CostSharingSetting);
        }
        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Setting/Default.aspx?{0}=3", AppConstants.TABID));
        }

        public void Commit()
        {
            _controller.Commit();
        }
    }
}




