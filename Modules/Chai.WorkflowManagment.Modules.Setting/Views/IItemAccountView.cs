using Chai.WorkflowManagment.CoreDomain.Setting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public interface IItemAccountView
    {
        IList<ItemAccount> ItemAccount { get; set; }
        string ItemAccountName { get; }
        string ItemAccountCode { get; }
     
    }
}




