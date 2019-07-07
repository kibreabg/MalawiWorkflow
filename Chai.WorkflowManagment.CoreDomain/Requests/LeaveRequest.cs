using Chai.WorkflowManagment.CoreDomain.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Request
{
    public partial class LeaveRequest : IEntity
    {

        public LeaveRequest()
        {
            this.LeaveRequestStatuses = new List<LeaveRequestStatus>();
        }
        public int Id { get; set; }
        public string RequestNo { get; set; }
        public string EmployeeNo { get; set; }
        public int RequestedDays { get; set; }
        public int Requester { get; set; }

        public int Forward { get; set; }
        public int Balance { get; set; }
        public virtual LeaveType LeaveType { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public string CompassionateReason { get; set; }
        public DateTime RequestedDate { get; set; }

        public int CurrentApprover { get; set; }
        public int CurrentLevel { get; set; }
        public string CurrentStatus { get; set; }
        public string ProgressStatus { get; set; }
        public string Addresswhileonleave { get; set; }
        
        public virtual IList<LeaveRequestStatus> LeaveRequestStatuses { get; set; }

        public virtual LeaveRequestStatus GetLeaveRequestStatus(int Id)
        {

            foreach (LeaveRequestStatus LRS in LeaveRequestStatuses)
            {
                if (LRS.Id == Id)
                    return LRS;

            }
            return null;
        }
        public virtual LeaveRequestStatus GetLeaveRequestStatusworkflowLevel(int workflowLevel)
        {

            foreach (LeaveRequestStatus LRS in LeaveRequestStatuses)
            {
                if (LRS.WorkflowLevel == workflowLevel)
                    return LRS;

            }
            return null;
        }
        public virtual IList<LeaveRequestStatus> GetLeaveRequestStatusByRequestId(int RequestId)
        {
            IList<LeaveRequestStatus> LRS = new List<LeaveRequestStatus>();
            foreach (LeaveRequestStatus AR in LeaveRequestStatuses)
            {
                if (AR.LeaveRequest.Id == RequestId)
                    LRS.Add(AR);

            }
            return LRS;
        }
        public virtual void RemoveLeaveRequestStatus(int Id)
        {

            foreach (LeaveRequestStatus LRS in LeaveRequestStatuses)
            {
                if (LRS.Id == Id)
                    LeaveRequestStatuses.Remove(LRS);
                break;
            }

        }
       

    }
}
