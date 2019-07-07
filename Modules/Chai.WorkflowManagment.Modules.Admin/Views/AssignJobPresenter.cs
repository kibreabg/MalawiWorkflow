using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Web;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.ObjectBuilder;

using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.CoreDomain.Setting;

namespace Chai.WorkflowManagment.Modules.Admin.Views
{
    public class AssignJobPresenter : Presenter<IAssignJobView>
    {
        private AdminController _controller;

        private AssignJob _assignJob;

        public AssignJobPresenter([CreateNew] AdminController controller)
        {
            _controller = controller;

        }
        public override void OnViewLoaded()
        {

        }
        public override void OnViewInitialized()
        {

        }

        public AssignJob CurrentAssignJob
        {
            get
            {
                if (_assignJob == null)
                {
                    int id = View.GetId;
                    if (id > 0)
                        _assignJob = _controller.GetAssignedJobbycurrentuser();
                    else
                        _assignJob = new AssignJob();
                }
                return _assignJob;
            }
        }
        public void SaveOrUpdateAssignJob()
        {
            AssignJob assignjob = CurrentAssignJob;

            if (assignjob.Id <= 0)

                assignjob.EmployeePosition = _controller.GetCurrentUser().EmployeePosition;
            assignjob.AssignedTo = View.GetAssisnTo;
            assignjob.Status = View.Getstatus;
            assignjob.AppUser = _controller.GetUser(_controller.GetCurrentUser().Id);

            _controller.SaveOrUpdateEntity(assignjob);
        }
        public void CancelPage()
        {
            _controller.Navigate(String.Format("../Default.aspx?{0}=0", AppConstants.TABID));
        }
        public void RedirectPage(string url)
        {
            _controller.Navigate(url);
        }

        public AppUser GetUser()
        {
            return _controller.GetUser(_controller.GetCurrentUser().Id);
        }
        public void SaveorUpdateUser(bool isAssigned)
        {
            AppUser user = _controller.GetUser(_controller.GetCurrentUser().Id);
            user.IsAssignedJob = isAssigned;
            _controller.SaveOrUpdateUser(user);
        }
        public IList<AppUser> GetUsers()
        {
            return _controller.GetUsers();
        }
        public IList<AppUser> GetApprovers()
        {
            return _controller.GetApprovers();
        }
        public AppUser GetUser(int userid)
        {
            return _controller.GetUser(userid);
        }
        public IList<EmployeePosition> GetEmployeePositions()
        {
            return _controller.GetEmployeePositions();
        }
        public EmployeePosition GetEmployeePosition(int EmployeePositionId)
        {
            return _controller.GetEmployeePosition(EmployeePositionId);
        }
        public AssignJob GetAssignedJobbycurrentuser()
        {
            return _controller.GetAssignedJobbycurrentuser();
        }
    }
}





