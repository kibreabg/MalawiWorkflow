using System;
using System.Web.UI.WebControls;
using Microsoft.Practices.ObjectBuilder;
using Chai.WorkflowManagment.Modules.Admin;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Shared;

namespace Chai.WorkflowManagment.Modules.Admin.Views
{
    public partial class Roles : Microsoft.Practices.CompositeWeb.Web.UI.Page, IRolesView
    {
        private RolesPresenter _presenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
            }
            this._presenter.OnViewLoaded();
        }

        [CreateNew]
        public RolesPresenter Presenter
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
     
        protected void btnNew_Click(object sender, EventArgs e)
        {
            _presenter.AddNewRole();
        }

        #region IRolesView Members

        public void BindRoles()
        {
            this.rptRoles.DataSource = _presenter.GetAllRoles();
            this.rptRoles.DataBind();
        }

        #endregion
         protected void rptRoles_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
         {
             Role role = e.Item.DataItem as Role;
             if (role != null)
             {
                 HyperLink hplEdit = (HyperLink)e.Item.FindControl("hplEdit");
                 Image imgRole = (Image)e.Item.FindControl("imgRole");
                 if (role.Name == "Administrator" || role.Name == "Anonymous user")
                 {
                     imgRole.ImageUrl = "~/Admin/Images/lock.png";
                     hplEdit.Visible = false;
                 }
                 else
                 {
                     imgRole.Visible = false;
                     hplEdit.NavigateUrl = String.Format("~/Admin/RoleEdit.aspx?{0}=0&{1}={2}", AppConstants.TABID, AppConstants.ROLEID, role.Id);
                 }
                 // Permissions
                 Label lblPermissions = (Label)e.Item.FindControl("lblPermissions");
                 lblPermissions.Text = role.PermissionsString;
             }
         }
}
}

