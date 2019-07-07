using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Report;
using System.Data;
using Chai.WorkflowManagment.Modules.Admin;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Modules.Setting;

namespace Chai.WorkflowManagment.Modules.Report.Views
{
    public class frmLeaveReportPresenter : Presenter<IfrmLeaveReportView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private Chai.WorkflowManagment.Modules.Report.ReportController _controller;
        private AdminController _adminController;
        private SettingController _settingController;
        public frmLeaveReportPresenter([CreateNew] Chai.WorkflowManagment.Modules.Report.ReportController controller, AdminController adminController, SettingController settingController)
        {
            _controller = controller;
            _adminController = adminController;
            _settingController = settingController;
        }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }
        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public IList<LeaveType> GetLeaveTypes()
        {
            return _settingController.GetLeaveTypes();
        }

        public IList<AppUser> GetAllUsers()
        {
            return _adminController.GetUsers();
        }
        public IList<LeaveReport> GetLeaveReporto(string DateFrom,string DateTo)
        {
           return _controller.GetLeaveReporto(DateFrom, DateTo);
        }
        public DataSet GetLeaveReport(int EmployeeName, int LeaveType)
        {
            return _controller.GetLeaveReport(EmployeeName, LeaveType);
        }
        // TODO: Handle other view events and set state in the view
    }
}




