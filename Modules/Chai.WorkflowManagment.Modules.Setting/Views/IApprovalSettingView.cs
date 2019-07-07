using Chai.WorkflowManagment.CoreDomain.Setting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public interface IApprovalSettingView
    {
        ApprovalSetting ApprovalSetting { get; set; }
        string RequestType { get; }
        int ApprovalSettingId { get; }

      
    }
}




