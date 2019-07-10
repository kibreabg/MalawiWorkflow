using System;
using System.Collections;
using System.Web.UI.WebControls;
using Microsoft.Practices.ObjectBuilder;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.Enums;

namespace Chai.WorkflowManagment.Modules.Admin.Views
{
    public partial class RoleEdit : Microsoft.Practices.CompositeWeb.Web.UI.Page, IRoleEditView
    {
        private RoleEditPresenter _presenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                BindPermission();
                BindRoleControls();
            }
            this._presenter.OnViewLoaded();
        }

        [CreateNew]
        public RoleEditPresenter Presenter
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
        private void BindRoleControls()
        {
            this.txtName.Text = _presenter.CurrentRole.Name;
            this.btnDelete.Visible = (_presenter.CurrentRole.Id > 0);
            this.btnDelete.Attributes.Add("onclick", "return confirm(\"Are you sure?\")");
        }
        
        private void BindPermission()
        {
            this.cblRoles.DataSource = Enum.GetValues(typeof(AccessLevel));
            this.cblRoles.DataBind();
            if (_presenter.CurrentRole.Permissions != null)
            {
                foreach (AccessLevel accessLevel in _presenter.CurrentRole.Permissions)
                {
                    ListItem li = cblRoles.Items.FindByText(accessLevel.ToString());
                    li.Selected = true;
                }
            }
        }

        private void SetPermissions()
        {
            int tmpLevel = 0;
            foreach (ListItem li in this.cblRoles.Items)
            {
                if (li.Selected)
                {
                    tmpLevel += (int)(AccessLevel)Enum.Parse(typeof(AccessLevel), li.Text, true);
                }
            }
            if (tmpLevel > 0)
            {
                _presenter.CurrentRole.PermissionLevel = (int)tmpLevel;
            }
        }

        #region IRoleEditView Members
        
        public string GetRoleId
        {
            get { return Request.QueryString[AppConstants.ROLEID]; }
        }

        public string GetRoleName
        {
            get { return this.txtName.Text; }
        }

        #endregion

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (this.IsValid)
            {
                SetPermissions();
                if (_presenter.CurrentRole.PermissionLevel == -1)
                {
                    Master.ShowMessage(new AppMessage("Please select one or more Permission(s)", RMessageType.Error));
                }
                else
                {
                    SaveRole();
                }
            }	            
        }

        private void SaveRole()
        {
            try
            {
                if (_presenter.CurrentRole.Id <=0)
                {
                    _presenter.SaveOrUpdateRole();
                    Master.TransferMessage(new AppMessage("Role saved", RMessageType.Info));
                    _presenter.CancelPage();
                }
                else
                {
                    _presenter.SaveOrUpdateRole();
                    Master.ShowMessage(new AppMessage("Role saved", RMessageType.Info));
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            _presenter.CancelPage();
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }
}
}

