using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Admin.Views
{
    public interface IModuleEditView
    {
        string GetModuleId { get; }
        string GetName { get; }
        string GetFolderPath { get; }
    }
}




