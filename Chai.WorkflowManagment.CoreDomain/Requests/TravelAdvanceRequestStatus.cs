using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    [Table("TravelAdvanceRequestStatuses")]
    public partial class TravelAdvanceRequestStatus : IEntity
    {
        public int Id { get; set; }
        public Nullable<DateTime> Date { get; set; }
        public string ApprovalStatus { get; set; }
        public int Approver { get; set; }
        public int ApproverPosition { get; set; }
        public string PaymentType { get; set; }
        public string AssignedBy { get; set; }
        public int WorkflowLevel { get; set; }
        public string RejectedReason { get; set; }
        public virtual TravelAdvanceRequest TravelAdvanceRequest { get; set; }
    }
}
