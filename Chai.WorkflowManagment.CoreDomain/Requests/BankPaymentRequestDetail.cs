using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Setting;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class BankPaymentRequestDetail : IEntity
    {
        public int Id { get; set; }
        public DateTime ProcessDate { get; set; }
        public int BankCode { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }    
        public string Currency { get; set; }
        public string Reference { get; set; }
        public virtual Account Account { get; set; }
        public virtual BankPaymentRequest BankPaymentRequest { get; set; }
    }
}
