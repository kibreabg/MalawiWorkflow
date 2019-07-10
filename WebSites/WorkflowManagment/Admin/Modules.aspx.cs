using System;
using System.Web.UI.WebControls;
using Microsoft.Practices.ObjectBuilder;

using Chai.WorkflowManagment.CoreDomain.Admins;
using Chai.WorkflowManagment.Shared;

namespace Chai.WorkflowManagment.Modules.Admin.Views
{
	public partial class Modules : Microsoft.Practices.CompositeWeb.Web.UI.Page, IModulesView
	{
		private ModulesPresenter _presenter;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				this._presenter.OnViewInitialized();
                hplNewnode.NavigateUrl = this.ResolveUrl(String.Format("~/Admin/ModuleEdit.aspx?{0}=0&{1}=0", AppConstants.TABID, AppConstants.MODULEID));
                BindModules();
			}
			this._presenter.OnViewLoaded();
		}

		[CreateNew]
		public ModulesPresenter Presenter
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
        
        private void BindModules()
        {
            grvNodes.DataSource = _presenter.GetListOfModules();
            grvNodes.DataBind();
        }

        protected void grvNodes_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            PocModule mod = e.Row.DataItem as PocModule;
            if (mod != null)
            {
                HyperLink hpl = e.Row.FindControl("hplEdit") as HyperLink;
                string url = String.Format("~/Admin/ModuleEdit.aspx?{0}=0&{1}={2}", AppConstants.TABID, AppConstants.MODULEID, mod.Id);

                hpl.NavigateUrl = this.ResolveUrl(url);
            }
        }

	}
}

