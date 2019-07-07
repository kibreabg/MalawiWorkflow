using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Admin.Views
{
    public interface ITaskpanEditView
    {
        string GetTabId { get; }
        string GetTaskpanId { get; }
        string GetTitle { get; }
        string GetImagePath { get; }

        void BindTaskpan();
        void BindTaskpanNodes();
        
    }
}




