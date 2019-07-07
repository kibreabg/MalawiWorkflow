using System;
using System.Web;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;

using Chai.WorkflowManagment.CoreDomain.Admins;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Services;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.Modules.Admin.Util;

namespace Chai.WorkflowManagment.Modules.Admin.Views
{
    public class NodeEditPresenter : Presenter<INodeEditView>
    {
        private AdminController _controller;
        private Node _node;

        public NodeEditPresenter([CreateNew] AdminController controller)
         {
         		_controller = controller;
        }

        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {
            View.BindNode();
            View.BindRoles();
        }

        public IList<Role> GetRoles
        {
            get { return _controller.GetRoles; }
        }

        public IList<PocModule> GetModules
        {
            get { return _controller.GetListOfAllPocModules(); }
        }

        public string ServerPath(string path)
        {
            return _controller.GetCurrentContext().Context.Server.MapPath(path);
        }

        public PocModule GetModuleById(int moduleid)
        {
            return _controller.GetModuleById(moduleid);
        }

        public Node CurrentNode
        {
            get
            {
                if (_node == null)
                {
                    int id = int.Parse(View.GetNodeId);
                    if (id > 0)
                        _node = _controller.GetNodeById(id);
                    else
                        _node = new Node();
                }
                return _node;
            }
        }

        public void SaveOrUpdateNode()
        {
            Node node = CurrentNode;
                               
            View.SetRoles(node);

            node.Title = View.GetTitle;
            node.Description = View.GetDescription;
            node.FilePath = View.GetFolderPath;
            node.ImagePath = View.GetImagePath;
            node.PageId = View.GetPageID;

            if (node.PocModule == null)
                node.PocModule = GetModuleById(int.Parse(View.GetModuleId));
            
            _controller.SaveOrUpdateEntity<Node>(node);
                        
            if (int.Parse(View.GetNodeId) <= 0)
            {
                string url = String.Format("~/Admin/NodeEdit.aspx?{0}=0&{1}={2}", AppConstants.TABID, AppConstants.NODEID, node.Id);
                _controller.Navigate(url);
            }
        }
        
        public void CancelIt()
        {  
            string url = String.Format("~/Admin/Nodes.aspx?{0}=0", AppConstants.TABID);
            _controller.Navigate(url);
        }

        public void DeleteIt()
        {
            Node node = CurrentNode;
            if (node != null && node.Id >0)
            {
                _controller.DeleteEntity<Node>(node);
            }
        }

        public Role GetRole(int roleid)
        {
            return _controller.GetRoleById(roleid);
        }

        public ListOfDirectoryItems DirectoryItems
        {
            get { return _controller.GetCurrentContext().Context.Session["LST_DIRECTORY_ITEM"] as ListOfDirectoryItems; }
            set { _controller.GetCurrentContext().Context.Session["LST_DIRECTORY_ITEM"] = value; }
        }
    }
}




