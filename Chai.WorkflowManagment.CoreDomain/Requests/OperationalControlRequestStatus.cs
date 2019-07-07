using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Setting;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    [Table("OperationalControlRequestStatuses")]
    public partial class OperationalControlRequestStatus : IEntity
    {
        public int Id { get; set; }
        public Nullable<DateTime> Date { get; set; }
        public int Approver { get; set; }
        public string AssignedBy { get; set; }
        public string ApprovalStatus { get; set; }
        public string RejectedReason { get; set; }
        public int WorkflowLevel { get; set; }
        public virtual Account Account { get; set; }
        public virtual OperationalControlRequest OperationalControlRequest { get; set; }
    }
}
