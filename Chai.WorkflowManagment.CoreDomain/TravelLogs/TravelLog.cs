using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Requests;

namespace Chai.WorkflowManagment.CoreDomain.TravelLogs
{
    public partial class TravelLog : IEntity
    {
        public int Id { get; set; }
        public string RequestNo { get; set; }
        public string DeparturePlace { get; set; }
        public string ArrivalPlace { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public decimal StartKmReading { get; set; }
        public decimal EndKmReading { get; set; }
        public int FuelPrice { get; set; }
        public virtual VehicleRequest VehicleRequest { get; set; }
    }
}
