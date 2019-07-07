using Chai.WorkflowManagment.CoreDomain.Setting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public interface IVehicleView
    {
        IList<Vehicle> Vehicles { get; set; }
        string GetPlateNo { get; }
     
    }
}




