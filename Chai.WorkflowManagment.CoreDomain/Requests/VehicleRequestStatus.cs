using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;
namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    [Table("VehicleRequestStatuses")]
    public partial class VehicleRequestStatus : IEntity
    {
        public int Id { get; set; }
        public Nullable<DateTime> Date { get; set; }
        public string ApprovalStatus { get; set; }
        public string Comment { get; set; }
        public string RejectedReason { get; set; }        
        public int Approver { get; set; }
        public string AssignedBy { get; set; }
        public int WorkflowLevel { get; set; }
        public virtual VehicleRequest VehicleRequest { get; set; }
        
    }
}
