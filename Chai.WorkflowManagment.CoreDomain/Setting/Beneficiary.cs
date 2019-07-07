using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Request;

namespace Chai.WorkflowManagment.CoreDomain.Setting
{
    public partial class Beneficiary : IEntity
    {
        public int Id { get; set; }
        public string BeneficiaryName { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string SortCode { get; set; }
        public string AccountNumber { get; set; }
        public string Status { get; set; }
        
       
    }
}
