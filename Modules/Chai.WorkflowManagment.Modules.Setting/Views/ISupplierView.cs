using Chai.WorkflowManagment.CoreDomain.Setting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public interface ISupplierView
    {
        IList<Supplier> Supplier { get; set; }
        string SupplierName { get; }
      
     
    }
}




