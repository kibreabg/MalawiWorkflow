using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Request;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.CoreDomain.Setting;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public class DashboardPresenter : Presenter<IDashboardView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
         private Chai.WorkflowManagment.Modules.Request.RequestController _controller;
         private Chai.WorkflowManagment.Modules.Setting.SettingController _settingcontroller;
         private LeaveRequest _leaverequest;
         public DashboardPresenter([CreateNew] Chai.WorkflowManagment.Modules.Request.RequestController controller, [CreateNew] Chai.WorkflowManagment.Modules.Setting.SettingController settingcontroller)
         {
         		_controller = controller;
                _settingcontroller = settingcontroller;
         }

         public override void OnViewLoaded()
         {
            
         }
      
         public override void OnViewInitialized()
         {
            
         }
         
         public void CancelPage()
         {
             _controller.Navigate(String.Format("~/Setting/Default.aspx?{0}=3", AppConstants.TABID));
         }

       
         public AppUser CurrentUser()
         {
             return _controller.GetCurrentUser();
         }

         public void Commit()
         {
             _controller.Commit();
         }
    }
}




