using Chai.WorkflowManagment.CoreDomain.Setting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public interface ICarRentalView
    {
        IList<CarRental> CarRentals { get; set; }
        string GetName { get; }
     
    }
}




