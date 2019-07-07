using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class ExpenseLiquidationRequest : IEntity
    {
        public ExpenseLiquidationRequest()
        {
            this.ExpenseLiquidationRequestStatuses = new List<ExpenseLiquidationRequestStatus>();
            this.ExpenseLiquidationRequestDetails = new List<ExpenseLiquidationRequestDetail>();
            this.ELRAttachments = new List<ELRAttachment>();
        }
        public int Id { get; set; }
        public Nullable<DateTime> RequestDate { get; set; }
        public Nullable<DateTime> TravelAdvRequestDate { get; set; }
        public string ExpenseType { get; set; }        
        public string Comment { get; set; }
        public string ExpenseReimbersmentType { get; set; }
        public string ReimbersmentNo { get; set; }
        public string ExportStatus { get; set; }
        public string PaymentMethod { get; set; }
        public int CurrentApprover { get; set; }
        public int CurrentLevel { get; set; }
        public string CurrentStatus { get; set; }
        public string ProgressStatus { get; set; }
        public decimal TotalActualExpenditure { get; set; }
        public decimal TotalTravelAdvance { get; set; }
        public string AdditionalComment { get; set; }

        [Required]
        public virtual TravelAdvanceRequest TravelAdvanceRequest { get; set; }
        public virtual IList<ExpenseLiquidationRequestDetail> ExpenseLiquidationRequestDetails { get; set; }
        public virtual IList<ExpenseLiquidationRequestStatus> ExpenseLiquidationRequestStatuses { get; set; }
        public virtual IList<ELRAttachment> ELRAttachments { get; set; }

        #region LiquidationDetails
        public virtual ExpenseLiquidationRequestDetail GetExpenseLiquidationRequestDetail(int Id)
        {

            foreach (ExpenseLiquidationRequestDetail ExpenseLiquidationRequestDetail in ExpenseLiquidationRequestDetails)
            {
                if (ExpenseLiquidationRequestDetail.Id == Id)
                    return ExpenseLiquidationRequestDetail;
            }
            return null;
        }

        public virtual IList<ExpenseLiquidationRequestDetail> GetExpenseLiquidationRequestDetailByLiquidationId(int liquidationId)
        {
            IList<ExpenseLiquidationRequestDetail> LiquidationDetails = new List<ExpenseLiquidationRequestDetail>();
            foreach (ExpenseLiquidationRequestDetail ExpenseLiquidationRequestDetail in ExpenseLiquidationRequestDetails)
            {
                if (ExpenseLiquidationRequestDetail.ExpenseLiquidationRequest.Id == liquidationId)
                    LiquidationDetails.Add(ExpenseLiquidationRequestDetail);

            }
            return LiquidationDetails;
        }
        public virtual void RemoveExpenseLiquidationRequestDetail(int Id)
        {

            foreach (ExpenseLiquidationRequestDetail ExpenseLiquidationRequestDetail in ExpenseLiquidationRequestDetails)
            {
                if (ExpenseLiquidationRequestDetail.Id == Id)
                    ExpenseLiquidationRequestDetails.Remove(ExpenseLiquidationRequestDetail);
                break;
            }

        }

        #endregion
        #region ELAttachment

        public virtual void RemoveELAttachment(string FilePath)
        {
            foreach (ELRAttachment cpa in ELRAttachments)
            {
                if (cpa.FilePath == FilePath)
                {
                    ELRAttachments.Remove(cpa);
                    break;
                }
            }
        }
        #endregion
    }
}
