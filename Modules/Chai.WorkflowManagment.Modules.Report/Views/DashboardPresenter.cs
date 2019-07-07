using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Request;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.CoreDomain.Setting;

namespace Chai.WorkflowManagment.Modules.Report.Views
{
    public class DashboardPresenter : Presenter<IDashboardView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private Chai.WorkflowManagment.Modules.Report.ReportController _controller;
       
         public DashboardPresenter([CreateNew] Chai.WorkflowManagment.Modules.Report.ReportController controller)
         {
         		_controller = controller;
             
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

         
    }
}




