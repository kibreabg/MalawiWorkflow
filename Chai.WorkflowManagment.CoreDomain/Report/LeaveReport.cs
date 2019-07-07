using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Report
{
    //[ComplexType]
    public class LeaveReport
    {
        public string StaffName { get; set; }
        public int Leave_Days_Opening_Balance { get; set; }
        public string Leave_Type { get; set; }
        public string Period_Leave_Taken { get; set; }
        public int Number_of_Leave_Taken { get; set; }
        public int Leave_Balance_Carried_Forward { get; set; }
       
    }
}
