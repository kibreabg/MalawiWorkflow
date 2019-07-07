using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Report
{
    [ComplexType]
    public class CashPaymentReport
    {
        public DateTime Date { get; set; }
        public string Reference_No { get; set; }
        public string Description { get; set; }
        public string AccountCode { get; set; }
        public string Project_ID { get; set; }
        public string Grant_ID { get; set; }
        
        
       
    }
}
