using Chai.WorkflowManagment.CoreDomain.Setting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public interface IGrantView
    {
        IList<Grant> grant { get; set; }
        string GrantName { get; }
        string GrantCode { get; }
     
    }
}




