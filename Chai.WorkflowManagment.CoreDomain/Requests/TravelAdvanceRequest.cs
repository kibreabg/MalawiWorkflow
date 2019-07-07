using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class TravelAdvanceRequest : IEntity
    {
        public TravelAdvanceRequest()
        {
            this.TravelAdvanceRequestDetails = new List<TravelAdvanceRequestDetail>();
            this.TravelAdvanceRequestStatuses = new List<TravelAdvanceRequestStatus>();
        }
        public int Id { get; set; }
        public string TravelAdvanceNo { get; set; }
        public Nullable<DateTime> RequestDate { get; set; }
        public string VisitingTeam { get; set; }
        public string PurposeOfTravel { get; set; }
        public string Comments { get; set; }
        public decimal TotalTravelAdvance { get; set; }
        public string PaymentMethod { get; set; }
        public int CurrentApprover { get; set; }
        public int CurrentApproverPosition { get; set; }
        public int CurrentLevel { get; set; }
        public string CurrentStatus { get; set; }        
        public string ProgressStatus { get; set; }
        public string ExpenseLiquidationStatus { get; set; }
        public string ExportStatus { get; set; }
        public virtual Project Project { get; set; }
        public virtual Account Account { get; set; }
        public virtual Grant Grant { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual ExpenseLiquidationRequest ExpenseLiquidationRequest { get; set; }
        public virtual IList<TravelAdvanceRequestDetail> TravelAdvanceRequestDetails { get; set; }
        public virtual IList<TravelAdvanceRequestStatus> TravelAdvanceRequestStatuses { get; set; }

        #region TravelAdvanceRequestDetail
        public virtual TravelAdvanceRequestDetail GetTravelAdvanceRequestDetail(int Id)
        {
            foreach (TravelAdvanceRequestDetail TARD in TravelAdvanceRequestDetails)
            {
                if (TARD.Id == Id)
                    return TARD;
            }
            return null;
        }

        public virtual IList<TravelAdvanceRequestDetail> GetTravelAdvanceRequestDetailsByTARId(int tarId)
        {
            IList<TravelAdvanceRequestDetail> TARDs = new List<TravelAdvanceRequestDetail>();
            foreach (TravelAdvanceRequestDetail TARD in TravelAdvanceRequestDetails)
            {
                if (TARD.TravelAdvanceRequest.Id == tarId)
                    TARDs.Add(TARD);
            }
            return TARDs;
        }
        public virtual void RemoveTravelAdvanceRequestDetail(int Id)
        {
            foreach (TravelAdvanceRequestDetail TARD in TravelAdvanceRequestDetails)
            {
                if (TARD.Id == Id)
                    TravelAdvanceRequestDetails.Remove(TARD);
                break;
            }
        }
        #endregion

    }
}
