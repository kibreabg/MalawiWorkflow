using Chai.WorkflowManagment.CoreDomain.Setting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public interface IBeneficiaryView
    {
        IList<Beneficiary> Beneficiaries { get; set; }
        string GetName { get; }
     
    }
}




