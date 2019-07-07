using System;
using System.Collections.Generic;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.TravelLogs;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public interface ITravelLogView
    {
        IList<TravelLog> TravelLogs { get; set; }
        int GetRequestId { get; }
     
    }
}




