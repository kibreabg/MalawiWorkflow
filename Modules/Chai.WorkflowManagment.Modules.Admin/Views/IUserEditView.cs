using System;
using System.Collections.Generic;
using System.Text;
using Chai.WorkflowManagment.CoreDomain;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.Modules.Admin.Views
{
    public interface IUserEditView
    {
        string GetUserId { get; }
        string GetUserName { get; }
        string GetFirstName { get; }
        string GetLastName { get; }
        string GetEmail { get; }
        bool GetIsActive { get; }
        string GetPassword { get; }
        int Superviser { get; }
        string GetEmployeeNo { get; }
        EmployeePosition EmployeePosition { get; }
       
       

        //IList<Role> Roles { set; }
        
    }
}




