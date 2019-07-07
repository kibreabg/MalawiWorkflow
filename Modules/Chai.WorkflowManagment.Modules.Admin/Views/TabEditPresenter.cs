using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;

using Chai.WorkflowManagment.CoreDomain.Admins;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared;

namespace Chai.WorkflowManagment.Modules.Admin.Views
{
    public class TabEditPresenter : Presenter<ITabEditView>
    {
        private AdminController _controller;
        private Tab _tab;

        public TabEditPresenter([CreateNew] AdminController controller)
        {
            _controller = controller;            
        }

        public override void OnViewLoaded()
        {
            _controller.CurrentObject = null;
        }

        public override void OnViewInitialized()
        {
            View.BindTab();
            View.BindTaskPans();
            View.BindPopupMenus();
            View.BindRoles();
        }

        public IEnumerable<Node> GetNodesByModuleId(int modid)
        {
            return _controller.GetListOfNodeByModuleId(modid);
        }

        public Node GetNode(int nodeid)
        {
            return _controller.GetNodeById(nodeid);
        }
        public Role GetRole(int roleid)
        {
            return _controller.GetRoleById(roleid);
        }
        public IList<Role> GetRoles()
        {
            return _controller.GetRoles;
        }
        public IList<Node> GetNodes()
        {
            return _controller.GetListOfAllNodes();
        }
        public IList<PocModule> GetModules()
        {
            return _controller.GetListOfAllPocModules();
        }

        public Tab CurrentTab
        {
            get
            {
                if (_tab == null)
                {
                    int id = int.Parse(View.GetNodeId);
                    if (id > 0)
                        _tab = _controller.GetTabById(id);
                    else
                        _tab = new Tab();
                }
                return _tab;
            }
        }
                        
        public int SaveOrUpdateTab()
        {
            Tab tab = CurrentTab;
                               
            View.SetRoles(tab);

            tab.TabName = View.GetTabName;
            tab.Description = View.GetDescription;
            tab.PocModule =   _controller.GetModuleById(int.Parse(View.GetModuleId));

            _controller.SaveOrUpdateEntity<Tab>(tab);

            return tab.Id;
        }

        public void RemoveTabRoles()
        {
            if (CurrentTab.TabRoles.Count > 0)
            {
                TabRole[] uroles = new TabRole[CurrentTab.TabRoles.Count];
                CurrentTab.TabRoles.CopyTo(uroles, 0);
                CurrentTab.TabRoles.Clear();
                _controller.RemoveListOfObjects<TabRole>(uroles);
            }
        }

        public void MoveUpTab()
        {
            _controller.MoveTabUp(CurrentTab);
        }

        public void MoveDownTab()
        {
            _controller.MoveTabDown(CurrentTab);
        }

        public void MoveUpTaskPan(int panid)
        {
            _controller.MoveUpTaskPan(panid);
        }

        public void MoveDownTaskPan(int panid)
        {
          _controller.MoveDownTaskPan(panid);
        }

        public void CancelIt()
        {  
            string url = String.Format("~/Admin/Default.aspx?{0}=0", AppConstants.TABID);
            _controller.Navigate(url);
        }

        public void DeleteIt()
        {
            _controller.DeleteEntity<Tab>(CurrentTab);
        }

        public void Navigate(string url)
        {
            _controller.Navigate(url);
        }
    }
}




