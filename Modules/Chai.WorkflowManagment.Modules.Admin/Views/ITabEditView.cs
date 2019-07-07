using System;
using System.Collections.Generic;
using Chai.WorkflowManagment.CoreDomain.Admins;

namespace Chai.WorkflowManagment.Modules.Admin.Views
{
    public interface ITabEditView
    {
        string GetTabId { get; }
        string GetNodeId { get; }
        string GetModuleId { get; }
        string GetTabName { get; }
        string GetDescription { get; }
        
        void BindTab();
        void BindPopupMenus();
        void BindTaskPans();
        void BindRoles();
        void SetRoles(Tab node);
    }
}




