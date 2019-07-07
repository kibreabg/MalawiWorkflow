using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class VehicleRequestDetail : IEntity
    {
        public int Id { get; set; }
        public string PlateNo { get; set; }
        public string AssignedVehicle { get; set; }

        public string FuelCardNumber { get; set; }
        public virtual CarRental CarRental { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual VehicleRequest VehicleRequest { get; set; }
    }
}
