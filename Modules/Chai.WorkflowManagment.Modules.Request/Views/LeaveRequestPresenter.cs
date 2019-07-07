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
    public class LeaveRequestPresenter : Presenter<ILeaveRequestView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private Chai.WorkflowManagment.Modules.Request.RequestController _controller;
        private Chai.WorkflowManagment.Modules.Setting.SettingController _settingcontroller;
        private LeaveRequest _leaverequest;
        public LeaveRequestPresenter([CreateNew] Chai.WorkflowManagment.Modules.Request.RequestController controller, [CreateNew] Chai.WorkflowManagment.Modules.Setting.SettingController settingcontroller)
        {
            _controller = controller;
            _settingcontroller = settingcontroller;
        }

        public override void OnViewLoaded()
        {
            if (View.LeaveRequestId > 0)
            {
                _controller.CurrentObject = _controller.GetLeaveRequest(View.LeaveRequestId);
            }
            CurrentLeaveRequest = _controller.CurrentObject as LeaveRequest;
        }
        public LeaveRequest CurrentLeaveRequest
        {
            get
            {
                if (_leaverequest == null)
                {
                    int id = View.LeaveRequestId;
                    if (id > 0)
                        _leaverequest = _controller.GetLeaveRequest(id);
                    else
                        _leaverequest = new LeaveRequest();
                }
                return _leaverequest;
            }
            set { _leaverequest = value; }
        }
        public override void OnViewInitialized()
        {
            if (_leaverequest == null)
            {
                int id = View.LeaveRequestId;
                if (id > 0)
                    _controller.CurrentObject = _controller.GetLeaveRequest(id);
                else
                    _controller.CurrentObject = new LeaveRequest();
            }
        }
        public IList<LeaveRequest> GetLeaveRequests()
        {
            return _controller.GetLeaveRequests();
        }

        public AppUser Approver(int Position)
        {
            return _controller.Approver(Position);
        }
        public AppUser GetUser(int UserId)
        {
            return _controller.GetSuperviser(UserId);
        }
        public AppUser GetSuperviser(int superviser)
        {
            return _controller.GetSuperviser(superviser);
        }
        public void SaveOrUpdateLeaveRequest(LeaveRequest LeaveRequest)
        {
            _controller.SaveOrUpdateEntity(LeaveRequest);
        }
        public int GetLastLeaveRequestId()
        {
            return _controller.GetLastLeaveRequestId();
        }
        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Setting/Default.aspx?{0}=3", AppConstants.TABID));
        }
        public void DeleteLeaveRequestg(LeaveRequest LeaveRequest)
        {
            _controller.DeleteEntity(LeaveRequest);
        }
        public LeaveRequest GetLeaveRequestById(int id)
        {
            return _controller.GetLeaveRequest(id);
        }
        public ApprovalSetting GetApprovalSetting(string RequestType, int value)
        {
            return _settingcontroller.GetApprovalSettingforProcess(RequestType, value);
        }
        public IList<LeaveRequest> ListLeaveRequests(string requestNo, string RequestDate)
        {
            return _controller.ListLeaveRequests(requestNo, RequestDate);
        }
        public EmployeeLeave GetEmployeeLeave()
        {
            return _settingcontroller.GetActiveEmployeeLeaveRequest(CurrentUser().Id, true);
        }
        public LeaveType GetLeaveType(int Id)
        {
            return _settingcontroller.GetLeaveType(Id);
        }
        public IList<LeaveType> GetLeaveTypes()
        {
            return _settingcontroller.GetLeaveTypes();
        }
        public AssignJob GetAssignedJobbycurrentuser()
        {
            return _controller.GetAssignedJobbycurrentuser();
        }
        public AssignJob GetAssignedJobbycurrentuser(int UserId)
        {
            return _controller.GetAssignedJobbycurrentuser(UserId);
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




