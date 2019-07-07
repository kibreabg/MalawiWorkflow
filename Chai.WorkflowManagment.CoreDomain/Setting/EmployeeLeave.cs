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
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.CoreDomain.Setting
{
    [Table("EmployeeLeaves")]
    public partial class EmployeeLeave : IEntity
    {
        public EmployeeLeave()
        {

        }
       
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual AppUser AppUser { get; set; }
        public decimal LeaveTaken { get; set; }
        public decimal BeginingBalance { get; set; }
        public bool Status { get; set; }

        public decimal Rate { get; set; }

    }

}
