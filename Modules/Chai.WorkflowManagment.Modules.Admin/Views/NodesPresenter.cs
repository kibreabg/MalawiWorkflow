using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;

using Chai.WorkflowManagment.CoreDomain.Admins;
using Chai.WorkflowManagment.Services;
using Chai.WorkflowManagment.Enums;


namespace Chai.WorkflowManagment.Modules.Admin.Views
{
    public class NodesPresenter : Presenter<INodesView>
    {
        private AdminController _controller;
        
        public NodesPresenter([CreateNew] AdminController controller)
        {
            _controller = controller;
        }


        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public IList<Node> GetNodes()
        {
            return _controller.GetListOfAllNodes();
        }
        public IList<Node> GetNodes(int moduleid)
        {
            return _controller.GetListOfNodeByModuleId(moduleid);
        }
        public IList<PocModule> GetModules()
        {
            return _controller.GetListOfAllPocModules();
        }
    }
}




