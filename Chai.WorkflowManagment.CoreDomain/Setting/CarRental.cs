using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Request;

namespace Chai.WorkflowManagment.CoreDomain.Setting
{
    public partial class CarRental : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNo { get; set; }
        public string ContactAddress { get; set; }
        public string Status { get; set; }

        //public IList<VehicleRequestStatus> VehicleRequestStatuses { get; set; }
    }
}
