using System;
using System.Web.UI.WebControls;
using Microsoft.Practices.ObjectBuilder;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.CoreDomain.Admins;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Shared;

namespace Chai.WorkflowManagment.Modules.Admin.Views
{
    public partial class TabEdit : Microsoft.Practices.CompositeWeb.Web.UI.Page, ITabEditView
    {
        private TabEditPresenter _presenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                PopModuleToDDL();
                this._presenter.OnViewInitialized();
                PopNodesToDdl();
                btnDelete.Attributes.Add("onclick", "return confirm(\"Are you sure?\")");
                
            }
            this._presenter.OnViewLoaded();
        }

        [CreateNew]
        public TabEditPresenter Presenter
        {
            get
            {
                return this._presenter;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this._presenter = value;
                this._presenter.View = this;
            }
        }

        public void BindTab()
        {
            Tab tab = _presenter.CurrentTab;

            if (tab.Id > 0)
            {
                txtTitle.Text = tab.TabName;
                txtDescription.Text = tab.Description;
                ddlModule.SelectedValue = tab.PocModule.Id.ToString();
                ddlModule.Enabled = false;
                btnDelete.Enabled = true;
                butAddaction.Enabled = true;
                hplAddtaskpan.NavigateUrl = Page.ResolveUrl(String.Format("~/Admin/TaskPanEdit.aspx?{0}=0&{1}={2}&{3}=0", AppConstants.TABID, AppConstants.NODEID, tab.Id, AppConstants.TASKPANID));
            }
            else
            {
                ddlModule.Enabled = true;
                btnDelete.Enabled = false;
                butAddaction.Enabled = false;
                ibtDown.Enabled = false;
                ibtUp.Enabled = false;
            }
        }

        private void PopModuleToDDL()
        {
            ddlModule.DataSource = _presenter.GetModules();
            ddlModule.DataBind();

            ddlModule.Items.Insert(0, new ListItem("---Select Module---", "0"));
            ddlModule.SelectedIndex = 0;
        }

        private void PopNodesToDdl()
        {
            ddlNodes.Items.Clear();
            foreach (Node node in _presenter.GetNodes())
            {
                if (!_presenter.CurrentTab.NodeWasAddedToPopupMenu(node.Id))
                    ddlNodes.Items.Add(new ListItem(node.Title, node.Id.ToString()));
            }

            ddlNodes.Items.Insert(0, new ListItem("---Select Node---", "0"));
            ddlNodes.SelectedIndex = 0;
        }

        public void BindPopupMenus()
        {
            lsbNodes.Items.Clear();
            foreach (PopupMenu  tn in _presenter.CurrentTab.PopupMenus)
            {
                lsbNodes.Items.Add(new ListItem(tn.Node.Title, tn.Id.ToString()));
            }

            if (lsbNodes.Items.Count > 0)
                butRemoveaction.Enabled = true;
            else
                butRemoveaction.Enabled = false;
        }

        protected void butAddaction_Click(object sender, EventArgs e)
        {
            try
            {
                PopupMenu  pm = new PopupMenu();
                pm.Tab = _presenter.CurrentTab;
                pm.Node = _presenter.GetNode(int.Parse(ddlNodes.SelectedValue));
                pm.Position = _presenter.CurrentTab.PopupMenus.Count + 1;
                _presenter.CurrentTab.PopupMenus.Add(pm);
                                                
                _presenter.SaveOrUpdateTab();
                PopNodesToDdl();
                BindPopupMenus();
                Master.ShowMessage(new AppMessage("Popup menu was added successfully.", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to add Popup menu." + ex.Message, RMessageType.Error));
            }

        }
       
        protected void butRemoveaction_Click(object sender, EventArgs e)
        {
            int id = int.Parse(lsbNodes.SelectedValue);
            
            try
            {
                PopupMenu pm = _presenter.CurrentTab.GetPopupMenu(id);

                if (pm != null)
                {
                    _presenter.CurrentTab.PopupMenus.Remove(pm);
                    _presenter.SaveOrUpdateTab();
                    PopNodesToDdl();
                    BindPopupMenus();
                    Master.ShowMessage(new AppMessage("Popup menu was removed successfully.", RMessageType.Info));
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to remove popup menu. " + ex.Message, RMessageType.Error));
            }
        }

        public void BindTaskPans()
        {
            grvTaskpans.DataSource = _presenter.CurrentTab.TaskPans;
            grvTaskpans.DataBind();
        }

        protected void grvNodes_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            TaskPan pan = e.Row.DataItem as TaskPan;
            if (pan != null)
            {
                HyperLink hpl = e.Row.FindControl("hplEdit") as HyperLink;
                string url = String.Format("~/Admin/TaskPanEdit.aspx?{0}=0&{1}={2}&{3}={4}", AppConstants.TABID, AppConstants.NODEID, pan.Tab.Id, AppConstants.TASKPANID, pan.Id);
                hpl.NavigateUrl = this.ResolveUrl(url);

                ImageButton ibt = e.Row.FindControl("ibtUp") as ImageButton;
                ibt.CommandArgument = pan.Id.ToString();

                ibt = e.Row.FindControl("ibtDown") as ImageButton;
                ibt.CommandArgument = pan.Id.ToString();
            }
        }

        public void BindRoles()
        {
            this.rptRoles.DataSource = _presenter.GetRoles();
            this.rptRoles.DataBind();
        }

        public void SetRoles(Tab tab)
        {
            _presenter.RemoveTabRoles();

            foreach (RepeaterItem ri in rptRoles.Items)
            {
                CheckBox chkView = (CheckBox)ri.FindControl("chkViewAllowed");
                if (chkView.Checked)
                {
                    if (!tab.Exists(Convert.ToInt32(ViewState[ri.ClientID])))
                    {
                        TabRole np = new TabRole();
                        np.Tab = tab;
                        np.Role = _presenter.GetRole((int)ViewState[ri.ClientID]);
                        np.ViewAllowed = chkView.Checked;

                        tab.TabRoles.Add(np);
                    }
                }
            }
        }

        protected void rptRoles_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Role role = e.Item.DataItem as Role;
            if (role != null)
            {
                CheckBox chkView = (CheckBox)e.Item.FindControl("chkViewAllowed");
                if (_presenter.CurrentTab != null)
                    chkView.Checked = this._presenter.CurrentTab.ViewAllowed(role);
                
                this.ViewState[e.Item.ClientID] = role.Id;
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //try
            //{
                int id = _presenter.SaveOrUpdateTab();

                if (int.Parse(GetNodeId) <= 0)
                {
                    Master.TransferMessage(new AppMessage("Tab was saved successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
                    string url = String.Format("~/Admin/TabEdit.aspx?{0}=0&{1}={2}", AppConstants.TABID, AppConstants.NODEID, id);
                    _presenter.Navigate(url);
                }
                else
                    Master.ShowMessage(new AppMessage("Tab was saved successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
            //}
            //catch (Exception ex)
            //{
            //    Master.ShowMessage(new AppMessage("Error: Unable to save Tab. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            //}
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            _presenter.CancelIt();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                _presenter.DeleteIt();
                Master.TransferMessage(new AppMessage("Tab was deleted successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
                _presenter.CancelIt();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Tab. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }

        #region ITabEditView Members

        public string GetTabId
        {
            get { return Request.QueryString[AppConstants.TABID]; }
        }

        public string GetNodeId
        {
            get { return Request.QueryString[AppConstants.NODEID]; }
        }
            
        public string GetModuleId
        {
            get { return ddlModule.SelectedValue; }
        }

        public string GetTabName
        {
            get { return txtTitle.Text; }
        }

        public string GetDescription
        {
            get { return txtDescription.Text; }
        }

        #endregion

        private string SelfUrl()
        {
            return String.Format("~/Admin/TabEdit.aspx?{0}=0&{1}={2}", AppConstants.TABID, AppConstants.NODEID, _presenter.CurrentTab.Id);
        }
        protected void ibtUp_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (_presenter.CurrentTab.Id > 0)
            {
                string url = SelfUrl();
                _presenter.MoveUpTab();
                _presenter.Navigate(url);
            }
        }
        
        protected void ibtDown_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (_presenter.CurrentTab.Id >0)
            {
                string url = SelfUrl();
                _presenter.MoveDownTab();                
                _presenter.Navigate(url);
            }
        }

        protected void grvTaskpans_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string url = SelfUrl();

            if(e.CommandName =="MoveUp")
            {                                
                _presenter.MoveUpTaskPan(Convert.ToInt32(e.CommandArgument));
                _presenter.Navigate(url);
            }
            else if (e.CommandName == "MoveDown")
            {
                _presenter.MoveDownTaskPan(Convert.ToInt32(e.CommandArgument));
                _presenter.Navigate(url);
            }
        }
}
}

