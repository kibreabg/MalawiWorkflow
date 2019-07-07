using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Report
{
    [ComplexType]
    public class VehicleReport
    {
        public DateTime Date { get; set; }
        public string Name_of_Requester { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ReturningDate { get; set; }
        public string Driver { get; set; }
        public string Car_Rental { get; set; }
        public string Vehicle_Reg_Number { get; set; }

        public string NoOfPassengers { get; set; }
        
       
    }
}
