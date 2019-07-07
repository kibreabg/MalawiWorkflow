using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;

using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Services;
using Chai.WorkflowManagment.Shared;

namespace Chai.WorkflowManagment.Modules.Admin.Views
{
    public class RoleEditPresenter : Presenter<IRoleEditView>
    {
        private AdminController _controller;
        private Role _role;
        
        public RoleEditPresenter([CreateNew] AdminController controller)
        {
            _controller = controller;
        }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public Role CurrentRole
        {
            get
            {
                if (_role == null)
                {
                    int id = int.Parse(View.GetRoleId);
                    if (id > 0)
                        _role = _controller.GetRoleById(id);
                    else
                        _role = new Role();
                }
                return _role;
            }
        }

        public void SaveOrUpdateRole()
        {
            Role role = CurrentRole;
            role.Name = View.GetRoleName;

            _controller.SaveOrUpdateEntity<Role>(role);
        }

        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Admin/Roles.aspx?{0}=0", AppConstants.TABID));
        }
    }
}




