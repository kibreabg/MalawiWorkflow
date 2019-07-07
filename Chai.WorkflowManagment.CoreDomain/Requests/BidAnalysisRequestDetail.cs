using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.CoreDomain.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    [Table("BidAnalysisRequestDetails")]
    public partial class BidAnalysisRequestDetail : IEntity
    {
        public int Id { get; set; }
        public int Qty { get; set; }
        public decimal Priceperunit { get; set; }
        public decimal EstimatedCost { get; set; }
        public ItemAccount ItemAccount { get; set; }
        public string AccountCode { get; set; }
        public Project project { get; set; }
        public Grant Grant { get; set; }

        public BidAnalysisRequest BidAnalysisRequest { get; set; }

    }
}
