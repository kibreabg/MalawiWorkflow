using System;
using System.Web.UI.WebControls;
using Microsoft.Practices.ObjectBuilder;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.Modules.Admin.Views
{
    public partial class Users : Microsoft.Practices.CompositeWeb.Web.UI.Page, IUsersView
    {
        private UsersPresenter _presenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
            }
            this._presenter.OnViewLoaded();
        }

        [CreateNew]
        public UsersPresenter Presenter
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
        
        public void BindUsers()
        {
            this.grvUser.DataSource = _presenter.SearchUser(txtUsername.Text.Trim());
            this.grvUser.DataBind();
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindUsers();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            _presenter.AddNewUser();
        }
        
        protected void grvUser_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            AppUser user = e.Row.DataItem as AppUser;
            if (user != null)
            {
                HyperLink hpl = e.Row.FindControl("hplEdit") as HyperLink;
                string url = string.Format("~/Admin/UserEdit.aspx?{0}=0&{1}={2}", AppConstants.TABID, AppConstants.USERID, user.Id);
                hpl.NavigateUrl = this.ResolveUrl(url);
            }
        }

        protected void grvUser_PageIndexChanging(object sender,  GridViewPageEventArgs e)
        {
            grvUser.PageIndex = e.NewPageIndex;
            BindUsers();
        }
  
}
}

