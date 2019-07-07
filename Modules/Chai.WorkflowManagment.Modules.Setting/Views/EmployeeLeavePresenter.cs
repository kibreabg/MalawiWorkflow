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
    public class EmployeeLeavePresenter : Presenter<IEmployeeLeave>
    {

        
        private Chai.WorkflowManagment.Modules.Setting.SettingController _controller;
        public EmployeeLeavePresenter([CreateNew] Chai.WorkflowManagment.Modules.Setting.SettingController controller)
        {
            _controller = controller;
        }

        public override void OnViewLoaded()
        {
           
        }

        public override void OnViewInitialized()
        {
            
        }
        public AppUser GetUser(int Userd) 
        {
            return _controller.GetUser(Userd);
        }
        public IList<AppUser> GetEmployeeList()
        {
            return _controller.GetEmployeeList();
        }
        public IList<EmployeeLeave> GetEmployeeLeaves()
        {
            return _controller.GetEmployeeLeaves();
        }
        public EmployeeLeave GetEmployeeLeave(int Id)
        {
            return _controller.GetEmployeeLeave( Id);
        }
        public IList<EmployeeLeave> GetEmployeeLeaves(int UserId)
        {
            return _controller.GetEmployeeLeaves(UserId);
        }
        public EmployeeLeave GetActiveEmployeeLeave(int UserId, bool Status)
        {
            return _controller.GetActiveEmployeeLeave(UserId, Status);
        }
      

        public void SaveOrUpdateEmployeeleave(EmployeeLeave EmployeeLeave)
        {
            _controller.SaveOrUpdateEntity(EmployeeLeave);
        }
        public void SaveOrUpdateAppUser(AppUser AppUser)
        {
            _controller.SaveOrUpdateEntity(AppUser);
        }
        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Setting/frmEmployeeLeave.aspx?{0}=3", AppConstants.TABID));
        }

        public void Commit()
        {
            _controller.Commit();
        }
    }
}




