using System;
using System.Web.UI.WebControls;
using Microsoft.Practices.ObjectBuilder;
using System.Linq;
using System.Linq.Expressions;

using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.CoreDomain.Admins;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Shared;

namespace Chai.WorkflowManagment.Modules.Admin.Views
{
	public partial class TaskpanEdit : Microsoft.Practices.CompositeWeb.Web.UI.Page, ITaskpanEditView
	{
		private TaskpanEditPresenter _presenter;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				this._presenter.OnViewInitialized();
                PopNodesToDdl();
                btnDelete.Attributes.Add("onclick", "return confirm(\"Are you sure?\")");
			}
			this._presenter.OnViewLoaded();
		}

		[CreateNew]
		public TaskpanEditPresenter Presenter
		{
			get
			{
				return this._presenter;
			}
			set
			{
				if(value == null)
					throw new ArgumentNullException("value");

				this._presenter = value;
				this._presenter.View = this;
			}
		}

        public void BindTaskpan()
        {
            lblTabname.Text = _presenter.CurrentTab.TabName;

            TaskPan pan = _presenter.CurrentTaskpan;
            if (pan.Id >0)
            {
                txtTitle.Text = pan.Title;
                txtImage.Text = pan.ImagePath;
                btnDelete.Enabled = true;
                butAddaction.Enabled = true;
            }
            else
            {
                btnDelete.Enabled = false;
                butAddaction.Enabled = false;
            }
        }

        
        private void PopNodesToDdl()
        {
            ddlNodes.Items.Clear();
            foreach (Node node in _presenter.GetNodes())
            {
                if (!_presenter.CurrentTaskpan.NodeWasAddedToPan(node.Id))
                    ddlNodes.Items.Add(new ListItem(node.Title, node.Id.ToString()));
            }

            ddlNodes.Items.Insert(0, new ListItem("---Select Node---", "0"));
            ddlNodes.SelectedIndex = 0;
        }

        public void BindTaskpanNodes()
        {
            grvNodes.DataSource = _presenter.CurrentTaskpan.TaskPanNodes;
            grvNodes.DataBind();            
        }

        protected void butAddaction_Click(object sender, EventArgs e)
        {
            if (ddlNodes.SelectedValue == "0")
                return;

            try
            {
                TaskPanNode pm = new TaskPanNode();
                pm.TaskPan = _presenter.CurrentTaskpan;
                pm.Node = _presenter.GetNode(int.Parse(ddlNodes.SelectedValue));
                pm.Position = _presenter.CurrentTaskpan.TaskPanNodes.Count + 1;
                _presenter.CurrentTaskpan.TaskPanNodes.Add(pm);
                
                _presenter.SaveOrUpdateTab();
                PopNodesToDdl();
                BindTaskpanNodes();
                Master.ShowMessage(new AppMessage("Popup menu was added successfully.", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to add Popup menu." + ex.Message, RMessageType.Error));
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                _presenter.SaveOrUpdateTab();
                if (int.Parse(GetTaskpanId) <= 0)
                {
                    Master.TransferMessage(new AppMessage("Task pan was saved successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
                    int panid = _presenter.CurrentTab.TaskPans.Last<TaskPan>().Id; //[_presenter.CurrentTab.TaskPans.Count - 1].Id;
                    string url = String.Format("~/Admin/TaskpanEdit.aspx?{0}=0&{1}={2}&{3}={4}", AppConstants.TABID, AppConstants.NODEID, GetTabId, AppConstants.TASKPANID, panid);
                    _presenter.Navigate(url);
                }
                else
                {
                    Master.ShowMessage(new AppMessage("Task pan was saved successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to save Tab. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            _presenter.CancelIt();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string url = String.Format("~/Admin/TabEdit.aspx?{0}=0&{1}={2}", AppConstants.TABID, AppConstants.NODEID, GetTabId);
                _presenter.DeleteIt();
                Master.TransferMessage(new AppMessage("Task pan was deleted successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
                _presenter.Navigate(url);
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Task pan." + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }

        #region ITabEditView Members

        
        public string GetTabId
        {
            get { return Request.QueryString[AppConstants.NODEID]; }
        }

        public string GetTaskpanId
        {
            get { return Request.QueryString[AppConstants.TASKPANID]; }
        }

        public string GetTitle
        {
            get { return txtTitle.Text; }
        }

        public string GetImagePath
        {
            get { return txtImage.Text; }
        }

        #endregion

        private string SelfUrl()
        {
            return String.Format("~/Admin/TaskpanEdit.aspx?{0}=0&{1}={2}&{3}={4}", AppConstants.TABID, AppConstants.NODEID, GetTabId, AppConstants.TASKPANID, _presenter.CurrentTaskpan.Id);
        }

        protected void grvNodes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            TaskPanNode pan = e.Row.DataItem as TaskPanNode;
            if (pan != null)
            {                
                ImageButton ibt = e.Row.FindControl("ibtUp") as ImageButton;
                ibt.CommandArgument = pan.Id.ToString();

                ibt = e.Row.FindControl("ibtDown") as ImageButton;
                ibt.CommandArgument = pan.Id.ToString();

                LinkButton lbt = e.Row.FindControl("lbtDelete") as LinkButton;
                lbt.CommandArgument = pan.Id.ToString();
            }

        }

        protected void grvNodes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string url = SelfUrl();
            if (e.CommandName == "MoveUp")
            {
                _presenter.MoveUpPanNode(Convert.ToInt32(e.CommandArgument));
                _presenter.Navigate(url);
            }
            else if (e.CommandName == "MoveDown")
            {
                _presenter.MoveDownPanNode(Convert.ToInt32(e.CommandArgument));
                _presenter.Navigate(url);
            }
            else if(e.CommandName == "Delete")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                
                try
                {
                    TaskPanNode pm = _presenter.CurrentTaskpan.GetTaskPanNodeById(id);

                    if (pm != null)
                    {
                        _presenter.CurrentTaskpan.TaskPanNodes.Remove(pm);

                        _presenter.SaveOrUpdateTab();
                        PopNodesToDdl();
                        BindTaskpanNodes();
                        Master.ShowMessage(new AppMessage("Task pan node was removed successfully.", RMessageType.Info));
                    }
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to remove task pan node. " + ex.Message, RMessageType.Error));
                }
            }
        }
}
}

