using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Shared;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public class LeaveTypePresenter : Presenter<ILeaveTypeView>
    {

        
        private Chai.WorkflowManagment.Modules.Setting.SettingController _controller;
        public LeaveTypePresenter([CreateNew] Chai.WorkflowManagment.Modules.Setting.SettingController controller)
        {
            _controller = controller;
        }

        public override void OnViewLoaded()
        {
            View.leavetype = _controller.GetLeaveTypes();
        }

        public override void OnViewInitialized()
        {
            
        }
        public IList<LeaveType> GetLeaveTypes()
        {
            return _controller.GetLeaveTypes();
        }

        public void SaveOrUpdateLeaveType(LeaveType leavetype)
        {
            _controller.SaveOrUpdateEntity(leavetype);
        }

        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Setting/Default.aspx?{0}=3", AppConstants.TABID));
        }

        public void DeleteLeaveType(LeaveType LeaveType)
        {
            _controller.DeleteEntity(LeaveType);
        }
        public LeaveType GetLeaveTypeById(int id)
        {
            return _controller.GetLeaveType(id);
        }

        public IList<LeaveType> ListLeaveTypes()
        {
            return _controller.ListLeaveTypes();
          
        }
        public void Commit()
        {
            _controller.Commit();
        }
    }
}




