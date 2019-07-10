using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Microsoft.Practices.ObjectBuilder;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.Shared.Settings;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Enums;

namespace Chai.WorkflowManagment.Modules.Admin.Views
{
    public partial class UserEdit : Microsoft.Practices.CompositeWeb.Web.UI.Page, IUserEditView
    {
        private UserEditPresenter _presenter;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                PopEmployeePostion();
                //PopProgram();
                PopUser();

                BindUserControls();
                BindRoles();
            }
            this._presenter.OnViewLoaded();
        }

        [CreateNew]
        public UserEditPresenter Presenter
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
        private void PopUser()
        {
            ddlSuperviser.Items.Add(new ListItem("None", "0"));
            ddlSuperviser.DataSource = _presenter.GetUsers();
            ddlSuperviser.DataBind();
        }
       
        private void PopEmployeePostion()
        {

            ddlEmployeePostion.DataSource = _presenter.GetEmployeePositions();
            ddlEmployeePostion.DataBind();
        }
        private void BindUserControls()
        {
            if (_presenter.CurrentUser.Id >0)
            {
                this.txtUsername.Visible = false;
                this.lblUsername.Text = _presenter.CurrentUser.UserName;
                this.lblUsername.Visible = true;
                this.rfvUsername.Enabled = false;
            }
            else
            {
                this.txtUsername.Text = _presenter.CurrentUser.UserName;
                this.txtUsername.Visible = true;
                this.lblUsername.Visible = false;
                this.rfvUsername.Enabled = true;
            }
            this.txtFirstname.Text = _presenter.CurrentUser.FirstName;
            this.txtLastname.Text = _presenter.CurrentUser.LastName;
            this.txtEmployeeNo.Text = _presenter.CurrentUser.EmployeeNo;
            this.txtEmail.Text = _presenter.CurrentUser.Email;
            this.ddlEmployeePostion.SelectedValue = _presenter.CurrentUser.EmployeePosition != null ? _presenter.CurrentUser.EmployeePosition.Id.ToString():"0";
           
            this.ddlSuperviser.SelectedValue = _presenter.CurrentUser.Superviser.ToString();
            this.chkActive.Checked = _presenter.CurrentUser.IsActive;
            this.btnDelete.Visible = (_presenter.CurrentUser.Id > 0);
            this.btnDelete.Attributes.Add("onclick", "return confirm(\"Are you sure you want to delete this user?\")");
        }

        private void BindRoles()
        {
            IList<Role> roles = _presenter.GetRoles();
            FilterAnonymousRoles(roles);
            this.rptRoles.ItemDataBound += new RepeaterItemEventHandler(rptRoles_ItemDataBound);
            this.rptRoles.DataSource = roles;
            this.rptRoles.DataBind();
        }

        private void FilterAnonymousRoles(IList<Role> roles)
        {
            int roleCount = roles.Count;
            for (int i = roleCount - 1; i >= 0; i--)
            {
                Role role = roles[i];
                if (role.PermissionLevel == (int)AccessLevel.Anonymous)
                {
                    roles.Remove(role);
                }
            }
        }

        private void SetRoles()
        {
            _presenter.RemoveUserRoles();
            
            foreach (RepeaterItem ri in rptRoles.Items)
            {
                CheckBox chkRole = (CheckBox)ri.FindControl("chkRole");
                if (chkRole.Checked)
                {
                    
                    int roleId = (int)this.ViewState[ri.UniqueID];
                    
                    Role role = _presenter.GetRoleById(roleId);
                    AppUserRole urole = new AppUserRole()
                    {
                        AppUser = _presenter.CurrentUser,
                        Role = role
                    };
                    
                    _presenter.CurrentUser.AppUserRoles.Add(urole);
                }
            }

            string adminRole = UserSettings.GetAdministratorRole;
            if (_presenter.CurrentUser.UserName == UserSettings.GetSuperUser
                && ! _presenter.CurrentUser.IsInRole(adminRole))
            {
                throw new Exception(String.Format("The user '{0}' has to have the '{1}' role."
                    , _presenter.CurrentUser.UserName, adminRole));
            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                //try
                //{
                    SetRoles();
                    if (_presenter.CurrentUser.Id == 0)
                    {
                        _presenter.SaveOrUpdateUser();
                        Master.TransferMessage(new AppMessage("User created successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
                        _presenter.RedirectPage(String.Format("~/Admin/UserEdit.aspx?{0}=0&{1}={2}", AppConstants.TABID, AppConstants.USERID, _presenter.CurrentUser.Id));
                    }
                    else
                    {
                        _presenter.SaveOrUpdateUser();
                        Master.ShowMessage(new AppMessage("User saved", Chai.WorkflowManagment.Enums.RMessageType.Info));
                    }
                //}
                //catch (Exception ex)
                //{
                //    Master.ShowMessage(new AppMessage("Error: " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
                //}
            }
        }

        #region IUserEditView Members

        public string GetUserId
        {
            get { return Request.QueryString[AppConstants.USERID];}
        }
     
        public string GetUserName
        {
            get { return this.txtUsername.Text; }
        }

        public string GetFirstName
        {
            get { return this.txtFirstname.Text; }
        }

        public string GetLastName
        {
            get { return this.txtLastname.Text; }
        }
        public string GetEmployeeNo
        {
            get { return this.txtEmployeeNo.Text; }
        }
        public string GetEmail
        {
            get { return this.txtEmail.Text; }
        }

        public bool GetIsActive
        {
            get { 
                return this.chkActive.Checked; 
            }
        }

        public string GetPassword
        {
            get 
            {
                return this.txtPassword1.Text; 
            }
        }
        public CoreDomain.Setting.EmployeePosition EmployeePosition
        {
            get
            {
                return _presenter.GetEmployeePosition(int.Parse(ddlEmployeePostion.SelectedValue));
            }

        }

       
        public int Superviser
        {
            get { return int.Parse(ddlSuperviser.SelectedValue); }
        }
        #endregion

        protected void rptRoles_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Role role = e.Item.DataItem as Role;
            if (role != null)
            {
                CheckBox chkView = (CheckBox)e.Item.FindControl("chkRole");
                chkView.Checked = this._presenter.CurrentUser.IsInRole(role);

                // Add RoleId to the ViewState with the ClientID of the repeateritem as key.
                this.ViewState[e.Item.UniqueID] = role.Id;
            }
        }
                
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            _presenter.CancelPage();
        }


        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                _presenter.DeleteUser();
                Master.TransferMessage(new AppMessage("User was deleted", Chai.WorkflowManagment.Enums.RMessageType.Info));
                _presenter.CancelPage();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }


        
    }
}

