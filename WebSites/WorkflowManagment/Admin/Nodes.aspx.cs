using System;
using System.Web.UI.WebControls;
using Microsoft.Practices.ObjectBuilder;

using Chai.WorkflowManagment.CoreDomain.Admins;
using Chai.WorkflowManagment.Shared;

namespace Chai.WorkflowManagment.Modules.Admin.Views
{
    public partial class Nodes : Microsoft.Practices.CompositeWeb.Web.UI.Page, INodesView
    {
        private NodesPresenter _presenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                PopModules();
                this._presenter.OnViewInitialized();
                BindNodes(0);
                string url = String.Format("~/Admin/NodeEdit.aspx?{0}=0&{1}=0", AppConstants.TABID, AppConstants.NODEID);
                hplNewnode.NavigateUrl = this.ResolveUrl(url);
            }

            this._presenter.OnViewLoaded();
        }

        [CreateNew]
        public NodesPresenter Presenter
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
        private void PopModules()
        {
            ddlModule.DataSource = _presenter.GetModules();
            ddlModule.DataBind();

            ddlModule.Items.Insert(0, new ListItem("---Select---", "0"));
            ddlModule.SelectedIndex = 0;
        }

        private void BindNodes(int modid)
        {
            if (modid == 0)
                grvNodes.DataSource = _presenter.GetNodes();
            else
                grvNodes.DataSource = _presenter.GetNodes(modid);
            grvNodes.DataBind();
        }

        protected void grvNodes_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            Node node = e.Row.DataItem as Node;
            if (node != null)
            {
                HyperLink hpl = e.Row.FindControl("hplEdit") as HyperLink;
                string url = String.Format("~/Admin/NodeEdit.aspx?{0}=0&{1}={2}", AppConstants.TABID, AppConstants.NODEID, node.Id);

                hpl.NavigateUrl = this.ResolveUrl(url);
            }
        }

        protected void butFiliter_Click(object sender, EventArgs e)
        {
            BindNodes(int.Parse(ddlModule.SelectedValue));
        }
}
}

