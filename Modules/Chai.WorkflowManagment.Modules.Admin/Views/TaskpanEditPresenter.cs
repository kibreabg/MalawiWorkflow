using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;

using Chai.WorkflowManagment.CoreDomain.Admins;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared;

namespace Chai.WorkflowManagment.Modules.Admin.Views
{
    public class TaskpanEditPresenter : Presenter<ITaskpanEditView>
    {
        private AdminController _controller;
        private Tab _tab;
        
        public TaskpanEditPresenter([CreateNew] AdminController controller)
        {
            _controller = controller;
        }

        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {
            View.BindTaskpan();
            View.BindTaskpanNodes();
        }

        public IList<Node> GetNodesByModuleId(int modid)
        {
            return _controller.GetListOfNodeByModuleId(modid);
        }

        public Node GetNode(int nodeid)
        {
            return _controller.GetNodeById(nodeid);
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
                    int id = int.Parse(View.GetTabId);
                    if (id > 0)
                        _tab = _controller.GetTabById(id);
                    else
                        _tab = new Tab();
                }
                return _tab;
            }
        }

        public TaskPan CurrentTaskpan
        {
            get
            {
                int id =int.Parse(View.GetTaskpanId);
                if (id > 0)
                    return CurrentTab.TaskPans.Single<TaskPan>(x => x.Id == id);  //.GetTaskpanById(id);
                else
                {
                    return new TaskPan();
                }
            }
        }

        public void SaveOrUpdateTab()
        {
            TaskPan pan = CurrentTaskpan;

            pan.Title = View.GetTitle;
            pan.ImagePath = View.GetImagePath;

            if (pan.Id <= 0)
            {
                int? pos = CurrentTab.TaskPans.Select<TaskPan, int?>(x => x.Position).Max(); 

                pan.Position = pos.HasValue ? pos.Value + 1 : 1;
                CurrentTab.TaskPans.Add(pan);
            }
            
            _controller.SaveOrUpdateEntity<Tab>(CurrentTab);
        }
        
        public void CancelIt()
        {
            string url = String.Format("~/Admin/TabEdit.aspx?{0}=0&{1}={2}", AppConstants.TABID, AppConstants.NODEID, CurrentTab.Id);
            _controller.Navigate(url);
        }

        public void DeleteIt()
        {
            CurrentTab.TaskPans.Remove(CurrentTaskpan);
            _controller.SaveOrUpdateEntity<Tab>(CurrentTab);
        }

        public void Navigate(string url)
        {
            _controller.Navigate(url);
        }

        public void MoveUpPanNode(int panid)
        {
             _controller.MoveUpPanNode(panid);
        }

        public void MoveDownPanNode(int panid)
        {
            _controller.MoveDownPanNode(panid);
        }
    }
}




