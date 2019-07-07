
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.CoreDomain.Admins;
using Chai.WorkflowManagment.CoreDomain.Setting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Chai.WorkflowManagment.CoreDomain.Users
{
    [Table("AssignJobs")]
    public class AssignJob : IEntity
    {
        public AssignJob()
        {
           
        }
        public int Id { get; set; }
       
        public virtual EmployeePosition EmployeePosition { get; set; }
        public virtual AppUser AppUser { get; set; }
        public int AssignedTo { get; set; }
        public bool Status { get; set; }
        

        

    }

}
