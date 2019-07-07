using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Shared;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public class GrantPresenter : Presenter<IGrantView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private Chai.WorkflowManagment.Modules.Setting.SettingController _controller;
        public GrantPresenter([CreateNew] Chai.WorkflowManagment.Modules.Setting.SettingController controller)
        {
            _controller = controller;
        }

        public override void OnViewLoaded()
        {
            View.grant = _controller.ListGrants(View.GrantName,View.GrantCode);
        }

        public override void OnViewInitialized()
        {
            
        }
        public IList<Grant> GetGrants()
        {
            return _controller.GetGrants();
        }

        public void SaveOrUpdateGrant(Grant grant)
        {
            _controller.SaveOrUpdateEntity(grant);
        }

        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Setting/Default.aspx?{0}=3", AppConstants.TABID));
        }

        public void DeleteGrant(Grant Grant)
        {
            _controller.DeleteEntity(Grant);
        }
        public Grant GetGrantById(int id)
        {
            return _controller.GetGrant(id);
        }

        public IList<Grant> ListGrants(string GrantName,string GrantCode)
        {
            return _controller.ListGrants(GrantName, GrantCode);
          
        }
        public void Commit()
        {
            _controller.Commit();
        }
    }
}




