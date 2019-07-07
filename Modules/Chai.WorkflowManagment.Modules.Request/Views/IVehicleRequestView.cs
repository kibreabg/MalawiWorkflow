using Chai.WorkflowManagment.CoreDomain.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public interface IVehicleRequestView
    {
        IList<VehicleRequest> VehicleRequests { get; set; }
        int GetVehicleRequestId { get; }
        string GetRequestNo { get; }        
        DateTime GetDepartureDate { get; }
        DateTime GetReturningDate { get; }
        string GetDepartureTime { get; }
      
        string GetPurposeOfTravel { get; }
        string GetDestination { get; }
        string GetComment { get; }
        int GetNoOfPassengers { get; }
        int GetProjectId { get; }
        int GetGrantId { get; }
     
    }
}




