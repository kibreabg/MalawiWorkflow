using System;
using System.Collections.Generic;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Admins;


namespace Chai.WorkflowManagment.Modules.Admin.Views
{
    public interface INodeEditView
    {
        string GetTabId { get; }
        string GetNodeId { get; }
        string GetModuleId { get; }
        string GetTitle { get; }
        string GetDescription { get; }
        string GetFolderPath { get; }
        string GetImagePath { get; }
        string GetPageID { get; }
        void BindNode();
        void BindRoles();
        void SetRoles(Node node);
    }
}




